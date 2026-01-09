using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;
using System.Diagnostics;

namespace Progrart.Core.Graphics
{
    public class RoundRectangle : BaseElement
	{

		float StrokeWidth;
		float rx;
		float ry;
		SKPoint Start;
		SKPoint Size;
		SKColorF Color;
		bool IsStroke;
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
				__object.Set("StrokeWidth", 1);
				__object.Set("rx", 0);
				__object.Set("ry", 0);
				__object.Set("IsStroke", true);
				__object.Set("Color", ProgrartFunctions.color(engine, 1, 1, 1, 1));
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("Size", point);
				}

			}

		}
		public override void LoadProperties()
		{
			if (__object is not null)
			{
				StrokeWidth = (float)__object.Get("StrokeWidth").AsNumber();
				rx = (float)__object.Get("rx").AsNumber();
				ry = (float)__object.Get("ry").AsNumber();
				IsStroke = (bool)__object.Get("IsStroke").AsBoolean();
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
			SKPoint Position = context.TranslatePoint(Start);
            SKPoint Size = context.TranslatePoint(this.Size);
			float rx = context.TranslateSize(this.rx);
			float ry = context.TranslateSize(this.ry);
			Trace.WriteLine($"Draw Rectangle from {Position} to {Size} using {Color} with size of {StrokeWidth}.");
			context.DrawingCore.canvas.DrawRoundRect(
				Position.X, Position.Y,
				Size.X, Size.Y, rx, ry,
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
