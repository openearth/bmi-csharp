using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BasicModelInterface.Reflection;

namespace BasicModelInterface
{
    /// <summary>
    ///     Loads <paramref name="libraryPath" /> as a dynamic library and redirects all <see cref="IBasicModelInterface" />.
    /// </summary>
    public class BasicModelInterfaceLibrary : IBasicModelInterface
    {
        public const int MAXDIMS = 6;
        
        public const int MAXSTRLEN = 1024;

        private string originalCurrentDirectory;
        
        private dynamic lib;

        public BasicModelInterfaceLibrary(string libraryPath, CallingConvention callingConvention = CallingConvention.Cdecl)
        {
            lib = new DynamicDllImport(libraryPath, CharSet.Auto, callingConvention);
        }

        public DateTime StartTime
        {
            get
            {
                var startTime = 0.0;

                // FlexibleMeshModelDll.get_start_time(ref startTime);

                var sb = new StringBuilder(MAXSTRLEN);
                lib.get_string_attribute("refdat", sb);

                var refDate = new DateTime();

                try
                {
                    refDate = DateTime.ParseExact(sb.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    return refDate;
                }

                return refDate.AddSeconds(startTime);
            }
        }

        public DateTime StopTime { get; private set; }

        public DateTime CurrentTime
        {
            get
            {
                double t = 0.0;
                lib.get_current_time(ref t);

                return StartTime.AddSeconds(t);
            }
        }

        public TimeSpan TimeStep { get; set; }

        public void Initialize(string path)
        {
            originalCurrentDirectory = Directory.GetCurrentDirectory();

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.SetCurrentDirectory(dir);
            }

            var configFile = Path.GetFileName(path);

            if (!string.IsNullOrEmpty(configFile))
            {
                configFile = configFile.PadRight(MAXSTRLEN, '\0'); // make FORTRAN friendly
            }

            lib.initialize(configFile);
        }

        public void Update()
        {
            double totalSeconds = TimeStep.TotalSeconds;
            lib.update(ref totalSeconds);
        }

        public void Finish()
        {
            lib.finalize();

            if (string.IsNullOrEmpty(originalCurrentDirectory))
            {
                return;
            }

            Directory.SetCurrentDirectory(originalCurrentDirectory);
        }

        public int[] GetIntValues1D(string variable)
        {
            int rank = 0;
            lib.get_var_rank(variable, ref rank);

            if (rank > 1)
            {
                throw new NotImplementedException("Only variables with rank 1 are supported");
            }

            var shape = new int[MAXDIMS];
            lib.get_var_shape(variable, shape);

            if (rank == 1)
            {
                var valuesPointer = new IntPtr();
                lib.get_1d_int(variable, ref valuesPointer);
                int length = shape[0];
                var values = new int[length];
                if (length > 0)
                {
                    Marshal.Copy(valuesPointer, values, 0, length);
                }

                return values;
            }

            return null;
        }

        public int[,] GetIntValues2D(string variable, out int[] shape)
        {
            int rank = 0;
            lib.get_var_rank(variable, ref rank);

            if (rank > 2)
            {
                throw new NotImplementedException("Only variables with rank 1 are supported");
            }

            shape = new int[lib.MAXDIMS];
            lib.get_var_shape(variable, shape);

            if (rank == 2)
            {
                var valuesPointer = new IntPtr();
                lib.get_2d_int(variable, ref valuesPointer);

                lib.get_var_shape(variable, shape);

                int length = shape[0]*shape[1];
                var values = new int[length];
                Marshal.Copy(valuesPointer, values, 0, length);

                var values2d = new int[shape[0], shape[1]];
                for (var i = 0; i < shape[0]; i++)
                {
                    for (var j = 0; j < shape[1]; j++)
                    {
                        values2d[i, j] = values[i * shape[0] + j];
                    }
                }

                return values2d;
            }

            return new int[,] {};
        }

        public double[] GetDoubleValues1D(string variable)
        {
            int rank = 0;
            lib.get_var_rank(variable, ref rank);

            if (rank > 1)
            {
                throw new NotImplementedException("Only variables with rank 1 are supported");
            }

            var shape = new int[MAXDIMS];
            lib.get_var_shape(variable, shape);

            if (rank == 1)
            {
                var valuesPointer = new IntPtr();
                lib.get_1d_double(variable, ref valuesPointer);
                int length = shape[0];
                var values = new double[length];
                if (length > 0)
                {
                    Marshal.Copy(valuesPointer, values, 0, length);
                }

                return values;
            }

            return null;
        }

        public void SetDoubleValue1DAtIndex(string variable, int index, double valueDouble)
        {
            lib.set_1d_double_at_index(variable, ref index, ref valueDouble);
        }
    }
}