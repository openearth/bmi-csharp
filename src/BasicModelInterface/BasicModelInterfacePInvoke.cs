using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Deltares.Models.FlowFM
{
    public static class FlexibleMeshModelDll
    {
        public const int MAXDIMS = 6;
        public const int MAXSTRLEN = 1024;

        [DllImport("unstruc", EntryPoint = "initialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialize([In] string config_file);

        [DllImport("unstruc", EntryPoint = "finalize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void finalize();

        [DllImport("unstruc", EntryPoint = "get_start_time", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_start_time([In, Out] ref double t);

        [DllImport("unstruc", EntryPoint = "get_end_time", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_end_time([In, Out] ref double t);
        
        [DllImport("unstruc", EntryPoint = "get_current_time", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_current_time([In, Out] ref double t);

        [DllImport("unstruc", EntryPoint = "get_time_step", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_time_step([In, Out] ref double dt);

        [DllImport("unstruc", EntryPoint = "get_string_attribute", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_string_attribute([In] string name, [Out] StringBuilder value);

        [DllImport("unstruc", EntryPoint = "update", CallingConvention = CallingConvention.Cdecl)]
        public static extern void update([In] ref double dt);

        [DllImport("unstruc", EntryPoint = "get_n_variables", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_n_variables([In, Out] ref int count); // non-BMI

        [DllImport("unstruc", EntryPoint = "get_variable_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_variable_name([In, Out] ref int index, [Out] StringBuilder variable); // non-BMI

        [DllImport("unstruc", EntryPoint = "get_var_shape", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_var_shape([In] string variable, [Out] int[] shape);

        [DllImport("unstruc", EntryPoint = "get_var_rank", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_var_rank([In] string variable, [In, Out] ref int rank);

        [DllImport("unstruc", EntryPoint = "get_1d_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_1d_double([In] string variable, [In, Out] ref IntPtr values);

        [DllImport("unstruc", EntryPoint = "get_1d_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_1d_int([In] string variable, [In, Out] ref IntPtr values);

        [DllImport("unstruc", EntryPoint = "get_2d_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_2d_int([In] string variable, [In, Out] ref IntPtr values);

        [DllImport("unstruc", EntryPoint = "set_1d_double_at_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_1d_double_at_index(string variable, [In] ref int index, [In] ref double value);

        [DllImport("unstruc", EntryPoint = "triang", CallingConvention = CallingConvention.Cdecl)]
        public static extern void triangulate([In] ref IntPtr sx, [In] ref IntPtr sy, [In] ref IntPtr sv, [In] ref int numSamples, // samples
                                              [In] ref IntPtr dx, [In] ref IntPtr dy, [In] ref int numDestination, //destination points
                                              [In, Out] ref IntPtr values); //triangulated result values

        [DllImport("unstruc", EntryPoint = "get_snapped_feature", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_snapped_feature([In] string feature_type, [In] ref int numIn, [In] ref IntPtr xin,
                                                      [In] ref IntPtr yin, [In, Out] ref int numOut,
                                                      [In, Out] ref IntPtr xout, [In, Out] ref IntPtr yout,
                                                      [In, Out] ref IntPtr feature_ids, ref int errorCode);

        [DllImport("unstruc", EntryPoint = "find_cells", CallingConvention = CallingConvention.Cdecl)]
        public static extern void find_cells(string netFilePath, [In, Out] ref int numCells, [In, Out] ref int maxPerCell, [In, Out] ref IntPtr netElemNode);
    }
}