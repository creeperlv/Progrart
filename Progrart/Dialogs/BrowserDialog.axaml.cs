using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using DialogHostAvalonia;
using Progrart.Controls;
using System;
using System.Threading.Tasks;

namespace Progrart.Dialogs;

public partial class BrowserDialog : UserControl
{
	public Func<IStorageItem, Task<bool>>? onOK = null;
	public Func<bool>? onCancel = null;
	public string Title
	{
		get => DialogTitle.Text ?? "";
		set => DialogTitle.Text = value;
	}
	public BrowserDialog()
	{
		InitializeComponent();
		OKBtn.Click += OKBtn_Click;
		CancelBtn.Click += CancelBtn_Click;
	}
	IStorageItem? currentItem = null;
	public void SetFileITem(FileItem item)
	{
		FileRoot.Children.Add(item);
		item.onSelected = async (it) =>
		{
			currentItem = it.CurrentItem;
		};
	}
	private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		Cancel();
	}

	private void Cancel()
	{
		if (!(onCancel?.Invoke()) ?? true)
		{
			DialogHost.Close(null);
		}
	}

	private async void OKBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		await Confirmed();
	}

	private async Task Confirmed()
	{
		if (currentItem is null) return;
		if (onOK is not null)
		{
			if (!await onOK(currentItem))
			{
				DialogHost.Close(null);
			}
		}
		else
			DialogHost.Close(null);
	}
}