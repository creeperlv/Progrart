using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Progrart.Controls.TabSystem;
using Progrart.Icons;
using Progrart.Pages;
using System.Threading.Tasks;

namespace Progrart.Controls;

public partial class FileItem : UserControl
{
	IStorageItem currentItem;
	bool isOpen = false;
	string extension = "";
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
			await OpenItem();
		};
		OpenFileMenuItem.Click += async (a, b) =>
		{
			await OpenItem();
		};
	}
	async Task OpenItem()
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
				await LoadAll(folder);
			}
		}
		else
			if (currentItem is IStorageFile file)
			{

				if (EditorProvider.TryGetEditor(extension, out var page))
				{
					if (page is ITabPage editor)
					{
						if (page is IEditorPage editor_page)
						{
							editor_page.LoadDocument(file);
						}
						EditorProvider.OpenEditor(editor);
					}
				}
			}
	}
	async Task LoadAll(IStorageFolder folder)
	{
		await foreach (var item in folder.GetItemsAsync())
		{
			ItemContainer.Children.Add(new FileItem(item));
		}
	}
	void RemoveAll()
	{
		ItemContainer.Children.Clear();
	}
}