using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Sharer.Client.Helpers {
	/// <summary>
	///     Provides functions to capture the entire screen, or a particular window, and save it to a file.
	/// </summary>
	public static class ScreenCaptureHelper {
		/// <summary>
		///     Creates an Image object containing a screen shot of the entire desktop
		/// </summary>
		/// <returns></returns>
		public static Image CaptureScreen() {
			return CaptureWindow(User32.GetDesktopWindow());
		}

		/// <summary>
		///     Creates an Image object containing a screen shot of the entire virtual desktop
		/// </summary>
		/// <returns></returns>
		public static Image CaptureScreens() {
			//Screen[] screens = Screen.AllScreens;
			//int noOfScreens = screens.Length, maxWidth = 0, maxHeight = 0;
			//int left = screens.Min(s => s.Bounds.X);
			//int top = screens.Min(s => s.Bounds.Y);
			//for (int i = 0; i < noOfScreens; i++) {
			//	if (maxWidth < screens[i].Bounds.X + screens[i].Bounds.Width) {
			//		maxWidth = screens[i].Bounds.X + screens[i].Bounds.Width;
			//	}
			//	if (maxHeight < screens[i].Bounds.Y + screens[i].Bounds.Height) {
			//		maxHeight = screens[i].Bounds.Y + screens[i].Bounds.Height;
			//	}
			//}
			//return CaptureScreen(left, top, maxWidth, maxHeight);
			return CaptureScreen(SystemInformation.VirtualScreen.X,
								 SystemInformation.VirtualScreen.Y,
								 SystemInformation.VirtualScreen.Width,
								 SystemInformation.VirtualScreen.Height);
		}

		public static Image CaptureScreenRectangle(Rectangle r) {
			return CaptureWindow(User32.GetDesktopWindow(), r);
		}

		/// <summary>
		/// Creates an Image object containing a screen shot of a specific window
		/// </summary>
		/// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
		/// <returns></returns>
		public static Image CaptureWindow(IntPtr handle)
		{
			// get te hDC of the target window
			IntPtr hdcSrc = User32.GetWindowDC(handle);
			// get the size
			User32.RECT windowRect = new User32.RECT();
			User32.GetWindowRect(handle, ref windowRect);
			int width = windowRect.right - windowRect.left;
			int height = windowRect.bottom - windowRect.top;
			// create a device context we can copy to
			IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			// create a bitmap we can copy it to,
			// using GetDeviceCaps to get the width/height
			IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
			// select the bitmap object
			IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
			// bitblt over
			GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
			// restore selection
			GDI32.SelectObject(hdcDest, hOld);
			// clean up 
			GDI32.DeleteDC(hdcDest);
			User32.ReleaseDC(handle, hdcSrc);
			// get a .NET image object for it
			Image img = Image.FromHbitmap(hBitmap);
			// free up the Bitmap object
			GDI32.DeleteObject(hBitmap);
			return img;
		}
		public static Image CaptureWindow(IntPtr handle, Rectangle r)
		{
			// get te hDC of the target window
			IntPtr hdcSrc = User32.GetWindowDC(handle);
			// get the size
			User32.RECT windowRect = new User32.RECT() { 
				left = r.Left,
				top = r.Top,
				right = r.Right,
				bottom = r.Bottom
			};
			//User32.GetWindowRect(handle, ref windowRect);
			int width = r.Width;
			int height = r.Height;
			// create a device context we can copy to
			IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			// create a bitmap we can copy it to,
			// using GetDeviceCaps to get the width/height
			IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
			// select the bitmap object
			IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
			// bitblt over
			GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, r.Left, r.Top, GDI32.SRCCOPY);
			// restore selection
			GDI32.SelectObject(hdcDest, hOld);
			// clean up 
			GDI32.DeleteDC(hdcDest);
			User32.ReleaseDC(handle, hdcSrc);
			// get a .NET image object for it
			Image img = Image.FromHbitmap(hBitmap);
			// free up the Bitmap object
			GDI32.DeleteObject(hBitmap);
			return img;
		}
		/// <summary>
		/// Captures a screen shot of a specific window, and saves it to a file
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
		{
			Image img = CaptureWindow(handle);
			img.Save(filename, format);
		}
		/// <summary>
		/// Captures a screen shot of the entire desktop, and saves it to a file
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public static void CaptureScreenToFile(string filename, ImageFormat format)
		{
			Image img = CaptureScreen();
			img.Save(filename, format);
		}

		// https://www.codeproject.com/Articles/546006/Screen-Capture-on-Multiple-Monitors
		//function to capture screen section
		public static Image CaptureScreen(int x, int y, int width, int height) {
			//create DC for the entire virtual screen
			IntPtr hdcSrc = GDI32.CreateDC("DISPLAY", null, null, IntPtr.Zero);
			IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
			GDI32.SelectObject(hdcDest, hBitmap);

			// set the destination area White - a little complicated
			Bitmap bmp = new Bitmap(width, height);
			Image ii = (Image)bmp;
			Graphics gf = Graphics.FromImage(ii);
			IntPtr hdc = gf.GetHdc();
			//use whiteness flag to make destination screen white
			GDI32.BitBlt(hdcDest, 0, 0, width, height, hdc, 0, 0, 0x00FF0062);
			gf.Dispose();
			ii.Dispose();
			bmp.Dispose();

			//Now copy the areas from each screen on the destination hbitmap
			Screen[] screendata = Screen.AllScreens;
			int X, X1, Y, Y1;
			for (int i = 0; i < screendata.Length; i++) {
				if (screendata[i].Bounds.X > (x + width) || (screendata[i].Bounds.X +
				   screendata[i].Bounds.Width) < x || screendata[i].Bounds.Y > (y + height) ||
				   (screendata[i].Bounds.Y + screendata[i].Bounds.Height) < y) { // no common area
				} else {
					// something  common
					if (x < screendata[i].Bounds.X) X = screendata[i].Bounds.X; else X = x;
					if ((x + width) > (screendata[i].Bounds.X + screendata[i].Bounds.Width))
						X1 = screendata[i].Bounds.X + screendata[i].Bounds.Width;
					else X1 = x + width;
					if (y < screendata[i].Bounds.Y) Y = screendata[i].Bounds.Y; else Y = y;
					if ((y + height) > (screendata[i].Bounds.Y + screendata[i].Bounds.Height))
						Y1 = screendata[i].Bounds.Y + screendata[i].Bounds.Height;
					else Y1 = y + height;
					// Main API that does memory data transfer
					// SRCCOPY AND CAPTUREBLT
					GDI32.BitBlt(hdcDest, X - x, Y - y, X1 - X, Y1 - Y, hdcSrc, X, Y, 0x40000000 | 0x00CC0020);
				}
			}

			// send image to clipboard
			Image imf = Image.FromHbitmap(new IntPtr((int)hBitmap));
			//Clipboard.SetImage(imf);
			GDI32.DeleteDC(hdcSrc);
			GDI32.DeleteDC(hdcDest);
			GDI32.DeleteObject(hBitmap);
			return imf;
		}

		/// <summary>
		///     Helper class containing Gdi32 API functions
		/// </summary>
		private class GDI32 {
			public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
			[DllImport("gdi32.dll")]
			public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
				int nWidth, int nHeight, IntPtr hObjectSource,
				int nXSrc, int nYSrc, int dwRop);
			[DllImport("gdi32.dll")]
			public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
			[DllImport("gdi32.dll")]
			public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
			[DllImport("gdi32.dll")]
			public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);
			[DllImport("gdi32.dll")]
			public static extern bool DeleteDC(IntPtr hDC);
			[DllImport("gdi32.dll")]
			public static extern bool DeleteObject(IntPtr hObject);
			[DllImport("GDI32.dll")]
			public static extern int GetDeviceCaps(int hdc, int nIndex);
			[DllImport("gdi32.dll")]
			public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
		}

		/// <summary>
		/// Helper class containing User32 API functions
		/// </summary>
		private class User32 {
			[StructLayout(LayoutKind.Sequential)]
			public struct RECT {
				public int left;
				public int top;
				public int right;
				public int bottom;
			}
			[DllImport("user32.dll")]
			public static extern IntPtr GetDesktopWindow();
			[DllImport("user32.dll")]
			public static extern IntPtr GetWindowDC(IntPtr hWnd);
			[DllImport("user32.dll")]
			public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
			[DllImport("user32.dll")]
			public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
		}
	}
}
