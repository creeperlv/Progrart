using Avalonia.Controls;

namespace Progrart.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		this.ExtendClientAreaToDecorationsHint = true;
		this.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
		this.Closing += MainWindow_Closing;
	}

	private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
	{
		e.Cancel=!MainAppView.TryExit();
		
	}
}