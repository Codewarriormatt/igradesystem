using Mysqlx.Session;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GradingSystem1
{
    public partial class AdminDashboardForm : Form
    {
        // Form references declared at the class level to prevent multiple instances.
        private Form userForm; 
        private Form studentForm;
        private Form subjectForm;
        private Form assignSubjectForm;
        private Form reportForm;
        private Form logsForm;
        private Form assignStudentSubjectForm;

        private string currentUsername; // Store the current username for display or other purposes

        public AdminDashboardForm(string username) // 👈 Constructor that accepts a username parameter
        {
            InitializeComponent();
            currentUsername = username; // 👈 Store the passed username
            // Optional: show username somewhere
            //MessageBox.Show("You are logged in as " + currentUsername);
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Logs out the user by restarting the entire application.
        /// This clears all session data and reopens the login form.
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart(); // Closes and restarts the application
            // Why it's good:
            // Resets everything: memory, sessions, forms.
            // Guarantees clean logout without manually managing forms.
        }

        /// <summary>
        /// Opens a form if it's not already open.
        /// If the form is already open, it brings it to the front.
        /// Prevents multiple instances of the same form from appearing.
        /// </summary>
        /// <param name="form">Reference to the form instance</param>
        /// <param name="createForm">Function that creates a new instance of the form</param>
        private void OpenForm(ref Form form, Func<Form> createForm)
        {
            // If the form is null or has been closed
            if (form == null || form.IsDisposed)
            {
                form = createForm();  // Create a new instance of the form
                form.Show();          // Open it non-modally (dashboard remains usable)
            }
            else
            {
                form.BringToFront();  // Focus on the form if it's already open
            }
        }

        // Opens the User Management form.
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            OpenForm(ref userForm, () => new UserForm(currentUsername)); // Opens the UserForm if not already open
        }

        // Opens the Student Management form.
        private void btnManageStudents_Click(object sender, EventArgs e)
        {
            OpenForm(ref studentForm, () => new StudentForm(currentUsername));
        }

        // Opens the Subject Management form.
        private void btnManageSubjects_Click(object sender, EventArgs e)
        {
            OpenForm(ref subjectForm, () => new SubjectForm(currentUsername));
        }

        // Opens the Assign Subject form.
        private void btnAssignSubjects_Click(object sender, EventArgs e)
        {
            OpenForm(ref assignSubjectForm, () => new AssignSubjectForm(currentUsername));
        }

        // Opens the View Logs form.    
        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            OpenForm(ref logsForm, () => new ActivityLogForm());
        }

        // Opens the Generate Reports form.
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            OpenForm(ref reportForm, () => new ReportsForm());
        }

        private void btnAssignStudentSubjects_Click(object sender, EventArgs e)
        {
            OpenForm(ref assignStudentSubjectForm, () => new AssignStudentSubjectForm(currentUsername));
        }
    }
}
