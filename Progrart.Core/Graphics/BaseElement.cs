using Jint;
using Jint.Native;

namespace Progrart.Core.Graphics
{
	public class BaseElement
	{
		public List<BaseElement> Children = new List<BaseElement>();
		public JsObject? __object = null;
		public virtual void Add(BaseElement element)
		{
			Children.Add(element);
		}
		public virtual void Remove(BaseElement element)
		{
			Children.Remove(element);
		}
		public virtual void Render(RenderContext context)
		{
		}
		public virtual void LoadProperties() { }
		public virtual void SetupProperties(Engine engine) { }
	}
}
