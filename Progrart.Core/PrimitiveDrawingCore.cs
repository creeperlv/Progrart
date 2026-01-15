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
		Dictionary<string, SKTypeface> Fonts = new Dictionary<string, SKTypeface>();
		public SKTypeface? GetFont(string fontName)
		{
			if (Fonts.ContainsKey(fontName))
				return Fonts[fontName];
			else
			{
				var task = StorageProvider.TryOpenRead(fontName);
				task.Wait();
				var stream = task.Result;
				if (stream != null)
				{

					SKTypeface typeface = SKTypeface.FromStream(stream);
					Fonts[fontName] = typeface;
					return typeface;
				}
			}
			return null;
		}
		public PrimitiveDrawingCore(int W, int H, IStorageProvider storageProvider)
		{
			Width = W;
			Height = H;
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
		public SKData ToData(SKEncodedImageFormat format)
		{
			return surface.Snapshot().Encode(format, 100);
		}
		public SKData ToData(string extension)
		{
			SKEncodedImageFormat format = SKEncodedImageFormat.Png;
			if (extension.StartsWith('.'))
				extension = extension[1..];
			switch (extension)
			{
				case "jpg":
				case "jpeg":
					format = SKEncodedImageFormat.Jpeg;
					break;
				case "bmp":
					format = SKEncodedImageFormat.Bmp;
					break;
				case "webp":
					format = SKEncodedImageFormat.Webp;
					break;
				case "ico":
					format = SKEncodedImageFormat.Ico;
					break;
				case "heif":
					format = SKEncodedImageFormat.Heif;
					break;
				case "avif":
					format = SKEncodedImageFormat.Avif;
					break;
				case "jxl":
					format = SKEncodedImageFormat.Jpegxl;
					break;
				default:
					break;
			}
			return surface.Snapshot().Encode(format, 100);
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
