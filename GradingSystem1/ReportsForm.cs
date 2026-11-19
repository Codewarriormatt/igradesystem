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
using Microsoft.Reporting.WinForms;

namespace GradingSystem1
{
    public partial class ReportsForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        public ReportsForm()
        {
            InitializeComponent();
            reportViewer1.ProcessingMode = ProcessingMode.Local;
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void btnUsersReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT UserID, Username, Role, CreatedBy FROM Users WHERE DeletedBy IS NULL";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Set RDLC report path and data
                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "UsersReport.rdlc"); // Adjust if placed in subfolder
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("UsersDataSet", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating User Report: " + ex.Message);
            }
        }

        private void btnStudentsReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT StudentID, FirstName, LastName, Course, YearLevel, CreatedBy 
                             FROM Students 
                             WHERE DeletedBy IS NULL";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "StudentsReport.rdlc"); // Adjust path if needed
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("StudentsDataSet", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Student Report: " + ex.Message);
            }
        }

        private void btnSubjectsReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT SubjectID, SubjectCode, SubjectName, Units, CreatedBy 
                             FROM Subjects 
                             WHERE DeletedBy IS NULL";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "SubjectsReport.rdlc"); // Adjust path if needed
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("SubjectsDataSet", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Subject Report: " + ex.Message);
            }
        }

        private void btnAssignmentsReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    ts.ID,
                    u.Username AS Teacher,
                    s.SubjectName AS Subject,
                    ts.CreatedBy
                FROM 
                    TeacherSubjects ts
                INNER JOIN Users u ON u.UserID = ts.TeacherID
                INNER JOIN Subjects s ON s.SubjectID = ts.SubjectID
                WHERE ts.DeletedBy IS NULL
                ORDER BY u.Username";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "AssignmentsReport.rdlc");
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("AssignmentsDataSet", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Assignments Report: " + ex.Message);
            }
        }

        private void btnActivityLogReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    LogID, ActionType, TableName, RecordID, PerformedBy, Timestamp 
                FROM ActivityLog
                ORDER BY Timestamp DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath, "ActivityLogReport.rdlc");
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ActivityLogDataSet", dt));
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Activity Log Report: " + ex.Message);
            }
        }
    }
}
