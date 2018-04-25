using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace Scheduler
{
    public class MyJobListener : IJobListener
    {
        string name;
        public string Name { get { return name; } set { name = value; } }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            LogHelper.Log(string.Format("{0}开始执行:{1}", ((Quartz.Impl.JobDetailImpl)context.JobDetail).Name, DateTime.Now + Environment.NewLine));
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            LogHelper.Log(string.Format("{0}执行完毕:{1}", ((Quartz.Impl.JobDetailImpl)context.JobDetail).Name, DateTime.Now + Environment.NewLine));

            var fun = (Action<IScheduler>)((Quartz.Impl.JobDetailImpl)((Quartz.Impl.JobExecutionContextImpl)context).JobDetail).JobDataMap["successCallBackFun"];
            fun(context.Scheduler);
            

            //var jobId = ((Scheduler.Class11)((Quartz.Impl.JobExecutionContextImpl)context).JobInstance).JOB_ID;

        }
    }
}
