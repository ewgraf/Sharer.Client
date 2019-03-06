using System;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Linq;

namespace Sharer.Client {
	public static class Program {
		private const string MutexName = "{56628ce2-91ad-464c-a005-a19e09a5c9a2}";
		private static Mutex _mutex = new Mutex(true, MutexName);

		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			string filePath = args.SingleOrDefault();

			if (_mutex.WaitOne(TimeSpan.Zero, true)) {
				try {
					MainForm form;
					if (filePath != null) {
						form = new MainForm(filePath, _mutex);
					} else {
						form = new MainForm(_mutex);
					}
					Application.Run(form);
				} finally {
					_mutex.ReleaseMutex();
				}
			} else if (filePath != null) {
				try {
					using (var client = new TcpClient()) {
						client.Connect(Sharer.EndPoint);
						NetworkStream networkStream = client.GetStream();
						byte[] message = Encoding.UTF8.GetBytes(filePath);
						networkStream.Write(message, 0, message.Length);
						networkStream.Flush();
						networkStream.Close();
						networkStream.Dispose();
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString());
				}
			}
		}
	}
}

////MessageBox.Show($"open with trying send {filePath}");
////               // send our Win32 message to make the currently running instance
////               // jump on top of all the other windows
////               //GCHandle GCH = GCHandle.Alloc(filePath, GCHandleType.Pinned);
////               //IntPtr pStr = Marshal.StringToHGlobalUni(filePath);
////               IntPtr pStr = Marshal.StringToCoTaskMemAuto(filePath);
////               //IntPtr pFilePath = GCH.AddrOfPinnedObject();
////               //string Str = Marshal.PtrToStringUni(pFilePath);
////               //NativeMethods.PostMessage(
////               //    (IntPtr)NativeMethods.HWND_BROADCAST,
////               //    NativeMethods.WM_SHAREFILE,
////               //    IntPtr.Zero,
////               //    pFilePath);
////               // Find the window with the name of the main form
////               //MessageBox.Show(ptrWnd.ToString());
////               NativeMethods.SendMessage(
////                   ptrWnd,
////                   NativeMethods.WM_SHAREFILE,
////                   IntPtr.Zero,
////                   pStr);
//               string windowTitle = "Sharer";
//               IntPtr ptrWnd = NativeMethods.FindWindow(null, windowTitle);
//               // Create the data structure and fill with data
//               NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT();
//               copyData.dwData = new IntPtr(2);    // Just a number to identify the data type
//               copyData.cbData = filePath.Length + 1;  // One extra byte for the \0 character
//               copyData.lpData = Marshal.StringToHGlobalAnsi(filePath);
//               // Allocate memory for the data and copy
//               IntPtr ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
//               Marshal.StructureToPtr(copyData, ptrCopyData, false);
//               // Send the message
//               NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_SHAREFILE, IntPtr.Zero, ptrCopyData);
