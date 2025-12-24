using Jint.Native;
using Progrart.Core.Graphics;

namespace Progrart.Core.JSExecution
{
    public static class ProgrartFunctions
	{
		public static JsObject WrapObject(ProgrartExecutor executor, int Handle)
		{
			var obj = new JsObject(executor.engine.Engine);
			string handle_str = $"{Handle}";
			obj.Set("id", handle_str);
			obj.Set("add", JsValue.FromObject(executor.engine.Engine, new Action<JsObject>((obj) =>
			{
				executor.ObjectPool[handle_str].Add(executor.ObjectPool[$"{obj.Get("id")}"]);
			})));
			return obj;
		}
		public static JsObject color(ProgrartExecutor executor, double r, double g, double b, double a)
		{
			var obj = new JsObject(executor.engine.Engine);
			obj.Set("r", r);
			obj.Set("g", g);
			obj.Set("b", b);
			obj.Set("a", a);
			return obj;
		}
		public static JsObject color(ProgrartExecutor executor, double r, double g, double b)
		{
			var obj = new JsObject(executor.engine.Engine);
			obj.Set("r", r);
			obj.Set("g", g);
			obj.Set("b", b);
			obj.Set("a", 1);
			return obj;
		}
		public static JsObject CreateVisualRoot(ProgrartExecutor executor)
		{
			VisualRoot root = new();
			var obj = WrapObject(executor, executor.RegisterObject(root));
			root.__object = obj;
			return obj;
		}
	}
}
