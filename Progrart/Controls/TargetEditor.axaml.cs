using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Progrart.Core.ProjectSystem;

namespace Progrart;

public partial class TargetEditor : UserControl
{
	public TargetEditor()
	{
		InitializeComponent();
	}
	public TargetEditor(Progrart.Core.ProjectSystem.BuildItem item)
	{
		InitializeComponent();
		SourceFileBox.Text = item.Source;
		OutputFileBox.Text = item.Target;
		SizeBox.Value = item.Size;
		this.Arguments.LoadArguments(item.Arguments);
	}
	public BuildItem GetItem()
	{
		BuildItem item = new BuildItem();
		item.Source = SourceFileBox.Text ?? "";
		item.Target = OutputFileBox.Text ?? "";
		item.Size = (int)(SizeBox.Value ?? 0);
		item.Arguments = this.Arguments.ObtainArguments();
		return item;
	}
}