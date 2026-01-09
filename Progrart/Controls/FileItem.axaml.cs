using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DialogHostAvalonia;
using Progrart.Controls.TabSystem;
using Progrart.Dialogs;
using Progrart.Icons;
using Progrart.Pages;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Progrart.Controls;

public partial class FileItem : UserControl
{
	IStorageItem? currentItem;
	bool isOpen = false;
	string extension = "";
	public FileItem()
	{

		InitializeComponent();
	}
	public FileItem(IStorageItem storageItem)
	{
		InitializeComponent();
		currentItem = storageItem;
		if (storageItem is IStorageFolder folder)
		{
			FolderIcon.IsVisible = true;
			GenericFileIcon.IsVisible = false;
			IconContainer.IsVisible = false;
		}
		else if (storageItem is IStorageFile file)
		{
			NewFiles.IsVisible = false;
			FolderIcon.IsVisible = false;
			GenericFileIcon.IsVisible = true;
			IconContainer.Children.Clear();
			var dotIndex = file.Name.LastIndexOf('.');
			if (dotIndex >= 0)
			{
				extension = file.Name[(dotIndex + 1)..];
				if (IconProvider.TryGetIcon(extension, out var icon))
				{
					GenericFileIcon.IsVisible = false;
					IconContainer.Children.Add(icon);
				}
			}
			IconContainer.IsVisible = true;
		}
		NameBlock.Text = storageItem.Name;
		MainButton.DoubleTapped += async (_, _) =>
		{
			OpenItem();
		};
		OpenFileMenuItem.Click += async (a, b) =>
		{
			OpenItem();
		};
		CreateFolderItem.Click += async (a, b) =>
		{
			InputDialog content = new();
			content.SetDialogContent("Create New Folder", "Specify a name that is legal for your filesystem.");
			content.onOK = async (v) =>
			{
				if (currentItem is IStorageFolder folder)
				{
					try
					{
						var fldr = await folder.CreateFolderAsync(v);
						if (fldr is not null)
						{
							if (isOpen)
								Dispatcher.UIThread.Invoke(() =>
								{
									var fitem = new FileItem(fldr);
									ItemContainer.Children.Add(fitem);
								});
						}
						else
						{
							content.SetErrorMessage($"{v} is not a valid name for target filesystem.");
							return true;
						}
					}
					catch (System.Exception e)
					{
						content.SetErrorMessage($"{v} is not a valid name for target filesystem.\nMsg:{e.Message}");
						return true;
					}
				}
				return false;
			};
			await DialogHost.Show(content);
		};
		CreateProgrartItem.Click += async (a, b) =>
		{
			InputDialog content = new();
			content.SetDialogContent("Create A PROGRART Image", "Specify a name that is legal for your filesystem.");
			content.onOK = async (v) =>
			{
				if (currentItem is IStorageFolder folder)
				{
					try
					{
						var fldr = await folder.CreateFileAsync($"{v}.progrart");
						if (fldr is not null)
						{
							if (isOpen)
								Dispatcher.UIThread.Invoke(() =>
								{
									var fitem = new FileItem(fldr);
									ItemContainer.Children.Add(fitem);
								});
						}
						else
						{
							content.SetErrorMessage($"{v} is not a valid name for target filesystem.");
							return true;
						}
					}
					catch (System.Exception e)
					{
						content.SetErrorMessage($"{v} is not a valid name for target filesystem.\nMsg:{e.Message}");
						return true;
					}
				}
				return false;
			};
			await DialogHost.Show(content);
		};
	}
	void OpenItem()
	{

		if (currentItem is IStorageFolder folder)
		{
			if (isOpen)
			{
				RemoveAll();
				isOpen = false;
			}
			else
			{
				isOpen = true;
				Task.Run(async () =>
				{
					await LoadAll(folder);
				});
			}
		}
		else
			if (currentItem is IStorageFile file)
			{
				var btn = EditorProvider.IsOpen(file);
				if (btn is not null)
				{
					EditorProvider.SelectTabPage(btn);
				}
				else
					if (EditorProvider.TryGetEditor(extension, out var page))
					{
						if (page is ITabPage editor)
						{
							try
							{
								if (page is IEditorPage editor_page)
								{
									editor_page.LoadDocument(file);
								}
								EditorProvider.OpenEditor(editor);
							}
							catch (System.Exception e)
							{
								Trace.WriteLine(e);
							}
						}
					}
			}
	}
	async Task LoadAll(IStorageFolder folder)
	{
		await foreach (var item in folder.GetItemsAsync())
		{
			Dispatcher.UIThread.Invoke(() =>
			{
				var fitem = new FileItem(item);
				ItemContainer.Children.Add(fitem);
			});
		}
	}
	void RemoveAll()
	{
		ItemContainer.Children.Clear();
	}
}