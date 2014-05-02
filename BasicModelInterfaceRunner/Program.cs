using System;
using NDesk.Options;

namespace BasicModelInterfaceRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = null;
            var help = false;
            string bmiLibrary = null;
            string initializePath = null;
            
            var optionSet = new OptionSet()
              .Add("h|?|help", delegate(string v) { help = v != null; })
              .Add("l|bmi-library=", delegate(string v) { bmiLibrary = v; })
              .Add("i|initialize-path=", delegate(string v) { initializePath = v; });

            try
            {
                optionSet.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (help || bmiLibrary == null || initializePath == null)
            {
                optionSet.WriteOptionDescriptions(Console.Out);
                return;
            }

            RunModel(bmiLibrary, initializePath);
        }

        private static void RunModel(string bmiLibrary, string configPath)
        {
            var library = new BasicModelInterface.BasicModelInterfaceLibrary(bmiLibrary);

            library.Initialize(configPath);
            library.Update();
            library.Finish();
        }
    }
}
