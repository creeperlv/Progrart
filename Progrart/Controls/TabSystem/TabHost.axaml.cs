using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Progrart.Controls.TabSystem;

public partial class TabHost : UserControl
{
	public TabHost()
	{
		InitializeComponent();
	}
	public ITabPage? GetCurrentPage()
	{
		foreach (var item in PageContainer.Children)
		{
			if (item.IsVisible)
			{
				if (item is ITabPage page) return page;
			}
		}
		return null;
	}
	public void AddPage(ITabPage page)
	{
		TabButton button = new TabButton(page, this);
		this.TabContainer.Children.Add(button);
		if (page is Control pageControl)
			PageContainer.Children.Add(pageControl);
		page.BindButton(button);
		SelectButton(button);
	}
	public void SelectButton(TabButton button)
	{
		foreach (var item in TabContainer.Children)
		{
			if (item is TabButton itemBtn)
				itemBtn.SetSelectState(item == button);
		}
	}
	public void RemoveButton(TabButton button)
	{
		TabContainer.Children.Remove(button);
		if (button.page is Control pageControl)
			PageContainer.Children.Remove(pageControl);
	}
}