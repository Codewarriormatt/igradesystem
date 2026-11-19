namespace GradingSystem1
{
    partial class TeacherDashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeacherDashboardForm));
            this.btnMySubjects = new System.Windows.Forms.Button();
            this.btnManageGrades = new System.Windows.Forms.Button();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMySubjects
            // 
            this.btnMySubjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMySubjects.Image = ((System.Drawing.Image)(resources.GetObject("btnMySubjects.Image")));
            this.btnMySubjects.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnMySubjects.Location = new System.Drawing.Point(271, 186);
            this.btnMySubjects.Margin = new System.Windows.Forms.Padding(2);
            this.btnMySubjects.Name = "btnMySubjects";
            this.btnMySubjects.Size = new System.Drawing.Size(275, 44);
            this.btnMySubjects.TabIndex = 0;
            this.btnMySubjects.Text = "My Subjects";
            this.btnMySubjects.UseVisualStyleBackColor = true;
            this.btnMySubjects.Click += new System.EventHandler(this.btnMySubjects_Click);
            // 
            // btnManageGrades
            // 
            this.btnManageGrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageGrades.Image = ((System.Drawing.Image)(resources.GetObject("btnManageGrades.Image")));
            this.btnManageGrades.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnManageGrades.Location = new System.Drawing.Point(271, 247);
            this.btnManageGrades.Margin = new System.Windows.Forms.Padding(2);
            this.btnManageGrades.Name = "btnManageGrades";
            this.btnManageGrades.Size = new System.Drawing.Size(275, 44);
            this.btnManageGrades.TabIndex = 1;
            this.btnManageGrades.Text = "Manage Grades";
            this.btnManageGrades.UseVisualStyleBackColor = true;
            this.btnManageGrades.Click += new System.EventHandler(this.btnManageGrades_Click);
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReports.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateReports.Image")));
            this.btnGenerateReports.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnGenerateReports.Location = new System.Drawing.Point(271, 309);
            this.btnGenerateReports.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(275, 44);
            this.btnGenerateReports.TabIndex = 2;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnLogout.Location = new System.Drawing.Point(711, 460);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(119, 34);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "LOG OUT";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Impact", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(143, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(483, 80);
            this.label1.TabIndex = 5;
            this.label1.Text = "TEACHERS PORTAL";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(115, 80);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // TeacherDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(861, 505);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.btnManageGrades);
            this.Controls.Add(this.btnMySubjects);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TeacherDashboardForm";
            this.Text = "TeacherDashboardForm";
            this.Load += new System.EventHandler(this.TeacherDashboardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMySubjects;
        private System.Windows.Forms.Button btnManageGrades;
        private System.Windows.Forms.Button btnGenerateReports;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}