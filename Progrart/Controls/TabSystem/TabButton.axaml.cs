using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Progrart.Controls.TabSystem;

public partial class TabButton : UserControl
{
	public string? Title { get => MainButton.Content as string; set => MainButton.Content = value; }
	public string? TooltipText { get => ToolTip.GetTip(MainButton) as string; set => ToolTip.SetTip(MainButton, value); }
	public ITabPage page;
	public TabHost Host;
	public TabButton(ITabPage page, TabHost host)
	{
		InitializeComponent();
		this.page = page;
		Host = host;
		MainButton.Click += (_, _) =>
		{
			Host.SelectButton(this);
		};
		CloseButton.Click += (_, _) =>
		{
			Host.RemoveButton(this);
		};
	}
	public void SetSelectState(bool v)
	{
		if (page is Control pageControl)
			pageControl.IsVisible = v;
		this.HintBackground.IsVisible = v;
	}
}