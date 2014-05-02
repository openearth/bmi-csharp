using System;

namespace BasicModelInterface
{
    /// <summary>
    ///     C#-friendly version of Basic Model Interface.
    /// </summary>
    public interface IBasicModelInterface
    {
        DateTime StartTime { get; }

        DateTime StopTime { get; }

        DateTime CurrentTime { get; }

        TimeSpan TimeStep { get; set; }
        
        /// <summary>
        /// Initializes model engine.
        /// </summary>
        /// <param name="path">Configuration file path of the model, connection string, or any other path used to initialize model.</param>
        void Initialize(string path);

        void Update();

        void Finish();

        string[] VariableNames { get; }

        int[] GetIntValues1D(string variable);

        int[,] GetIntValues2D(string variable);

        double[] GetDoubleValues1D(string variable);

        void SetDoubleValue1DAtIndex(string variable, int index, double value);
    }
}