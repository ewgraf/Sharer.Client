using Sharer.Client.Smth;

namespace Sharer.Client
{
    partial class AuthForm
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
			this.buttonAuth = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.checkBox_RememberMe = new System.Windows.Forms.CheckBox();
			this.waterMarkTextBox_Email = new Client.Smth.WaterMarkTextBox();
			this.waterMarkTextBox_Password = new Client.Smth.WaterMarkTextBox();
			this.SuspendLayout();
			// 
			// buttonAuth
			// 
			this.buttonAuth.BackColor = System.Drawing.Color.White;
			this.buttonAuth.Location = new System.Drawing.Point(155, 100);
			this.buttonAuth.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.buttonAuth.Name = "buttonAuth";
			this.buttonAuth.Size = new System.Drawing.Size(108, 32);
			this.buttonAuth.TabIndex = 3;
			this.buttonAuth.Text = "Auth";
			this.buttonAuth.UseVisualStyleBackColor = false;
			this.buttonAuth.Click += new System.EventHandler(this.buttonAuth_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(45, 6);
			this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(132, 18);
			this.label1.TabIndex = 3;
			this.label1.Text = "Please, auth in";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(172, 6);
			this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(60, 18);
			this.linkLabel1.TabIndex = 4;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Sharer";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// checkBox_RememberMe
			// 
			this.checkBox_RememberMe.AutoSize = true;
			this.checkBox_RememberMe.Checked = true;
			this.checkBox_RememberMe.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_RememberMe.Location = new System.Drawing.Point(12, 105);
			this.checkBox_RememberMe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.checkBox_RememberMe.Name = "checkBox_RememberMe";
			this.checkBox_RememberMe.Size = new System.Drawing.Size(145, 22);
			this.checkBox_RememberMe.TabIndex = 5;
			this.checkBox_RememberMe.Text = "Remember me";
			this.checkBox_RememberMe.UseVisualStyleBackColor = true;
			// 
			// waterMarkTextBox_Email
			// 
			this.waterMarkTextBox_Email.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.waterMarkTextBox_Email.Location = new System.Drawing.Point(12, 30);
			this.waterMarkTextBox_Email.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.waterMarkTextBox_Email.Name = "waterMarkTextBox_Email";
			this.waterMarkTextBox_Email.Size = new System.Drawing.Size(249, 26);
			this.waterMarkTextBox_Email.TabIndex = 1;
			this.waterMarkTextBox_Email.WaterMarkColor = System.Drawing.Color.Gray;
			this.waterMarkTextBox_Email.WaterMarkText = "Water Mark";
			this.waterMarkTextBox_Email.KeyDown += new System.Windows.Forms.KeyEventHandler(this.waterMarkTextBox_Username_KeyDown);
			// 
			// waterMarkTextBox_Password
			// 
			this.waterMarkTextBox_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.waterMarkTextBox_Password.Location = new System.Drawing.Point(12, 66);
			this.waterMarkTextBox_Password.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.waterMarkTextBox_Password.Name = "waterMarkTextBox_Password";
			this.waterMarkTextBox_Password.PasswordChar = '*';
			this.waterMarkTextBox_Password.Size = new System.Drawing.Size(249, 26);
			this.waterMarkTextBox_Password.TabIndex = 2;
			this.waterMarkTextBox_Password.WaterMarkColor = System.Drawing.Color.Gray;
			this.waterMarkTextBox_Password.WaterMarkText = "Water Mark";
			this.waterMarkTextBox_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.waterMarkTextBox_Password_KeyDown);
			// 
			// AuthForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(275, 144);
			this.Controls.Add(this.buttonAuth);
			this.Controls.Add(this.checkBox_RememberMe);
			this.Controls.Add(this.waterMarkTextBox_Email);
			this.Controls.Add(this.waterMarkTextBox_Password);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.Name = "AuthForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Auth";
			this.Load += new System.EventHandler(this.AuthForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAuth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Smth.WaterMarkTextBox waterMarkTextBox_Password;
        private Smth.WaterMarkTextBox waterMarkTextBox_Email;
        private System.Windows.Forms.CheckBox checkBox_RememberMe;
    }
}
