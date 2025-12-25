using Jint;
using Jint.Native;
using Progrart.Core.JSExecution;
using SkiaSharp;

namespace Progrart.Core.Graphics
{
	public class Line : BaseElement
	{
		float Size;
		float StartX;
		float StartY;
		float EndX;
		float EndY;
		float ColorR;
		float ColorG;
		float ColorB;
		float ColorA;
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
						StartX = (float)Start.Get("x").AsNumber();
						StartY = (float)Start.Get("y").AsNumber();
					}
				}
				{
					if (__object.Get("End") is JsObject End)
					{
						EndX = (float)End.Get("x").AsNumber();
						EndY = (float)End.Get("y").AsNumber();
					}
				}
			}
		}
		public override void Render(RenderContext context)
		{
			base.Render(context);
			context.DrawingCore.canvas.DrawLine(
				context.TranslatePoint((float)StartX, (float)StartY),
				context.TranslatePoint((float)StartX, (float)StartY),
				new SkiaSharp.SKPaint()
				{
					ColorF = new SkiaSharp.SKColorF(ColorR, ColorG, ColorB, ColorA),
					StrokeWidth = Size,

				}
			);

		}
	}
}
