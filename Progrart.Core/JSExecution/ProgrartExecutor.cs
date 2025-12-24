using Jint;
using Jint.Native;
using Jint.Native.Function;
using Progrart.Core.Graphics;

namespace Progrart.Core.JSExecution
{
	public class ProgrartExecutor
	{
		public ExecutionEngine engine;
		public Dictionary<string, BaseElement> ObjectPool = new();
		public ProgrartExecutor()
		{
			engine = new ExecutionEngine();
			engine.Engine.SetValue("visual_root", new Func<JsObject>(() =>
			{
				return ProgrartFunctions.CreateVisualRoot(this);
			}
			));
			engine.Engine.SetValue("color4", new Func<JsNumber, JsNumber, JsNumber, JsNumber, JsObject>((r, g, b, a) =>
			{
				return ProgrartFunctions.color(this
					, r.AsNumber()
					, g.AsNumber()
					, b.AsNumber()
					, a.AsNumber()
					);
			}
			));
			engine.Engine.SetValue("color3", new Func<JsNumber, JsNumber, JsNumber, JsObject>((r, g, b) =>
			{
				return ProgrartFunctions.color(this
					, r.AsNumber()
					, g.AsNumber()
					, b.AsNumber()
					);
			}
			));
		}
		public RenderContext RenderImage(int Scale, string script, Dictionary<string, string> arguments)
		{
			int width = 1;
			int height = 1;
			foreach (var item in arguments)
			{
				engine.Engine.SetValue(item.Key, item.Value);
			}
			if (engine.Engine.GetValue("Width") is JsNumber js_width)
			{
				width = (int)(js_width.AsNumber());
			}
			if (engine.Engine.GetValue("Height") is JsNumber js_height)
			{
				height = (int)(js_height.AsNumber());
			}
			RenderContext renderContext = new RenderContext(width * Scale, width * Scale);
			ImageRoot imageRoot = new ImageRoot();
			engine.Execute(script);
			var img = engine.Engine.Call("main");
			imageRoot.Add(ObjectPool[$"{img.Get("id")}"]);
			imageRoot.Render(renderContext);
			return renderContext;
		}
		public int RegisterObject(object obj)
		{
			return obj.GetHashCode();
		}
	}
}
