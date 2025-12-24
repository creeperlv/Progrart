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
		public SKCanvas canvas { get; }
		public PrimitiveDrawingCore(int W, int H)
		{
			info = new SKImageInfo(W, H);
			surface = SKSurface.Create(info);
			canvas = surface.Canvas;
		}
		public void Dispose()
		{
			if (isDisposed) return;
			//GC.SuppressFinalize(this);
			isDisposed = true;
			surface.Dispose();
		}
	}

	[Serializable]
	public class RenderConfig
	{
		public int Scale;
	}
}
