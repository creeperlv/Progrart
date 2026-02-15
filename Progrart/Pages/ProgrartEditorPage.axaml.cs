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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Progrart;

public partial class ProgrartEditorPage : UserControl, ITabPage, IEditorPage
{
	IStorageFile? file = null;
	TabButton? btn = null;
	Bitmap? image;
	string lastSave = "";
	LayoutDirection direction = LayoutDirection.Vertical;
	LayoutMode mode = LayoutMode.Splitted;
	public ProgrartEditorPage()
	{
		InitializeComponent();
		CodeEditor.onSaveCmd = Save;
		ApplyLayout();
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
		return lastSave != CodeEditor.Text;
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
			try
			{
				using var stream = await file.OpenReadAsync();
				using StreamReader sr = new StreamReader(stream);
				var text = await sr.ReadToEndAsync();
				Dispatcher.UIThread.Invoke(() =>
				{
					try
					{
						CodeEditor.Text = text;
						lastSave = text;
					}
					catch (Exception e)
					{
						Trace.WriteLine(e);
					}
				});
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
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
		else await SaveAs();
	}

	public async Task SaveAs()
	{
		var provider = TopLevel.GetTopLevel(this)?.StorageProvider;
		if (provider is null)
		{
			return;
		}
		var file = await provider.SaveFilePickerAsync(new FilePickerSaveOptions
		{
			SuggestedStartLocation = App.CurrentOpenFolder,
			FileTypeChoices = new[]
			{
				new FilePickerFileType("Progrart File")
				{
					Patterns = new[] { "*.progrart" },
					MimeTypes = new[] { "text/plain" }
				}
			},
			Title = "Save Progrart File"
		});
		if (file is not null)
		{

			string content = "";
			await Dispatcher.UIThread.InvokeAsync(() => { content = CodeEditor.Text; });
			this.file = file;
			using var stream = await file.OpenWriteAsync();
			stream.SetLength(0);
			using var sw = new StreamWriter(stream);
			await sw.WriteAsync(content);
			await sw.FlushAsync();
		}
		return;
	}
	public void SetHost(TabHost host)
	{

	}

	private void LayoutButtonV_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (LayoutButtonV.IsChecked == true)
		{

			direction = LayoutDirection.Vertical;
			ApplyLayout();
		}
	}

	private void LayoutButtonH_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (LayoutButtonH.IsChecked == true)
		{

			direction = LayoutDirection.Horizontal;
			ApplyLayout();
		}
	}

	private void LayoutSplited_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (LayoutSplited?.IsChecked == true)
		{
			mode = LayoutMode.Splitted;
			ApplyLayout();
		}
	}

	private void LayoutCodeOnly_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (LayoutCodeOnly.IsChecked == true)
		{
			mode = LayoutMode.Code;
			ApplyLayout();
		}
	}

	private void LayoutImageOnly_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		if (LayoutImageOnly.IsChecked == true)
		{
			mode = LayoutMode.Image;
			ApplyLayout();
		}
	}
	public void ApplyLayout()
	{
		switch (mode)
		{
			case LayoutMode.Splitted:
				CodeEditor.IsVisible = true;
				PreviewHolder.IsVisible = true;
				if (direction == LayoutDirection.Vertical)
				{
					Grid.SetColumn(Splitter, 1);
					Grid.SetRow(Splitter, 0);
					Grid.SetRowSpan(Splitter, 3);
					Grid.SetColumnSpan(Splitter, 1);

					Grid.SetRow(CodeEditor, 0);
					Grid.SetColumn(CodeEditor, 0);
					Grid.SetRowSpan(CodeEditor, 3);
					Grid.SetColumnSpan(CodeEditor, 1);

					Grid.SetColumn(PreviewHolder, 2);
					Grid.SetRow(PreviewHolder, 0);
					Grid.SetRowSpan(PreviewHolder, 3);
					Grid.SetColumnSpan(PreviewHolder, 1);
					VerticalSplitterVisualElement.IsVisible = true;
					HorizontalSplitterVisualElement.IsVisible = false;
				}
				else
				{
					Grid.SetColumn(Splitter, 0);
					Grid.SetRow(Splitter, 1);
					Grid.SetRowSpan(Splitter, 1);
					Grid.SetColumnSpan(Splitter, 3);

					Grid.SetRow(CodeEditor, 0);
					Grid.SetColumn(CodeEditor, 0);
					Grid.SetRowSpan(CodeEditor, 1);
					Grid.SetColumnSpan(CodeEditor, 3);

					Grid.SetRow(PreviewHolder, 2);
					Grid.SetColumn(PreviewHolder, 0);
					Grid.SetRowSpan(PreviewHolder, 1);
					Grid.SetColumnSpan(PreviewHolder, 3);
					VerticalSplitterVisualElement.IsVisible = false;
					HorizontalSplitterVisualElement.IsVisible = true;
				}
				break;
			case LayoutMode.Code:
				CodeEditor.IsVisible = true;
				PreviewHolder.IsVisible = false;
				Grid.SetRow(CodeEditor, 0);
				Grid.SetRow(PreviewHolder, 0);
				Grid.SetColumn(CodeEditor, 0);
				Grid.SetColumn(PreviewHolder, 0);
				Grid.SetRowSpan(CodeEditor, 3);
				Grid.SetRowSpan(PreviewHolder, 3);
				Grid.SetColumnSpan(CodeEditor, 3);
				Grid.SetColumnSpan(PreviewHolder, 3);
				VerticalSplitterVisualElement.IsVisible = false;
				HorizontalSplitterVisualElement.IsVisible = false;
				break;
			case LayoutMode.Image:
				CodeEditor.IsVisible = false;
				PreviewHolder.IsVisible = true;
				Grid.SetRow(CodeEditor, 0);
				Grid.SetRow(PreviewHolder, 0);
				Grid.SetColumn(CodeEditor, 0);
				Grid.SetColumn(PreviewHolder, 0);
				Grid.SetRowSpan(CodeEditor, 3);
				Grid.SetRowSpan(PreviewHolder, 3);
				Grid.SetColumnSpan(CodeEditor, 3);
				Grid.SetColumnSpan(PreviewHolder, 3);
				VerticalSplitterVisualElement.IsVisible = false;
				HorizontalSplitterVisualElement.IsVisible = false;
				break;
		}
	}
	enum LayoutDirection
	{
		Vertical, Horizontal
	}
	enum LayoutMode
	{
		Splitted, Code, Image
	}
}