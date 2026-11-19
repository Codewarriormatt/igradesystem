using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
    public partial class TeacherReportsForm : Form
    {

        private string connectionString =
            ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;

        private string currentUsername;
        private int currentTeacherId = -1;
        private int currentSubjectId = -1;
        public TeacherReportsForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrWhiteSpace(username) ? "Teacher" : username;

            /* wire events */
            this.Load += TeacherReportsForm_Load;
            btnGradesReport.Click += btnGradesReport_Click;
            cmbSubjects.SelectedIndexChanged += cmbSubjects_SelectedIndexChanged;

            reportViewer1.ProcessingMode = ProcessingMode.Local;
        }

        private void TeacherReportsForm_Load(object sender, EventArgs e)
        {
            ResolveTeacherId();
            LoadTeacherSubjects();
            this.reportViewer1.RefreshReport();
        }
        private void ResolveTeacherId()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "SELECT UserID FROM Users WHERE Username = @u AND Role='Teacher' AND DeletedBy IS NULL",
                    conn);
                cmd.Parameters.AddWithValue("@u", currentUsername);
                object obj = cmd.ExecuteScalar();
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
                    string sql = @"
                SELECT sb.SubjectID,
                       CONCAT(sb.SubjectCode,' - ',sb.SubjectName) AS SubjectFull
                FROM TeacherSubjects ts
                INNER JOIN Subjects sb ON sb.SubjectID = ts.SubjectID
                WHERE ts.TeacherID = @tid
                  AND ts.DeletedBy IS NULL
                  AND sb.DeletedBy IS NULL
                ORDER BY sb.SubjectCode";

                    var da = new MySqlDataAdapter(sql, conn);
                    var dt = new DataTable();
                    da.SelectCommand.Parameters.AddWithValue("@tid", currentTeacherId);
                    da.Fill(dt);

                    // ✅ Set DisplayMember and ValueMember before DataSource
                    cmbSubjects.DisplayMember = "SubjectFull";
                    cmbSubjects.ValueMember = "SubjectID";
                    cmbSubjects.DataSource = dt;

                    cmbSubjects.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }


        private void btnGradesReport_Click(object sender, EventArgs e)
        {
            if (cmbSubjects.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject first.");
                return;
            }

            int selectedSubjectId;
            if (!int.TryParse(cmbSubjects.SelectedValue?.ToString(), out selectedSubjectId))
            {
                MessageBox.Show("Invalid subject selection.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"
                        SELECT 
                            s.StudentID,
                            CONCAT(s.FirstName,' ',s.LastName) AS FullName,
                            subj.SubjectCode,
                            subj.SubjectName,
                            g.Grade,
                            g.Remarks
                        FROM Grades g
                        INNER JOIN Students s    ON s.StudentID  = g.StudentID
                        INNER JOIN Subjects subj ON subj.SubjectID = g.SubjectID
                        INNER JOIN TeacherSubjects ts ON ts.SubjectID = g.SubjectID
                        WHERE g.DeletedBy IS NULL
                          AND s.DeletedBy IS NULL
                          AND subj.DeletedBy IS NULL
                          AND ts.TeacherID = @tid
                          AND g.SubjectID  = @sid
                        ORDER BY s.StudentID";
                        // ORDER BY s.LastName, s.FirstName";
                    

                var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tid", currentTeacherId);
                    cmd.Parameters.AddWithValue("@sid", currentSubjectId);

                    var da = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "GradesPerSubjectReport.rdlc");
                    reportViewer1.LocalReport.DataSources
                        .Add(new ReportDataSource("GradesPerSubjectDataTable", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Grades report: " + ex.Message);
            }
        }

        private void cmbSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubjects.SelectedIndex != -1)
            {
                object selected = cmbSubjects.SelectedValue;
                if (selected != null && int.TryParse(selected.ToString(), out int sid))
                {
                    currentSubjectId = sid;
                }
            }
        }

        private void btnPassFailReport_Click(object sender, EventArgs e)
        {
            if (cmbSubjects.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject first.");
                return;
            }

            int selectedSubjectId;
            if (!int.TryParse(cmbSubjects.SelectedValue?.ToString(), out selectedSubjectId))
            {
                MessageBox.Show("Invalid subject selection.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    subj.SubjectCode,
                    subj.SubjectName,
                    COUNT(g.GradeID) AS TotalStudents,
                    SUM(CASE WHEN g.Remarks = 'Passed' THEN 1 ELSE 0 END) AS TotalPassed,
                    SUM(CASE WHEN g.Remarks = 'Failed' THEN 1 ELSE 0 END) AS TotalFailed
                FROM Grades g
                INNER JOIN Subjects subj ON subj.SubjectID = g.SubjectID
                INNER JOIN TeacherSubjects ts ON ts.SubjectID = g.SubjectID
                WHERE g.DeletedBy IS NULL
                  AND subj.DeletedBy IS NULL
                  AND ts.TeacherID = @tid
                  AND g.SubjectID = @sid
                GROUP BY subj.SubjectCode, subj.SubjectName
                ORDER BY subj.SubjectCode";

                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tid", currentTeacherId);
                    cmd.Parameters.AddWithValue("@sid", selectedSubjectId);

                    var da = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "PassFailSummaryReport.rdlc");
                    reportViewer1.LocalReport.DataSources
                        .Add(new ReportDataSource("PassFailSummaryDataTable", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Pass/Fail report: " + ex.Message);
            }
        }

        private void btnStudentsPerSubject_Click(object sender, EventArgs e)
        {
            if (cmbSubjects.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject first.");
                return;
            }

            int selectedSubjectId;
            if (!int.TryParse(cmbSubjects.SelectedValue?.ToString(), out selectedSubjectId))
            {
                MessageBox.Show("Invalid subject selected.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"
                SELECT 
                    st.StudentID,
                    CONCAT(st.FirstName, ' ', st.LastName) AS FullName,
                    st.Course,
                    st.YearLevel,
                    sb.SubjectCode,
                    sb.SubjectName
                FROM StudentSubjects ss
                INNER JOIN Students st ON st.StudentID = ss.StudentID
                INNER JOIN Subjects sb ON sb.SubjectID = ss.SubjectID
                INNER JOIN TeacherSubjects ts ON ts.SubjectID = ss.SubjectID
                WHERE st.DeletedBy IS NULL
                  AND sb.DeletedBy IS NULL
                  AND ts.DeletedBy IS NULL
                  AND ss.DeletedBy IS NULL
                  AND ts.TeacherID = @tid
                  AND ss.SubjectID = @sid
                ORDER BY st.StudentID";
                //ORDER BY st.LastName, st.FirstName;";

                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tid", currentTeacherId);
                    cmd.Parameters.AddWithValue("@sid", selectedSubjectId);

                    var da = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "StudentsPerSubjectReport.rdlc");
                    reportViewer1.LocalReport.DataSources
                        .Add(new ReportDataSource("StudentsPerSubjectDataTable", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating student list: " + ex.Message);
            }
        }
    }
}
