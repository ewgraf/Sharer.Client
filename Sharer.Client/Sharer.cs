using System;
using System.IO;
using System.Net;

namespace Sharer.Client {
	public static class Sharer {
		public const string Me = "Sharer";
		public const string Version = "2";
		public const string SharerFolder = "sharer";
		public const string ExeFilename = "sharer.exe";
		public const string LnkFilename = "sharer.lnk";
		public const int Port = 42000;
		public const int MaxMb = 128;
		public const int MaxFileSize = MaxMb * 1024 * 1024; // MaxMb Mb in bytes

		public static bool FileSizeCorrect(string filePath) => new FileInfo(filePath).Length < MaxFileSize; // bytes to Mb
		public static string MyDirectory => AppDomain.CurrentDomain.BaseDirectory;
		public static string LastUploadFilePath => $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\sharer\last.jpg";
		public static IPEndPoint EndPoint = new IPEndPoint(IPAddress.Loopback, Port);
		
		public static class Uris {
			public const string SharerServer = "http://sharer.su";
			//public const string SharerServer = "http://localhost:5000";
			public static string AuthToken = $"{SharerServer}/token";
			public static string AccountPage = $"{SharerServer}/account";
			public static string Auth = $"{SharerServer}/api/user/auth";
		}
	}
}
