using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Progrart.Controls.TabSystem;
using System.IO;
using System.Threading.Tasks;

namespace Progrart.Pages;

public partial class EditorPage : UserControl, ITabPage, IEditorPage
{
	public EditorPage()
	{
		InitializeComponent();
	}

	public void BindButton(TabButton button)
	{
		button.Title = "Editor Page";
	}

	public void Execute()
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
			using var stream = await file.OpenReadAsync();
			using StreamReader sr = new StreamReader(stream);
			var text = sr.ReadToEnd();
			Dispatcher.UIThread.Invoke(() =>
			{
				CodeEditBox.Text = text;
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