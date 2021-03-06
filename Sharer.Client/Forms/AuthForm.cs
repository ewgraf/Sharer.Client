﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Sharer.Client.Entities;

namespace Sharer.Client {
	public partial class AuthForm : Form {
		private readonly Sharer _sharer;

		public Account Account { get; private set; }

		public AuthForm(Sharer sharer, Account account) {
			_sharer = sharer;
			Account = account;

			InitializeComponent();
		}

		private void AuthForm_Load(object sender, EventArgs e) {
			this.BackColor = Color.White;
			this.waterMarkTextBox_Email.TextAlign = HorizontalAlignment.Center;
			this.waterMarkTextBox_Password.TextAlign = HorizontalAlignment.Center;
			this.waterMarkTextBox_Email.WaterMarkText    = "Email";
			this.waterMarkTextBox_Password.WaterMarkText = "Password";

			ToolTip hint_RememberMe = new ToolTip();
			hint_RememberMe.ShowAlways = true;
			hint_RememberMe.InitialDelay = 1;
			hint_RememberMe.AutoPopDelay = 10000;
			hint_RememberMe.SetToolTip(this.checkBox_RememberMe, $@"Stores Email and Password in Registry at HKEY_CURRENT_USER\Software\Sharer\Account.");

			if (Account != null) {
				this.waterMarkTextBox_Email.Text = Account.Email;
				this.waterMarkTextBox_Password.Text = Account.Password;
				Auth();
			} else {
				Account = new Account();
			}
		}

		// "Please, auth in Sharer" label
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start($"{Sharer.Uris.SharerServer}/index.html");
		}

		// "Username" textbox
		private void waterMarkTextBox_Password_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				buttonAuth_Click(sender, e);
			}
		}
		// "Password" textbox
		private void waterMarkTextBox_Username_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				buttonAuth_Click(sender, e);
			}
		}

		private void buttonAuth_Click(object sender, EventArgs e) {
			Auth();
		}

		public async void Auth() {
			this.waterMarkTextBox_Email.Enabled = false;
			this.waterMarkTextBox_Password.Enabled = false;
			this.buttonAuth.Enabled = false;
			this.checkBox_RememberMe.Enabled = false;
			bool userExists = false;
			try {
				userExists = await _sharer.TryAuthenticate(waterMarkTextBox_Email.Text, waterMarkTextBox_Password.Text);
			} catch { }
			if (userExists) {
				this.BackColor = Color.Green;
				Account.Email = waterMarkTextBox_Email.Text;
				Account.Password = waterMarkTextBox_Password.Text;
				Account.RememberMe = checkBox_RememberMe.Checked;
				Account.IsAuthorized = true;
				this.DialogResult = DialogResult.OK;
			} else {
				this.BackColor = Color.Red;
				MessageBox.Show($"Please check email&password and Internet connection, and try again", "Authentication failure");
				this.BackColor = Color.White;
				this.waterMarkTextBox_Email.Enabled = true;
				this.waterMarkTextBox_Password.Enabled = true;
				this.buttonAuth.Enabled = true;
				this.checkBox_RememberMe.Enabled = true;
			}
		}
	}
}
