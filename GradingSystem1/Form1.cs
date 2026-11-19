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
using MySql.Data.MySqlClient;

namespace GradingSystem1
{
    // Partial class for the login form
    public partial class LoginForm : Form
    {
        // Constructor: Initializes the login form and its components
        public LoginForm()
        {
            InitializeComponent();
        }
        
        // Connection string for connecting to the MySQL database
        string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;

        // Event handler for form load (currently not used)
        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        // Event handler for the Login button click
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            // Get user inputs from the username and password textboxes
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Input validation: Check if either field is empty
            if (username == "" || password == "")
            {
                lblMessage.Text = "Please enter both username and password.";
                return;
            }

            try
            {
                // Create a new MySQL connection using the connection string
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Prepare a parameterized SQL query to prevent SQL injection
                    string query = "SELECT * FROM Users WHERE Username = @username AND DeletedBy IS NULL";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    // Execute the query and read the results
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Post-submission fix: the stored password is a PBKDF2 hash, so it is checked in code
                    if (reader.Read() && PasswordHasher.VerifyPassword(password, reader["Password"].ToString()))
                    {
                        // If a matching user is found, retrieve the user's role
                        string role = reader["Role"].ToString();

                        // Show a success message with the user's role
                        MessageBox.Show($"Login successful as {role} {username}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Hide the login form and redirect to the appropriate dashboard based on role
                        this.Hide();
                        if (role == "Admin")
                        {
                            // Placeholder for opening the Admin dashboard
                            AdminDashboardForm adminForm = new AdminDashboardForm(username); // Pass the username to the Admin dashboard
                            adminForm.Show();
                            this.Hide(); 
                        }
                        else if (role == "Teacher")
                        {
                            // Placeholder for opening the Teacher dashboard
                            TeacherDashboardForm teacherForm = new TeacherDashboardForm(username);
                            teacherForm.Show();
                            this.Hide();
                            // this.Close(); Close the login form after showing the dashboard
                        }
                    }
                    else
                    {
                        // If login fails, show an error message and clear the input fields
                        lblMessage.Text = "Invalid username or password.";
                        txtUsername.Clear();
                        txtPassword.Clear();
                        txtUsername.Focus();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during database connection or query execution
                MessageBox.Show("Database connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
