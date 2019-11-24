using System;
using WixSharp;

namespace Sharer.Client.MSI {
	public class Script {
		public static void Main(string[] args) {
			Compiler.WixLocation = Environment.ExpandEnvironmentVariables(@"%HOMEPATH%\.nuget\packages\wixsharp.wix.bin\3.11.2\tools\bin");
			string prefix = @"..\Sharer.Client\bin\Debug";
			var project = new Project("Sharer",
				new Dir(@"%AppData%\Sharer",
					new File($@"{prefix}\sharer.exe", 
						new FileShortcut("Sharer", @"%AppData%\Microsoft\Windows\Start Menu\Programs"),
						new FileShortcut("Sharer", @"%Startup%")
					),
					new File($@"{prefix}\sharer.exe.config"),
					new File($@"{prefix}\Microsoft.WindowsAPICodePack.dll"),
					new File($@"{prefix}\Microsoft.WindowsAPICodePack.Shell.dll")
				),
				// http://www.reza-aghaei.com/how-to-add-item-to-windows-shell-context-menu-to-open-your-application/
				new RegValue(RegistryHive.ClassesRoot, @"*\shell\Share", "", "Share"),
				new RegValue(RegistryHive.ClassesRoot, @"*\shell\Share", "Icon", "[INSTALLDIR]sharer.exe"),
				new RegValue(RegistryHive.ClassesRoot, @"*\shell\Share\command", "", "\"[INSTALLDIR]sharer.exe\" \"%1\"")
			);

			project.BackgroundImage = "sharer-background.jpg";
			project.ControlPanelInfo.Comments = "Sharer is a program for uploading and sharing screenshots & files.";
			project.ControlPanelInfo.HelpLink = "https://sharer.su";
			project.ControlPanelInfo.Manufacturer = "http://github.com/ewgraf";
			project.ControlPanelInfo.ProductIcon = @"..\Sharer.Client\sharerIcon6.ico";
			project.GUID = new Guid("56628ce2-91ad-464c-a005-a19e09a5c9a2");
			project.LicenceFile = "Licence.rtf";
			project.Version = new Version(3, 0);

			Compiler.BuildMsi(project);
		}
	}
}
