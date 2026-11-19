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
    public partial class AssignStudentSubjectForm : Form
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        private readonly string currentUsername;
        public AssignStudentSubjectForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "System" : username;

            txtSearch.TextChanged += (s, e) => LoadAssignments();
            btnAssign.Click += btnAssign_Click;
            btnDelete.Click += btnDelete_Click;
        }

        private void AssignStudentSubjectForm_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadSubjects();
            LoadAssignments();
        }

        // Replace all instances of "using var" with explicit using statements to ensure compatibility with C# 7.3.

        private void LoadStudents()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT StudentID, CONCAT(FirstName, ' ', LastName) AS FullName FROM Students WHERE DeletedBy IS NULL";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var dt = new DataTable();
                            dt.Load(reader);
                            cmbStudent.DataSource = dt;
                            cmbStudent.DisplayMember = "FullName";
                            cmbStudent.ValueMember = "StudentID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }

        private void LoadSubjects()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT SubjectID, SubjectName FROM Subjects WHERE DeletedBy IS NULL";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var dt = new DataTable();
                            dt.Load(reader);
                            cmbSubject.DataSource = dt;
                            cmbSubject.DisplayMember = "SubjectName";
                            cmbSubject.ValueMember = "SubjectID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }

        private void LoadAssignments()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            ss.ID,
                            CONCAT(st.FirstName, ' ', st.LastName) AS Student,
                            sb.SubjectName AS Subject,
                            ss.CreatedBy
                        FROM StudentSubjects ss
                        INNER JOIN Students st ON ss.StudentID = st.StudentID
                        INNER JOIN Subjects sb ON ss.SubjectID = sb.SubjectID
                        WHERE ss.DeletedBy IS NULL
                            AND (
                                ss.ID LIKE @search OR
                                st.FirstName LIKE @search OR
                                st.LastName LIKE @search OR
                                sb.SubjectName LIKE @search
                            )
                        ORDER BY ss.ID DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);
                            dgvAssignments.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading assignments: " + ex.Message);
            }
        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex == -1 || cmbSubject.SelectedIndex == -1)
            {
                MessageBox.Show("Please select both a student and a subject.");
                return;
            }

            int studentId = Convert.ToInt32(cmbStudent.SelectedValue);
            int subjectId = Convert.ToInt32(cmbSubject.SelectedValue);

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check before inserting
                    string checkQuery = @"SELECT COUNT(*) FROM StudentSubjects 
                                          WHERE StudentID = @studentId 
                                            AND SubjectID = @subjectId 
                                            AND DeletedBy IS NULL";
                    using (var checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@studentId", studentId);
                        checkCmd.Parameters.AddWithValue("@subjectId", subjectId);

                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            MessageBox.Show("This student is already assigned to this subject.");
                            return;
                        }
                    }

                    // Insert only if not exists
                    string insertQuery = @"INSERT INTO StudentSubjects (StudentID, SubjectID, CreatedBy)
                                           VALUES (@studentId, @subjectId, @createdBy)";
                    using (var insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@studentId", studentId);
                        insertCmd.Parameters.AddWithValue("@subjectId", subjectId);
                        insertCmd.Parameters.AddWithValue("@createdBy", currentUsername);
                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Student assigned successfully.");
                    LoadAssignments(); // Safe to call once after insert
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error assigning student: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAssignments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an assignment to remove.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to remove this assignment?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            int assignmentId = Convert.ToInt32(dgvAssignments.SelectedRows[0].Cells["ID"].Value);

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string deleteQuery = "UPDATE StudentSubjects SET DeletedBy = @deletedBy WHERE ID = @id";
                    using (var cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@deletedBy", currentUsername);
                        cmd.Parameters.AddWithValue("@id", assignmentId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Assignment removed.");
                    LoadAssignments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting assignment: " + ex.Message);
            }
        }
    }
}
