using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GradingSystem1
{
    public partial class TeacherDashboardForm : Form
    {

        // Child-form references (prevent multiple instances)
        private Form mySubjectsForm;
        private Form gradesForm;
        private Form reportsForm;
        //private Form whatIfForm;

        private string currentUsername;   // who is logged in
        public TeacherDashboardForm(string username)
        {
            InitializeComponent();
            currentUsername = string.IsNullOrEmpty(username) ? "Teacher" : username;
        }

        private void TeacherDashboardForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Opens a child form if not already shown; otherwise brings it forward.
        /// </summary>
        private void OpenForm(ref Form formRef, Func<Form> factory)
        {
            if (formRef == null || formRef.IsDisposed)
            {
                formRef = factory();
                formRef.Show();          // non-modal; dashboard stays usable
            }
            else
            {
                formRef.BringToFront();
            }
        }

        private void Logout()
        {
            Application.Restart();       // simplest clean logout
        }

        // “My Subjects” – lists subjects assigned to this teacher
        private void btnMySubjects_Click(object sender, EventArgs e)
        {
            OpenForm(ref mySubjectsForm, () => new MySubjectsForm(currentUsername));
        }

        // “Manage Grades” – enter / edit grades
        private void btnManageGrades_Click(object sender, EventArgs e)
        {
            OpenForm(ref gradesForm, () => new ManageGradesForm(currentUsername));
        }

        // “Generate Reports” – teacher-scoped reports
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            OpenForm(ref reportsForm, () => new TeacherReportsForm(currentUsername));
        }

        // “What-If Calculator” – optional grade simulation
        //private void btnWhatIfCalc_Click(object sender, EventArgs e)
        //{
        //    //OpenForm(ref whatIfForm, () => new WhatIfCalculatorForm(currentUsername));
        //}

        // “Logout”
        private void btnLogout_Click(object sender, EventArgs e) => Logout();
        
    }
}
