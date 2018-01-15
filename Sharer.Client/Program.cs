using System;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;

namespace Sharer.Client {
	public static class Program {
		private const string MutexName = "{56628ce2-91ad-464c-a005-a19e09a5c9a2}";
		private static Mutex _mutex = new Mutex(true, MutexName);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//args = new[] { @"C:\Users\Ewgraf\Downloads\img\14807056194942.jpg" };
			//args = new[] { @"C:\Users\Ewgraf\Downloads\img\IMG_14022017_204823.png" };
			//System.Drawing.Bitmap image = new System.Drawing.Bitmap(args[0]);
			//Application.Run(new Forms.EditCaptureForm(image, new System.Drawing.Point(100, 100)));
			//return;

			string filePath = null;
			if (args.Length == 1) {
				filePath = args[0];
				//MessageBox.Show($"args[0]: {args[0]}");
			}

			if (_mutex.WaitOne(TimeSpan.Zero, true)) {
				try {
					Application.Run(new MainForm(filePath, _mutex)); // filePath = null || filePath
				} finally {
					_mutex.ReleaseMutex();
				}
			} else if (filePath != null) {
				try {
					using (TcpClient client = new TcpClient()) {
						client.Connect(Sharer.EndPoint);
						NetworkStream networkStream = client.GetStream();
						byte[] mesasge = Encoding.UTF8.GetBytes(filePath);
						networkStream.Write(mesasge, 0, mesasge.Length);
						networkStream.Flush();
						networkStream.Close();
						networkStream.Dispose();
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString());
				} finally {
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
