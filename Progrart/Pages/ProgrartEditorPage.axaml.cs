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
			int Scale = 1024;
			if (args is not null)
			{
				if (args.data.TryGetValue("Scale", out var scale))
				{
					if (!int.TryParse(scale, out Scale)) Scale = 1024;
				}
			}
			var result = executor.RenderImage(1024, CodeEditor.Text, args ?? new ExecuteArguments());
			var data = result.DrawingCore.ToData();
			var imgFile = Path.GetTempFileName();
			Trace.WriteLine($"Generated to:{imgFile}");
			using (var stream = File.OpenWrite(imgFile))
			{
				data.SaveTo(stream);
				stream.Flush();
				stream.Close();
			}
			Bitmap image = new Bitmap(imgFile);
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
		if (file is not null)
		{
			string content = CodeEditor.Text;
			Task.Run(async () =>
			{
				using var stream = await file.OpenWriteAsync();
				stream.SetLength(0);
				using var sw = new StreamWriter(stream);
				await sw.WriteAsync(content);
				await sw.FlushAsync();
			});
		}
	}

	public void SetHost(TabHost host)
	{

	}
}