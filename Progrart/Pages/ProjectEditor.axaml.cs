using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Jint.Runtime;
using Newtonsoft.Json;
using Progrart.Controls;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using Progrart.Core.ProjectSystem;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Progrart.Pages;

public partial class ProjectEditor : UserControl, ITabPage, IEditorPage
{
	public ProjectEditor()
	{
		InitializeComponent();
		AddButton.Click += (_, _) =>
		{
			ConfigurationHolder.Children.Add(new ConfigEditor());
		};
	}

	public void Execute(ExecuteArguments? args = null)
	{
	}

	public bool IsSameFile(IStorageFile file)
	{
		return (this.file?.Path == file.Path);
	}
	IStorageFile? file = null;
	public void LoadDocument(IStorageFile file)
	{
		this.file = file;
		Task.Run(async () =>
		{
			using var stream = await file.OpenReadAsync();
			using var sr = new StreamReader(stream);
			Project? project = JsonConvert.DeserializeObject<Project>(sr.ReadToEnd());
			if (project is null)
			{
				return;
			}
			Dispatcher.UIThread.Invoke(() =>
			{
				Load(project);
			});
		});
	}
	void Load(Project project)
	{
		this.OutputDirBox.Text = project.OutputDir;
		ProjectWideArguments.LoadArguments(project.Arguments);
		foreach (var config in project.Configurations)
		{
			var editor = new ConfigEditor();
			editor.LoadConfig(config);
			ConfigurationHolder.Children.Add(editor);
		}
	}
	public async Task Save()
	{
		if (file is null) return;
		Project project = new Project();
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			project.OutputDir = this.OutputDirBox.Text ?? "";
			project.Arguments = ProjectWideArguments.ObtainArguments();
			foreach (var item in ConfigurationHolder.Children)
			{
				if (item is ConfigEditor editor)
				{
					project.Configurations.Add(editor.ObtainConfiguration());
				}
			}
		});
		using var stream = await file.OpenWriteAsync();
		using var sw = new StreamWriter(stream);
		sw.WriteLine(JsonConvert.SerializeObject(project, Formatting.Indented));
		sw.Flush();
		sw.Close();
	}
	public async Task SaveAs()
	{
		var provider = TopLevel.GetTopLevel(this)?.StorageProvider;
		if (provider is null)
		{
			return;
		}
		var file = await provider.SaveFilePickerAsync(new FilePickerSaveOptions
		{
			SuggestedStartLocation = App.CurrentOpenFolder,
			FileTypeChoices =
			[
				new FilePickerFileType("Progrart Project File")
				{
					Patterns = ["*.progrart-project"],
					MimeTypes = ["application/json"]
				}
			],
			Title = "Save Progrart Project"
		});
		if (file is not null)
		{
			Project project = new Project();
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				project.OutputDir = this.OutputDirBox.Text ?? "";
				project.Arguments = ProjectWideArguments.ObtainArguments();
				foreach (var item in ConfigurationHolder.Children)
				{
					if (item is ConfigEditor editor)
					{
						project.Configurations.Add(editor.ObtainConfiguration());
					}
				}
			});
			this.file = file;
			using var stream = await file.OpenWriteAsync();
			stream.SetLength(0);
			using var sw = new StreamWriter(stream);
			sw.WriteLine(JsonConvert.SerializeObject(project, Formatting.Indented));
			sw.Flush();
			sw.Close();
		}
		return;
	}
	public void SetHost(TabHost host)
	{
	}

	public void BindButton(TabButton button)
	{
		button.Title = "Project";
	}

	public bool IsModified()
	{
		return true;
	}
}