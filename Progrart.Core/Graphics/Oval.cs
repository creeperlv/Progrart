using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;

namespace Progrart.Core.Graphics
{
    public class Oval : BaseElement
	{

		float StrokeWidth;
		SKPoint Position;
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
				IsStroke = (bool)__object.Get("IsStroke").AsBoolean();
				{
					if (__object.Get("Position") is JsObject Start)
					{
						this.Position = ProgrartConversion.ObtainSKPointFromJsObject(Start);
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
			SKPoint FinalStartPos = context.TranslatePoint(Position);
			SKPoint FinalEndPos = context.TranslatePoint(Size);
			context.canvas.DrawOval(
				FinalStartPos.X, FinalStartPos.Y,
				FinalEndPos.X, FinalEndPos.Y,
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
