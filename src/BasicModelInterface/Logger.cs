using System.Runtime.InteropServices;
using System.Text;

namespace BasicModelInterface
{
    public enum Level { LEVEL_ALL, LEVEL_DEBUG, LEVEL_INFO, LEVEL_WARNING, LEVEL_ERROR, LEVEL_FATAL, LEVEL_NONE };

    public delegate void Logger([In] Level level, [In][MarshalAs(UnmanagedType.LPStr)] string message);
}