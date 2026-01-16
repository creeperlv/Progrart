using Jint;
using Jint.Native;
using Progrart.Core.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Progrart.Core.JSExecution
{
	public static class ProgrartConversion
	{
		public static SKPoint ObtainSKPointFromJsObject(JsObject jsObject)
		{
			return new SKPoint((float)jsObject.Get("x").AsNumber(), (float)jsObject.Get("y").AsNumber());
		}
		public static SKRect ObtainSKRectFromJsObject(JsObject jsObject)
		{
			return new SKRect((float)jsObject.Get("x").AsNumber(),
							 (float)jsObject.Get("y").AsNumber(),
							 (float)jsObject.Get("w").AsNumber(),
							 (float)jsObject.Get("h").AsNumber()
							 );
		}
		public static SKRect ObtainSKRectFromJsObject_XYWH_Style(JsObject jsObject)
		{
			float x = (float)jsObject.Get("x").AsNumber();
			float y = (float)jsObject.Get("y").AsNumber();
			float w = (float)jsObject.Get("w").AsNumber();
			float h = (float)jsObject.Get("h").AsNumber();
			return new SKRect(x, y, x + w, y + h);
		}
		public static SKColorF ObtainSKColorFFromJsObject(JsObject jsObject)
		{
			var r = (float)jsObject.Get("r").AsNumber();
			var g = (float)jsObject.Get("g").AsNumber();
			var b = (float)jsObject.Get("b").AsNumber();
			float a = 1;
			if (jsObject.TryGetValue("a", out var a_str))
			{
				a = (float)a_str.AsNumber();
			}
			return new SKColorF(r, g, b, a);
		}
		public static SKShader? ObtainFromJsObject(JsObject jsObject)
		{
			var type_str = jsObject.Get("Type").AsString();
			if (Enum.TryParse<ShaderType>(type_str, out var type))
			{
				switch (type)
				{
					case ShaderType.LinearGradient:
						{
							var sx = (float)jsObject.Get("Start").Get("x").AsNumber();
							var sy = (float)jsObject.Get("Start").Get("y").AsNumber();
							var ex = (float)jsObject.Get("End").Get("x").AsNumber();
							var ey = (float)jsObject.Get("End").Get("y").AsNumber();
							SKColor C0 = SKColors.Black;
							SKColor C1 = SKColors.White;
							float p0 = 0;
							float p1 = 1;
							if (jsObject.Get("Colors") is JsArray color_array)
							{
								if (color_array[0] is JsObject c0)
									if (color_array[1] is JsObject c1)
									{

										C0 = (SKColor)ObtainSKColorFFromJsObject(c0);
										C1 = (SKColor)ObtainSKColorFFromJsObject(c1);

									}
							}
							if (jsObject.Get("Positions") is JsArray pos_array)
							{
								if (pos_array[0] is JsNumber P0)
									if (pos_array[1] is JsNumber P1)
									{
										p0 = (float)P0.AsNumber();
										p1 = (float)P1.AsNumber();
									}
							}
							var TileMode_str = jsObject.Get("TileMode").AsString();
							if (Enum.TryParse<SKShaderTileMode>(TileMode_str, out var SKShaderTileMode))
								return SKShader.CreateLinearGradient(new SKPoint(sx, sy), new SKPoint(ex, ey),
								[C0, C1], [p0, p1], SKShaderTileMode);
						}
						break;
					case ShaderType.RadialGradient:
						{
							var sx = (float)jsObject.Get("Center").Get("x").AsNumber();
							var sy = (float)jsObject.Get("Center").Get("y").AsNumber();
							var radius = (float)jsObject.Get("Radius").AsNumber();
							SKColor C0 = SKColors.Black;
							SKColor C1 = SKColors.White;
							float p0 = 0;
							float p1 = 1;
							if (jsObject.Get("Colors") is JsArray color_array)
							{
								if (color_array[0] is JsObject c0)
									if (color_array[1] is JsObject c1)
									{

										C0 = (SKColor)ObtainSKColorFFromJsObject(c0);
										C1 = (SKColor)ObtainSKColorFFromJsObject(c1);

									}
							}
							if (jsObject.Get("Positions") is JsArray pos_array)
							{
								if (pos_array[0] is JsNumber P0)
									if (pos_array[1] is JsNumber P1)
									{
										p0 = (float)P0.AsNumber();
										p1 = (float)P1.AsNumber();
									}
							}
							var TileMode_str = jsObject.Get("TileMode").AsString();
							if (Enum.TryParse<SKShaderTileMode>(TileMode_str, out var SKShaderTileMode))
								return SKShader.CreateRadialGradient(new SKPoint(sx, sy), radius,
								[C0, C1], [p0, p1], SKShaderTileMode);
						}
						break;
					case ShaderType.Picture:
						break;
					default:
						break;
				}
			}
			return null;
		}
	}
}
