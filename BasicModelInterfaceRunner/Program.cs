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
            string configPath = null;
            
            var optionSet = new OptionSet()
              .Add("h|?|help", delegate(string v) { help = v != null; })
              .Add("d|bmi_library=", delegate(string v) { bmiLibrary = v; })
              .Add("c|config_path=", delegate(string v) { configPath = v; });

            try
            {
                optionSet.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (help || bmiLibrary == null || configPath == null)
            {
                optionSet.WriteOptionDescriptions(Console.Out);
                return;
            }

            RunModel(bmiLibrary, configPath);
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
