using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Progrart.Core;

namespace Progrart.Controls;

public partial class ExecuteArgumentsEditor : UserControl
{
	public ExecuteArgumentsEditor()
	{
		InitializeComponent();
		AddButton.Click += (_, _) =>
		{
			ItemHolder.Children.Add(new ExecuteArgumentItem());
		};
	}
	public void LoadArguments(ExecuteArguments args)
	{
		foreach (var item in args.data)
		{
			ItemHolder.Children.Add(new ExecuteArgumentItem(item.Key, item.Value));
		}
	}
}