using System;
using Microsoft.Win32;
using Sharer.Client.Entities;

namespace Sharer.Client.Helpers {
	public static class RegistryHelper {
		private const string AccountKeyPath = @"Software\Sharer\Account";

		public static Account FindAccount() {			
			RegistryKey key = Registry.CurrentUser.OpenSubKey(AccountKeyPath);
			if (key == null) {
				//throw new InvalidOperationException($@"Failed opening 'HKEY_CURRENT_USER\{AccountKeyPath}' registry key. Try to auth in Sharer on your computer or reinstall one.");
				return null;
			}

			Account account = new Account {
				Email    = (string)key.GetValue("Email"),
				Password = (string)key.GetValue("Password"),
			};

			key.Close();

			if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password)) {
				throw new InvalidOperationException($"Account's Email or Password are null or empty. {account}");
			}
			return account;
		}

		public static void SetAccount(Account account) {
			RegistryKey key = Registry.CurrentUser.OpenSubKey(AccountKeyPath, true);
			key.SetValue("Email", account.Email);
			key.SetValue("Password", account.Password);
			key.Close();
		}

		public static void ClearAccount() {
			SetAccount(new Account {
				Email    = string.Empty,
				Password = string.Empty,
			});
		}
	}
}
