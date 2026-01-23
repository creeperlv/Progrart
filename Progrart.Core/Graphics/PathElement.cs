using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;
using System.Diagnostics;

namespace Progrart.Core.Graphics
{
	public class PathElement : BaseElement
	{
		float StrokeWidth;
		SKColorF Color;
		SKShader? shader = null;
		public override void SetupProperties(Engine engine)
		{
			base.SetupProperties(engine);
			if (__object != null)
			{
				__object.Set("StrokeWidth", 1);
				__object.Set("Color", ProgrartFunctions.color(engine, 1, 1, 1, 1));
			}
		}
		public override void LoadProperties(RenderContext context)
		{
			if (__object is not null)
			{
				StrokeWidth = (float)__object.Get("StrokeWidth").AsNumber();
				{
					if (__object.Get("Color") is JsObject Color)
					{
						this.Color = ProgrartConversion.ObtainSKColorFFromJsObject(Color);
					}
				}

				if (__object.Get("Shader") is JsObject shaderObj)
					shader = ProgrartConversion.ObtainFromJsObject(context, shaderObj);
			}
		}
		public override void Render(RenderContext context)
		{
			base.Render(context);
			LoadProperties(context);
			foreach (var item in this.Children)
			{
				if (item is Path p)
				{
					p.Render(context);
					context.canvas.DrawPath(p.RealPath,
				new SKPaint()
				{
					ColorF = Color,
					StrokeWidth = context.TranslateSize(StrokeWidth),
					Shader = shader,
					IsAntialias = true
				});
				}
			}
		}
	}
}
