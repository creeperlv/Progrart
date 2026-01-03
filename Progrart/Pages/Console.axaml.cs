using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Jint;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using System;
using System.Threading.Tasks;

namespace Progrart.Pages;

public partial class Console : UserControl, ITabPage, IEditorPage
{
	Engine engine;
	public Console()
	{
		engine = new Engine();
		InitializeComponent();
		engine.SetValue("log", new Action<object>((obj) =>
		{
			Output.Text += $"{obj}\n";
		}));
		engine.SetValue("getWDName", new Func<string>(() =>
		{
			return workdirectory?.Name ?? "<null>";
		}));
		engine.SetValue("setWD", new Action<string>((str) =>
		{
			if (workdirectory != null)
			{
				Task.Run(async () => workdirectory = await workdirectory.GetFolderAsync(str));
			}
		}));
		engine.SetValue("mkdir", new Action<string>((str) =>
		{
			if (workdirectory != null)
			{
				Task.Run(async () => await workdirectory.CreateFolderAsync(str));
			}
		}));
		ExecuteBtn.Click += (_, _) =>
		{
			engine.Execute(CommandBox.Text ?? "");
		};
	}

	public void BindButton(TabButton button)
	{
		button.Title = "Console";
	}

	public void Execute(ExecuteArguments? args = null)
	{
	}

	public bool IsModified()
	{
		return false;
	}

	public bool IsSameFile(IStorageFile file)
	{
		return false;
	}
	IStorageFolder? workdirectory;
	public void LoadDocument(IStorageFile file)
	{
		Task.Run(async () =>
		{
			workdirectory = await file.GetParentAsync();
		});
	}

	public async Task Save()
	{
		return;
	}

	public void SetHost(TabHost host)
	{
	}
}