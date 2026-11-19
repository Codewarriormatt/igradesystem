namespace GradingSystem1
{
    partial class ReportsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsForm));
            this.btnStudentsReport = new System.Windows.Forms.Button();
            this.btnSubjectsReport = new System.Windows.Forms.Button();
            this.btnAssignmentsReport = new System.Windows.Forms.Button();
            this.btnActivityLogReport = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnUsersReport = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStudentsReport
            // 
            this.btnStudentsReport.Location = new System.Drawing.Point(11, 159);
            this.btnStudentsReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStudentsReport.Name = "btnStudentsReport";
            this.btnStudentsReport.Size = new System.Drawing.Size(130, 41);
            this.btnStudentsReport.TabIndex = 1;
            this.btnStudentsReport.Text = "Generate Student List";
            this.btnStudentsReport.UseVisualStyleBackColor = true;
            this.btnStudentsReport.Click += new System.EventHandler(this.btnStudentsReport_Click);
            // 
            // btnSubjectsReport
            // 
            this.btnSubjectsReport.Location = new System.Drawing.Point(11, 214);
            this.btnSubjectsReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSubjectsReport.Name = "btnSubjectsReport";
            this.btnSubjectsReport.Size = new System.Drawing.Size(130, 41);
            this.btnSubjectsReport.TabIndex = 2;
            this.btnSubjectsReport.Text = "Generate Subject List";
            this.btnSubjectsReport.UseVisualStyleBackColor = true;
            this.btnSubjectsReport.Click += new System.EventHandler(this.btnSubjectsReport_Click);
            // 
            // btnAssignmentsReport
            // 
            this.btnAssignmentsReport.Location = new System.Drawing.Point(9, 271);
            this.btnAssignmentsReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAssignmentsReport.Name = "btnAssignmentsReport";
            this.btnAssignmentsReport.Size = new System.Drawing.Size(130, 41);
            this.btnAssignmentsReport.TabIndex = 3;
            this.btnAssignmentsReport.Text = "Generate Assignments";
            this.btnAssignmentsReport.UseVisualStyleBackColor = true;
            this.btnAssignmentsReport.Click += new System.EventHandler(this.btnAssignmentsReport_Click);
            // 
            // btnActivityLogReport
            // 
            this.btnActivityLogReport.Location = new System.Drawing.Point(9, 325);
            this.btnActivityLogReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnActivityLogReport.Name = "btnActivityLogReport";
            this.btnActivityLogReport.Size = new System.Drawing.Size(130, 41);
            this.btnActivityLogReport.TabIndex = 4;
            this.btnActivityLogReport.Text = "Generate Activity Logs";
            this.btnActivityLogReport.UseVisualStyleBackColor = true;
            this.btnActivityLogReport.Click += new System.EventHandler(this.btnActivityLogReport_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(150, 0);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(708, 437);
            this.reportViewer1.TabIndex = 5;
            // 
            // btnUsersReport
            // 
            this.btnUsersReport.Location = new System.Drawing.Point(11, 99);
            this.btnUsersReport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnUsersReport.Name = "btnUsersReport";
            this.btnUsersReport.Size = new System.Drawing.Size(130, 41);
            this.btnUsersReport.TabIndex = 0;
            this.btnUsersReport.Text = "Generate User List";
            this.btnUsersReport.UseVisualStyleBackColor = true;
            this.btnUsersReport.Click += new System.EventHandler(this.btnUsersReport_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Green;
            this.panelButtons.Controls.Add(this.btnUsersReport);
            this.panelButtons.Controls.Add(this.btnActivityLogReport);
            this.panelButtons.Controls.Add(this.btnStudentsReport);
            this.panelButtons.Controls.Add(this.btnAssignmentsReport);
            this.panelButtons.Controls.Add(this.btnSubjectsReport);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(150, 437);
            this.panelButtons.TabIndex = 6;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 437);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panelButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ReportsForm";
            this.Text = "ReportsForm";
            this.Load += new System.EventHandler(this.ReportsForm_Load);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnStudentsReport;
        private System.Windows.Forms.Button btnSubjectsReport;
        private System.Windows.Forms.Button btnAssignmentsReport;
        private System.Windows.Forms.Button btnActivityLogReport;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Button btnUsersReport;
        private System.Windows.Forms.Panel panelButtons;
    }
}