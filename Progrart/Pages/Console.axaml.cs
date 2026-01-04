using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Jint;
using Progrart.Commands;
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
		engine.SetValue("getWD", new Func<string>(() =>
		{
			return workdirectory?.Name ?? "<null>";
		}));
		engine.SetValue("setWD", new Action<string>((str) =>
		{
			if (workdirectory != null)
			{
				Task.Run(async () => workdirectory = await workdirectory.GetFolderAsync(str));
				Output.Text += $"Executed\n";
			}
		}));
		engine.SetValue("mkdir", new Action<string>((str) =>
		{
			if (workdirectory != null)
			{
				Task.Run(async () => await workdirectory.CreateFolderAsync(str));
				Output.Text += $"Executed\n";
			}
		}));
		CommandBox.KeyBindings.Add(new Avalonia.Input.KeyBinding()
		{
			Gesture = new Avalonia.Input.KeyGesture(Avalonia.Input.Key.Enter),
			Command = new GenericCommand()
			{
				Checker = (_) => true,
				onExecute = (_) => executeCmd()
			}
		});
		ExecuteBtn.Click += (_, _) =>
		{
			executeCmd();
		};
	}
	void executeCmd()
	{
		Output.Text += $"{engine.Evaluate(CommandBox.Text ?? "")}\n";

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