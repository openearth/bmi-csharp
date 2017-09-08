using System.Runtime.InteropServices;
using System.Text;

namespace BasicModelInterface
{
    public enum Level { Level_All, Level_Debug, Level_Info, Level_Warning, Level_Error, Level_Fatal, Level_None };

    public delegate void Logger([In] Level level, [In][MarshalAs(UnmanagedType.LPStr)] string message);
}