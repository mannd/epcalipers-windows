using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace EPCalipersWinUI3.Helpers
{
    public static class GraphicsHelper
    {

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
			public int Width { get { return Right - Left; } }
			public int Height { get { return Bottom - Top; } }
		}

		public static Stream CaptureScreenshot(WindowEx window, ImageFormat format)
		{
			MemoryStream result = null;

			var windowHandle = window.GetWindowHandle();

			RECT rect;
			if (GetWindowRect(new HandleRef(null, windowHandle), out rect))
			{
				using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height))
				{
					using (Graphics g = Graphics.FromImage(bitmap))
					{
						g.CopyFromScreen(new System.Drawing.Point(rect.Left, rect.Top), System.Drawing.Point.Empty, new System.Drawing.Size(rect.Width, rect.Height));
					}
					result = new MemoryStream();
					bitmap.Save(result, format);
					result.Position = 0;
				}
			}
			return result;
		}


		public static void CaptureScreenshot()
		{
			var ms = CaptureScreenshot(AppHelper.AppMainWindow, ImageFormat.Jpeg);
			using (FileStream file = new FileStream("C:\\Users\\mannd\\OneDrive\\Documents\\test\\testEPC3.jpg", FileMode.Open, FileAccess.Read))
				file.CopyTo(ms);
		
		}
    }
}
