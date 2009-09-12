namespace BooDslExampleApp.QuoteGeneration
{
  public class RequirementsInformation
  {
    public RequirementsInformation(int userCount, params string[] requestedModules)
    {
      UserCount = userCount;
      RequestedModules = requestedModules;
    }

    public int UserCount { get; set; }

    public string[] RequestedModules { get; set; }
  }
}