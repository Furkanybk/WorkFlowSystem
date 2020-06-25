using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.web.BackServices;

namespace WFS.web.Services
{
    public class BackgroundWorker
    {
        public void RunJob()
        {
            try
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                IScheduler sched = schedFact.GetScheduler();
                if (!sched.IsStarted)
                    sched.Start();
                IJobDetail jobX2 = JobBuilder.Create<Clean>().WithIdentity("Clean", null).Build();
                ISimpleTrigger triggerX2 = (ISimpleTrigger)TriggerBuilder.Create().WithIdentity("Clean").StartAt(DateTime.UtcNow).WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever()).Build();
                sched.ScheduleJob(jobX2, triggerX2);
            }
            catch
            {
            }
        }
    }
}