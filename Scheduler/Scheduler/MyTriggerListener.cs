using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace Scheduler
{
    public class MyTriggerListener : ITriggerListener
    {
        private string name;

        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            LogHelper.Log(string.Format("{0}开始执行:{1}", ((Quartz.Impl.JobDetailImpl)context.JobDetail).Name, DateTime.Now + Environment.NewLine));
        }
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            LogHelper.Log(string.Format("{0}job执行时调用:{1}", ((Quartz.Impl.JobDetailImpl)context.JobDetail).Name, DateTime.Now + Environment.NewLine));
        }
        public void TriggerMisfired(ITrigger trigger)
        {
            LogHelper.Log(string.Format("{0}错过触发时调用(例：线程不够用的情况下:{1}", ((Quartz.Impl.Triggers.AbstractTrigger)trigger).JobKey, DateTime.Now + Environment.NewLine));
        }
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            //Trigger触发后，job执行时调用本方法。true即否决，job后面不执行。
            return false;
        }
        public string Name { get { return name; } set { name = value; } }
    }
}
