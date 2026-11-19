using MySql.Data.MySqlClient; // Ensure you have the MySQL Connector/NET installed
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

namespace GradingSystem1
{
    public partial class StudentForm : Form
    {
        private string currentUsername; // 👈 Store the username

        private string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        public StudentForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "System" : username;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
        }

        private void StundentForm_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents(string searchTerm = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query;

                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        //query = "SELECT StudentID, FirstName, LastName, Course, YearLevel FROM Students WHERE DeletedBy IS NULL";

                        // Use the view for active students
                        query = "SELECT * FROM ViewActiveStudents ORDER BY StudentID DESC";
                    }
                    else
                    {
                        //query = @"SELECT StudentID, FirstName, LastName, Course, YearLevel FROM Students 
                        //          WHERE CONCAT(StudentID, FirstName, LastName, Course, YearLevel) 
                        //          LIKE @search AND DeletedBy IS NULL";

                        // Use the view for active students with search term
                        query = @"SELECT * FROM ViewActiveStudents
                        WHERE CONCAT(StudentID, FirstName, LastName, Course, YearLevel)
                        LIKE @search ORDER BY StudentID DESC";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvStudents.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text) ||
                cmbYearLevel.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtCourse.Clear();
            cmbYearLevel.SelectedIndex = -1;
            txtFirstName.Focus();
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtFirstName.Text = dgvStudents.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                txtLastName.Text = dgvStudents.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
                txtCourse.Text = dgvStudents.Rows[e.RowIndex].Cells["Course"].Value.ToString();
                cmbYearLevel.Text = dgvStudents.Rows[e.RowIndex].Cells["YearLevel"].Value.ToString();
            }
        }

        

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        //private void btnSearch_Click(object sender, EventArgs e)
        //{
        //    LoadUsers(txtSearch.Text);
        //}

        private void btnAddStudent_Click_1(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Check for existing student
                        string checkQuery = "SELECT COUNT(*) FROM Students WHERE FirstName = @firstName AND LastName = @lastName AND Course = @course";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                        checkCmd.Parameters.AddWithValue("@lastName", txtLastName.Text);
                        checkCmd.Parameters.AddWithValue("@course", txtCourse.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Student already exists.");
                            return;
                        }

                        // Insert new student using stored procedure
                        MySqlCommand cmd = new MySqlCommand("InsertStudentStudentForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure;  // Specify that this is a stored procedure
                        cmd.Parameters.AddWithValue("@p_firstname", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@p_lastname", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@p_course", txtCourse.Text);
                        cmd.Parameters.AddWithValue("@p_yearlevel", Convert.ToInt32(cmbYearLevel.Text));
                        cmd.Parameters.AddWithValue("@p_createdby", currentUsername); // for activitylog table
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student added successfully.");
                        LoadStudents();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding student: " + ex.ToString());
                }
            }
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to update.");
                return;
            }

            if (ValidateFields())
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["StudentID"].Value);

                        // Update student using stored procedure
                        MySqlCommand cmd = new MySqlCommand("UpdateStudentStudentForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure; // Specify that this is a stored procedure
                        cmd.Parameters.AddWithValue("@p_firstname", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@p_lastname", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@p_course", txtCourse.Text);
                        cmd.Parameters.AddWithValue("@p_yearlevel", Convert.ToInt32(cmbYearLevel.Text));
                        cmd.Parameters.AddWithValue("@p_studentid", studentId);
                        cmd.Parameters.AddWithValue("@p_updatedby", currentUsername); // for activitylog table
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student updated.");
                        LoadStudents();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating student: " + ex.Message);
                }
            }
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["StudentID"].Value);

                        // Delete student using stored procedure
                        MySqlCommand cmd = new MySqlCommand("DeleteStudentStudentForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure; // Specify that this is a stored procedure
                        cmd.Parameters.AddWithValue("@p_studentid", studentId);
                        cmd.Parameters.AddWithValue("@p_deletedby", currentUsername); // for activitylog table
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student deleted.");
                        LoadStudents();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting student: " + ex.Message);
                }
            }
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
