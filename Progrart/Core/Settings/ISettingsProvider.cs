using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Progrart.Core.Settings
{
	public interface ISettingsProvider
	{
		void SaveSettings(AppConfig config);
		AppConfig LoadSettings();
	}
	[Serializable]
	public class AppConfig
	{
		public bool useParallelBuild { get; set; }
		public int JobCount { get; set; }
	}
	[JsonSerializable(typeof(AppConfig))]
	public partial class SerializeContext : JsonSerializerContext
	{

	}
}
