using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using System;
using System.Windows.Input;
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
    public Action? onSaveCmd;
    public BaseEditor()
    {
        InitializeComponent();
        {
            var _textEditor = CodeEditBox;
            _registryOptions = new RegistryOptions(ThemeName.DarkPlus);
            _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);
            _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".js").Id));
        }
        var saveBinding = new KeyBinding
        {
            Gesture = new KeyGesture(Key.S, KeyModifiers.Control),
            Command = new saveCmd() { onSave = Save }
        };

        CodeEditBox.KeyBindings.Add(saveBinding);
    }
    void Save()
    {
        onSaveCmd?.Invoke();
    }
    class saveCmd : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public Action? onSave;
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            onSave?.Invoke();
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

    private void Copy_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CodeEditBox.Copy();
    }

    private void Paste_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CodeEditBox.Paste();
    }

    private void Cut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CodeEditBox.Cut();
    }
}