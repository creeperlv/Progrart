using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Progrart.Controls.TabSystem;

public partial class TabButton : UserControl
{
	public string? Title { get => TitleBlock.Text; set => TitleBlock.Text = value; }
	public string? TooltipText { get => ToolTip.GetTip(MainButton) as string; set => ToolTip.SetTip(MainButton, value); }
	public ITabPage? page = null;
	public TabHost? Host = null;
	public TabButton()
	{
		InitializeComponent();
		MainButton.Click += (_, _) =>
		{
			Host?.SelectButton(this);
		};
		CloseButton.Click += (_, _) =>
		{
			Host?.RemoveButton(this);
		};
	}
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
	public bool IsVisibleInHost()
	{
		if (page is Control pageControl)
			return pageControl.IsVisible;
		return false;
	}
	public void SetSelectState(bool v)
	{
		if (page is Control pageControl)
			pageControl.IsVisible = v;
		this.HintBackground.IsVisible = v;
	}
	public void ModificationCheck()
	{
		if (page is ITabPage tabPage)
		{
			ModificationIndicator.IsVisible= tabPage.IsModified();
		}
	}
	public bool IsModified()
	{
		if (page is ITabPage tabPage)
		{
			return tabPage.IsModified();
		}
		return false;
	}
}