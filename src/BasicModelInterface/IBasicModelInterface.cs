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

        TimeSpan TimeStep { get; }
        
        /// <summary>
        /// Initializes model engine.
        /// </summary>
        /// <param name="path">Configuration file path of the model, connection string, or any other path used to initialize model.</param>
        void Initialize(string path);

        void Update(double dt);

        void Finish();

        string[] VariableNames { get; }

        Array GetValues(string variable);

        void SetValues(string variable, Array values);

        void SetValues(string variable, int[] start, int[] count, Array values);

        Logger Logger { get; set; }
    }
}