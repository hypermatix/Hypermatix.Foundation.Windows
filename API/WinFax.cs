using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;

namespace Hypermatix.Foundation.Windows.API
{
    /// <summary>
    /// Description: Helper class for P/Invoke's winfax api functions (not all)
    /// Last Mod: 28/5/04
    /// Author: Brendan Whelan
    /// </summary>
 
    public class WinFax
    {
        #region Native API structures
        [StructLayout(LayoutKind.Sequential)]
        public struct FAX_JOB_ENTRY
        {
            public int SizeOfStruct;    //structure size, in bytes
            public int JobId;           //identifier of fax job
            [MarshalAs(UnmanagedType.LPTStr)]
            public String UserName;        //pointer to user who submitted job
            public int JobType;         //job type (send/receive)
            public int QueueStatus;     //set of job status bit flags
            public int Status;          //status code for device
            public int Size;            //size of document, in bytes
            public int PageCount;       //total pages in transmission
            [MarshalAs(UnmanagedType.LPTStr)]
            public String RecipientNumber; //pointer to recipient's fax number
            [MarshalAs(UnmanagedType.LPTStr)]
            public String RecipientName;   //pointer to recipient's name
            [MarshalAs(UnmanagedType.LPTStr)]
            public String Tsid;            //pointer to transmitting station identifier
            [MarshalAs(UnmanagedType.LPTStr)]
            public String SenderName;      //pointer to sender's name
            [MarshalAs(UnmanagedType.LPTStr)]
            public String SenderCompany;   //pointer to sender's company
            [MarshalAs(UnmanagedType.LPTStr)]
            public String SenderDept;      //pointer to sender's department
            [MarshalAs(UnmanagedType.LPTStr)]
            public String BillingCode;     //pointer to billing code
            public int ScheduleAction;  //job scheduling action code
            SYSTEMTIME ScheduleTime;    //local time to schedule job
            public int DeliveryReportType; //e-mail delivery report type
            [MarshalAs(UnmanagedType.LPTStr)]
            public String DeliveryReportAddress; //pointer to e-mail address
            [MarshalAs(UnmanagedType.LPTStr)]
            public String DocumentName;    //pointer to document name to display
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)]
            public short Year;
            [MarshalAs(UnmanagedType.U2)]
            public short Month;
            [MarshalAs(UnmanagedType.U2)]
            public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short Day;
            [MarshalAs(UnmanagedType.U2)]
            public short Hour;
            [MarshalAs(UnmanagedType.U2)]
            public short Minute;
            [MarshalAs(UnmanagedType.U2)]
            public short Second;
            [MarshalAs(UnmanagedType.U2)]
            public short Milliseconds;
        }
        #endregion

        #region Native API functions
        [DllImport("winfax.dll", SetLastError = true)]
        public static extern bool FaxConnectFaxServer(string name, ref IntPtr FaxHandle);

        [DllImport("winfax.dll", SetLastError = true)]
        public static extern bool FaxClose(IntPtr FaxHandle);

        [DllImport("winfax.dll", SetLastError = true)]
        public static extern bool FaxGetJob(IntPtr FaxHandle, uint JobId,
            ref IntPtr JobEntry);

        [DllImport("winfax.dll", SetLastError = true)]
        public static extern bool FaxGetPageData(IntPtr FaxHandle, int JobId,
            [Out] StringBuilder buffer, out int BufferSize,
            out int ImageWidth, out int ImageHeight);

        [DllImport("winfax.dll", SetLastError = true)]
        public static extern void FaxFreeBuffer(IntPtr buffer);
        #endregion
    }
}
