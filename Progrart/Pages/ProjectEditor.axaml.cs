using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Progrart.Controls;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using System;
using System.Threading.Tasks;

namespace Progrart.Pages;

public partial class ProjectEditor : UserControl, ITabPage, IEditorPage
{
	public ProjectEditor()
	{
		InitializeComponent();
		AddButton.Click += (_, _) =>
		{
			ConfigurationHolder.Children.Add(new ConfigEditor());
		};
	}

	public void Execute(ExecuteArguments? args = null)
	{
	}

	public bool IsSameFile(IStorageFile file)
	{
		return (this.file?.Path == file.Path);
	}
	IStorageFile? file = null;
	public void LoadDocument(IStorageFile file)
	{
		this.file = file;
	}

	public async Task Save()
	{
	}

	public void SetHost(TabHost host)
	{
	}

	public void BindButton(TabButton button)
	{
		button.Title = "Project";
	}

	public bool IsModified()
	{
		return true;
	}
}