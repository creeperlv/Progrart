using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Progrart.Controls.TabSystem;
using Progrart.Core.Settings;

namespace Progrart.Pages;

public partial class SettingsPage : UserControl, ITabPage
{
	public SettingsPage()
	{
		InitializeComponent();
		if ((App.SettingsProvider) != null)
		{
			var settings = App.SettingsProvider.LoadSettings();
			{
				useParallelBuilding.IsChecked = settings.useParallelBuild;
				JobCont.Value = settings.JobCount;
			}
		}
	}

	public void BindButton(TabButton button)
	{
		button.Title = "Settings";
	}

	public bool IsModified()
	{
		return false;
	}

	public void SetHost(TabHost host)
	{
	}


	private void SaveSettings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (App.SettingsProvider is null) return;

		AppConfig config = new AppConfig()
		{
			useParallelBuild = useParallelBuilding.IsChecked ?? false,
			JobCount = (int)(JobCont.Value ?? -1)
		};
		App.SettingsProvider.SaveSettings(config);
	}
}