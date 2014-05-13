using BasicModelInterface;
using DocoptNet;

namespace BasicModelInterfaceRunner
{
    static class Program
    {
        private const string usage = @"Run a BMI model
    Usage:
        bmi-runner <engine> <config>

    Positional arguments:
        engine      model engine name, this is either name of the library (e.g. model1) or full path to the BMI library (/usr/lib/libmodel1.so.5 or C:\opt\model1.dll)
        config      model config file, used to initialize model

    Options:
        -h, --help  show this help message and exit
    ";

        static void Main(string[] args)
        {
            var version = typeof(Program).Assembly.GetName().Version;
            var arguments = new Docopt().Apply(usage, args, version: string.Format("bmi_runner {0}", version), exit: true);

            var bmiLibrary = arguments["<engine>"].ToString();
            var initializePath = arguments["<config>"].ToString();

            BasicModelInterfaceLibrary.Run(bmiLibrary, initializePath);
        }

    }
}
