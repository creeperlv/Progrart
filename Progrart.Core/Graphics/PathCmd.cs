using SkiaSharp;

namespace Progrart.Core.Graphics
{
    public class PathCmd
	{
		public virtual void ApplyCommand(RenderContext context, SKPath path) { }
	}
	public class LineToCmd(float x, float y) : PathCmd
	{
		public readonly float x = x;
		public readonly float y = y;
		public override void ApplyCommand(RenderContext context, SKPath path)
		{
			base.ApplyCommand(context, path);
			var pos = context.TranslatePoint(x, y);
			path.LineTo(pos);
		}
	}
	public class MoveToCmd(float x, float y) : PathCmd
	{
		public readonly float x = x;
		public readonly float y = y;
		public override void ApplyCommand(RenderContext context, SKPath path)
		{
			base.ApplyCommand(context, path);
			var pos = context.TranslatePoint(x, y);
			path.MoveTo(pos);
		}
	}
}
