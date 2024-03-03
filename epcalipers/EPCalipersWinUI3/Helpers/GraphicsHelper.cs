using EPCalipersWinUI3.Models.Calipers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
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
					Debug.Print(rect.ToString());
					using (Graphics g = Graphics.FromImage(bitmap))
					{
						g.CopyFromScreen(new System.Drawing.Point(rect.Left, rect.Top), System.Drawing.Point.Empty, new System.Drawing.Size(rect.Width, rect.Height));
						//g.Dispose();
					}
					result = new MemoryStream();
					bitmap.Save(result, format);
					result.Position = 0;
				}
			}
			return result;
		}
		public static Bitmap CaptureScreenshotBitmap(WindowEx window, ImageFormat format)
		{
			MemoryStream result = null;

			var windowHandle = window.GetWindowHandle();

			RECT rect;
			if (GetWindowRect(new HandleRef(null, windowHandle), out rect))
			{
				rect.Top += 50;
				rect.Left += 100;
				using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height))
				{
					using (Graphics g = Graphics.FromImage(bitmap))
					{
						g.CopyFromScreen(new System.Drawing.Point(rect.Left, rect.Top), System.Drawing.Point.Empty, new System.Drawing.Size(rect.Width, rect.Height));
					}
					result = new MemoryStream();
					bitmap.Save(result, format);
					return bitmap;
					result.Position = 0;
				}
			}
			return null;
		}

		//MemoryStream ms = new MemoryStream();
		//imgstream.CopyTo(ms);
  //  var decoder = await BitmapDecoder.CreateAsync(ms.AsRandomAccessStream());
		//var softBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);



		public static async Task<SoftwareBitmap> CaptureScreenshot()
		{
			var ms = CaptureScreenshot(AppHelper.AppMainWindow, ImageFormat.Jpeg);
			//using (FileStream file = new FileStream("C:\\Users\\mannd\\OneDrive\\Documents\\test\\testEPC3.jpg", FileMode.Open, FileAccess.ReadWrite))
			//	file.CopyTo(ms);
			var decoder = await BitmapDecoder.CreateAsync(ms.AsRandomAccessStream());
			var softBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
			return softBitmap;
		}

		public static Bitmap CaptureScreenshotBitmap()
		{
			var bmp = CaptureScreenshotBitmap(AppHelper.AppMainWindow, ImageFormat.Jpeg);
			return bmp;
		}
    }
}
