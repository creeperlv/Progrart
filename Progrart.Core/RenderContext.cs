using Progrart.Core.Storage;
using SkiaSharp;

namespace Progrart.Core
{
	public class RenderContext
	{
		public PrimitiveDrawingCore DrawingCore { get; }
		public IStorageProvider StorageProvider { get => DrawingCore.StorageProvider; }
		public SKCanvas canvas { get => DrawingCore.canvas; }
		public float LogicalW;
		public float LogicalH;
		public RenderContext(PrimitiveDrawingCore core)
		{
			this.DrawingCore = core;
		}
		public SKPoint TranslatePoint(float x, float y)
		{
			return new SKPoint(x * DrawingCore.Width, y * DrawingCore.Height);
		}
		public SKPoint TranslatePoint(SKPoint point)
		{
			return TranslatePoint(point.X, point.Y);
		}
		public float TranslateSize(float s)
		{
			return (float)(s * Math.Sqrt(DrawingCore.Width * DrawingCore.Width + DrawingCore.Height * DrawingCore.Height) / Math.Sqrt(LogicalH * LogicalH + LogicalW * LogicalW));
		}
		public RenderContext(int W, int H, IStorageProvider StorageProvider)
		{
			DrawingCore = new PrimitiveDrawingCore(W, H, StorageProvider);
			LogicalW=W;
			LogicalH=H;
		}
	}
	[Serializable]
	public class ExecuteArguments
	{
		public Dictionary<string, string> data = new Dictionary<string, string>();
		public ExecuteArguments Clone()
		{
			ExecuteArguments args = new ExecuteArguments();
			foreach (var item in data)
			{
				args.data.Add(item.Key, item.Value);
			}
			return args;
		}
		public void MergeFrom(ExecuteArguments args)
		{

			foreach (var item in args.data)
			{
				args.data.Add(item.Key, item.Value);
			}
		}
	}
}
