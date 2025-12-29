using Jint;
using Jint.Native;
using Progrart.Core.Graphics;
using SkiaSharp;

namespace Progrart.Core.JSExecution
{
	public static class ProgrartFunctions
	{
		public static JsObject WrapObject(ProgrartExecutor executor, string Handle)
		{
			var obj = new JsObject(executor.engine.Engine);
			string handle_str = $"{Handle}";
			obj.Set("id", handle_str);
			obj.Set("add", JsValue.FromObject(executor.engine.Engine, new Action<JsObject>((obj) =>
			{
				if (executor.ObjectPool[handle_str] is BaseElement self)
				{

					if (executor.ObjectPool[$"{obj.Get("id")}"] is BaseElement element)
					{
						self.Add(element);
						return;
					}
					else
						throw new Exception($"Object {obj} is not a Progrart Element!");
				}
				else
					throw new Exception($"Object with id \"{handle_str}\" is not a Progrart Element!");
			})));
			return obj;
		}
		public static JsObject linear_gradient(Engine engine)
		{
			var obj = new JsObject(engine);

			{
				JsObject point = new JsObject(engine);
				point.Set("x", 0);
				point.Set("y", 0);
				obj.Set("Start", point);
			}
			{
				JsObject point = new JsObject(engine);
				point.Set("x", 0);
				point.Set("y", 0);
				obj.Set("End", point);
			}
			{
				obj.Set("Colors", new JsArray(engine, [
					color(engine, 1, 1, 1, 1) ,
					color(engine, 1, 1, 1, 1)
					]
				));
			}
			{
				obj.Set("Positions", new JsArray(engine, new[] { new JsNumber(0), new JsNumber(1) }));
			}
			obj.Set("TileMode", $"{SKShaderTileMode.Repeat}");
			obj.Set("Type", $"{ShaderType.LinearGradient}");
			return obj;
		}
		public static JsObject radial_gradient(Engine engine)
		{
			var obj = new JsObject(engine);

			{
				JsObject point = new JsObject(engine);
				point.Set("x", 0);
				point.Set("y", 0);
				obj.Set("Center", point);
			}
			{
				obj.Set("Colors", new JsArray(engine, [
					color(engine, 1, 1, 1, 1) ,
					color(engine, 1, 1, 1, 1)
					]
				));
			}
			{
				obj.Set("Positions", new JsArray(engine, new[] { new JsNumber(0), new JsNumber(1) }));
			}
			obj.Set("TileMode", $"{SKShaderTileMode.Repeat}");
			obj.Set("Type", $"{ShaderType.RadialGradient}");
			return obj;
		}
		public static JsObject color(Engine engine, double r, double g, double b, double a)
		{
			var obj = new JsObject(engine);
			obj.Set("r", r);
			obj.Set("g", g);
			obj.Set("b", b);
			obj.Set("a", a);
			return obj;
		}
		public static JsObject color(Engine engine, double r, double g, double b)
		{
			var obj = new JsObject(engine);
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
			root.SetupProperties(executor.engine.Engine);
			return obj;
		}
		public static JsObject CreateLine(ProgrartExecutor executor)
		{
			Line element = new();
			var obj = WrapObject(executor, executor.RegisterObject(element));
			element.__object = obj;
			element.SetupProperties(executor.engine.Engine);
			return obj;
		}
		public static JsObject CreateElement<T>(ProgrartExecutor executor) where T: BaseElement
		{
			T element=Activator.CreateInstance<T>() as T;
			var obj = WrapObject(executor, executor.RegisterObject(element));
			element.__object = obj;
			element.SetupProperties(executor.engine.Engine);
			return obj;
		}
	}
}
