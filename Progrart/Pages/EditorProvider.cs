using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Progrart.Controls.TabSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Progrart.Pages
{
	static public class EditorProvider
	{
		static Dictionary<string, string> Names = new();
		static Dictionary<string, string> ExtensionMapping = new();
		static Dictionary<string, Type> EditorTypes = new();
		static TabHost? currentHost = null;
		public static void setHost(TabHost host)
		{
			currentHost = host;
		}
		public static void Register(string id, string name, Type t)
		{
			Names[id] = name;
			EditorTypes[id] = t;
		}
		public static void BindFileType(string type, string id)
		{
			ExtensionMapping[type] = id;
		}
		public static void OpenEditor(ITabPage page)
		{
			currentHost?.AddPage(page);
		}
		public static bool TryGetEditor(string extension, [MaybeNullWhen(false)] out Control editorPage)
		{
			if (ExtensionMapping.TryGetValue(extension.ToLower(), out var id))
			{

				if (EditorTypes.TryGetValue(id, out var editorType))
				{
					var obj = Activator.CreateInstance(editorType);
					if (obj is Control control)
					{
						editorPage = control;
						return true;
					}
				}
			}
			editorPage = null;
			return false;
		}
	}
	public interface IEditorPage
	{
		void LoadDocument(IStorageFile file);
		void Save();
		void Execute();
	}
}