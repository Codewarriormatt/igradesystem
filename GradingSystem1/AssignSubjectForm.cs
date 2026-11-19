using MySql.Data.MySqlClient;  // Assuming you have MySql.Data installed via NuGet
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
    public partial class AssignSubjectForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        private string currentUsername;
        public AssignSubjectForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "System" : username;

            cmbTeacher.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSubject.DropDownStyle = ComboBoxStyle.DropDownList;

            txtSearch.TextChanged += (s, e) => LoadAssignments();
        }

        private void AssignSubjectForm_Load(object sender, EventArgs e)
        {
            LoadTeachers();
            LoadSubjects();
            LoadAssignments();
        }

        // 🔁 Load teachers with Role = 'Teacher'
        private void LoadTeachers()
        {
            try
            {
                cmbTeacher.Items.Clear();
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT UserID, Username FROM Users WHERE Role = 'Teacher' AND DeletedBy IS NULL";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbTeacher.DataSource = dt;
                    cmbTeacher.DisplayMember = "Username";
                    cmbTeacher.ValueMember = "UserID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading teachers: " + ex.Message);
            }
        }

        // 📚 Load active subjects
        private void LoadSubjects()
        {
            try
            {
                cmbSubject.Items.Clear();
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT SubjectID, SubjectName FROM Subjects WHERE DeletedBy IS NULL";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbSubject.DataSource = dt;
                    cmbSubject.DisplayMember = "SubjectName";
                    cmbSubject.ValueMember = "SubjectID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }

        // 📄 Load all active assignments with filters
        private void LoadAssignments()
        {
            try
            {
                // Create and open a new MySQL connection using the connection string
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL query to select assignment records, joining teacher and subject info, and filtering by search text
                    //string query = @"
                    //    SELECT ts.ID, u.Username AS Teacher, s.SubjectName AS Subject
                    //    FROM TeacherSubjects ts
                    //    INNER JOIN Users u ON u.UserID = ts.TeacherID
                    //    INNER JOIN Subjects s ON s.SubjectID = ts.SubjectID
                    //    WHERE ts.DeletedBy IS NULL
                    //      AND (u.Username LIKE @search OR s.SubjectName LIKE @search)
                    //    ORDER BY ts.ID DESC";

                    string query = @"
                    SELECT ts.ID, u.Username AS Teacher, s.SubjectName AS Subject
                    FROM TeacherSubjects ts
                    INNER JOIN Users u ON u.UserID = ts.TeacherID
                    INNER JOIN Subjects s ON s.SubjectID = ts.SubjectID
                    WHERE ts.DeletedBy IS NULL
                      AND (
                            CAST(ts.ID AS CHAR) LIKE @search OR 
                            u.Username LIKE @search OR 
                            s.SubjectName LIKE @search
                          )
                    ORDER BY ts.ID DESC";


                    //  ORDER BY u.Username";

                    // Prepare the SQL command with the query and connection
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Add the @search parameter, using the text from the search box (txtSearch)
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    // Create a data adapter to execute the query and fill a DataTable
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table); // Fill the DataTable with the query results

                    // Bind the DataTable to the DataGridView to display the assignments
                    dgvAssignments.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show("Error loading assignments: " + ex.Message);
            }
        }

        // ➕ Assign subject to teacher
        private void btnAssign_Click(object sender, EventArgs e)
        {
            if (cmbTeacher.SelectedIndex == -1 || cmbSubject.SelectedIndex == -1)
            {
                MessageBox.Show("Please select both a teacher and a subject.");
                return;
            }

            int teacherId = Convert.ToInt32(cmbTeacher.SelectedValue);
            int subjectId = Convert.ToInt32(cmbSubject.SelectedValue);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check for existing assignment (and not soft-deleted)
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM TeacherSubjects 
                        WHERE TeacherID = @teacherId AND SubjectID = @subjectId AND DeletedBy IS NULL";

                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@teacherId", teacherId);
                    checkCmd.Parameters.AddWithValue("@subjectId", subjectId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("This subject is already assigned to the selected teacher.");
                        return;
                    }

                    string insertQuery = @"
                        INSERT INTO TeacherSubjects (TeacherID, SubjectID, CreatedBy)
                        VALUES (@teacherId, @subjectId, @createdBy)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);
                    cmd.Parameters.AddWithValue("@subjectId", subjectId);
                    cmd.Parameters.AddWithValue("@createdBy", currentUsername);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Subject assigned successfully.");
                    LoadAssignments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error assigning subject: " + ex.Message);
            }
        }

        // ❌ Soft-delete selected assignment
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvAssignments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an assignment to remove.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to remove this assignment?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int assignmentId = Convert.ToInt32(dgvAssignments.SelectedRows[0].Cells["ID"].Value);

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "UPDATE TeacherSubjects SET DeletedBy = @deletedBy WHERE ID = @id";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@deletedBy", currentUsername);
                        cmd.Parameters.AddWithValue("@id", assignmentId);
                        cmd.ExecuteNonQuery();

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
}
