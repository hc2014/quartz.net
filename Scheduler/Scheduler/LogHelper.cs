using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
   public class LogHelper
    {
        public static object obj = new object();

        public static void Log(string val)
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;

            lock (obj)
            {
                System.IO.File.AppendAllText(string.Format("{0}/Quartx.txt", dir),val);
            }
        }
    }
}
