using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ExampleEditor
{
  public class CompletionData : ICompletionData
  {
    string _description;

    public bool AddSpaceAfterInsertion { get; set; }

    public string DescriptionText
    {
      get
      {
        if (_description == null) return Text;
        return _description;
      }
      set { _description = value; }
    }

    #region ICompletionData Members

    public ImageSource Image
    {
      get { return null; }
    }

    public string Text { get; set; }

    public object Content
    {
      get { return Text; }
    }

    public object Description
    {
      get { return DescriptionText; }
    }

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
      if (AddSpaceAfterInsertion)
      {
        textArea.Document.Replace(completionSegment, Text + " ");
      }
      else
      {
        textArea.Document.Replace(completionSegment, Text);
      }
    }

    #endregion
  }
}