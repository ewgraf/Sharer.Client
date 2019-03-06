using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Sharer.Client.Helpers {
	public static class NetHelper {
		private static readonly HttpClient _client = new HttpClient();

		public static HttpWebResponse GetResponseNoException(this HttpWebRequest req) {
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
			using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII)) {
				return reader.ReadToEnd();
			}
		}
	}
}
