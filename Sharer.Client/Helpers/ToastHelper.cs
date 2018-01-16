using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace Sharer.Client.Helpers {
	public class ToastHelper {
		public static void ShowToast(string link, string imagePath, TypedEventHandler<ToastNotification, object> onActivated) {
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
			// Fill in the text elements
			XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
			stringElements[0].AppendChild(toastXml.CreateTextNode("Uploaded at"));
			stringElements[1].AppendChild(toastXml.CreateTextNode(link));

			// Specify the absolute path to an image
			XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
			imageElements[0].Attributes.GetNamedItem("src").NodeValue = "file:///" + imagePath;

			// Create the toast and attach event listeners
			var toast = new ToastNotification(toastXml);
			toast.Activated += onActivated;

			// Show the toast. Be sure to specify the AppUserModelId
			// on your application's shortcut!
			ToastNotificationManager.CreateToastNotifier("Sharer").Show(toast);
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
