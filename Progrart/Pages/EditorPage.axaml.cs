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
	IStorageFile? file = null;
	TabButton? btn = null;
	public EditorPage()
	{
		InitializeComponent();
	}

	public void BindButton(TabButton button)
	{
		btn = button;
		button.Title = "Editor Page";
		if (file is not null)
		{
			btn.Title = file.Name;
			var path = file.TryGetLocalPath();
			btn.TooltipText = path;
		}
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
		this.file = file;
		if (btn is not null)
		{
			btn.Title = file.Name;
			var path= file.TryGetLocalPath();
			btn.TooltipText= path;
		}
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