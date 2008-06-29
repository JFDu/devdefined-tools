using System;
using CommandLine;

namespace HeatSite
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("HeatSite version: {0}", typeof (Program).Assembly.GetName().Version);

            var distiller = new SiteDistiller();

            if (Parser.ParseArgumentsWithUsage(args, distiller) == false)
            {
                Environment.Exit(2);
            }

            distiller.Execute();
        }
    }
}