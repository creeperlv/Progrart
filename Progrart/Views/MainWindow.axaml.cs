using Avalonia.Controls;

namespace Progrart.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		this.ExtendClientAreaToDecorationsHint = true;
		this.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
	}
}