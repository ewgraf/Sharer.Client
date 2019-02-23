using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sharer.Client.Entities;

namespace Sharer.Client.Helpers {
	public static class NetHelper {
		private static readonly HttpClient _client = new HttpClient();

		public static async Task<string> UploadPath(string path, Account account, CancellationToken token) {
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

		public static HttpResponseMessage POST(string uri, Dictionary<string, string> values) {
			FormUrlEncodedContent content = new FormUrlEncodedContent(values);
			return _client.PostAsync(uri, content).Result;
		}

		public static HttpResponseMessage POST(string uri, MultipartFormDataContent content, Account account, CancellationToken token) {
			//client.DefaultRequestHeaders.Add("Authorization", $"Bearer {account.Token}");
			string credentials = $"{account.Email}:{account.Password}";
			string base64credentials = Base64Encode(credentials);
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64credentials}");
			return _client.PostAsync(uri, content, token).Result;
		}

		public static async Task<bool> TryAuthenticate(string email, string password) {
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

		public static string Base64Encode(string data) {
			var dataBytes = Encoding.UTF8.GetBytes(data);
			return Convert.ToBase64String(dataBytes);
		}

		//public static string GetToken(string username, string password, out string token) {
		//	HttpResponseMessage response = POST(Sharer.Uris.AuthToken, new Dictionary<string, string> {
		//		{ "grant_type", "password" },
		//		{ "username", username },
		//		{ "password", password },
		//	});

		//	try {
		//		////JSONResponce jresponce = GetJSONResponce(response);
		//		////token = jresponce.access_token;
		//		//token = JsonConvert.DeserializeObject<JSONResponce>(Task.Run(() => response.Content.ReadAsStringAsync()).Result).access_token;
		//		token = string.Empty;
		//		return string.Empty; // no exceptions, but success not guaranteed, need to check 
		//	} catch (WebException wex) {
		//		token = null;
		//		var httpResponse = wex.Response as HttpWebResponse;
		//		if (httpResponse != null) {
		//			return string.Format(
		//				"Remote server call {0} {1} resulted in a http error {2} {3}.{5}Inner: {4}",
		//				"POST",
		//				Sharer.Uris.AuthToken,
		//				httpResponse.StatusCode,
		//				httpResponse.StatusDescription, 
		//				wex.Message,
		//				Environment.NewLine);
		//		} else {
		//			return string.Format(
		//				"Remote server call {0} {1} resulted in an error.{5}Inner: {4}",
		//				"POST",
		//				Sharer.Uris.AuthToken,
		//				wex.Message,
		//				Environment.NewLine);
		//		}
		//	} catch (Exception) {
		//		throw;
		//	} finally {
		//		response.Dispose();
		//	}
		//}
		
		public static HttpWebResponse GetResponseNoException(this HttpWebRequest req)
		{
			try {
				return (HttpWebResponse)req.GetResponse();
			} catch (WebException we) {
				var resp = we.Response as HttpWebResponse;
				if (resp == null) {
					throw;
				}                    
				return resp;
			}
		}

		public static byte[] ASCIIEncode(string data) {
			return new ASCIIEncoding().GetBytes(data);
		}

		public static string GetStringResponse(HttpWebResponse response) {
			WebHeaderCollection header = response.Headers;
			
			using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII)) {
				return reader.ReadToEnd();
			}
		}

		//public static JSONResponce GetJSONResponce(HttpResponseMessage response)
		//{
		//	JSONResponce jresponce;
		//	JavaScriptSerializer js = new JavaScriptSerializer();
		//	try {
		//		using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result)) {
		//			jresponce = js.Deserialize<JSONResponce>(reader.ReadToEnd());
		//		}
		//	} finally {
		//		js = null;
		//	}
		//	return jresponce;
		//}
	}
}
