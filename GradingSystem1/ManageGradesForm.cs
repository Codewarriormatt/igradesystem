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
    public partial class ManageGradesForm : Form
    {
        private readonly string connectionString =
           ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;

        private readonly string currentUsername;
        private int currentTeacherId = -1;
        private int currentSubjectId = -1;

        public ManageGradesForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "Teacher" : username;

            // Wire UI events
            btnLoad.Click += btnLoad_Click;
            btnSaveGrades.Click += btnSaveGrades_Click;
            btnRefresh.Click += (s, e) => ClearFilters();
            txtSearch.TextChanged += (s, e) => FilterGrid();
            dgvGrades.CellValidating += dgvGrades_CellValidating;
        }

        private void ManageGradesForm_Load(object sender, EventArgs e)
        {
            ResolveTeacherId();
            LoadTeacherSubjects();
        }

        // Replace the 'using var' declaration with an explicit using statement block to ensure compatibility with C# 7.3.

        private void ResolveTeacherId()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "SELECT UserID FROM Users WHERE Username = @u AND Role='Teacher' AND DeletedBy IS NULL",
                    conn);
                cmd.Parameters.AddWithValue("@u", currentUsername);
                var obj = cmd.ExecuteScalar();
                if (obj == null)
                {
                    MessageBox.Show("Teacher account not found or inactive.");
                    Close();
                    return;
                }
                currentTeacherId = Convert.ToInt32(obj);
            }
        }

        private void LoadTeacherSubjects()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var sql = @"SELECT sb.SubjectID,
                                       CONCAT(sb.SubjectCode,' - ',sb.SubjectName) AS SubjectFull
                                FROM TeacherSubjects ts
                                INNER JOIN Subjects sb ON sb.SubjectID = ts.SubjectID
                                WHERE ts.TeacherID = @tid
                                  AND ts.DeletedBy IS NULL
                                  AND sb.DeletedBy IS NULL
                                ORDER BY sb.SubjectCode";
                    var da = new MySqlDataAdapter(sql, conn);
                    da.SelectCommand.Parameters.AddWithValue("@tid", currentTeacherId);
                    var dt = new DataTable();
                    da.Fill(dt);

                    cmbSubjects.DataSource = dt;
                    cmbSubjects.DisplayMember = "SubjectFull";
                    cmbSubjects.ValueMember = "SubjectID";
                    cmbSubjects.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (cmbSubjects.SelectedIndex == -1)
            {
                MessageBox.Show("Select a subject first.");
                return;
            }
            currentSubjectId = Convert.ToInt32(cmbSubjects.SelectedValue);
            LoadGradesGrid();
        }

        private void LoadGradesGrid()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var sql = @"
                        SELECT 
                            g.GradeID,
                            st.StudentID,
                            st.FirstName,
                            st.LastName,
                            IFNULL(g.Grade, '')     AS Grade,
                            IFNULL(g.Remarks, '')   AS Remarks
                        FROM StudentSubjects ss
                        INNER JOIN Students st ON st.StudentID = ss.StudentID
                        LEFT  JOIN Grades  g ON g.StudentID = st.StudentID
                                            AND g.SubjectID = @sid
                                            AND g.DeletedBy IS NULL
                        WHERE ss.SubjectID = @sid
                          AND ss.DeletedBy IS NULL
                          AND st.DeletedBy IS NULL
                        ORDER BY st.StudentID ASC";
                        //ORDER BY st.LastName, st.FirstName";
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sid", currentSubjectId);

                    var da = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvGrades.DataSource = dt;

                    if (dgvGrades.Columns.Contains("GradeID"))
                        dgvGrades.Columns["GradeID"].Visible = false;

                    dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    lblStudentCount.Text = "Total Students: " + dt.Rows.Count;

                    FilterGrid(); // apply any search filter
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading grades: " + ex.Message);
            }
        }

        /* ─────────────────── Search Filter ─────────────────── */
        private void FilterGrid()
        {
            var dt = dgvGrades.DataSource as DataTable;
            if (dt == null) return;

            var kw = txtSearch.Text.Trim().ToLower();
            if (kw == "")
            {
                dt.DefaultView.RowFilter = "";
            }
            else
            {
                kw = kw.Replace("'", "''"); // escape quotes for the RowFilter expression
                dt.DefaultView.RowFilter =
                    $"Convert(StudentID,'System.String') LIKE '%{kw}%' " +
                    $"OR FirstName LIKE '%{kw}%' OR LastName LIKE '%{kw}%'";
            }
            lblStudentCount.Text = "Total Students: " + dt.DefaultView.Count;
        }
        private void btnSaveGrades_Click(object sender, EventArgs e)
        {
            if (currentSubjectId == -1)
            {
                MessageBox.Show("Load a subject first.");
                return;
            }

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataGridViewRow row in dgvGrades.Rows)
                        {
                            int studentId = Convert.ToInt32(row.Cells["StudentID"].Value);
                            decimal gradeValue;
                            bool hasGrade = decimal.TryParse(row.Cells["Grade"].Value?.ToString(), out gradeValue);

                            if (!hasGrade) continue; // skip ungraded rows

                            // Determine if record exists
                            int gradeId;
                            if (row.Cells["GradeID"].Value == DBNull.Value ||
                                row.Cells["GradeID"].Value.ToString() == "")
                            {
                                gradeId = 0; // insert
                            }
                            else
                            {
                                gradeId = Convert.ToInt32(row.Cells["GradeID"].Value);
                            }

                            if (gradeId == 0)
                            {
                                string insert = @"INSERT INTO Grades
                                                  (StudentID, SubjectID, Grade, CreatedBy)
                                                  VALUES (@sid, @sub, @grade, @by)";
                                var cmd = new MySqlCommand(insert, conn, tx);
                                cmd.Parameters.AddWithValue("@sid", studentId);
                                cmd.Parameters.AddWithValue("@sub", currentSubjectId);
                                cmd.Parameters.AddWithValue("@grade", gradeValue);
                                cmd.Parameters.AddWithValue("@by", currentUsername);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string update = @"UPDATE Grades
                                                  SET Grade = @grade, UpdatedBy = @by
                                                  WHERE GradeID = @gid";
                                var cmd = new MySqlCommand(update, conn, tx);
                                cmd.Parameters.AddWithValue("@grade", gradeValue);
                                cmd.Parameters.AddWithValue("@by", currentUsername);
                                cmd.Parameters.AddWithValue("@gid", gradeId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        MessageBox.Show("Grades saved successfully.");
                        LoadGradesGrid(); // refresh to update remarks
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        MessageBox.Show("Save failed: " + ex.Message);
                    }
                }
            }
        }

        /* ─────────────────── Refresh ─────────────────── */
        private void ClearFilters()
        {
            txtSearch.Clear();
            lblStudentCount.Text = "";
            dgvGrades.DataSource = null;
            cmbSubjects.SelectedIndex = -1;
            currentSubjectId = -1;
        }

        private void dgvGrades_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvGrades.Columns[e.ColumnIndex].Name != "Grade") return;

            var newVal = e.FormattedValue?.ToString();
            if (string.IsNullOrWhiteSpace(newVal)) return; // blank allowed (no grade yet)

            decimal g;
            if (!decimal.TryParse(newVal, out g) || g < 1 || g > 100)
            {
                e.Cancel = true;
                MessageBox.Show("Grade must be a number between 1 and 100.");
            }
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
