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
	public class QuadToCmd(float x1, float y1, float x2, float y2) : PathCmd
	{
		public readonly float x1 = x1;
		public readonly float y1 = y1;
		private readonly float x2 = x2;
		private readonly float y2 = y2;

		public override void ApplyCommand(RenderContext context, SKPath path)
		{
			base.ApplyCommand(context, path);
			var pos = context.TranslatePoint(x1, y1);
			var pos2 = context.TranslatePoint(x2, y2);
			path.QuadTo(pos, pos2);
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
