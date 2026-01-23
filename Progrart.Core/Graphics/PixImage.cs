using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;

namespace Progrart.Core.Graphics
{
	public class PixImage : BaseElement
	{

		float StrokeWidth;
		SKPoint Start;
		SKPoint Size;
		SKColorF Color;
		SKRect sourceRect;
		bool useClip = false;
		bool IsStroke;
		string? imgFile = null;
		SKShader? shader = null;
		public override void SetupProperties(Engine engine)
		{
			base.SetupProperties(engine);
			if (__object != null)
			{
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("Position", point);
				}
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					point.Set("w", 0);
					point.Set("h", 0);
					__object.Set("Clip", point);
				}
				__object.Set("StrokeWidth", 1);
				__object.Set("IsStroke", true);
				__object.Set("UseClip", false);
				__object.Set("Source", "");
				__object.Set("Color", ProgrartFunctions.color(engine, 1, 1, 1, 1));
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("Size", point);
				}

			}

		}
		public override void LoadProperties(RenderContext context)
		{
			if (__object is not null)
			{
				StrokeWidth = (float)__object.Get("StrokeWidth").AsNumber();
				IsStroke = (bool)__object.Get("IsStroke").AsBoolean();
				useClip = (bool)__object.Get("UseClip").AsBoolean();
				{
					if (__object.Get("Position") is JsObject Start)
					{
						this.Start = ProgrartConversion.ObtainSKPointFromJsObject(Start);
					}
				}
				{
					if (__object.Get("Size") is JsObject End)
					{
						this.Size = ProgrartConversion.ObtainSKPointFromJsObject(End);
					}
				}
				{
					if (__object.Get("Clip") is JsObject End)
					{
						this.sourceRect = ProgrartConversion.ObtainSKRectFromJsObject_XYWH_Style(End);
					}
				}
				{
					if (__object.Get("Color") is JsObject Color)
					{
						this.Color = ProgrartConversion.ObtainSKColorFFromJsObject(Color);
					}
				}

				if (__object.Get("Shader") is JsObject shaderObj)
					shader = ProgrartConversion.ObtainFromJsObject(context, shaderObj);

				if (__object.Get("Source") is JsString source)
					this.imgFile = source.AsString();
			}
		}
		public override void Render(RenderContext context)
		{
			base.Render(context);
			LoadProperties(context);
			SKPoint FinalStartPos = context.TranslatePoint(Start);
			SKPoint Size = context.TranslatePoint(this.Size);
			if (imgFile == null) return;
			var bitmap = context.GetBitmap(imgFile);
			if (bitmap == null)
			{
				return;
			}
			SKRect dest = new(FinalStartPos.X, FinalStartPos.Y, FinalStartPos.X + Size.X, FinalStartPos.Y + Size.Y);
			if (useClip)
			{
				context.canvas.DrawBitmap(bitmap, sourceRect, dest,
										new SKPaint()
										{
											ColorF = Color,
											StrokeWidth = context.TranslateSize(StrokeWidth),
											Shader = shader,
											IsStroke = IsStroke,
											IsAntialias = true
										}
				);
			}
			else
			{
				context.canvas.DrawBitmap(bitmap, new SKRect(0, 0, bitmap.Width, bitmap.Height),
					dest,
					new SKPaint()
					{
						ColorF = Color,
						StrokeWidth = context.TranslateSize(StrokeWidth),
						Shader = shader,
						IsStroke = IsStroke,
						IsAntialias = true
					}
				);
			}
		}
	}
}
