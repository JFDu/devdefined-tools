using System.Windows.Documents;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ExampleEditor
{
  public static class WordUtilities
  {
    public static string FindPreviousWord(TextArea textArea)
    {
      int startOfWord = TextUtilities.GetNextCaretPosition(textArea.Document, textArea.Caret.Offset, LogicalDirection.Backward, CaretPositioningMode.WordStart);

      int endOfWord = TextUtilities.GetNextCaretPosition(textArea.Document, textArea.Caret.Offset, LogicalDirection.Backward,
                                                         CaretPositioningMode.WordBorder);

      if (endOfWord <= startOfWord) return null;

      return textArea.Document.GetText(startOfWord, endOfWord - startOfWord);
    }
  }
}