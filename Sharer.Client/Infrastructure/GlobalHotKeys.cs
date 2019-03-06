using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Sharer.Client
{
	/// <summary> This class allows you to manage a hotkey </summary>
	public class GlobalHotKeys : IDisposable
	{
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);
		[DllImport("user32", SetLastError = true)]
		public static extern int UnregisterHotKey(IntPtr hwnd, int id);
		[DllImport("kernel32", SetLastError = true)]
		public static extern short GlobalAddAtom(string lpString);
		[DllImport("kernel32", SetLastError = true)]
		public static extern short GlobalDeleteAtom(short nAtom);

		public const int MOD_ALT = 0x1;
		public const int MOD_CONTROL = 0x2;
		public const int MOD_SHIFT = 0x4;
		public const int MOD_WIN = 0x8;

		public const int WM_HOTKEY = 0x312;

		public GlobalHotKeys()
		{
			//this.Handle = Process.GetCurrentProcess().Handle;
			this.Handle = IntPtr.Zero;
		}

		/// <summary>Handle of the current process</summary>
		public IntPtr Handle;

		/// <summary>The ID for the hotkey</summary>
		public short HotkeyID { get; private set; }

		/// <summary>Register the hotkey</summary>
		//public void RegisterGlobalHotKey(int hotkey, int modifiers, IntPtr handle)
		//{
		//	UnregisterGlobalHotKey();
		//	this.Handle = handle;
		//	RegisterGlobalHotKey(hotkey, modifiers);
		//}

		/// <summary>Register the hotkey</summary>
		public void RegisterGlobalHotKey(int hotkey, int modifiers)
		{
			UnregisterGlobalHotKey();

			try {
				// use the GlobalAddAtom API to get a unique ID (as suggested by MSDN)
				string atomName = Thread.CurrentThread.ManagedThreadId.ToString("X8") + this.GetType().FullName;
				//string atomName = Guid.NewGuid().ToString("N");
				HotkeyID = GlobalAddAtom(atomName);
				if (HotkeyID == 0)
					throw new Exception("Unable to generate unique hotkey ID. Error: " + Marshal.GetLastWin32Error().ToString());

				// register the hotkey, throw if any error
				//if (!RegisterHotKey(this.Handle, HotkeyID, (uint)modifiers, (uint)hotkey))
				if (!RegisterHotKey(Handle, HotkeyID, (uint)modifiers, (uint)hotkey))
					//if (!RegisterHotKey(IntPtr.Zero, HotkeyID, (uint)modifiers, (uint)hotkey))
					throw new Exception("Unable to register hotkey. Error: " + Marshal.GetLastWin32Error().ToString());

			} catch (Exception ex) {
				// clean up if hotkey registration failed
				Dispose();
				//Console.WriteLine(ex);
				MessageBox.Show($"{ex.Message}. 1400 means other program registered these hotkeys. It could be OneDrive.");
			}
		}

		/// <summary>Unregister the hotkey</summary>
		public void UnregisterGlobalHotKey()
		{
			if (this.HotkeyID != 0) {
				UnregisterHotKey(this.Handle, HotkeyID);
				// clean up the atom list
				GlobalDeleteAtom(HotkeyID);
				HotkeyID = 0;
			}
		}

		public void Dispose()
		{
			UnregisterGlobalHotKey();
		}
	}
}
