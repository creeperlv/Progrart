using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;

namespace Progrart.Core.Graphics
{
	public class Text : BaseElement
	{

		float StrokeWidth;
		SKPoint Position;
		float Size;
		SKColorF Color;
		string str = "";
		bool IsStroke;
		SKShader? shader = null;
		string? fontFamily = null;
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
				__object.Set("Size", 1);
				__object.Set("Text", "");
				__object.Set("IsStroke", true);
				__object.Set("Color", ProgrartFunctions.color(engine, 1, 1, 1, 1));

			}

		}
		public override void LoadProperties()
		{
			if (__object is not null)
			{
				StrokeWidth = (float)__object.Get("StrokeWidth").AsNumber();
				str = __object.Get("Text").AsString();
				Size = (float)__object.Get("Size").AsNumber();
				IsStroke = (bool)__object.Get("IsStroke").AsBoolean();
				{
					if (__object.Get("Position") is JsObject Start)
					{
						this.Position = ProgrartConversion.ObtainSKPointFromJsObject(Start);
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
				if (__object.Get("Font") is JsString fontObj)
					fontFamily = fontObj.AsString();
			}
		}
		public override void Render(RenderContext context)
		{
			base.Render(context);
			LoadProperties();
			SKPoint pos = context.TranslatePoint(Position);
			float Size = context.TranslateSize(this.Size);
			if (fontFamily is not null)
			{
				var face = context.GetFont(fontFamily);
				if (face != null)
				{

					context.canvas.DrawText(str,
						pos.X, pos.Y, SKTextAlign.Center, new SKFont(face, Size) { },
						new SKPaint()
						{
							ColorF = Color,
							StrokeWidth = context.TranslateSize(StrokeWidth),
							Shader = shader,
							IsStroke = IsStroke,
							IsAntialias = true
						}
					);
					return;
				}
			}
			context.canvas.DrawText(str,
				pos.X, pos.Y, SKTextAlign.Center, new SKFont(SKTypeface.Default, Size) { },
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
