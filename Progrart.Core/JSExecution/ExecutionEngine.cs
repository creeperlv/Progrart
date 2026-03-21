using Jint;
using Jint.Native;
using Jint.Runtime.Modules;
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
			JsObject _obj = new JsObject(Engine);
			Engine.SetValue("math", _obj);
			Random r = new Random();
			Engine.SetValue("def", (string k) => { return Symbols.ContainsKey(k); });
			Engine.SetValue("to_bool", (JsValue v) => { return bool.Parse(v.AsString().ToLower()); });
			Engine.SetValue("to_float", (JsValue v) => { return float.Parse(v.AsString().ToLower()); });
			Engine.SetValue("to_int", (JsValue v) => { return int.Parse(v.AsString().ToLower()); });
			Engine.SetValue("query", (string k, string fallback) =>
			{
				if (Symbols.TryGetValue(k, out var val)) return val;
				return fallback;
			});
			_obj.Set("random", JsObject.FromObject(Engine, new Func<double>(() =>
			{
				return r.NextDouble();
			})));
			_obj.Set("seed", JsObject.FromObject(Engine, new Action<JsNumber>((n) =>
			{
				r = new Random((int)n.AsNumber());
			})));
			_obj.Set("abs", JsObject.FromObject(Engine, (object)MathFunctions.abs));
			_obj.Set("sin", JsObject.FromObject(Engine, (object)MathFunctions.sin));
			_obj.Set("cos", JsObject.FromObject(Engine, (object)MathFunctions.cos));
			_obj.Set("tan", JsObject.FromObject(Engine, (object)MathFunctions.tan));
			_obj.Set("tanh", JsObject.FromObject(Engine, (object)MathFunctions.tanh));
			_obj.Set("asin", JsObject.FromObject(Engine, (object)MathFunctions.asin));
			_obj.Set("acos", JsObject.FromObject(Engine, (object)MathFunctions.acos));
			_obj.Set("atan", JsObject.FromObject(Engine, (object)MathFunctions.atan));
			_obj.Set("atan2", JsObject.FromObject(Engine, (object)MathFunctions.atan2));
			_obj.Set("atanh", JsObject.FromObject(Engine, (object)MathFunctions.atanh));
			_obj.Set("sqrt", JsObject.FromObject(Engine, (object)MathFunctions.sqrt));
			_obj.Set("cbrt", JsObject.FromObject(Engine, (object)MathFunctions.cbrt));
			_obj.Set("pow", JsObject.FromObject(Engine, (object)MathFunctions.pow));
			_obj.Set("log", JsObject.FromObject(Engine, (object)MathFunctions.log));
			_obj.Set("log_base", JsObject.FromObject(Engine, (object)MathFunctions.log_base));
			_obj.Set("log2", JsObject.FromObject(Engine, (object)MathFunctions.log2));
			_obj.Set("log10", JsObject.FromObject(Engine, (object)MathFunctions.log10));
			_obj.Set("exp", JsObject.FromObject(Engine, (object)MathFunctions.exp));
			_obj.Set("ceiling", JsObject.FromObject(Engine, (object)MathFunctions.ceiling));
			_obj.Set("floor", JsObject.FromObject(Engine, (object)MathFunctions.floor));
			_obj.Set("sinh", JsObject.FromObject(Engine, (object)MathFunctions.sinh));
			_obj.Set("cosh", JsObject.FromObject(Engine, (object)MathFunctions.cosh));
			Engine.SetValue("random", JsObject.FromObject(Engine, (int seed) =>
			{
				var obj = new JsObject(Engine);
				Random random = new Random(seed);
				obj.Set("Next", JsObject.FromObject(Engine, () =>
				{
					return random.Next();
				}));
				obj.Set("NextMax", JsObject.FromObject(Engine, (int max) =>
				{
					return random.Next(max);
				}));
				obj.Set("NextFloat", JsObject.FromObject(Engine, () => random.NextDouble()));
				return obj;
			}));
			Engine.SetValue("random_undetermined", JsObject.FromObject(Engine, () =>
			{
				var obj = new JsObject(Engine);
				Random random = new Random();
				obj.Set("Next", JsObject.FromObject(Engine, () =>
				{
					return random.Next();
				}));
				obj.Set("NextMax", JsObject.FromObject(Engine, (int max) =>
				{
					return random.Next(max);
				}));
				obj.Set("NextFloat", JsObject.FromObject(Engine, () => random.NextDouble()));
				return obj;
			}));
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
