using System;
using System.Runtime.InteropServices;
using System.Text;

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
        int initialize([In] string config_file);

        int update([In] ref double dt);

        int finalize();

        void set_logger(ref Logger logger);

        
        void get_start_time([In, Out] ref double t);

        void get_end_time([In, Out] ref double t);

        void get_current_time([In, Out] ref double t);

        void get_time_step([In, Out] ref double dt);

        
        void get_var_count([In, Out] ref int count); // non-BMI

        void get_var_name([In, Out] ref int index, [Out] StringBuilder variable); // non-BMI

        void get_var_shape([In] string variable, [Out] int[] shape);

        void get_var_rank([In] string variable, [In, Out] ref int rank);


        void get_var_values([In] string variable, [In, Out] ref IntPtr values);

        void set_var_values([In] string variable, [In, Out] ref IntPtr values);

        void get_var_values([In] string variable, [Out] int[] start, [Out] int[] stop, [Out] int[] step, [In, Out] ref IntPtr values);

        void set_var_values([In] string variable, [Out] int[] start, [Out] int[] stop, [Out] int[] step, [In, Out] ref IntPtr values);
    }
}