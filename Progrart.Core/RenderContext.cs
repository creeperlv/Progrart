using SkiaSharp;

namespace Progrart.Core
{
	public class RenderContext
	{
		public PrimitiveDrawingCore DrawingCore { get; }

		public SKCanvas canvas { get => DrawingCore.canvas; }
		public RenderContext(PrimitiveDrawingCore core)
		{
			this.DrawingCore = core;
		}
		public SKPoint TranslatePoint(float x, float y)
		{
			return new SKPoint(x * DrawingCore.Width, y * DrawingCore.Height);
		}
		public RenderContext(int W, int H)
		{
			DrawingCore = new PrimitiveDrawingCore(W, H);
		}
	}
	public class PrimitiveDrawingCore : IDisposable
	{
		internal bool isDisposed = false;
		SKSurface surface;
		SKImageInfo info;
		public readonly int Width;
		public readonly int Height;
		public SKCanvas canvas { get; }
		public PrimitiveDrawingCore(int W, int H)
		{
			Width = W;
			Height = H;
			info = new SKImageInfo(W, H);
			surface = SKSurface.Create(info);
			canvas = surface.Canvas;

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

	public class ExecuteArguments
	{
		public Dictionary<string, string> data = new Dictionary<string, string>();
	}
	[Serializable]
	public class RenderConfig
	{
		public int Scale;
	}
}
