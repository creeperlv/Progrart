using Avalonia;
using Avalonia.Browser;
using Progrart;
using Progrart.Browser.Settings;
using System.Threading.Tasks;

internal sealed partial class Program
{
	private static Task Main(string[] args)
	{
		App.SettingsProvider=new BrowserSettingsProvider();
		return BuildAvaloniaApp()
			.WithInterFont()
			.StartBrowserAppAsync("out");
	}

	public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}