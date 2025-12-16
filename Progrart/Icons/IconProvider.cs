using Avalonia.Controls;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Progrart.Icons
{
	public static class IconProvider
	{
		static List<IIconProvider> providers = new List<IIconProvider>();
		public static void Register(IIconProvider provider)
		{
			providers.Add(provider);
		}
		public static bool TryGetIcon(string name, [MaybeNullWhen(false)] out Control icon)
		{
			foreach (var provider in providers)
			{
				if (provider.TryGetIcon(name, out icon)) return true;
			}
			icon = null;
			return false;
		}
	}
}