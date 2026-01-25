using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;
using System;
using System.Threading.Tasks;

namespace Progrart.Dialogs;

public partial class CloseConfirmationDialog : UserControl
{
	public Func<Task<bool>>? onOK = null;
	public Func<Task<bool>>? onDiscard = null;
	public Func<bool>? onCancel = null;
	public CloseConfirmationDialog()
	{
		InitializeComponent();
		OKBtn.Click += OKBtn_Click;
		CancelBtn.Click += CancelBtn_Click;
		DiscardBtn.Click += async (sender, e) => await Discard();
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
		if (onOK is not null)
		{
			if (!await onOK())
			{
				DialogHost.Close(null);
			}
		}
		else
			DialogHost.Close(null);
	}
	private async Task Discard()
	{
		if (onDiscard is not null)
		{
			if (!await onDiscard())
			{
				DialogHost.Close(null);
			}
		}
		else
			DialogHost.Close(null);
	}
}