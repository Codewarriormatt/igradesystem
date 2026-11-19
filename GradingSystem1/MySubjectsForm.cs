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
    public partial class MySubjectsForm : Form
    {

        private  string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;
        private  string currentUsername;
        private int currentTeacherId = -1;     // resolved on load
      
        public MySubjectsForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "System" : username;

            // ─── UI wiring ────────────────────────────────────────────────
            txtSearch.TextChanged += (s, e) => LoadSubjects();
            cmbUnits.SelectedIndexChanged += (s, e) => LoadSubjects();
            btnRefresh.Click += (s, e) => RefreshFilters();
        }

        private void MySubjectsForm_Load(object sender, EventArgs e)
        {
            ResolveTeacherId();
            InitUnitFilter();
            LoadSubjects();
        }

        private void ResolveTeacherId()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string q = "SELECT UserID FROM Users WHERE Username = @uname AND Role = 'Teacher' AND DeletedBy IS NULL";
                    using (var cmd = new MySqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@uname", currentUsername);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Teacher account not found or inactive.");
                            Close();
                            return;
                        }
                        currentTeacherId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error resolving teacher ID: " + ex.Message);
                Close();
            }
        }

        private void InitUnitFilter()
        {
            cmbUnits.Items.Clear();
            cmbUnits.Items.Add("All");
            cmbUnits.Items.AddRange(new object[] { "1", "2", "3" });
            cmbUnits.SelectedIndex = 0; // default to “All”
        }

        private void RefreshFilters()
        {
            txtSearch.Clear();
            cmbUnits.SelectedIndex = 0;
            txtSearch.Focus();
            LoadSubjects();
        }

        private void LoadSubjects()
        {
            if (currentTeacherId == -1) return; // safety

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string baseQuery = @"
                        SELECT 
                            ts.ID             AS AssignmentID,
                            s.SubjectID,
                            s.SubjectCode,
                            s.SubjectName,
                            s.Units,
                            ts.CreatedBy
                        FROM TeacherSubjects ts
                        INNER JOIN Subjects s ON s.SubjectID = ts.SubjectID
                        WHERE ts.TeacherID = @tid
                          AND ts.DeletedBy IS NULL
                          AND s.DeletedBy  IS NULL";

                    // ─── Dynamic filters ──────────────────────────────
                    string searchTerm = txtSearch.Text.Trim();
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        baseQuery += " AND (s.SubjectCode LIKE @search OR s.SubjectName LIKE @search)";
                    }

                    if (cmbUnits.SelectedIndex > 0) // “All” = index 0
                    {
                        baseQuery += " AND s.Units = @units";
                    }

                    baseQuery += " ORDER BY s.SubjectCode";

                    using (var cmd = new MySqlCommand(baseQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@tid", currentTeacherId);

                        if (!string.IsNullOrEmpty(searchTerm))
                            cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        if (cmbUnits.SelectedIndex > 0)
                            cmd.Parameters.AddWithValue("@units", Convert.ToInt32(cmbUnits.SelectedItem));

                        var adapter = new MySqlDataAdapter(cmd);
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvSubjects.DataSource = dt;

                        // Update count label
                        lblSubjectCount.Text = dt.Rows.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }

    }
}
