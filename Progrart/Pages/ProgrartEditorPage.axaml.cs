using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using Progrart.Core.JSExecution;
using Progrart.Pages;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Progrart;

public partial class ProgrartEditorPage : UserControl, ITabPage, IEditorPage
{
	IStorageFile? file = null;
	TabButton? btn = null;
	public ProgrartEditorPage()
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
		using ProgrartExecutor executor = new ProgrartExecutor();
		try
		{
			var result = executor.RenderImage(100, CodeEditor.Text, new ExecuteArguments());
			var data = result.DrawingCore.ToData();
			using MemoryStream stream = new MemoryStream();
			data.SaveTo(stream);
			Bitmap image = new Bitmap(stream);
			PreviewImage.SetImage(image);
		}
		catch (System.Exception e)
		{
			Trace.WriteLine(e);
		}
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
			var path = file.TryGetLocalPath();
			btn.TooltipText = path;
		}
		{
			CodeEditor.SetGrammerByExtension(".js");
		}
		Task.Run(async () =>
		{
			using var stream = await file.OpenReadAsync();
			using StreamReader sr = new StreamReader(stream);
			var text = sr.ReadToEnd();
			Dispatcher.UIThread.Invoke(() =>
			{
				CodeEditor.Text = text;
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