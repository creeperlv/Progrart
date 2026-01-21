using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Progrart.Controls.TabSystem;
using Progrart.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progrart.Pages
{
	static public class EditorProvider
	{
		static Dictionary<string, FileHandler> handlers = new();
		static Dictionary<string, List<string>> ExtensionMapping = new();
		//static Dictionary<string, Type> EditorTypes = new();
		static TabHost? currentHost = null;
		public static void setHost(TabHost host)
		{
			currentHost = host;
		}
		public static void Register(string id, FileHandler handler)
		{
			handlers[id] = handler;
		}
		public static void BindFileType(string type, string id)
		{
			if (ExtensionMapping.TryGetValue(type, out var list))
			{
				list.Add(id);
			}
			else
			{
				ExtensionMapping[type] = [id];
			}
		}
		public static TabButton? IsOpen(IStorageFile file)
		{
			return currentHost?.IsOpen(file);
		}
		public static void SelectTabPage(TabButton button)
		{
			currentHost?.SelectButton(button);
		}
		public static void OpenEditor(ITabPage page)
		{
			currentHost?.AddPage(page);
		}

		public static bool TryGetHandler(string extension, [MaybeNullWhen(false)] out FileHandler handler)
		{
			if (ExtensionMapping.TryGetValue(extension.ToLower(), out var id))
			{

				if (handlers.TryGetValue(id.First(), out var editorType))
				{
					handler = editorType;
					return true;
				}
			}
			handler = null;
			return false;
		}
		public static List<FileHandler> GetHandlerCollection(string extension)
		{
			List<FileHandler> handlersList = new();
			if (ExtensionMapping.TryGetValue(extension.ToLower(), out var id))
			{
				foreach (var item in id)
				{
					handlersList.Add(handlers[item]);
				}
			}
			return handlersList;
		}
	}
	public interface IEditorPage
	{
		void LoadDocument(IStorageFile file);
		Task Save();
		bool IsSameFile(IStorageFile file);
		void Execute(ExecuteArguments? args = null);
	}
}