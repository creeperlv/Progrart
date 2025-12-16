using Avalonia.Controls;
using Avalonia.Platform.Storage;
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
		Trace.Listeners.Add(new ConsoleLogger());

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

			// Open the folder picker
			var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
			{
				Title = "Select Folder",
				AllowMultiple = false
			});

			if (folders.Count >= 1)
			{
				// Get the first selected folder path
				// Note: TryGetLocalPath() returns null if the file isn't on a local disk (e.g., cloud storage)
				var folderPath = folders[0].TryGetLocalPath();
				var folder = folders[0];
				FileContainer.Children.Add(new FileItem(folder));
				if (folderPath != null)
				{
					// Do something with the path
					System.Diagnostics.Debug.WriteLine($"Picked: {folderPath}");
				}
			}
		};
		LeftPanelToggle.IsChecked = true;
	}
	public void Write(string message)
	{
		Output.Text += message;
	}
	public void WriteLine(string message)
	{
		Output.Text += $"{message}\n";
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