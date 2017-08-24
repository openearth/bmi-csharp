using System.Runtime.InteropServices;
using System.Text;

namespace BasicModelInterface
{
    public enum Level { All, Debug, Info, Warning, Error, Fatal, Off };

    public delegate void Logger([In] Level level, [In][MarshalAs(UnmanagedType.LPStr)] string message);
}