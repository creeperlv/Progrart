using Jint.Native;
using System;
using System.Collections.Generic;
using System.Text;

namespace Progrart.Core.Graphics
{
	public class BaseElement
	{
		public JsObject? __object = null;
		public virtual void Add(BaseElement element) { }
		public virtual void Remove(BaseElement element) { }
		public virtual void Render(RenderContext context)
		{
		}
		public virtual void SetProperty(string key, string value)
		{

		}
	}
	public class ImageRoot : BaseElement
	{
	}
	public class VisualRoot : BaseElement
	{
	}
}
