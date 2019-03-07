using System;
using WixSharp;

namespace Sharer.Client.MSI {
	public class Script {
		public static void Main(string[] args) {
			string prefix = @"..\Sharer.Client\bin\Release";
			var project = new Project("Sharer",
				new Dir(@"%ProgramFiles%\Sharer",
					new File($@"{prefix}\sharer.exe", 
						new FileShortcut("Sharer", @"%AppData%\Microsoft\Windows\Start Menu\Programs")
					),
					new File($@"{prefix}\sharer.exe.config"),
					new File($@"{prefix}\Microsoft.WindowsAPICodePack.dll"),
					new File($@"{prefix}\Microsoft.WindowsAPICodePack.Shell.dll")
				)
			);

			project.GUID = new Guid("56628ce2-91ad-464c-a005-a19e09a5c9a2");
			project.ControlPanelInfo.Manufacturer = "http://github.com/ewgraf";
			project.ControlPanelInfo.
			project.LicenceFile = "Licence.rtf";
			project.BackgroundImage = "sharer-banner.jpg";
			project.Version = new Version(3, 0);

			Compiler.BuildMsi(project);
		}
	}
}
