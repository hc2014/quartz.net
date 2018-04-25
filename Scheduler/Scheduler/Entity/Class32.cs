using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace Scheduler
{

    public class Class32:JobBase
    {
        public string EntityName { get; set; }

        public string JobName { get; set; }

        public string JOB_ID { get; set; }

        public Class32()
        {
            EntityName = "Class32";
            JOB_ID = "4330";
        }

        

        public override IJobDetail GetJobDetail(IScheduler scheduler)
        {
            //var jobName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            IJobDetail job = JobBuilder.Create<Class32>().WithIdentity(new JobKey(JOB_ID)).Build();
            return job;
        }

        public override void DoExecute(IJobExecutionContext context)
        {
            JobEntity.UpdateJobRunDate(JOB_ID);

            LogHelper.Log(string.Format("{0}执行:{1}", JOB_ID, DateTime.Now + Environment.NewLine));
        }
    }
}
