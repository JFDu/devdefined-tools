using System;
using System.Collections.Generic;
using System.IO;
using BooDslExampleApp.QuoteGeneration;

namespace BooDslExampleApp
{
  internal class Program
  {
    static void Main(string[] args)
    {
      string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.boo");

      // user needs the vacation module of the application, and has 200 users total.

      var info = new RequirementsInformation(200, "vacations");

      Console.WriteLine("Requesting quote for module \"vacations\" with 200 users\r\n");

      List<SystemModule> modules = QuoteGenerator.Generate(url, info);     

      DisplayModuleRequirements(modules);

      Console.WriteLine("\r\nPress enter to exit...");

      Console.ReadLine();
    }

    static void DisplayModuleRequirements(IEnumerable<SystemModule> modules)
    {
      foreach (SystemModule module in modules)
      {
        Console.WriteLine("Required module {0}", module.Name);

        if (module.Requirements.Count > 0)
        {
          foreach (string requirement in module.Requirements)
          {
            Console.WriteLine("\tRequires {0}", requirement);
          }
        }
        if (module.UsersPerMachine != 0)
        {
          Console.WriteLine("\tUsers per machine {0}", module.UsersPerMachine);
        }
        if (module.MinMemory != 0)
        {
          Console.WriteLine("\tServer memory required {0}mb", module.MinMemory);
        }
        if (module.MinCpuCount != 0)
        {
          Console.WriteLine("\tServer CPU count required {0}", module.MinCpuCount);
        }
        if (module.OnSameMachineWith.Count > 0)
        {
          foreach (string requirement in module.OnSameMachineWith)
          {
            Console.WriteLine("\tOn same machine as {0}", requirement);
          }
        }
      }
    }
  }
}