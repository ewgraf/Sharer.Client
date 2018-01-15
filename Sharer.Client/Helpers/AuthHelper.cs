using Sharer.Client.Entities;
using System.Windows.Forms;

namespace Sharer.Client.Helpers {
	public static class AuthHelper {
		public static Account TryAuth() {
			var account = ConfigHelper.FindAccount();
			using (var form = new AuthForm(account)) {
				if (form.ShowDialog() != DialogResult.OK) {
					return null;
				}
				account = form._account;
			}
			if (account.RememberMe) {
				ConfigHelper.SetAccount(account);
			}
			return account;
		}
	}
}
