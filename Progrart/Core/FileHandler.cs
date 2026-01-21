using Avalonia.Platform.Storage;
using Progrart.Controls.TabSystem;
using Progrart.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Progrart.Core
{
	public class FileHandler
	{
		public string Name = "";
		public Func<IStorageFile,Task>? OpenAction = null;

		public FileHandler(string name, Func<IStorageFile, Task>? openAction)
		{
			Name = name;
			OpenAction = openAction;
		}
	}
	public class EditorFileHandler<T> : FileHandler where T : IEditorPage, ITabPage
	{
		public EditorFileHandler(string name) : base(name, async (item) =>
		{
			if (item is IStorageFile file)
			{
				var page = (T)Activator.CreateInstance(typeof(T))!;
				page.LoadDocument(file);
				EditorProvider.OpenEditor(page);
			}
		})
		{
		}
	}
}
