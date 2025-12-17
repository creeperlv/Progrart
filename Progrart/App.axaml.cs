using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Progrart.Views;
using Progrart.Icons;
using Progrart.Pages;

namespace Progrart;

public partial class App : Application
{
	public static bool isDesktop = false;
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		IconProvider.Register(new DefaultIconProvider());
		EditorProvider.Register("text", "Default Text Editor", typeof(EditorPage));
		EditorProvider.BindFileType("cs", "text");
		EditorProvider.BindFileType("c", "text");
		EditorProvider.BindFileType("cpp", "text");
		EditorProvider.BindFileType("h", "text");
		EditorProvider.BindFileType("hpp", "text");
		EditorProvider.BindFileType("txt", "text");
		EditorProvider.BindFileType("cpp", "text");
		EditorProvider.BindFileType("json", "text");
		EditorProvider.BindFileType("sh", "text");
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
			// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
			isDesktop = true;
			DisableAvaloniaDataAnnotationValidation();
			desktop.MainWindow = new MainWindow();
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
		{
			singleViewPlatform.MainView = new MainView();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void DisableAvaloniaDataAnnotationValidation()
	{
		// Get an array of plugins to remove
		var dataValidationPluginsToRemove =
			BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

		// remove each entry found
		foreach (var plugin in dataValidationPluginsToRemove)
		{
			BindingPlugins.DataValidators.Remove(plugin);
		}
	}
}