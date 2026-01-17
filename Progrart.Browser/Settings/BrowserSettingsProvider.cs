using Progrart.Core.Settings;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace Progrart.Browser.Settings
{
	public partial class BrowserSettingsProvider : ISettingsProvider
	{
		private SerializeContext context=new SerializeContext();
		[JSImport("globalThis.localStorage.setItem")]
		private static partial void SetItem(string key, string value);

		[JSImport("globalThis.localStorage.getItem")]
		private static partial string GetItem(string key);
		public AppConfig LoadSettings()
		{
			var json = GetItem("progrart.settings");
			return json != null
				? JsonSerializer.Deserialize<AppConfig>(json,context.AppConfig)??new AppConfig()
				: new AppConfig();
		}

		public void SaveSettings(AppConfig config)
		{
			var json = JsonSerializer.Serialize(config,context.AppConfig)??"";
			SetItem("progrart.settings", json);
		}
	}
}
