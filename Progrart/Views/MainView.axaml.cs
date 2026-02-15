using Acornima.Ast;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DialogHostAvalonia;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Progrart.Commands;
using Progrart.Controls;
using Progrart.Core;
using Progrart.Core.JSExecution;
using Progrart.Core.ProjectSystem;
using Progrart.Core.Settings;
using Progrart.Core.Storage;
using Progrart.Dialogs;
using Progrart.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
			MainTabHost.AddPage(new ProgrartEditorPage());
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
				var folder = folders[0];
				App.CurrentOpenFolder = folder;
				FileItem item1 = new(folder, null, false);
				FileContainer.Children.Add(item1);
				await foreach (var item in folder.GetItemsAsync())
				{
					if (item.Name.ToLower().EndsWith(".progrart-project", StringComparison.OrdinalIgnoreCase))
					{
						using var stream = await (item as IStorageFile)!.OpenReadAsync();
						using var reader = new StreamReader(stream);
						var txt = await reader.ReadToEndAsync();
						App.LoadedProject = JsonConvert.DeserializeObject<Project>(txt);
						App.ProjectLoad();
						break;
					}
				}
				await item1.OpenItem();
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

		ConfigBox.Items.Clear();
		App.ProjectLoadHandler = () =>
		{
			ConfigBox.Items.Clear();
			if (App.LoadedProject is null) return;
			foreach (var item in App.LoadedProject.Configurations)
			{
				ConfigBox.Items.Add(item.Name);
			}
			ConfigBox.SelectedIndex = 0;
		};
		Task.Run(async () =>
		{
			while (true)
			{
				await Task.Delay(500);
				await Dispatcher.UIThread.InvokeAsync(async () =>
				{
					await MainTabHost.ForeachButton(async (btn) =>
					{
						btn.ModificationCheck();
						return false;
					});
				});
			}
		});
		if (OperatingSystem.IsBrowser())
		{
			ExitItem.IsVisible = false;
			ExitItemSeparator.IsVisible = false;
		}
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
		SaveAll();
	}

	private void SaveAll(Action? OnDone = null)
	{
		Task.Run(async () =>
		{
			await MainTabHost.Foreach(async (page) =>
					{
						if (page is IEditorPage editor)
							await editor.Save();
						return false;
					});
			OnDone?.Invoke();
		});
	}

	private async void CreateProjectMenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(this);

		if (topLevel == null) return;

		var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
		{
			Title = "Save to ...",
			DefaultExtension = ".progrart-project",
			FileTypeChoices = new List<FilePickerFileType>() { new FilePickerFileType("Progrart Project"){
				 Patterns= new List<string>() { ".progrart-project" }
				}
			}
		});
		if ((file is null))
		{
			return;
		}
		using var stream = await file.OpenWriteAsync();
		var txt = JsonConvert.SerializeObject(new Project());
		using var stream_writer = new StreamWriter(stream);
		await stream_writer.WriteAsync(txt);
		await stream.FlushAsync();
	}

	private void BuildButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (App.LoadedProject is not null)
		{
			Builder builder = new Builder(App.LoadedProject, new AvaloniaStorageProvider(App.CurrentOpenFolder));
			var config = App.LoadedProject.Configurations[ConfigBox.SelectedIndex];
			var name = config.Name;
			if (config is null) return;
			if (name is null) return;
			int sum = 0;
			int max = config.Items.Count;
			ProgressGrid.IsVisible = true;
			MainProgress.Value = sum;
			MainProgress.Maximum = max;
			builder.OnProgressUpdate = (a, b) =>
			{
				Interlocked.Increment(ref sum);
				Dispatcher.UIThread.Invoke(() =>
				{
					MainProgress.Value = sum;
				});
			};
			builder.OnCompleted = () =>
			{
				Dispatcher.UIThread.Invoke(() =>
				{
					ProgressGrid.IsVisible = false;
				});
			};
			AppConfig app_config = new AppConfig();
			if (App.SettingsProvider != null)
				app_config = App.SettingsProvider.LoadSettings();
			Task.Run(async () => await builder.Build(name, app_config.useParallelBuild == false ? 1 : app_config.JobCount));
		}
	}

	private void SettingsMenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		MainTabHost.AddPage(new SettingsPage());
	}
	internal bool CloseCheck()
	{
		bool v = false;
		MainTabHost.ForeachButton((btn) =>
		{
			if (btn.IsModified())
			{
				v = true;
				return true;
			}
			return false;
		});
		return v;
	}
	public bool TryExit()
	{

		if (CloseCheck())
		{
			CloseConfirmationDialog dialog = new CloseConfirmationDialog();
			dialog.onOK = async () =>
			{
				SaveAll(() =>
				{
					Environment.Exit(0);
				});
				return false;
			};
			dialog.onDiscard = async () =>
			{
				Environment.Exit(0);
				return false;
			};
			DialogHost.Show(dialog);
			return false;
		}
		else
		{
			Environment.Exit(0);
			return true;
		}
	}
	private void ExitItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		TryExit();
	}

	private void MenuItem_SaveAs_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (MainTabHost.GetCurrentPage() is IEditorPage editor) Task.Run(async () => await editor.SaveAs());
	}

	private void SaveAsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (MainTabHost.GetCurrentPage() is IEditorPage editor) Task.Run(async () => await editor.SaveAs());
	}
}