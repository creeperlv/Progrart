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
	public void Clear()
	{
		ItemHolder.Children.Clear();
	}
	public ExecuteArguments ObtainArguments()
	{
		ExecuteArguments args = new ExecuteArguments();

		foreach (var item in ItemHolder.Children)
		{
			if (item is ExecuteArgumentItem eaItem)
			{
				var (key, value) = eaItem.GetItem();
				args.data[key] = value;
			}
		}

		return args;
	}
}