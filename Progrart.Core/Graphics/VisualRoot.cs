using Jint;
using Jint.Native;

namespace Progrart.Core.Graphics
{
	public class ImageRoot : BaseElement
	{
	}
	public class VisualRoot : BaseElement
	{
		public override void Render(RenderContext context)
		{
			base.Render(context);
			foreach (var item in Children)
			{
				item.Render(context);
			}
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
					__object.Set("translate", point);
				}
				{
					JsObject point = new JsObject(engine);
					point.Set("x", 0);
					point.Set("y", 0);
					__object.Set("scale", point);
				}
				{
					__object.Set("rotation", 0);
				}
				__object.Set("Width", 1);
				__object.Set("Height", 1);
			}
		}
	}
}
