using Jint;
using Jint.Native;
using System.Diagnostics;

namespace Progrart.Core.Graphics
{
	public class ImageRoot : BaseElement
	{
		public override void Render(RenderContext context)
		{
			base.Render(context);
			foreach (var item in Children)
			{
				item.Render(context);
			}
		}
	}
	public class VisualRoot : BaseElement
	{
		float w;
		float h;
		float tx;
		float ty;
		float scale;
		float rotate;
		void Transform(RenderContext context)
		{
			//Trace.WriteLine($"Visual Root: Rotation:{rotate}");
			context.canvas.Translate(tx, ty);
			context.canvas.RotateDegrees(rotate, context.DrawingCore.Width / 2, context.DrawingCore.Height / 2);
			if (scale != 1)
				context.canvas.Scale(scale, scale, context.DrawingCore.Width / 2, context.DrawingCore.Height / 2);
		}
		void Untransform(RenderContext context)
		{
			if (scale != 1)
				context.canvas.Scale(1 / scale, 1 / scale, context.DrawingCore.Width / 2, context.DrawingCore.Height / 2);
			context.canvas.RotateDegrees(-rotate, context.DrawingCore.Width / 2, context.DrawingCore.Height / 2);
			context.canvas.Translate(-tx, -ty);
		}
		public override void LoadProperties()
		{
			base.LoadProperties();
			if (__object is null) return;
			{
				if (__object.Get("Translate") is JsObject translate)
				{
					tx = (float)translate.Get("x").AsNumber();
					ty = (float)translate.Get("y").AsNumber();
				}
			}
			{
				scale = (float)__object.Get("Scale").AsNumber();
			}
			{
				rotate = (float)__object.Get("Rotation").AsNumber();
			}
			w = (float)__object.Get("Width").AsNumber();
			h = (float)__object.Get("Height").AsNumber();
		}
		public override void Render(RenderContext context)
		{
			base.Render(context);
			this.LoadProperties();
			Transform(context);
			foreach (var item in Children)
			{
				item.Render(context);
			}
			Untransform(context);
		}
		public override void SetupProperties(Engine engine)
		{
			base.SetupProperties(engine);
			if (__object != null)
			{
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("Translate", point);
				}
				{
					__object.Set("Scale", 1);
				}
				{
					__object.Set("Rotation", 0);
				}
				__object.Set("Width", 1);
				__object.Set("Height", 1);
			}
		}
	}
}
