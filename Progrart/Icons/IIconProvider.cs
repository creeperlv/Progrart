using Avalonia.Controls;
using System.Diagnostics.CodeAnalysis;

namespace Progrart.Icons
{
	public interface IIconProvider
	{
		bool TryGetIcon(string name, [MaybeNullWhen(false)] out Control icon);
	}
}