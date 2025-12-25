using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using System.Threading.Tasks;

namespace Progrart.Pages;

public partial class ImageViewPage : UserControl, ITabPage, IEditorPage
{
	public ImageViewPage()
	{
		InitializeComponent();
	}
	TabButton? TabButton = null;
	public void BindButton(TabButton button)
	{
		TabButton = button;
		TabButton.Title = "Image";
	}

	public void Execute(ExecuteArguments? args = null)
	{
	}

	public bool IsModified()
	{
		return false;
	}

	public void LoadDocument(IStorageFile file)
	{
		Task.Run(async () =>
		{

			using var fs = await file.OpenReadAsync();
			Bitmap bitmap = new Bitmap(fs);
			Dispatcher.UIThread.Invoke(() => {
				this.ImageViewer.SetImage(bitmap);
			});
		});
	}

	public void Save()
	{
	}

	public void SetHost(TabHost host)
	{
	}
}