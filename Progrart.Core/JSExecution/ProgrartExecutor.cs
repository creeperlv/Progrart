using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Json;
using Progrart.Core.Graphics;
using System.Diagnostics;
using System.Text.Json;

namespace Progrart.Core.JSExecution
{
	public class ProgrartExecutor : IDisposable
	{
		public ExecutionEngine engine;
		public Dictionary<string, object> ObjectPool = new();
		public ProgrartExecutor()
		{
			engine = new ExecutionEngine();
			SetupCalls();
		}
		public void SetupCalls()
		{
			engine.Engine.SetValue("visual_root", visual_root);
			Jint.Native.Json.JsonSerializer serializer = new Jint.Native.Json.JsonSerializer(engine.Engine);
			engine.Engine.SetValue("log", new Action<JsValue>((v) =>
			{
				if (v is JsObject obj)
				{
					Trace.WriteLine(serializer.Serialize(obj));
				}
				else
					Trace.WriteLine(v);
			}));
			engine.Engine.SetValue("line", line);
			engine.Engine.SetValue("color4", color4);
			engine.Engine.SetValue("color3", color3);
			engine.Engine.SetValue("linear_gradient", linear_gradient);
			engine.Engine.SetValue("radial_gradient", radial_gradient);
		}
		public JsObject color4(JsNumber r, JsNumber g, JsNumber b, JsNumber a)
		{
			return ProgrartFunctions.color(engine.Engine
				, r.AsNumber()
				, g.AsNumber()
				, b.AsNumber()
				, a.AsNumber()
				);
		}
		public JsObject color3(JsNumber r, JsNumber g, JsNumber b)
		{
			return ProgrartFunctions.color(engine.Engine
				, r.AsNumber()
				, g.AsNumber()
				, b.AsNumber()
				);
		}
		public JsObject visual_root()
		{
			return ProgrartFunctions.CreateVisualRoot(this);
		}
		public JsObject line()
		{
			return ProgrartFunctions.CreateLine(this);
		}
		public RenderContext RenderImage(int Scale, string script, ExecuteArguments arguments)
		{
			float width = 1;
			float height = 1;
			engine.Symbols = arguments.data;
			engine.Execute(script);
			if (engine.Engine.GetValue("Width") is JsNumber js_width)
			{
				width = (float)(js_width.AsNumber());
			}
			if (engine.Engine.GetValue("Height") is JsNumber js_height)
			{
				height = (float)(js_height.AsNumber());
			}
			RenderContext renderContext = new RenderContext((int)(width * Scale), (int)(width * Scale));
			ImageRoot imageRoot = new ImageRoot();

			var img = engine.Engine.Call("main");
			if (ObjectPool[$"{img.Get("id")}"] is BaseElement element)
				imageRoot.Add(element);
			imageRoot.Render(renderContext);
			renderContext.canvas.Flush();
			return renderContext;
		}
		public string RegisterObject(object obj)
		{
			string v = $"{obj.GetHashCode()}";
			ObjectPool[v] = obj;
			return v;
		}
		public JsObject linear_gradient() => ProgrartFunctions.linear_gradient(engine.Engine);
		public JsObject radial_gradient() => ProgrartFunctions.radial_gradient(engine.Engine);
		public void Dispose()
		{
			engine.Dispose();
		}
	}
}
