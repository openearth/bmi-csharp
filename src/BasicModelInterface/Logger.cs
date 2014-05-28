using System.Runtime.InteropServices;
using System.Text;

namespace BasicModelInterface
{
    public enum Level { Off, Debug, Info, Error, Fatal };

    public delegate void Logger([In] Level level, [In][MarshalAs(UnmanagedType.LPStr)] string message);
}