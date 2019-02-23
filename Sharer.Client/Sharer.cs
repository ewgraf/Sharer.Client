using System;
using System.IO;
using System.Net;

namespace Sharer.Client {
	public static class Sharer {
		public static class Uris {
			//public const string SharerServer = "https://sharer.su";
			public const string SharerServer = "http://localhost:5000";
			public static string AuthToken = $"{SharerServer}/token";
			public static string AccountPage = $"{SharerServer}/account";
			public static string Auth = $"{SharerServer}/api/user/auth";
		}

		public const int MaxMb = 128;
		private static int MaxFileSize = MaxMb * 1024 * 1024; // MaxMb Mb in bytes
		/* 
		also set iis's	 
		<system.webServer>
		  <security>
			<requestFiltering>
			  <requestLimits maxAllowedContentLength="134217728" /> <!-- 128 MB -->
			</requestFiltering>
		  </security>
		</system.webServer>
		*/

		public const string Me = "Sharer";
		public const string Manufacturer = "Sharer";
		public const string Version = "1";
		public const string SharerFolder = "sharer";
		public const string ExeFilename = "sharer.exe";
		public const string LnkFilename = "sharer.lnk";

		//public static bool FileSizeCorrect(string filePath) {
		//    try {
		//        return new FileInfo(filePath).Length < MaxFileSize; // bytes to Mb
		//    } catch (Exception ex) {
		//        //throw new AggregateException($"FileSizeCorrect({filePath})", ex);
		//    }
		//}
		public static bool FileSizeCorrect(string filePath) => new FileInfo(filePath).Length < MaxFileSize; // bytes to Mb
		public static string MyFolder => AppDomain.CurrentDomain.BaseDirectory;
		public static string LastUploadFile => $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\sharer\last.jpg";
		public static int Port = 42000;
		public static IPEndPoint EndPoint = new IPEndPoint(IPAddress.Loopback, Port);
	}
}
