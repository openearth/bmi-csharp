using System;
using System.Runtime.InteropServices;
using System.Text;
using BasicModelInterface.Reflection;

namespace BasicModelInterface
{
    public delegate void Logger(ref int level, StringBuilder message);
    
    /// <summary>
    /// This interface contains calls to external BMI library. 
    /// 
    /// It is used as a specification in the <see cref="DynamicDllImport"/> class to limit possible method calls.
    /// </summary>
    internal interface INativeBasicModelInterface
    {
        void initialize([In] string config_file);

        void update([In] ref double dt);

        void finalize();

        void get_start_time([In, Out] ref double t);

        void get_end_time([In, Out] ref double t);

        void get_current_time([In, Out] ref double t);

        void get_time_step([In, Out] ref double dt);

        void get_string_attribute([In] string name, [Out] StringBuilder value);

        void get_n_variables([In, Out] ref int count); // non-BMI

        void get_variable_name([In, Out] ref int index, [Out] StringBuilder variable); // non-BMI

        void get_var_shape([In] string variable, [Out] int[] shape);

        void get_var_rank([In] string variable, [In, Out] ref int rank);

        void get_1d_double([In] string variable, [In, Out] ref IntPtr values);

        void get_1d_int([In] string variable, [In, Out] ref IntPtr values);

        void get_2d_int([In] string variable, [In, Out] ref IntPtr values);

        void set_1d_double_at_index(string variable, [In] ref int index, [In] ref double value);

        void set_logger(ref Logger logger);
    }
}