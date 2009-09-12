using System;
using System.Collections.Generic;
using System.Linq;

namespace BooDslExampleApp.QuoteGeneration
{
  public abstract class QuoteGeneratorRule
  {
    SystemModule currentModule;
    RequirementsInformation information;

    public List<SystemModule> Modules = new List<SystemModule>();

    protected QuoteGeneratorRule(RequirementsInformation information)
    {
      this.information = information;
    }

    public int total_users
    {
      get { return information.UserCount; }
    }

    public void specification(string moduleName, Action action)
    {
      if (Array.IndexOf(information.RequestedModules, moduleName) == -1 &&
          Modules.Any(module => module.Requirements.Contains(moduleName)) == false)
        return;

      currentModule = new SystemModule(moduleName);
      Modules.Add(currentModule);
      action();
    }

    public void requires(string moduleName)
    {
      currentModule.Requirements.Add(moduleName);
    }

    public void users_per_machine(int count)
    {
      currentModule.UsersPerMachine = count;
    }

    public void same_machine_as(string moduleName)
    {
      currentModule.OnSameMachineWith.Add(moduleName);
    }

    public void min_memory(int minMemory)
    {
      currentModule.MinMemory = minMemory;
    }

    public void min_cpu_count(int minCpuCount)
    {
      currentModule.MinCpuCount = minCpuCount;
    }

    public abstract void Evaluate();
  }
}