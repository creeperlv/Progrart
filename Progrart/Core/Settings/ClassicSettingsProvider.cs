using System;
using System.IO;
using System.Text.Json;

namespace Progrart.Core.Settings
{
	public class ClassicSettingsProvider : ISettingsProvider
	{
		private readonly string _filePath;
		private SerializeContext context = new SerializeContext();
		public ClassicSettingsProvider()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appFolder = Path.Combine(folder, "Progrart");
			Directory.CreateDirectory(appFolder);
			_filePath = Path.Combine(appFolder, "settings.json");
		}

		public void SaveSettings(AppConfig settings)
		{
			var json = JsonSerializer.Serialize(settings, context.AppConfig);
			File.WriteAllText(_filePath, json);
		}

		public AppConfig LoadSettings()
		{
			if (!File.Exists(_filePath)) return new AppConfig();

			var json = File.ReadAllText(_filePath);
			return JsonSerializer.Deserialize<AppConfig>(json, context.AppConfig) ?? new AppConfig();
		}
	}
}
