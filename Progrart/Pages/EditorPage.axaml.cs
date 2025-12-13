using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Progrart.Controls.TabSystem;

namespace Progrart.Pages;

public partial class EditorPage : UserControl, ITabPage
{
	public EditorPage()
	{
		InitializeComponent();
	}

	public void BindButton(TabButton button)
	{
		button.Title="Editor Page";
	}

	public void Execute()
	{
	}

	public bool IsModified()
	{
		return false;
	}

	public void Save()
	{
	}

	public void SetHost(TabHost host)
	{

	}
}