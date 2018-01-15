using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sharer.Client
{
	public class OpenWithListener
	{
		public OpenWithListener() {}

		public async void Start(Func<string, bool> continueWith)
		{
			CancellationTokenSource cts = new CancellationTokenSource();
			TcpListener listener = new TcpListener(Sharer.EndPoint); // to do: dynamikly select port and store in repository
			try {
				listener.Start();
				//just fire and forget. We break from the "forgotten" async loops
				//in AcceptClientsAsync using a CancellationToken from `cts`
				await AcceptClientsAsync(listener, cts.Token, continueWith);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			} finally {
				listener.Stop();
			}
		}

		public async Task AcceptClientsAsync(TcpListener listener, CancellationToken ct, Func<string, bool> continueWith)
		{
			//once again, just fire and forget, and use the CancellationToken
			//to signal to the "forgotten" async invocation.
			while (!ct.IsCancellationRequested) {
				using (TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false)) {
					NetworkStream networkStream = client.GetStream();
					byte[] message = new byte[client.ReceiveBufferSize];
					networkStream.Read(message, 0, client.ReceiveBufferSize);
					networkStream.Close();
					networkStream.Dispose();
					string filePath = Encoding.UTF8.GetString(message).Replace("\0", ""); // due to buffer is 64k length and there are '0's after the string at 'message'
					continueWith(filePath);
				}
			}
		}
	}
}
