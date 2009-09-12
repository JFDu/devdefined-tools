using System.Collections.Generic;
using System.Linq;
using ExampleEditor;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;

namespace BooDslExampleApp
{
  public class CompletionManager
  {
    public void ShowCompletion(TextArea area)
    {
      var window = new CompletionWindow(area);

      IList<ICompletionData> data = window.CompletionList.CompletionData;

      foreach (CompletionData item in GenerateCompletionData("test.boo", area))
      {
        data.Add(item);
      }

      window.Show();
    }

    public CompletionData[] GenerateCompletionData(string fileName, TextArea textArea)
    {
      string name = WordUtilities.FindPreviousWord(textArea);

      if (name == null)
        return EmptySuggestion(textArea.Caret);

      if (name == "specification" || name == "requires" || name == "same_machine_as" || name == "@")
      {
        return ModulesSuggestions();
      }

      int temp;
      if (name == "users_per_machine" || int.TryParse(name, out temp))
      {
        return NumbersSuggestions();
      }

      return EmptySuggestion(textArea.Caret);
    }

    CompletionData[] NumbersSuggestions()
    {
      return new[]
               {
                 new CompletionData {Text = "50"},
                 new CompletionData {Text = "100"},
                 new CompletionData {Text = "150"}
               };
    }

    CompletionData[] ModulesSuggestions()
    {
      return new[] {"@vacations", "@external_connections", "@salary", "@pension", "@scheduling_work", "@health_insurance", "@taxes"}
        .Select(item => new CompletionData {Text = item}).ToArray();
    }

    CompletionData[] EmptySuggestion(Caret caret)
    {
      if (caret.Column == 1)
      {
        return new[]
                 {new CompletionData {Text = "specification", DescriptionText = "Define a specification for a specific module", AddSpaceAfterInsertion = true}}
          ;
      }
      return new[]
               {
                 new CompletionData {Text = "requires", DescriptionText = "Requires a particular module", AddSpaceAfterInsertion = true},
                 new CompletionData {Text = "same_machine_as", DescriptionText = "Needs to be on the same machine as the specified module", AddSpaceAfterInsertion = true},
                 new CompletionData {Text = "users_per_machine", DescriptionText = "Can support the specified amount of users", AddSpaceAfterInsertion = true}
               };
    }
  }
}