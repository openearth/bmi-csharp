using System;

namespace BasicModelInterface
{
    /// <summary>
    /// C# version of the Basic Model Interface. API implemented in <see cref="BasicModelInterfaceLibrary"/>.
    /// For C/C++ version of the API see https://github.com/openearth/bmi/blob/master/models/include/bmi.h
    /// </summary>
    public interface IBasicModelInterface
    {
        DateTime StartTime { get; }

        DateTime StopTime { get; }

        DateTime CurrentTime { get; }

        TimeSpan TimeStep { get; }
        
        string[] VariableNames { get; }

        int Initialize(string path);

        int Update(double dt);

        int Finish();

        Array GetValues(string variable);

        void SetValues(string variable, Array values);

        void SetValues(string variable, int[] start, int[] count, Array values);

        Logger Logger { get; set; }
    }
}