using System.Collections.Generic;
using Rhino.DSL;

namespace BooDslExampleApp.QuoteGeneration
{
  public static class QuoteGenerator
  {
    private static readonly DslFactory dslFactory;

    static QuoteGenerator()
    {
      dslFactory = new DslFactory();
      dslFactory.Register<QuoteGeneratorRule>(new QuoteGenerationDslEngine());
    }

    public static List<SystemModule> Generate(string url, RequirementsInformation parameters)
    {
      QuoteGeneratorRule rule = dslFactory.Create<QuoteGeneratorRule>(url, parameters);
      rule.Evaluate();
      return rule.Modules;
    }
  }
}