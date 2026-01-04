using Jint;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Progrart.Core.JSExecution
{
	public class MathFunctions
	{
		public static double abs(JsNumber v) => Math.Abs(v.AsNumber());
		public static double sin(JsNumber v) => Math.Sin(v.AsNumber());
		public static double cos(JsNumber v) => Math.Cos(v.AsNumber());
		public static double tan(JsNumber v) => Math.Tan(v.AsNumber());
		public static double tanh(JsNumber v) => Math.Tanh(v.AsNumber());
		public static double asin(JsNumber v) => Math.Asin(v.AsNumber());
		public static double acos(JsNumber v) => Math.Acos(v.AsNumber());
		public static double atan(JsNumber v) => Math.Atan(v.AsNumber());
		public static double atan2(JsNumber v, JsNumber v2) => Math.Atan2(v.AsNumber(), v2.AsNumber());
		public static double atanh(JsNumber v) => Math.Atanh(v.AsNumber());
		public static double sqrt(JsNumber v) => Math.Sqrt(v.AsNumber());
		public static double log(JsNumber v) => Math.Log(v.AsNumber());
		public static double log2(JsNumber v) => Math.Log2(v.AsNumber());
		public static double log10(JsNumber v) => Math.Log10(v.AsNumber());
		public static double exp(JsNumber v) => Math.Exp(v.AsNumber());
		public static double ceiling(JsNumber v) => Math.Ceiling(v.AsNumber());
		public static double floor(JsNumber v) => Math.Floor(v.AsNumber());
		public static double log_base(JsNumber v, JsNumber v2) => Math.Log(v.AsNumber(), v2.AsNumber());
		public static double pow(JsNumber v, JsNumber v2) => Math.Pow(v.AsNumber(), v2.AsNumber());
		public static double round(JsNumber v) => Math.Round(v.AsNumber());
		public static double sinh(JsNumber v) => Math.Sinh(v.AsNumber());
		public static double cosh(JsNumber v) => Math.Cosh(v.AsNumber());
		public static double cbrt(JsNumber v) => Math.Cbrt(v.AsNumber());
	}
	public class ExecutionEngine : IDisposable
	{
		public Engine Engine;
		public Dictionary<string, string> Symbols = new Dictionary<string, string>();
		public ExecutionEngine()
		{
			Engine = new Engine();
			Engine.SetValue("abs", MathFunctions.abs);
			Engine.SetValue("sin", MathFunctions.sin);
			Engine.SetValue("cos", MathFunctions.cos);
			Engine.SetValue("tan", MathFunctions.tan);
			Engine.SetValue("tanh", MathFunctions.tanh);
			Engine.SetValue("asin", MathFunctions.asin);
			Engine.SetValue("acos", MathFunctions.acos);
			Engine.SetValue("atan", MathFunctions.atan);
			Engine.SetValue("atan2", MathFunctions.atan2);
			Engine.SetValue("atanh", MathFunctions.atanh);
			Engine.SetValue("sqrt", MathFunctions.sqrt);
			Engine.SetValue("cbrt", MathFunctions.cbrt);
			Engine.SetValue("pow", MathFunctions.pow);
			Engine.SetValue("log", MathFunctions.log);
			Engine.SetValue("log_base", MathFunctions.log_base);
			Engine.SetValue("log2", MathFunctions.log2);
			Engine.SetValue("log10", MathFunctions.log10);
			Engine.SetValue("exp", MathFunctions.exp);
			Engine.SetValue("ceiling", MathFunctions.ceiling);
			Engine.SetValue("floor", MathFunctions.floor);
			Engine.SetValue("sinh", MathFunctions.sinh);
			Engine.SetValue("cosh", MathFunctions.cosh);
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
		}

		public void Dispose()
		{
			Engine.Dispose();
		}
	}
}
