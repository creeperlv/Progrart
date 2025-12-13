using Jint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Progrart.Core.JSExecution
{
	public class ExecutionEngine : IDisposable
	{
		public Engine Engine;
		public Dictionary<string, string> Symbols = new Dictionary<string, string>();
		public ExecutionEngine()
		{
			Engine = new Engine();
		}
		string formSymbol(Dictionary<string, string> symbols)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var item in symbols)
			{
				sb.AppendLine($"var {item.Key} = {item.Value}");
			}
			return sb.ToString();
		}
		public void Execute(string content)
		{
			Engine.Evaluate(formSymbol(Symbols));
			Engine.Evaluate(content);
			if (Engine.GetValue("config").AsObject().TryGetValue("width", out var w))
			{
				Trace.WriteLine(w);
			}
		}

		public void Dispose()
		{
			Engine.Dispose();

		}
	}
}
