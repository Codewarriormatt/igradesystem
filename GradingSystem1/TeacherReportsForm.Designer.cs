namespace GradingSystem1
{
    partial class TeacherReportsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGradesReport = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbSubjects = new System.Windows.Forms.ComboBox();
            this.btnStudentsPerSubject = new System.Windows.Forms.Button();
            this.btnPassFailReport = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGradesReport
            // 
            this.btnGradesReport.Location = new System.Drawing.Point(16, 138);
            this.btnGradesReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGradesReport.Name = "btnGradesReport";
            this.btnGradesReport.Size = new System.Drawing.Size(166, 48);
            this.btnGradesReport.TabIndex = 1;
            this.btnGradesReport.Text = "Grades per Subject";
            this.btnGradesReport.UseVisualStyleBackColor = true;
            this.btnGradesReport.Click += new System.EventHandler(this.btnGradesReport_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(11, 30);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(171, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Generate Report";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.cmbSubjects);
            this.panel1.Controls.Add(this.btnStudentsPerSubject);
            this.panel1.Controls.Add(this.btnPassFailReport);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.btnGradesReport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 427);
            this.panel1.TabIndex = 2;
            // 
            // cmbSubjects
            // 
            this.cmbSubjects.FormattingEnabled = true;
            this.cmbSubjects.Location = new System.Drawing.Point(16, 73);
            this.cmbSubjects.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbSubjects.Name = "cmbSubjects";
            this.cmbSubjects.Size = new System.Drawing.Size(140, 21);
            this.cmbSubjects.TabIndex = 4;
            this.cmbSubjects.SelectedIndexChanged += new System.EventHandler(this.cmbSubjects_SelectedIndexChanged);
            // 
            // btnStudentsPerSubject
            // 
            this.btnStudentsPerSubject.Location = new System.Drawing.Point(16, 263);
            this.btnStudentsPerSubject.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStudentsPerSubject.Name = "btnStudentsPerSubject";
            this.btnStudentsPerSubject.Size = new System.Drawing.Size(166, 48);
            this.btnStudentsPerSubject.TabIndex = 3;
            this.btnStudentsPerSubject.Text = " Students per Subject";
            this.btnStudentsPerSubject.UseVisualStyleBackColor = true;
            this.btnStudentsPerSubject.Click += new System.EventHandler(this.btnStudentsPerSubject_Click);
            // 
            // btnPassFailReport
            // 
            this.btnPassFailReport.Location = new System.Drawing.Point(16, 200);
            this.btnPassFailReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPassFailReport.Name = "btnPassFailReport";
            this.btnPassFailReport.Size = new System.Drawing.Size(166, 48);
            this.btnPassFailReport.TabIndex = 2;
            this.btnPassFailReport.Text = "Pass/Fail Summary";
            this.btnPassFailReport.UseVisualStyleBackColor = true;
            this.btnPassFailReport.Click += new System.EventHandler(this.btnPassFailReport_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(200, 0);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(759, 427);
            this.reportViewer1.TabIndex = 3;
            // 
            // TeacherReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 427);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TeacherReportsForm";
            this.Text = "TeacherReportsForm";
            this.Load += new System.EventHandler(this.TeacherReportsForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGradesReport;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStudentsPerSubject;
        private System.Windows.Forms.Button btnPassFailReport;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.ComboBox cmbSubjects;
    }
}