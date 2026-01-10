using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Progrart.Views;
using Progrart.Icons;
using Progrart.Pages;
using Avalonia.Platform.Storage;

namespace Progrart;

public partial class App : Application
{
	public static bool isDesktop = false;
	public static IStorageFolder? CurrentOpenFolder = null;
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		IconProvider.Register(new DefaultIconProvider());
		EditorProvider.Register("text", "Default Text Editor", typeof(EditorPage));
		EditorProvider.Register("image", "Default Image Viewer", typeof(ImageViewPage));
		EditorProvider.Register("progrart", "Default Progrart Editor", typeof(ProgrartEditorPage));
		EditorProvider.Register("project", "Progrart Project Editor", typeof(ProjectEditor));
		EditorProvider.Register("console", "Console", typeof(Console));
		EditorProvider.BindFileType("cs", "text");
		EditorProvider.BindFileType("c", "text");
		EditorProvider.BindFileType("cpp", "text");
		EditorProvider.BindFileType("h", "text");
		EditorProvider.BindFileType("hpp", "text");
		EditorProvider.BindFileType("txt", "text");
		EditorProvider.BindFileType("ini", "text");
		EditorProvider.BindFileType("cpp", "text");
		EditorProvider.BindFileType("json", "text");
		EditorProvider.BindFileType("sh", "text");
		EditorProvider.BindFileType("progrart", "progrart");
		EditorProvider.BindFileType("bashrc", "text");
		EditorProvider.BindFileType("png", "image");
		EditorProvider.BindFileType("bmp", "image");
		EditorProvider.BindFileType("jpg", "image");
		EditorProvider.BindFileType("progrart-project", "project");
		EditorProvider.BindFileType("wd", "console");
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