using System;
using System.IO;

namespace Sharer.Client.Helpers {
	public static class FileHelper {
		public static string[] Sizes = new[] { "Bytes", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

		public static string ToInformationPrefixString(this long value) {
			if (value == 0) {
				return "0 Byte";
			}
			var k = 1024;			
			var i = (int)Math.Floor(Math.Log(value) / Math.Log(k));
			return $"{(value / Math.Pow(k, i)):0.#} {Sizes[i]}";
		}

		public static bool IsLocked(string filePath) {
			FileStream stream = null;

			try {
				stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			} catch (IOException) {
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			} finally {
				if (stream != null) {
					stream.Close();
				}
			}

			//file is not locked
			return false;
		}
	}
}
