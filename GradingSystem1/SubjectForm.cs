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
using MySql.Data.MySqlClient; // Ensure you have the MySQL Connector/NET installed and referenced

namespace GradingSystem1
{
    public partial class SubjectForm : Form
    {
        private string currentUsername; // 👈 Store the username

        private string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        public SubjectForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "System" : username;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
        }

        private void SubjectForm_Load(object sender, EventArgs e)
        {
            LoadSubjects();
        }

        private void LoadSubjects(string searchTerm = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query;

                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        // query = "SELECT SubjectID, SubjectCode, SubjectName, Units FROM Subjects WHERE DeletedBy IS NULL";

                        // Use the view to get active subjects
                        query = "SELECT * FROM ViewActiveSubjects ORDER BY SubjectID DESC";
                    }
                    else
                    {
                        //query = @"SELECT SubjectID, SubjectCode, SubjectName, Units FROM Subjects 
                        //  WHERE CONCAT(SubjectID, SubjectCode, SubjectName, Units) 
                        //  LIKE @search AND DeletedBy IS NULL";

                        // Use the view to get active subjects with search term
                        query = @"SELECT * FROM ViewActiveSubjects
                        WHERE CONCAT(SubjectID, SubjectCode, SubjectName, Units)
                        LIKE @search ORDER BY SubjectID DESC";

                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvSubjects.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtSubjectCode.Text) ||
                string.IsNullOrWhiteSpace(txtSubjectName.Text) ||
                cmbUnits.SelectedIndex == -1) // Ensure a unit is selected
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }

            return true;
        }
        private void ClearFields()
        {
            txtSubjectCode.Clear();
            txtSubjectName.Clear();
            cmbUnits.SelectedIndex = -1;
            txtSubjectCode.Focus();
        }

        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Populate the input fields with selected row's data
                txtSubjectCode.Text = dgvSubjects.Rows[e.RowIndex].Cells["SubjectCode"].Value.ToString();
                txtSubjectName.Text = dgvSubjects.Rows[e.RowIndex].Cells["SubjectName"].Value.ToString();
                cmbUnits.Text = dgvSubjects.Rows[e.RowIndex].Cells["Units"].Value.ToString();
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSubjects(txtSearch.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }


        private void txtSubjectCode_TextChanged(object sender, EventArgs e)
        {

        }

        // Add a new subject
        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Check if the subject code already exists
                        string checkQuery = "SELECT COUNT(*) FROM Subjects WHERE SubjectCode = @subjectCode AND DeletedBy IS NULL";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@subjectCode", txtSubjectCode.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Subject code already exists.");
                            return;
                        }

                        // Insert new subject using stored procedure
                        MySqlCommand cmd = new MySqlCommand("InsertSubjectSubjectForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_subjectcode", txtSubjectCode.Text);
                        cmd.Parameters.AddWithValue("@p_subjectname", txtSubjectName.Text);
                        cmd.Parameters.AddWithValue("@p_units", Convert.ToInt32(cmbUnits.Text));
                        cmd.Parameters.AddWithValue("@p_createdby", currentUsername);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Subject added successfully.");
                        LoadSubjects();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding subject: " + ex.ToString());
                }
            }
        }

        // Update an existing subject
        private void btnUpdateSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a subject to update.");
                return;
            }

            if (ValidateFields())
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        int subjectId = Convert.ToInt32(dgvSubjects.SelectedRows[0].Cells["SubjectID"].Value);

                        MySqlCommand cmd = new MySqlCommand("UpdateSubjectSubjectForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_subjectcode", txtSubjectCode.Text);
                        cmd.Parameters.AddWithValue("@p_subjectname", txtSubjectName.Text);
                        cmd.Parameters.AddWithValue("@p_units", Convert.ToInt32(cmbUnits.Text));
                        cmd.Parameters.AddWithValue("@p_subjectid", subjectId);
                        cmd.Parameters.AddWithValue("@p_updatedby", currentUsername);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Subject updated successfully.");
                        LoadSubjects();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating subject: " + ex.Message);
                }
            }
        }

        // Delete a subject
        private void btnDeleteSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a subject to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        int subjectId = Convert.ToInt32(dgvSubjects.SelectedRows[0].Cells["SubjectID"].Value);

                        MySqlCommand cmd = new MySqlCommand("DeleteSubjectSubjectForm", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_subjectid", subjectId);
                        cmd.Parameters.AddWithValue("@p_deletedby", currentUsername);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Subject deleted.");
                        LoadSubjects();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting subject: " + ex.Message);
                }
            }
        }


    }
}
