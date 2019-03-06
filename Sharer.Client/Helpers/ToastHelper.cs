using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using MS.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Sharer.Client.ShellHelpers;
using IPersistFile = System.Runtime.InteropServices.ComTypes.IPersistFile;

namespace Sharer.Client {
	public class ToastHelper {
		// In order to display toasts, a desktop application must have a shortcut on the Start menu.
		// Also, an AppUserModelID must be set on that shortcut.
		// The shortcut should be created as part of the installer. The following code shows how to create
		// a shortcut and assign an AppUserModelID using Windows APIs. You must download and include the 
		// Windows API Code Pack for Microsoft .NET Framework for this code to function.
		//
		// Included in this project is a wxs file that be used with the WiX toolkit
		// to make an installer that creates the necessary shortcut. One or the other should be used.
		private bool TryCreateShortcut(string appId)
		{
			String shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Desktop Toasts Sample CS.lnk";
			if (!File.Exists(shortcutPath))
			{
				InstallShortcut(shortcutPath, appId);
				return true;
			}
			return false;
		}

		private void InstallShortcut(String shortcutPath, string appId)
		{
			// Find the path to the current executable
			String exePath = Process.GetCurrentProcess().MainModule.FileName;
			IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

			// Create a shortcut to the exe
			ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
			ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

			// Open the shortcut property store, set the AppUserModelId property
			IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

			using (PropVariant appIdProperty = new PropVariant(appId))
			{
				ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appIdProperty));
				ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
			}

			// Commit the shortcut to disk
			ShellHelpers.IPersistFile newShortcutSave = (ShellHelpers.IPersistFile)newShortcut;

			ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
		}

		public static void ShowToast(string link, string imagePath, TypedEventHandler<ToastNotification, object> onActivated) {
			var toastContent = new ToastContent {
				//Launch = link,
				Visual = new ToastVisual {
					BindingGeneric = new ToastBindingGeneric {
						//AppLogoOverride = new ToastGenericAppLogo {
						//	Source = $"file:///{imagePath}",
						//	HintCrop = ToastGenericAppLogoCrop.None
						//},
						Children = {
							new AdaptiveText {
								Text = $"Uploaded at {link}"
							}
						}
					}
				},

				//Actions = new ToastActionsCustom()
				//{
					
				//},

			};
			
			//var q = ToastNotificationManager.GetDefault().GetToastCollectionManager().;
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
			//// Fill in the text elements
			//XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
			//stringElements[0].AppendChild(toastXml.CreateTextNode("Uploaded at"));
			//stringElements[1].AppendChild(toastXml.CreateTextNode(link));

			//// Specify the absolute path to an image
			//XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
			//imageElements[0].Attributes.GetNamedItem("src").NodeValue = "file:///" + imagePath;

			// Create the toast and attach event listeners
			string content = toastContent.GetContent();
			var xmlContent = new XmlDocument();
			xmlContent.LoadXml(content);
			var toast = new ToastNotification(xmlContent);
			toast.Activated += onActivated;
			toast.Failed += (o, a) => {
				MessageBox.Show($"{o}{Environment.NewLine}{a}");
			};

			///////////////////////////////////////////////////////////////
			var toastContent2 = new ToastContent()
			{
				Scenario = ToastScenario.Default,
				Visual = new ToastVisual
				{
					BindingGeneric = new ToastBindingGeneric
					{
						Children =
						{
							new AdaptiveText
							{
								Text = "New toast notification (BackgroundTaskHelper)."
							}
						}
					}
				}
			};

			// Create & show toast notification

			xmlContent = new XmlDocument();
			xmlContent.LoadXml(toastContent2.GetContent());
			var toastNotification = new ToastNotification(xmlContent);
			ToastNotificationManager.CreateToastNotifier("Sharer").Show(toastNotification);
			///////////////////////////////////////////////////////////////

			// Show the toast. Be sure to specify the AppUserModelId
			// on your application's shortcut!
			try
			{
				ToastNotificationManager.CreateToastNotifier("Sharer").Show(toast);
			}
			catch (Exception ex)
			{
				;
			}

			;
		}

		public static void ShowToast(string title, string message) {
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
			XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
			stringElements[0].AppendChild(toastXml.CreateTextNode(title));
			stringElements[1].AppendChild(toastXml.CreateTextNode(message));
			ToastNotificationManager.CreateToastNotifier("Sharer").Show(new ToastNotification(toastXml));
		}
	}
}
