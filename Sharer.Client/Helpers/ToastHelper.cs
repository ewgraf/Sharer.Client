using System;
using System.Diagnostics;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using Sharer.Client.ShellHelpers;

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
		public static bool TryCreateShortcut(string appId) {
			string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Sharer.lnk";
			if (!File.Exists(shortcutPath)) {
				InstallShortcut(shortcutPath, appId);
				return true;
			}
			return false;
		}

		private static void InstallShortcut(string shortcutPath, string appId) {
			// Find the path to the current executable
			string exePath = Process.GetCurrentProcess().MainModule.FileName;
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

		// Create and show the toast.
		// See the "Toasts" sample for more detail on what can be done with toasts
		public static void ShowToast(string link, string imagePath, TypedEventHandler<ToastNotification, object> onActivated) {
			// Get a toast XML template
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
			// Fill in the text elements
			XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
			stringElements[0].AppendChild(toastXml.CreateTextNode("Uploaded at"));
			stringElements[1].AppendChild(toastXml.CreateTextNode(link));
			// Specify the absolute path to an image
			XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
			string fileImagePath = $"file:///{imagePath}";
			imageElements[0].Attributes.GetNamedItem("src").NodeValue = fileImagePath;
			// Create the toast and attach event listeners
			ToastNotification toast = new ToastNotification(toastXml);
			toast.Activated += onActivated;
			//toast.Dismissed
			//toast.Failed
			// Create & show toast notification.
			// Be sure to specify the AppUserModelId on your application's shortcut!
			ToastNotificationManager.CreateToastNotifier(Sharer.Me).Show(toast);
		}

		//private void ToastActivated(ToastNotification sender, object e) {
		//	Dispatcher.Invoke(() => {
		//		Activate();
		//		Output.Text = "The user activated the toast.";
		//	});
		//}

		//private void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs e) {
		//	String outputText = "";
		//	switch (e.Reason) {
		//		case ToastDismissalReason.ApplicationHidden:
		//			outputText = "The app hid the toast using ToastNotifier.Hide";
		//			break;
		//		case ToastDismissalReason.UserCanceled:
		//			outputText = "The user dismissed the toast";
		//			break;
		//		case ToastDismissalReason.TimedOut:
		//			outputText = "The toast has timed out";
		//			break;
		//	}

		//	Dispatcher.Invoke(() => {
		//		Output.Text = outputText;
		//	});
		//}

		//private void ToastFailed(ToastNotification sender, ToastFailedEventArgs e) {
		//	Dispatcher.Invoke(() => {
		//		Output.Text = "The toast encountered an error.";
		//	});
		//}

		public static void ShowToast(string title, string message) {
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
			XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
			stringElements[0].AppendChild(toastXml.CreateTextNode(title));
			stringElements[1].AppendChild(toastXml.CreateTextNode(message));
			ToastNotificationManager.CreateToastNotifier("Sharer").Show(new ToastNotification(toastXml));
		}
	}
}
