using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;
using System;
using System.Threading.Tasks;

namespace Progrart.Dialogs;

public partial class ConfirmationDialog : UserControl
{
	public Func<Task<bool>>? onOK = null;
	public Func<bool>? onCancel = null;
	public ConfirmationDialog()
	{
		InitializeComponent();
		SetupEvents();
	}
	public ConfirmationDialog(string title,string message)
	{
		InitializeComponent();
		SetupEvents();
		this.TitleBlock.Text = title;
		this.MessageBlock.Text = message;
	}
	void SetupEvents()
	{
		OKBtn.Click += OKBtn_Click;
		CancelBtn.Click += CancelBtn_Click;
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

	private async void OKBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		await Confirmed();
	}
}