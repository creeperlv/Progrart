using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using Progrart.Core.JSExecution;
using Progrart.Core.Storage;
using Progrart.Pages;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Progrart;

public partial class ProgrartEditorPage : UserControl, ITabPage, IEditorPage
{
	IStorageFile? file = null;
	TabButton? btn = null;
	Bitmap? image;
	string lastSave = "";
	public ProgrartEditorPage()
	{
		InitializeComponent();
		CodeEditor.onSaveCmd = Save;
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
	bool isRunning = false;
	public void Execute(ExecuteArguments? args = null)
	{
		if (isRunning) return;
		string src = CodeEditor.Text;
		ProgressRing.IsVisible = true;
		Task.Run(() =>
		{
			AvaloniaStorageProvider provider = new AvaloniaStorageProvider(App.CurrentOpenFolder);
			using ProgrartExecutor executor = new ProgrartExecutor(provider);
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
				var result = executor.RenderImage(Scale, src, args ?? new ExecuteArguments());
				var data = result.DrawingCore.ToData();
				using MemoryStream stream = new MemoryStream();
				data.SaveTo(stream);
				stream.Flush();
				stream.Position = 0;
				Bitmap image = new Bitmap(stream);
				Dispatcher.UIThread.Invoke(() =>
				{
					ProgressRing.IsVisible = false;
					PreviewImage.SetImage(image);
					this.image?.Dispose();
					this.image = image;
					isRunning = false;
				});
			}
			catch (System.Exception e)
			{
				Trace.WriteLine(e);
				Dispatcher.UIThread.Invoke(() =>
				{
					ProgressRing.IsVisible = false;
				});
				isRunning = false;
			}
		});
	}

	public bool IsModified()
	{
		return lastSave == CodeEditor.Text;
	}

	public bool IsSameFile(IStorageFile file)
	{
		return (this.file?.Path == file.Path);
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
		Trace.WriteLine($"File:{file.TryGetLocalPath() ?? "null"}");
		Task.Run(async () =>
		{
			using var stream = await file.OpenReadAsync();
			using StreamReader sr = new StreamReader(stream);
			var text = await sr.ReadToEndAsync();
			Dispatcher.UIThread.Invoke(() =>
			{
				CodeEditor.Text = text;
				lastSave = text;
			});
		});
	}

	public async Task Save()
	{
		if (file is not null)
		{
			string content = "";
			await Dispatcher.UIThread.InvokeAsync(() => { content = CodeEditor.Text; });
			lastSave = content;
			await Task.Run(async () =>
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