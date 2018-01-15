namespace Sharer.Client
{
    partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelScreenstream = new System.Windows.Forms.Label();
			this.labelArea = new System.Windows.Forms.Label();
			this.labelFullScreen = new System.Windows.Forms.Label();
			this.labelScreenstreamText = new System.Windows.Forms.Label();
			this.labelAreaText = new System.Windows.Forms.Label();
			this.labelFullScreenText = new System.Windows.Forms.Label();
			this.labelEmailText = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.linkLabelEmail = new System.Windows.Forms.LinkLabel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.logoutButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkBox_EditBeforeUpload = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Good day, sir!";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelScreenstream);
			this.groupBox1.Controls.Add(this.labelArea);
			this.groupBox1.Controls.Add(this.labelFullScreen);
			this.groupBox1.Controls.Add(this.labelScreenstreamText);
			this.groupBox1.Controls.Add(this.labelAreaText);
			this.groupBox1.Controls.Add(this.labelFullScreenText);
			this.groupBox1.Location = new System.Drawing.Point(15, 135);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(219, 67);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Hotkeys";
			// 
			// labelScreenstream
			// 
			this.labelScreenstream.AutoSize = true;
			this.labelScreenstream.Location = new System.Drawing.Point(103, 45);
			this.labelScreenstream.Name = "labelScreenstream";
			this.labelScreenstream.Size = new System.Drawing.Size(105, 14);
			this.labelScreenstream.TabIndex = 15;
			this.labelScreenstream.Text = "Ctrl+Shift+6 ^";
			// 
			// labelArea
			// 
			this.labelArea.AutoSize = true;
			this.labelArea.Location = new System.Drawing.Point(103, 31);
			this.labelArea.Name = "labelArea";
			this.labelArea.Size = new System.Drawing.Size(105, 14);
			this.labelArea.TabIndex = 14;
			this.labelArea.Text = "Ctrl+Shift+3 #";
			// 
			// labelFullScreen
			// 
			this.labelFullScreen.AutoSize = true;
			this.labelFullScreen.Location = new System.Drawing.Point(103, 17);
			this.labelFullScreen.Name = "labelFullScreen";
			this.labelFullScreen.Size = new System.Drawing.Size(105, 14);
			this.labelFullScreen.TabIndex = 13;
			this.labelFullScreen.Text = "Ctrl+Shift+2 @";
			// 
			// labelScreenstreamText
			// 
			this.labelScreenstreamText.AutoSize = true;
			this.labelScreenstreamText.Location = new System.Drawing.Point(8, 45);
			this.labelScreenstreamText.Name = "labelScreenstreamText";
			this.labelScreenstreamText.Size = new System.Drawing.Size(91, 14);
			this.labelScreenstreamText.TabIndex = 12;
			this.labelScreenstreamText.Text = "Upload file:";
			// 
			// labelAreaText
			// 
			this.labelAreaText.AutoSize = true;
			this.labelAreaText.Location = new System.Drawing.Point(8, 31);
			this.labelAreaText.Name = "labelAreaText";
			this.labelAreaText.Size = new System.Drawing.Size(42, 14);
			this.labelAreaText.TabIndex = 11;
			this.labelAreaText.Text = "Area:";
			// 
			// labelFullScreenText
			// 
			this.labelFullScreenText.AutoSize = true;
			this.labelFullScreenText.Location = new System.Drawing.Point(8, 17);
			this.labelFullScreenText.Name = "labelFullScreenText";
			this.labelFullScreenText.Size = new System.Drawing.Size(91, 14);
			this.labelFullScreenText.TabIndex = 10;
			this.labelFullScreenText.Text = "Full screen:";
			// 
			// labelEmailText
			// 
			this.labelEmailText.AutoSize = true;
			this.labelEmailText.Location = new System.Drawing.Point(8, 17);
			this.labelEmailText.Name = "labelEmailText";
			this.labelEmailText.Size = new System.Drawing.Size(49, 14);
			this.labelEmailText.TabIndex = 6;
			this.labelEmailText.Text = "Email:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(174, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(0, 14);
			this.label6.TabIndex = 8;
			// 
			// linkLabelEmail
			// 
			this.linkLabelEmail.AutoSize = true;
			this.linkLabelEmail.Location = new System.Drawing.Point(73, 16);
			this.linkLabelEmail.Name = "linkLabelEmail";
			this.linkLabelEmail.Size = new System.Drawing.Size(119, 14);
			this.linkLabelEmail.TabIndex = 9;
			this.linkLabelEmail.TabStop = true;
			this.linkLabelEmail.Text = "userEmail_Label1";
			this.linkLabelEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEmail_LinkClicked);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.logoutButton);
			this.groupBox2.Controls.Add(this.labelEmailText);
			this.groupBox2.Controls.Add(this.linkLabelEmail);
			this.groupBox2.Location = new System.Drawing.Point(14, 13);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(219, 62);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Account";
			// 
			// logoutButton
			// 
			this.logoutButton.Location = new System.Drawing.Point(74, 33);
			this.logoutButton.Name = "logoutButton";
			this.logoutButton.Size = new System.Drawing.Size(140, 23);
			this.logoutButton.TabIndex = 10;
			this.logoutButton.Text = "Logout";
			this.logoutButton.UseVisualStyleBackColor = true;
			this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkBox_EditBeforeUpload);
			this.groupBox3.Location = new System.Drawing.Point(15, 81);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(218, 48);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Capture";
			// 
			// checkBox_EditBeforeUpload
			// 
			this.checkBox_EditBeforeUpload.AutoSize = true;
			this.checkBox_EditBeforeUpload.Checked = true;
			this.checkBox_EditBeforeUpload.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_EditBeforeUpload.Location = new System.Drawing.Point(11, 21);
			this.checkBox_EditBeforeUpload.Name = "checkBox_EditBeforeUpload";
			this.checkBox_EditBeforeUpload.Size = new System.Drawing.Size(152, 18);
			this.checkBox_EditBeforeUpload.TabIndex = 0;
			this.checkBox_EditBeforeUpload.Text = "Edit before upload";
			this.checkBox_EditBeforeUpload.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(246, 213);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.groupBox1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sharer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelEmailText;
        private System.Windows.Forms.Label labelScreenstream;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelFullScreen;
        private System.Windows.Forms.Label labelScreenstreamText;
        private System.Windows.Forms.Label labelAreaText;
        private System.Windows.Forms.Label labelFullScreenText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabelEmail;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_EditBeforeUpload;
    }
}

