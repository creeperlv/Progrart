using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Progrart.Core;
using Progrart.Core.ProjectSystem;
using Progrart.Core.Settings;
using Progrart.Icons;
using Progrart.Pages;
using Progrart.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Progrart;

public partial class App : Application
{
	public static bool isDesktop = false;
	public static IStorageFolder? CurrentOpenFolder = null;
	public static Project? LoadedProject = null;
	internal static Action? ProjectLoadHandler = null;
	public static ISettingsProvider? SettingsProvider = null;
	public static void ProjectLoad()
	{
		ProjectLoadHandler?.Invoke();
	}
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}
	public async Task LoadProject(IStorageFile item)
	{

		using var stream = await (item).OpenReadAsync();
		using var reader = new StreamReader(stream);
		var txt = await reader.ReadToEndAsync();
		App.LoadedProject = JsonConvert.DeserializeObject<Project>(txt);
	}
	public override void OnFrameworkInitializationCompleted()
	{
		IconProvider.Register(new DefaultIconProvider());
		EditorProvider.Register("text", new EditorFileHandler<EditorPage>("Default Text Editor"));
		EditorProvider.Register("image", new EditorFileHandler<ImageViewPage>("Default Image Viewer"));
		EditorProvider.Register("progrart", new EditorFileHandler<ProgrartEditorPage>("Default Progrart Editor"));
		EditorProvider.Register("as_project", new FileHandler("Load Project",async (item) =>
		{
			await LoadProject(item);
			ProjectLoadHandler?.Invoke();
		}));
		EditorProvider.Register("project_editor", new EditorFileHandler<ProjectEditor>("Progrart Project Editor"));
		EditorProvider.Register("console", new EditorFileHandler<Pages.Console>("Console"));
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
		EditorProvider.BindFileType("progrart", "text");
		EditorProvider.BindFileType("bashrc", "text");
		EditorProvider.BindFileType("png", "image");
		EditorProvider.BindFileType("bmp", "image");
		EditorProvider.BindFileType("jpg", "image");
		EditorProvider.BindFileType("progrart-project", "as_project");
		EditorProvider.BindFileType("progrart-project", "project_editor");
		EditorProvider.BindFileType("progrart-project", "text");
		EditorProvider.BindFileType("wd", "console");
		if (!OperatingSystem.IsBrowser())
		{
			App.SettingsProvider = new ClassicSettingsProvider();
		}
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