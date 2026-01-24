using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Progrart.Pages;
using System;
using System.Threading.Tasks;

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
	public TabButton? IsOpen(IStorageFile file)
	{

		foreach (var item in TabContainer.Children)
		{
			if (item is TabButton itemBtn)
				if (itemBtn.page is IEditorPage page)
					if (page.IsSameFile(file)) return itemBtn;
		}
		return null;
	}
	public async Task Foreach(Func<ITabPage, Task<bool>> handler)
	{
		foreach (var item in PageContainer.Children)
		{
			if (item is ITabPage page)
			{
				if (await handler(page)) break;
			}
		}
	}
	public async Task ForeachButton(Func<TabButton, Task<bool>> handler)
	{
		foreach (var item in TabContainer.Children)
		{
			if (item is TabButton btn)
			{
				if (await handler(btn)) break;
			}
		}
	}
	public void ForeachButton(Func<TabButton, bool> handler)
	{
		foreach (var item in TabContainer.Children)
		{
			if (item is TabButton btn)
			{
				if (handler(btn)) break;
			}
		}
	}
	public void SelectButton(TabButton button)
	{
		foreach (var item in TabContainer.Children)
		{
			if (item is TabButton itemBtn)
				itemBtn.SetSelectState(item == button);
		}
	}
	public void SelectButton(int index)
	{
		for (int i = 0; i < TabContainer.Children.Count; i++)
		{
			Control? item = TabContainer.Children[i];
			if (item is TabButton itemBtn)
				itemBtn.SetSelectState(i == index);
		}
	}
	private int GetOpenIndex()
	{
		for (int i = 0; i < TabContainer.Children.Count; i++)
		{
			Control item = TabContainer.Children[i];
			if (item is TabButton itemBtn)
			{
				if (itemBtn.IsVisibleInHost()) return i;
			}
		}
		return -1;
	}
	public void RemoveButton(TabButton button)
	{
		var index = TabContainer.Children.IndexOf(button);
		int v = GetOpenIndex();
		TabContainer.Children.Remove(button);
		if (button.page is Control pageControl)
			PageContainer.Children.Remove(pageControl);

		if (index == v)
		{
			if (index - 1 > 0)
				SelectButton(index - 1);
			else
			{
				SelectButton(1);
			}
		}
		if (v == -1)
		{
			SelectButton(0);
		}
	}
}