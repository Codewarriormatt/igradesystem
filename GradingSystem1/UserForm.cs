using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient; // Ensure you have the MySQL Connector/NET installed

namespace GradingSystem1
{
    public partial class UserForm : Form
    {
        private string currentUsername; // 👈 Store the username

        private string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        
        // Add this parameterless constructor to allow the designer and any code using new UserForm() to work

        public UserForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "System" : username;
            //MessageBox.Show($"currentUsername: {currentUsername}"); // Debug message to verify username
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // Post-submission fix: hint for the edit flow (passwords are hashed and never shown)
            passwordHint.SetToolTip(txtPassword, "When updating a user, leave this blank to keep the current password.");
        }

        private readonly ToolTip passwordHint = new ToolTip();

        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
            //this.Text = $"Welcome, {currentUsername}!"; // 👈 Optional display
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // 🔄 Load all users or based on search term
        private void LoadUsers(string searchTerm = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query;

                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        // Post-submission fix: the Password column is no longer loaded into the grid
                        query = "SELECT UserID, Username, Role FROM Users WHERE DeletedBy IS NULL ORDER BY UserID DESC";

                        // Use the view for active users
                        //query = "SELECT * FROM ViewActiveUsers"; 
                    }
                    else
                    {
                        // Post-submission fix: the search no longer looks inside the Password column
                        query = @"SELECT UserID, Username, Role FROM Users
                                  WHERE CONCAT(UserID, Username, Role)
                                  LIKE @search AND DeletedBy IS NULL ORDER BY UserID DESC";

                        // Use the view for active users
                        //query = @"SELECT * FROM ViewActiveUsers
                        //WHERE CONCAT(UserID, Username, Role)
                        //LIKE @search;"; // Use the view for active users

                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvUsers.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Users: " + ex.Message);
            }
        }

        // ✅ Validate input fields
        // Post-submission fix: the password is optional on update (blank = keep the current password)
        private bool ValidateFields(bool passwordRequired = true)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                (passwordRequired && string.IsNullOrWhiteSpace(txtPassword.Text)) ||
                cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
            txtUsername.Focus();
        }

        // 🖱️ Populate fields when a row is selected
        private void dgvUsers_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtUsername.Text = dgvUsers.Rows[e.RowIndex].Cells["Username"].Value.ToString();
                txtPassword.Clear(); // Post-submission fix: passwords are hashed and never shown; leave blank to keep it
                cmbRole.Text = dgvUsers.Rows[e.RowIndex].Cells["Role"].Value.ToString();
            }
        }

        // ➕ Add new user
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Check for existing username
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists.");
                            return;
                        }

                        // from this : manually inserting the user using sql query
                        // string insertQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
                        // MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                        // to this : Im using a stored procedure
                        MySqlCommand cmd = new MySqlCommand("InsertUserUserForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure;  // Specify that this is a stored procedure
                        cmd.Parameters.AddWithValue("@p_username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@p_password", PasswordHasher.HashPassword(txtPassword.Text)); // Post-submission fix: store a PBKDF2 hash, not plain text
                        cmd.Parameters.AddWithValue("@p_role", cmbRole.Text);
                        cmd.Parameters.AddWithValue("@p_performedby", currentUsername); // for activitylog table
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User Added Successfully.");
                        LoadUsers();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding user: " + ex.ToString());
                }
            }
        }

        // 📝 Update existing user
        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a User to Update.");
                return;
            }

            if (ValidateFields(false))
            {
                // Post-submission fix: a blank password box keeps the current password
                bool keepPassword = string.IsNullOrWhiteSpace(txtPassword.Text);

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                        // from this : manually updating the user using sql query
                        // string updateQuery = "UPDATE Users SET Username=@username, Password=@password, Role=@role WHERE UserID=@id";
                        // MySqlCommand cmd = new MySqlCommand(updateQuery, conn);

                        // to this : Im using a stored procedure
                        MySqlCommand cmd = new MySqlCommand("UpdateUserUserForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure; // Specify that this is a stored procedure
                        cmd.Parameters.AddWithValue("@p_username", txtUsername.Text);
                        // NULL tells the stored procedure to keep the current password hash
                        cmd.Parameters.AddWithValue("@p_password", keepPassword ? (object)DBNull.Value : PasswordHasher.HashPassword(txtPassword.Text));
                        cmd.Parameters.AddWithValue("@p_role", cmbRole.Text);
                        cmd.Parameters.AddWithValue("@p_userid", userId);
                        cmd.Parameters.AddWithValue("@p_performedby", currentUsername); // for activitylog table
                        cmd.ExecuteNonQuery();

                        MessageBox.Show(keepPassword ? "User updated. Password unchanged." : "User updated.");
                        LoadUsers();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating user: " + ex.Message);
                }
            }
        }

        // ❌ Delete selected user
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are You Sure To Delete User?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                        // from this : manually deleting the user using sql query
                        // string deleteQuery = "DELETE FROM Users WHERE UserID=@id";
                        // MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        MySqlCommand cmd = new MySqlCommand("DeleteUserUserForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure; // Specify that this is a stored procedure
                        cmd.Parameters.AddWithValue("@p_userid", userId);
                        cmd.Parameters.AddWithValue("@p_performedby", currentUsername);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User Has Been Deleted!!.");
                        LoadUsers();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Deleting User: " + ex.Message);
                }
            }
        }

        // 🧹 Clear all input fields
        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // 🔍 Manual search button
        //private void btnSearch_Click(object sender, EventArgs e)
        //{
        //    LoadUsers(txtSearch.Text);
        //}

        // 🔍 Live search
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUsers(txtSearch.Text);
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
