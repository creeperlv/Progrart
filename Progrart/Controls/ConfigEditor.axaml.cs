using Avalonia.Controls;
using AvaloniaEdit.Editing;
using Progrart.Core.ProjectSystem;

namespace Progrart.Controls;

public partial class ConfigEditor : UserControl
{
	public ConfigEditor()
	{
		InitializeComponent();
		AddButton.Click += (_, _) =>
		{
			TargetHolder.Children.Add(new TargetEditor());
		};
		RemoveButton.Click += (_, _) =>
		{
			if (Parent is StackPanel panel)
			{
				panel.Children.Remove(this);
			}
		};
	}
	public void LoadConfig(BuildConfiguration config)
	{
		Arguments.LoadArguments(config.Arguments);
		NameBox.Text = config.Name;
		OutputDirBox.Text = config.OutputDir;
		foreach (var item in config.Items)
		{
			TargetHolder.Children.Add(new TargetEditor(item));
		}
	}
	public BuildConfiguration ObtainConfiguration()
	{
		BuildConfiguration configuration = new BuildConfiguration();
		configuration.OutputDir = OutputDirBox.Text;
		configuration.Name= NameBox.Text;
		configuration.Arguments = Arguments.ObtainArguments();
		foreach (var item in TargetHolder.Children)
		{
			if (item is TargetEditor editor)
			{
				configuration.Items.Add(editor.GetItem());
			}
		}
		return configuration;
	}
	private void NameButton_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		ConfigContent.IsVisible = NameButton.IsChecked ?? false;
	}
}