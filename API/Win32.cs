using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Hypermatix.Foundation.Windows.API
{
    /// <summary>
    /// Description: Helper class for P/Invoke's win32 api functions (not all) or 
    ///              some enhanced wrapper methods for the underlying api functions
    /// Last Mod: 28/5/04
    /// Author: Brendan Whelan
    /// </summary>
    public class Win32
    {
        #region Native API constants
        public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        public const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        #endregion

        #region Native API functions
        [DllImport("kernel32.dll",
             CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public unsafe static extern int FormatMessage(int dwFlags,
            ref IntPtr lpSource,
            int dwMessageId,
            int dwLanguageId,
            ref String lpBuffer, int nSize,
            IntPtr* Arguments);
        #endregion

        #region .Net helper wrapper functions
        public unsafe static String GetErrorMessage(int errorCode)
        {
            int messageSize = 255;
            String lpMsgBuf = "";
            int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM |
                FORMAT_MESSAGE_IGNORE_INSERTS;
            IntPtr ptrlpSource = new IntPtr();
            IntPtr prtArguments = new IntPtr();
            int retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0,
                ref lpMsgBuf, messageSize, &prtArguments);
            if (0 == retVal)
            {
                throw new Exception("Failed to format message for error code " + errorCode + ". ");
            }
            return lpMsgBuf;
        }
        #endregion
    }
}
