using Progrart.Core.Storage;
using SkiaSharp;
using System.Diagnostics;

namespace Progrart.Core
{
    public class PrimitiveDrawingCore : IDisposable
	{
		internal bool isDisposed = false;
		SKSurface surface;
		SKImageInfo info;
		public readonly int Width;
		public readonly int Height;
		public SKCanvas canvas { get; }
		internal IStorageProvider StorageProvider;
		public PrimitiveDrawingCore(int W, int H, IStorageProvider storageProvider)
		{
			Width = W;
			Height = H;
			Trace.WriteLine($"Createing Surface as: {W} x {H}");
			info = new SKImageInfo(W, H);
			surface = SKSurface.Create(info);
			canvas = surface.Canvas;
			canvas.Clear();
			StorageProvider = storageProvider;
		}
		public SKData ToData()
		{
			return surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100);
		}
		public void Dispose()
		{
			if (isDisposed) return;
			//GC.SuppressFinalize(this);
			isDisposed = true;
			surface.Dispose();
		}
	}
}
