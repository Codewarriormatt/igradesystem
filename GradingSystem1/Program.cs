using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GradingSystem1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());

            // Admin Dashboard
            //Application.Run(new UserForm());
            //Application.Run(new StudentForm("wow"));
            //Application.Run(new SubjectForm("wow"));
            //Application.Run(new ActivityLogForm("hehe"));
            //Application.Run(new AssignSubjectForm("hehe"));
            //Application.Run(new ReportsForm());

            // Teacher Dashboard
            // Application.Run(new TeacherDashboardForm("hehe"));    
        }
    }
}
