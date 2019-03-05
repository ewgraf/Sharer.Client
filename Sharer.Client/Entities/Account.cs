using System.Linq;

namespace Sharer.Client.Entities {
	public class Account {
		public bool IsAuthorized { get; set; }
		public bool RememberMe { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string ServerAdress { get; set; }

		public override string ToString() {
			string hiddenPassword = Password != null ? string.Join("", Enumerable.Repeat("*", Password.Length)) : "null";
			return $"Email: {Email} Password: {hiddenPassword}";
		}
	}
}
