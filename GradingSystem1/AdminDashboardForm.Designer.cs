namespace GradingSystem1
{
    partial class AdminDashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminDashboardForm));
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnManageStudents = new System.Windows.Forms.Button();
            this.btnManageSubjects = new System.Windows.Forms.Button();
            this.btnAssignSubjects = new System.Windows.Forms.Button();
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnAssignStudentSubjects = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.AutoEllipsis = true;
            this.btnManageUsers.BackColor = System.Drawing.Color.Transparent;
            this.btnManageUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageUsers.Image = ((System.Drawing.Image)(resources.GetObject("btnManageUsers.Image")));
            this.btnManageUsers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageUsers.Location = new System.Drawing.Point(148, 217);
            this.btnManageUsers.Margin = new System.Windows.Forms.Padding(2);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(354, 53);
            this.btnManageUsers.TabIndex = 0;
            this.btnManageUsers.Text = "MANAGE USERS";
            this.btnManageUsers.UseVisualStyleBackColor = false;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnManageStudents
            // 
            this.btnManageStudents.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageStudents.Image = ((System.Drawing.Image)(resources.GetObject("btnManageStudents.Image")));
            this.btnManageStudents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageStudents.Location = new System.Drawing.Point(148, 288);
            this.btnManageStudents.Margin = new System.Windows.Forms.Padding(2);
            this.btnManageStudents.Name = "btnManageStudents";
            this.btnManageStudents.Size = new System.Drawing.Size(354, 58);
            this.btnManageStudents.TabIndex = 1;
            this.btnManageStudents.Text = "MANAGE STUDENTS";
            this.btnManageStudents.UseVisualStyleBackColor = true;
            this.btnManageStudents.Click += new System.EventHandler(this.btnManageStudents_Click);
            // 
            // btnManageSubjects
            // 
            this.btnManageSubjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageSubjects.Image = ((System.Drawing.Image)(resources.GetObject("btnManageSubjects.Image")));
            this.btnManageSubjects.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageSubjects.Location = new System.Drawing.Point(148, 368);
            this.btnManageSubjects.Margin = new System.Windows.Forms.Padding(2);
            this.btnManageSubjects.Name = "btnManageSubjects";
            this.btnManageSubjects.Size = new System.Drawing.Size(354, 55);
            this.btnManageSubjects.TabIndex = 2;
            this.btnManageSubjects.Text = "MANAGE SUBJECTS";
            this.btnManageSubjects.UseVisualStyleBackColor = true;
            this.btnManageSubjects.Click += new System.EventHandler(this.btnManageSubjects_Click);
            // 
            // btnAssignSubjects
            // 
            this.btnAssignSubjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssignSubjects.Image = ((System.Drawing.Image)(resources.GetObject("btnAssignSubjects.Image")));
            this.btnAssignSubjects.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAssignSubjects.Location = new System.Drawing.Point(541, 288);
            this.btnAssignSubjects.Margin = new System.Windows.Forms.Padding(2);
            this.btnAssignSubjects.Name = "btnAssignSubjects";
            this.btnAssignSubjects.Size = new System.Drawing.Size(400, 87);
            this.btnAssignSubjects.TabIndex = 3;
            this.btnAssignSubjects.Text = "           ASSIGN SUBJECTS TO TEACHERS\r\n";
            this.btnAssignSubjects.UseVisualStyleBackColor = true;
            this.btnAssignSubjects.Click += new System.EventHandler(this.btnAssignSubjects_Click);
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewLogs.Image = ((System.Drawing.Image)(resources.GetObject("btnViewLogs.Image")));
            this.btnViewLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewLogs.Location = new System.Drawing.Point(546, 491);
            this.btnViewLogs.Margin = new System.Windows.Forms.Padding(2);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new System.Drawing.Size(395, 73);
            this.btnViewLogs.TabIndex = 4;
            this.btnViewLogs.Text = "View Activity Logs";
            this.btnViewLogs.UseVisualStyleBackColor = true;
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReports.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateReports.Image")));
            this.btnGenerateReports.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerateReports.Location = new System.Drawing.Point(546, 388);
            this.btnGenerateReports.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(395, 86);
            this.btnGenerateReports.TabIndex = 5;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Location = new System.Drawing.Point(974, 572);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(161, 42);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "LOG OUT";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnAssignStudentSubjects
            // 
            this.btnAssignStudentSubjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssignStudentSubjects.Image = ((System.Drawing.Image)(resources.GetObject("btnAssignStudentSubjects.Image")));
            this.btnAssignStudentSubjects.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAssignStudentSubjects.Location = new System.Drawing.Point(541, 201);
            this.btnAssignStudentSubjects.Margin = new System.Windows.Forms.Padding(2);
            this.btnAssignStudentSubjects.Name = "btnAssignStudentSubjects";
            this.btnAssignStudentSubjects.Size = new System.Drawing.Size(400, 81);
            this.btnAssignStudentSubjects.TabIndex = 7;
            this.btnAssignStudentSubjects.Text = "ASSIGN STUDENTS TO SUBJECT";
            this.btnAssignStudentSubjects.UseVisualStyleBackColor = true;
            this.btnAssignStudentSubjects.Click += new System.EventHandler(this.btnAssignStudentSubjects_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(211, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(74, 70);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Impact", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(409, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 80);
            this.label1.TabIndex = 9;
            this.label1.Text = "WELCOME ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Impact", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(300, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(518, 80);
            this.label2.TabIndex = 10;
            this.label2.Text = "ADMIN DASHBOARD";
            // 
            // AdminDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1152, 632);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnAssignStudentSubjects);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.btnViewLogs);
            this.Controls.Add(this.btnAssignSubjects);
            this.Controls.Add(this.btnManageSubjects);
            this.Controls.Add(this.btnManageStudents);
            this.Controls.Add(this.btnManageUsers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AdminDashboardForm";
            this.Text = "AdminDashboardForm";
            this.Load += new System.EventHandler(this.AdminDashboardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnManageStudents;
        private System.Windows.Forms.Button btnManageSubjects;
        private System.Windows.Forms.Button btnAssignSubjects;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Button btnGenerateReports;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnAssignStudentSubjects;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}