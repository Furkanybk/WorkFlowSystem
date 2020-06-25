using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WFS.web.BackServices
{
    public class Clean : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Process prs = Process.GetCurrentProcess();
            try
            {
                prs.MinWorkingSet = (IntPtr)(1);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            { 
        }
    }
}
}