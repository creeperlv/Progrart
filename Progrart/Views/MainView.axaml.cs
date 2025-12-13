using Avalonia.Controls;
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
			BottomPanel.IsVisible = BottomPanelToggle.IsChecked == true;
			SplitterVisual.IsVisible = BottomPanelToggle.IsChecked == true;
			Splitter.IsVisible = BottomPanelToggle.IsChecked == true;
			ContentGrid.RowDefinitions[2].Height = GridLength.Auto;
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