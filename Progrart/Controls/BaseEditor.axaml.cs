using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace Progrart.Controls;

public partial class BaseEditor : UserControl
{
	RegistryOptions _registryOptions;
	TextMate.Installation _textMateInstallation;
	public string Text
	{
		get => CodeEditBox.Text;
		set => CodeEditBox.Text = value;
	}
	public BaseEditor()
	{
		InitializeComponent();
		{
			var _textEditor = CodeEditBox;
			_registryOptions = new RegistryOptions(ThemeName.DarkPlus);
			_textMateInstallation = _textEditor.InstallTextMate(_registryOptions);
			_textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".js").Id));
		}
	}
	public void SetGrammerByExtension(string extension_name)
	{
		_textMateInstallation.SetGrammar(
			_registryOptions.GetScopeByLanguageId(
				_registryOptions.GetLanguageByExtension(extension_name).Id
				)
			);

	}
}