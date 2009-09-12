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

      if (name == "users_per_machine")
      {
        return NumbersSuggestions(50,100,150,200);
      }
      else if (name == "min_memory")
      {
          return NumbersSuggestions(1024,2048,4096,8*1024);
      }
      else if (name == "min_cpu_count")
      {
          return NumbersSuggestions(1, 2, 4, 8, 16);
      }

      return EmptySuggestion(textArea.Caret);
    }

    CompletionData[] NumbersSuggestions(params int[] numbers)
    {
        return numbers.Select(number => new CompletionData { Text = number.ToString() }).ToArray();
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
                 new CompletionData {Text = "users_per_machine", DescriptionText = "Can support the specified amount of users", AddSpaceAfterInsertion = true},
                 new CompletionData {Text = "min_cpu_count", DescriptionText = "Minimum number of cpus required", AddSpaceAfterInsertion = true},
                     new CompletionData {Text = "min_memory", DescriptionText = "Minimum ammount of memory required", AddSpaceAfterInsertion = true}
               };
    }
  }
}