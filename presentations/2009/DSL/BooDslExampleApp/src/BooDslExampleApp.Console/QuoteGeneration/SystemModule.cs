using System.Collections.Generic;

namespace BooDslExampleApp.QuoteGeneration
{
  public class SystemModule
  {
    readonly string name;
    List<string> _onSameMachineWith = new List<string>();
    List<string> _requirements = new List<string>();

    public SystemModule(string name)
    {
      this.name = name;
    }

    public int UsersPerMachine { get; set; }
    public int MinMemory { get; set; }
    public int MinCpuCount { get; set; }

    public string Name
    {
      get { return name; }
    }

    public List<string> Requirements
    {
      get { return _requirements; }
      set { _requirements = value; }
    }

    public List<string> OnSameMachineWith
    {
      get { return _onSameMachineWith; }
      set { _onSameMachineWith = value; }
    }
  }
}