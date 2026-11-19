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
using MySql.Data.MySqlClient; // Ensure you have the MySql.Data package installed via NuGet

namespace GradingSystem1
{
    public partial class ActivityLogForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = ConfigurationManager.ConnectionStrings["GradingSystemDB"].ConnectionString;

        // Constructor for the ActivityLogForm
        public ActivityLogForm()
        {
            InitializeComponent(); // Initialize form controls and components

            // Attach event handlers to search and date filter controls
            txtSearch.TextChanged += (s, e) => LoadLogs(); // Reload logs when search text changes
            dtFrom.ValueChanged += (s, e) => LoadLogs();   // Reload logs when 'from' date changes
            dtTo.ValueChanged += (s, e) => LoadLogs();     // Reload logs when 'to' date changes
        }

        // Event handler for form load
        private void ActivityLogForm_Load(object sender, EventArgs e)
        {
            // Set default date range: from 7 days ago to today
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today;
            LoadLogs(); // Load logs with default filters
        }

        // Loads activity logs from the database with current filters
        private void LoadLogs()
        {
            try
            {
                // Open a new MySQL connection
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open(); // Open the connection

                    // SQL query to select logs with search and date filters
                    //string query = @"SELECT LogID, ActionType, TableName, RecordID, PerformedBy, Timestamp 
                    //                 FROM ActivityLog
                    //                 WHERE 
                    //                    (ActionType LIKE @search OR TableName LIKE @search OR PerformedBy LIKE @search)
                    //                    AND Timestamp BETWEEN @from AND @to
                    //                 ORDER BY Timestamp DESC";

                    string query = @"SELECT LogID, ActionType, TableName, RecordID, PerformedBy, Timestamp 
                                     FROM ActivityLog
                                     WHERE 
                                        (CAST(LogID AS CHAR) LIKE @search OR ActionType LIKE @search OR TableName LIKE @search OR PerformedBy LIKE @search)
                                        AND Timestamp BETWEEN @from AND @to
                                     ORDER BY Timestamp DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn); // Prepare the command
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%"); // Add search parameter

                    // Get the date range from the DateTimePickers
                    DateTime fromDate = dtFrom.Value.Date; // Start of 'from' day
                    DateTime toDate = dtTo.Value.Date.AddDays(1).AddSeconds(-1); // End of 'to' day (inclusive)
                    cmd.Parameters.AddWithValue("@from", fromDate); // Add 'from' date parameter
                    cmd.Parameters.AddWithValue("@to", toDate);     // Add 'to' date parameter

                    // Fill a DataTable with the query results
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvLogs.DataSource = table; // Bind the results to the DataGridView

                    // Optional: format the Timestamp column for better readability
                    if (dgvLogs.Columns["Timestamp"] != null)
                        dgvLogs.Columns["Timestamp"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                }
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show("Error loading logs: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reset search box
            txtSearch.Clear();
            txtSearch.Focus();

            // Reset date pickers to default range (past 7 days to today)
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today;

            // Reload logs with default filters
            LoadLogs();
        }
    }
}
