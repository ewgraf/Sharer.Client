using Sharer.Client.Entities;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Sharer.Client.Helpers {
	public static class ConfigHelper {
		private static Configuration GetConfiguration() {
			string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string folder = Path.Combine(appdata, "sharer");
			if (!Directory.Exists(folder)) {
				Directory.CreateDirectory(folder);
			}
			string configPath = Path.Combine(folder, "sharer.config");
			if (!File.Exists(configPath)) {
				File.WriteAllText(configPath,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <startup>
        <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
    </startup>
</configuration>");
			}
			var fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = configPath;
			var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			if (!config.AppSettings.Settings.AllKeys.Contains("email")) {
				config.AppSettings.Settings.Add("email", string.Empty);
				config.Save(ConfigurationSaveMode.Minimal);
			}
			if (!config.AppSettings.Settings.AllKeys.Contains("password")) {
				config.AppSettings.Settings.Add("password", string.Empty);
				config.Save(ConfigurationSaveMode.Minimal);
			}
			return config;
		}

		public static string GetEmail() {
			var config = GetConfiguration();
			return config.AppSettings.Settings["email"].Value;
		}

		public static void SetEmail(string email) {
			var config = GetConfiguration();
			if (config.AppSettings.Settings["email"].Value != email) {
				config.AppSettings.Settings.Remove("email");
				config.AppSettings.Settings.Add("email", email);
				config.Save(ConfigurationSaveMode.Minimal);
			}
		}

		public static string GetPassword() {
			var config = GetConfiguration();
			return config.AppSettings.Settings["password"].Value;
		}

		public static void SetPassword(string password) {
			var config = GetConfiguration();
			if (config.AppSettings.Settings["password"].Value != password) {
				config.AppSettings.Settings.Remove("password");
				config.AppSettings.Settings.Add("password", password);
				config.Save(ConfigurationSaveMode.Minimal);
			}
		}

		public static Account FindAccount() {
			string email = GetEmail();
			string password = GetPassword();
			if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) {
				return null;
			}
			Account account = new Account {
				Email = email,
				Password = password,
			};
			return account;
		}

		public static void SetAccount(Account account) {
			SetEmail(account.Email);
			SetPassword(account.Password);
		}

		public static void ClearAccount() {
			SetAccount(new Account {
				Email = string.Empty,
				Password = string.Empty,
			});
		}
	}
}
