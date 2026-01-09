using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;

namespace Progrart.Core.Graphics
{
	public class Triangle : BaseElement
	{
		float StrokeWidth;
		SKPoint Vertex0;
		SKPoint Vertex1;
		SKPoint Vertex2;
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
					__object.Set("Vertex0", point);
				}
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("Vertex1", point);
				}
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("Vertex2", point);
				}
				__object.Set("StrokeWidth", 1);
				__object.Set("IsStroke", true);
				__object.Set("Color", ProgrartFunctions.color(engine, 1, 1, 1, 1));


			}

		}
		public override void LoadProperties()
		{
			if (__object is not null)
			{
				StrokeWidth = (float)__object.Get("StrokeWidth").AsNumber();
				IsStroke = (bool)__object.Get("IsStroke").AsBoolean();
				{
					if (__object.Get("Vertex0") is JsObject Start)
					{
						this.Vertex0 = ProgrartConversion.ObtainSKPointFromJsObject(Start);
					}
				}
				{
					if (__object.Get("Vertex1") is JsObject Start)
					{
						this.Vertex1 = ProgrartConversion.ObtainSKPointFromJsObject(Start);
					}
				}
				{
					if (__object.Get("Vertex2") is JsObject Start)
					{
						this.Vertex2 = ProgrartConversion.ObtainSKPointFromJsObject(Start);
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
			var v0 = context.TranslatePoint(Vertex0);
			var v1 = context.TranslatePoint(Vertex1);
			var v2 = context.TranslatePoint(Vertex2);
			using var p = new SKPath();
			p.MoveTo(v0);
			p.MoveTo(v0);
			//p.LineTo(v0);
			p.LineTo(v1);
			p.LineTo(v2);
			p.LineTo(v0);
			p.Close();

			context.DrawingCore.canvas.DrawPath(p,
				new SKPaint()
				{
					ColorF = Color,
					StrokeWidth = context.TranslateSize(StrokeWidth),
					Shader = shader,
					IsStroke = IsStroke,
					IsAntialias = true
				});
		}
	}
}
