using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;
using System.Diagnostics;

namespace Progrart.Core.Graphics
{
	public class Line : BaseElement
	{
		float Size;
		SKPoint Start;
		SKPoint End;
		SKColorF Color;
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
					__object.Set("Start", point);
				}
				__object.Set("Size", 1);
				__object.Set("Color", ProgrartFunctions.color(engine, 1, 1, 1, 1));
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("End", point);
				}

			}

		}
		public override void LoadProperties()
		{
			if (__object is not null)
			{
				Size = (float)__object.Get("Size").AsNumber();
				{
					if (__object.Get("Start") is JsObject Start)
					{
						this.Start = ProgrartConversion.ObtainSKPointFromJsObject(Start);
					}
				}
				{
					if (__object.Get("End") is JsObject End)
					{
						this.End = ProgrartConversion.ObtainSKPointFromJsObject(End);
					}
				}
				{
					if (__object.Get("Color") is JsObject Color)
					{
						this.Color = ProgrartConversion.ObtainSKColorFFromJsObject(Color);
					}
				}

				if (__object.Get("Shader") is JsObject shaderObj)
					shader = ProgrartConversion.ObtainFromJsObject(shaderObj);
			}
		}
		public override void Render(RenderContext context)
		{
			base.Render(context);
			LoadProperties();
			SKPoint FinalStartPos = context.TranslatePoint(Start);
			SKPoint FinalEndPos = context.TranslatePoint(End);
			Trace.WriteLine($"Draw Line from {FinalStartPos} to {FinalEndPos} using {Color} with size of {Size}.");
			context.DrawingCore.canvas.DrawLine(
				FinalStartPos,
				FinalEndPos,
				new SKPaint()
				{
					ColorF = Color,
					StrokeWidth = context.TranslateSize(Size),
					Shader = shader
				}
			);
		}
	}
}
