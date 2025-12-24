using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Progrart.Controls.TabSystem;
using SkiaSharp;

namespace Progrart.Pages;

public partial class AboutPage : UserControl,ITabPage
{
    public AboutPage()
    {
        InitializeComponent();

	}

	public void BindButton(TabButton button)
    {
        button.Title="About";
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