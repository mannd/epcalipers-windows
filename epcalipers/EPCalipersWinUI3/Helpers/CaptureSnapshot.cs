using System;
using Windows.Graphics.DirectX.Direct3D11;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Graphics.Capture;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;

// This screen capture code is from https://github.com/robmikh/WinUI3CaptureSample, covered under the MIT license

namespace EPCalipersWinUI3.Helpers
{
	static class CaptureSnapshot
	{
		static readonly Guid GraphicsCaptureItemGuid = new Guid("79C3F95B-31F7-4EC2-A464-632EF5D30760");

		public static GraphicsCaptureItem CreateItemForWindow(Windows.Win32.Foundation.HWND hwnd)
		{
			GraphicsCaptureItem item = null;
			unsafe
			{
				item = CreateItemForCallback((Windows.Win32.System.WinRT.Graphics.Capture.IGraphicsCaptureItemInterop interop, Guid* guid) =>
				{
					interop.CreateForWindow(hwnd, guid, out object raw);
					return raw;
				});
			}
			return item;
		}

		private unsafe delegate object InteropCallback(Windows.Win32.System.WinRT.Graphics.Capture.IGraphicsCaptureItemInterop interop, Guid* guid);

		private static GraphicsCaptureItem CreateItemForCallback(InteropCallback callback)
		{
			var interop = GraphicsCaptureItem.As<Windows.Win32.System.WinRT.Graphics.Capture.IGraphicsCaptureItemInterop>();
			GraphicsCaptureItem item = null;
			unsafe
			{
				var guid = GraphicsCaptureItemGuid;
				var guidPointer = (Guid*)Unsafe.AsPointer(ref guid);
				var raw = Marshal.GetIUnknownForObject(callback(interop, guidPointer));
				item = GraphicsCaptureItem.FromAbi(raw);
				Marshal.Release(raw);
			}
			return item;
		}

		public static async Task<IDirect3DSurface> CaptureAsync(IDirect3DDevice device, GraphicsCaptureItem item)
		{
			var framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
				device,
				DirectXPixelFormat.B8G8R8A8UIntNormalized,
				1,
				item.Size);
			var session = framePool.CreateCaptureSession(item);

			var taskCompletion = new TaskCompletionSource<Direct3D11CaptureFrame>();
			framePool.FrameArrived += (s, a) =>
			{
				var frame = s.TryGetNextFrame();
				taskCompletion.SetResult(frame);
			};
			session.StartCapture();

			var frame = await taskCompletion.Task;
			framePool.Dispose();
			session.Dispose();

			var surface = frame.Surface;
			return surface;
		}
	}
}


