using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BasicModelInterface.Interop;

namespace BasicModelInterface
{
    /// <summary>
    ///     Loads <paramref name="libraryPath" /> as a dynamic library and redirects all <see cref="IBasicModelInterface" />.
    /// </summary>
    public class BasicModelInterfaceLibrary : IBasicModelInterface
    {
        private const int MAXDIMS = 6;

        private const int MAXSTRLEN = 1024;

        private dynamic lib;
        
        private string originalCurrentDirectory;

        private string[] variableNames;
        
        private Logger logger;

        public BasicModelInterfaceLibrary(string libraryPath, CallingConvention callingConvention = CallingConvention.Cdecl)
        {
            lib = new DynamicDllImport(libraryPath, CharSet.Ansi, callingConvention);
            lib.ReturnTypes["initialize"] = typeof(int);
            lib.ReturnTypes["update"] = typeof(int);
            lib.ReturnTypes["finalize"] = typeof(int);
        }

        public DateTime StartTime
        {
            get
            {
                var t = 0.0;
                lib.get_start_time(ref t);
                return new DateTime().AddSeconds(t);
            }
        }

        public DateTime StopTime
        {
            get
            {
                var t = 0.0;
                lib.get_end_time(ref t);
                return new DateTime().AddSeconds(t);
            }
        }

        public DateTime CurrentTime
        {
            get
            {
                var t = 0.0;
                lib.get_current_time(ref t);
                return new DateTime().AddSeconds(t);
            }
        }

        public TimeSpan TimeStep
        {
            get
            {
                var dt = 0.0;
                lib.get_time_step(ref dt);
                return new TimeSpan(0, 0, 0, 0, (int)(dt * 1000));
            }
        }

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

        public int Initialize(string path)
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
                return lib.initialize(configFile);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalCurrentDirectory);
            }
        }

        public int Update(double timeStep)
        {
            return lib.update(timeStep);
        }

        public int Finish()
        {
            return lib.finalize();
        }

        public int[] GetShape(string variable)
        {
            var rank = 0;
            lib.get_var_rank(variable, ref rank);

            var shape = new int[MAXDIMS];
            lib.get_var_shape(variable, shape);

            return shape.Take(rank).ToArray();
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
            var ptr = IntPtr.Zero;
            lib.get_var(variable, ref ptr);

            if (ptr == IntPtr.Zero)
            {
                return null;
            }

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
            var typeName = typeNameBuilder.ToString();

            // copy to 1D array
            var totalLength = GetTotalLength(shape);

            var values1D = ToArray1D(ptr, typeName, totalLength);

            if (rank == 1)
            {
                return values1D;
            }

            // convert to nD array
            var valueType = ToType(typeName);
            var values = Array.CreateInstance(valueType, shape);
            Buffer.BlockCopy(values1D, 0, values, 0, values1D.Length * Marshal.SizeOf(valueType));

            return values;
        }

        public Array GetValues(string variable, int[] index)
        {
            throw new NotImplementedException();
        }

        public Array GetValues(string variable, int[] start, int[] count)
        {
            throw new NotImplementedException();
        }

        public void SetValues(string variable, Array values)
        {
            var valuesDouble1D = values as double[];
            if (valuesDouble1D != null)
            {
                lib.set_var(variable, valuesDouble1D);
                return;
            }

            var valuesDouble2D = values as double[,];
            if (valuesDouble2D != null)
            {
                lib.set_var(variable, valuesDouble2D);
                return;
            }

            var valuesInt2D = values as int[,];
            if (valuesInt2D != null)
            {
                lib.set_var(variable, valuesInt2D);
                return;
            }
        }

        public void SetValues(string variable, int[] start, int[] count, Array values)
        {
            var valuesDouble1D = values as double[];
            if (valuesDouble1D != null)
            {
                lib.set_var_slice(variable, start, count, valuesDouble1D);
                return;
            }

            var valuesDouble2D = values as double[,];
            if (valuesDouble2D != null)
            {
                lib.set_var_slice(variable, start, count, valuesDouble2D);
                return;
            }

            var valuesInt2D = values as int[,];
            if (valuesInt2D != null)
            {
                lib.set_var_slice(variable, start, count, valuesInt2D);
                return;
            }
        }

        public void SetValues(string variable, int[] index, Array values)
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
            var model = new BasicModelInterfaceLibrary(library)
            {
                Logger = (level, message) => Console.WriteLine("{0}: {1}", level, message)
            };
            model.Initialize(configPath);

            var sameTimeCounter = 0;

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

        internal static int GetTotalLength(int[] shape)
        {
            return shape.Aggregate(1, (current, t) => current * t);
        }
    }
}