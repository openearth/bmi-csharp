using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BasicModelInterface
{
    /// <summary>
    ///     Loads <paramref name="libraryPath" /> as a dynamic library and redirects all <see cref="IBasicModelInterface" />.
    /// </summary>
    public class BasicModelInterfaceLibrary : IBasicModelInterface
    {
        private const int MAXDIMS = 6;

        private const int MAXSTRLEN = 1024;

        private readonly dynamic lib;
        private string originalCurrentDirectory;

        private string[] variableNames;
        
        private Logger logger;

        public BasicModelInterfaceLibrary(string libraryPath, CallingConvention callingConvention = CallingConvention.Cdecl)
        {
            lib = new DynamicDllImport(libraryPath, CharSet.Ansi, callingConvention);
        }

        public DateTime StartTime
        {
            get
            {
                double t = 0.0;
                lib.get_start_time(ref t);
                return new DateTime().AddSeconds(t);
            }
        }

        public DateTime StopTime
        {
            get
            {
                double t = 0.0;
                lib.get_end_time(ref t);
                return new DateTime().AddSeconds(t);
            }
        }

        public DateTime CurrentTime
        {
            get
            {
                double t = 0.0;
                lib.get_current_time(ref t);
                return new DateTime().AddSeconds(t);
            }
        }

        public TimeSpan TimeStep { get; private set; }

        public Logger Logger
        {
            get
            {
                return logger;
            }
            set
            {
                logger = value;

                lib.set_logger(logger);
            }
        }

        public void Initialize(string path)
        {
            originalCurrentDirectory = Directory.GetCurrentDirectory();

            var configFile = path;

            if (!string.IsNullOrEmpty(path))
            {
                configFile = Path.GetFileName(path);

                configFile = configFile.PadRight(MAXSTRLEN, '\0'); // make FORTRAN friendly

                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.SetCurrentDirectory(dir);
                }
            }

            try
            {
                lib.initialize(configFile);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalCurrentDirectory);
            }
        }

        public void Update(double timeStep)
        {
            lib.update(timeStep);
        }

        public void Finish()
        {
            lib.finalize();
        }

        public string[] VariableNames
        {
            get
            {
                if (variableNames == null)
                {
                    GetVariableNames();
                }

                return variableNames;
            }
        }

        public Array GetValues(string variable)
        {
            Trace.Assert(!string.IsNullOrEmpty(variable));

            // get values (pointer)
            IntPtr ptr = IntPtr.Zero;
            lib.get_var(variable, ref ptr);

            // get rank
            int rank = 0;
            lib.get_var_rank(variable, ref rank);

            // get shape
            var shape = new int[MAXDIMS];
            lib.get_var_shape(variable, shape);
            shape = shape.Take(rank).ToArray();

            // get value type
            var typeNameBuilder = new StringBuilder(MAXSTRLEN);
            lib.get_var_type(variable, typeNameBuilder);
            string typeName = typeNameBuilder.ToString();

            // copy to 1D array
            int totalLength = GetTotalLength(shape);
            Array values1D = ToArray1D(ptr, typeName, totalLength);

            if (rank == 1)
            {
                return values1D;
            }

            // convert to nD array (unfotrunately can't copy to nD array at once :()
            Array values = Array.CreateInstance(ToType(typeName), shape);

            var index = new int[rank];
            int dim = rank - 1;
            bool reset = false;
            for (int i = 0; i < totalLength; i++)
            {
                // increment the lowest dimension
                while (index[dim] == shape[dim])
                {
                    for (int j = dim; j < rank; j++)
                    {
                        index[j] = 0;
                    }

                    dim--;
                    index[dim]++;
                    reset = true;
                }


                // reset to the last dimension
                if (reset)
                {
                    dim = rank - 1;
                    reset = false;
                }

                values.SetValue(values1D.GetValue(i), index);

                index[dim]++;
            }

            return values;
        }

        public void SetValues(string variable, Array values)
        {
            throw new NotImplementedException();
        }

        public void SetValues(string variable, int[] start, int[] count, Array values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Run model in one step from start to end.
        /// </summary>
        /// <param name="library"></param>
        /// <param name="configPath"></param>
        public static void Run(string library, string configPath)
        {
            var model = new BasicModelInterfaceLibrary(library);

            model.Initialize(configPath);

            int sameTimeCounter = 0;

            DateTime t = model.StartTime;
            while (t < model.StopTime)
            {
                // check if model time step increases
                if (t == model.CurrentTime)
                {
                    sameTimeCounter++;

                    if (sameTimeCounter == 100)
                    {
                        throw new InvalidOperationException("Model current_time did not increase after 100 updates");
                    }
                }

                t = model.CurrentTime;
                model.Update(-1.0);

                // reset model time step counter
                if (t != model.CurrentTime)
                {
                    sameTimeCounter = 0;
                }
            }

            model.Finish();
        }

        private void GetVariableNames()
        {
            int count = 0;
            lib.get_var_count(ref count);

            var strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                var variableNameBuffer = new StringBuilder(MAXSTRLEN);
                lib.get_var_name(i, variableNameBuffer);
                strings[i] = variableNameBuffer.ToString();
            }

            variableNames = strings;
        }

        private Type ToType(string typeName)
        {
            switch (typeName)
            {
                case "double":
                    return typeof (double);

                case "int":
                    return typeof (int);
            }

            throw new NotSupportedException("Unsupported type: " + typeName);
        }

        private Array ToArray1D(IntPtr ptr, string valueType, int totalLength)
        {
            if (valueType == "double")
            {
                var values = new double[totalLength];
                Marshal.Copy(ptr, values, 0, totalLength);
                return values;
            }

            if (valueType == "int")
            {
                var values = new int[totalLength];
                Marshal.Copy(ptr, values, 0, totalLength);
                return values;
            }

            throw new NotSupportedException("Unsupported type: " + valueType);
        }

        private static int GetTotalLength(int[] shape)
        {
            return shape.Aggregate(1, (current, t) => current * t);
        }
    }
}