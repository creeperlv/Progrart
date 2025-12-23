using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Progrart.Controls;
using Progrart.Core.JSExecution;
using Progrart.Pages;
using System;
using System.Diagnostics;

namespace Progrart.Views;

public partial class MainView : UserControl
{
	ExecutionEngine engine;
	static MainView? Instance;
	public MainView()
	{
		Instance = this;
		InitializeComponent();
		engine = new ExecutionEngine();
		//engine.Engine.SetValue("log", new Action<string>((s) => { Output.Text += $"{s}\n"; }));
		RunButton.Click += (_, _) =>
		{
			try
			{
				//engine.Execute(CodePane.Text);
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}
		};
		//Trace.Listeners.Add(new ConsoleLogger());
		EditorProvider.setHost(MainTabHost);
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
	}
	public void Write(string message)
	{
		//Dispatcher.UIThread.Invoke(() =>
		//Output.Text += message);
		try
		{
			Output.Text += message;
		}
		catch (Exception)
		{
		}
	}
	public void WriteLine(string message)
	{
		//Dispatcher.UIThread.Invoke(() =>);
		try
		{
			Output.Text += $"{message}\n";
		}
		catch (Exception)
		{
		}
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
}