using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace Progrart.Controls;

public partial class ImageViewer : UserControl
{
	public ImageViewer()
	{
		InitializeComponent();
		
	}
	public void SetImage(Bitmap image)
	{
		ImagePresenter.Source = image;
		ImagePresenter.Width = image.Size.Width;
		ImagePresenter.Height = image.Size.Height;
	}
}