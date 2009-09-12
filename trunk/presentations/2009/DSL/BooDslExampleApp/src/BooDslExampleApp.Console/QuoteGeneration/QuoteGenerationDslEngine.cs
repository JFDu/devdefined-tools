using Boo.Lang.Compiler;
using Rhino.DSL;

namespace BooDslExampleApp.QuoteGeneration
{
  public class QuoteGenerationDslEngine : DslEngine
  {
    protected override void CustomizeCompiler(
      BooCompiler compiler,
      CompilerPipeline pipeline,
      string[] urls)
    {
      pipeline.Insert(1,
                      new ImplicitBaseClassCompilerStep(
                        typeof(QuoteGeneratorRule),
                        "Evaluate",
                        "BooDslExampleApp.QuoteGeneration"));
      pipeline.Insert(2, new UseSymbolsStep());
    }
  }
}