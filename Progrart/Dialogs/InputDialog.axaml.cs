using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DialogHostAvalonia;
using System;
using System.Threading.Tasks;

namespace Progrart.Dialogs;

public partial class InputDialog : UserControl
{
	public Func<string, Task<bool>>? onOK = null;
	public Func<bool>? onCancel = null;
	public InputDialog()
	{
		InitializeComponent();
		OKBtn.Click += OKBtn_Click;
		CancelBtn.Click += CancelBtn_Click;
		InputBox.AddHandler(KeyDownEvent, async (sender, e) =>
		{
            switch (e.Key)
            {
                case Avalonia.Input.Key.Enter:
                    await Confirmed();
                    break;
                case Avalonia.Input.Key.Escape:
                    Cancel();
                    break;
            }
        }, Avalonia.Interactivity.RoutingStrategies.Tunnel);
	}
	public void SetDialogContent(string title, string content)
	{
		TitleBlock.Text = title;
		MessageBlock.Text = content;
	}
	public void SetErrorMessage(string msg)
	{
		InputBox.BorderBrush = new SolidColorBrush(Colors.Red);
		ErrorBlock.Text = msg;
		ErrorBlock.IsVisible = true;
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
			if (!await onOK(InputBox.Text ?? ""))
			{
				DialogHost.Close(null);
			}
		}
		else
			DialogHost.Close(null);
	}
}