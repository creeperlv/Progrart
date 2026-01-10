using Jint;
using Jint.Native;
using SkiaSharp;

namespace Progrart.Core.Graphics
{
	public class Path : BaseElement
	{
		SKPath path = new SKPath();
		public SKPath RealPath => path;
		internal List<PathCmd> Commands = new List<PathCmd>();
		public override void SetupProperties(Engine engine)
		{
			base.SetupProperties(engine);
			if (__object != null)
			{
				__object.Set("line_to", JsValue.FromObject(engine, (object)line_to));
				__object.Set("move_to", JsValue.FromObject(engine, (object)move_to));
				__object.Set("quad_to", JsValue.FromObject(engine, (object)quad_to));
			}
		}
		void line_to(float x, float y) => Commands.Add(new LineToCmd(x, y));
		void move_to(float x, float y) => Commands.Add(new MoveToCmd(x, y));
		void quad_to(float x0, float y0, float x1, float y1) => Commands.Add(new QuadToCmd(x0, y0, x1, y1));

		public override void Render(RenderContext context)
		{
			base.Render(context);
			foreach (var item in Commands)
			{
				item.ApplyCommand(context, this.path);
			}
		}
	}
}
