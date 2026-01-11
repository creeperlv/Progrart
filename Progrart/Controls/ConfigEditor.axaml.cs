using Avalonia.Controls;
using AvaloniaEdit.Editing;

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
	}
}