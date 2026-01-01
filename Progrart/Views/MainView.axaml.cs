using Acornima.Ast;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Progrart.Commands;
using Progrart.Controls;
using Progrart.Core;
using Progrart.Core.JSExecution;
using Progrart.Pages;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Progrart.Views;

public partial class MainView : UserControl
{
	static MainView? Instance;
	public MainView()
	{
		Instance = this;
		InitializeComponent();
		Trace.Listeners.Add(new ConsoleLogger());
		EditorProvider.setHost(MainTabHost);
		MenuButton.Focus();
		this.KeyBindings.Add(new Avalonia.Input.KeyBinding()
		{
			Gesture = new Avalonia.Input.KeyGesture(Avalonia.Input.Key.Space,
			Avalonia.Input.KeyModifiers.Control | Avalonia.Input.KeyModifiers.Shift),
			Command = new GenericCommand()
			{
				Checker = (_) => true,
				onExecute = (_) =>
				{
					MenuButton.Flyout?.ShowAt(MenuButton);
				}
			}
		});
		this.KeyBindings.Add(new Avalonia.Input.KeyBinding()
		{
			Gesture = new Avalonia.Input.KeyGesture(Avalonia.Input.Key.F5,
			Avalonia.Input.KeyModifiers.None),
			Command = new GenericCommand()
			{
				Checker = (_) => true,
				onExecute = (_) =>
				{
					Execute();
				}
			}
		});
		ToolTip.SetTip(MenuButton, "Press Shift+Ctrl+Space to open menu");
		ToolTip.SetIsOpen(MenuButton, true);

		BottomPanelToggle.IsCheckedChanged += (a, b) =>
		{
			bool v = BottomPanelToggle.IsChecked == true;
			BottomPanel.IsVisible = v;
			SplitterVisual.IsVisible = v;
			Splitter.IsVisible = v;
			ContentGrid.RowDefinitions[2].Height = GridLength.Auto;
			if (v)
			{
				ContentGrid.RowDefinitions[2].MinHeight = 48;
			}
			else
			{
				ContentGrid.RowDefinitions[2].MinHeight = 0;
			}
		};
		LeftPanelToggle.IsCheckedChanged += (a, b) =>
		{
			bool v = LeftPanelToggle.IsChecked == true;
			LeftPanel.IsVisible = v;
			VerticalSplitterVisual.IsVisible = v;
			VerticalSplitter.IsVisible = v;
			MainControlArea.ColumnDefinitions[0].Width = GridLength.Auto;
		};
		NewFileButton.Click += (_, _) =>
		{
			MainTabHost.AddPage(new EditorPage());
		};
		AboutItem.Click += (_, _) =>
		{
			MainTabHost.AddPage(new AboutPage());
		};
		OpenProjectMenuItem.Click += async (_, _) =>
		{
			var topLevel = TopLevel.GetTopLevel(this);

			if (topLevel == null) return;

			var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
			{
				Title = "Select Folder",
				AllowMultiple = false
			});

			if (folders.Count >= 1)
			{
				FileContainer.Children.Clear();
				var folderPath = folders[0].TryGetLocalPath();
				var folder = folders[0];
				FileContainer.Children.Add(new FileItem(folder));
			}
		};
		FileExplorerCloseButton.Click += (_, _) =>
		{
			LeftPanelToggle.IsChecked = false;
		};
		LeftPanelToggle.IsChecked = true;
		RunButton.Click += (_, _) =>
		{
			Execute();
		};
	}

	private void Execute()
	{
		if (MainTabHost.GetCurrentPage() is IEditorPage editor)
		{
			ExecuteArguments args = new ExecuteArguments();
			args.data["Scale"] = $"{SizeBox.Value}";
			args.data["Debug"] = $"{IsDebugBox.IsChecked ?? false}".ToLower();
			editor.Execute(args);
		}
	}

	private void OutputClear_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		Output.Text = "";
	}

	public void Write(string message)
	{
		Dispatcher.UIThread.Invoke(() =>
		Output.Text += message);
	}
	public void WriteLine(string message)
	{
		Dispatcher.UIThread.Invoke(() =>
		{
			Output.Text += $"{message}\n";
		});
	}
	private class ConsoleLogger : TraceListener
	{
		public override void Write(string? message)
		{
			Instance?.Write(message ?? "");
		}

		public override void WriteLine(string? message)
		{
			Instance?.WriteLine(message ?? "");
		}
	}

	private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (MainTabHost.GetCurrentPage() is IEditorPage editor)
		{
			editor.Save();
		}
	}

	private void MenuItem_Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (MainTabHost.GetCurrentPage() is IEditorPage editor) Task.Run(async () => await editor.Save());
	}

	private void MenuItem_SaveAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		Task.Run(async () => await MainTabHost.Foreach(async (page) =>
		{
			if (page is IEditorPage editor)
				await editor.Save();
			return false;
		}));
	}
}