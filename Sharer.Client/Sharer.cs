using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sharer.Client.Entities;

namespace Sharer.Client {
	public class Sharer {
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


		private readonly HttpClient _client = new HttpClient();

		public Sharer() {

		}

		public async Task<string> UploadPath(string path, Account account, CancellationToken token) {
			if (string.IsNullOrEmpty(path)) {
				throw new ArgumentOutOfRangeException(nameof(path));
			}
			if (account == null) {
				throw new ArgumentNullException(nameof(account));
			}

			string result = null;
			HttpResponseMessage response = null;
			int retryTimes = 3;
			for (int i = 0; i < retryTimes; i++) {
				if (token.IsCancellationRequested) {
					return null;
				}
				using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					var content = new MultipartFormDataContent("sharerClientBoundaryString");
					content.Add(new StreamContent(stream), "file", Path.GetFileName(path));

					response = POST($"{Sharer.Uris.SharerServer}/account/apiupload", content, account, token);
					if (token.IsCancellationRequested) {
						return null;
					}

					if (response.StatusCode == HttpStatusCode.Unauthorized) {
						// TODO: Logout
					}
					if (response.IsSuccessStatusCode) {
						result = await response.Content.ReadAsStringAsync(); // ["xraXn"]
						result = result.Substring(2, result.Length - 4);
						result = result.Replace("\"", string.Empty);
						if (result == "null") {
							continue;
						}
						if (result[0] == '-') {
							if (result.Contains("free space")) {
								return result;
							}
							continue;
						}
						return result;
					} else {
						// retry
						Thread.Sleep(500);
					}
				}
			}
			return result ?? $"Failed to upload file, reason: {response.ReasonPhrase}, status code: {response.StatusCode}";
		}

		
		public HttpResponseMessage POST(string uri, Dictionary<string, string> values) {
			FormUrlEncodedContent content = new FormUrlEncodedContent(values);
			return _client.PostAsync(uri, content).Result;
		}

		public HttpResponseMessage POST(string uri, MultipartFormDataContent content, Account account, CancellationToken token) {
			string credentials = $"{account.Email}:{account.Password}";
			string base64credentials = Base64Encode(credentials);
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64credentials}");
			return _client.PostAsync(uri, content, token).Result;
		}

		

		public string Base64Encode(string data) {
			var dataBytes = Encoding.UTF8.GetBytes(data);
			return Convert.ToBase64String(dataBytes);
		}

		public async Task<bool> TryAuthenticate(string email, string password) {
			string credentials = $"{email}:{password}";
			string base64credentials = Base64Encode(credentials);
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64credentials}");
			HttpResponseMessage responce;
			try {
				responce = await _client.PostAsync(Sharer.Uris.Auth, null);
			} catch (Exception ex) {
				return false;
			}
			if (responce.StatusCode == HttpStatusCode.OK) {
				return true;
			} else if (responce.StatusCode == HttpStatusCode.NotFound) {
				return false;
			} else {
				throw new InvalidOperationException($"Unexpected authentication responce status code: {responce.StatusCode}");
			}
		}

		public static class Uris {
			public const string SharerServer = "http://sharer.su";
			//public const string SharerServer = "http://localhost:5000";
			public static string AuthToken = $"{SharerServer}/token";
			public static string AccountPage = $"{SharerServer}/account";
			public static string Auth = $"{SharerServer}/api/user/auth";
		}
	}
}
