using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string ClassName, string Caption);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hwnd, IntPtr childAfter, string ClassName, string Caption);

        const int SWP_NOSIZE = 0x1;
        const string ProcessName = "StikyNot";
        const string ProcessClassName = "Sticky_Notes_Note_Window";
        static void Main(string[] args)
        {
            List<IntPtr> hwnds = new List<IntPtr>();
            var primary = System.Windows.Forms.SystemInformation.PrimaryMonitorSize;
            var virtualScreen = System.Windows.Forms.SystemInformation.VirtualScreen;
            var second = new Rectangle() { Width = virtualScreen.Width - primary.Width, Height = virtualScreen.Height };

            System.Diagnostics.Process[] processArray = System.Diagnostics.Process.GetProcessesByName(ProcessName);
            if (processArray.Count() == 0)
                return;

            Process p = processArray[0];

            IntPtr i = FindWindow(ProcessClassName, null);
            if (i != IntPtr.Zero)
            {
                //get all my windows
                hwnds.Add(i);
                do
                {
                    i = FindWindowEx(IntPtr.Zero, i, ProcessClassName, null);
                    if (i != IntPtr.Zero)
                        hwnds.Add(i);
                }
                while (i != IntPtr.Zero);
            }

            foreach (IntPtr hwnd in hwnds)
            {
                //repositon
                Rectangle current = new Rectangle();
                GetWindowRect(hwnd, ref current);
                IntPtr x = SetWindowPos(hwnd, 0, current.X + second.Width, current.Y, 0, 0, SWP_NOSIZE);
            }

        }
    }
}
