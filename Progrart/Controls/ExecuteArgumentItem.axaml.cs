using Avalonia.Controls;

namespace Progrart.Controls;

public partial class ExecuteArgumentItem : UserControl
{

	public ExecuteArgumentItem()
	{
		InitializeComponent();
		Init();
	}
	public ExecuteArgumentItem(string key, string value)
	{
		InitializeComponent();
		Init();
		KeyBox.Text = key;
		ValueBox.Text = value;
	}
	void Init()
	{
		RemoveBtn.Click += (_, _) =>
		{
			if (this.Parent is StackPanel panel)
			{
				panel.Children.Remove(this);
			}
		};
	}
	public (string, string) GetItme() => (KeyBox.Text ?? "", ValueBox.Text ?? "");
}