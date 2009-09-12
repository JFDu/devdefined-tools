using System.Windows;
using System.Windows.Input;
using System.Xml;
using BooDslExampleApp;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace ExampleEditor
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window
  {
    CompletionManager completionManager;

    public Window1()
    {
      InitializeComponent();
      InitializeEditor();
    }

    void InitializeEditor()
    {
      using (XmlReader reader = XmlReader.Create("Dsl.xshd"))
      {
        textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
      }

      textEditor.KeyDown += textEditor_KeyDown;

      completionManager = new CompletionManager();
    }

    void textEditor_KeyDown(object sender, KeyEventArgs e)
    {
      if (Keyboard.Modifiers != ModifierKeys.Control)
        return;

      if (e.Key != Key.Space)
        return;

      e.Handled = true;

      completionManager.ShowCompletion(textEditor.TextArea);
    }
  }
}