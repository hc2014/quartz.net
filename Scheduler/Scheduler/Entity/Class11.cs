using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace Scheduler
{
    public class Class11 : JobBase
    {
        public string EntityName { get; set; }
        public string JobName { get; set; }

        public string JOB_ID { get; set; }

        public Class11()
        {
            EntityName = "Class11";
            JOB_ID = "3667";
        }

     

        public override IJobDetail GetJobDetail(IScheduler scheduler)
        {
            //var jobName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            IJobDetail job = JobBuilder.Create<Class11>().WithIdentity(new JobKey(JOB_ID)).Build();
            return job;
        }

        public override void DoExecute(IJobExecutionContext context)
        {
            //3928
            //var time = ((Quartz.Impl.JobExecutionContextImpl)context).NextFireTimeUtc;


            JobEntity.UpdateJobRunDate(JOB_ID);
            

            LogHelper.Log(string.Format("{0}执行:{1}", JOB_ID, DateTime.Now + Environment.NewLine));

            context.Scheduler.PauseJob(new JobKey(JOB_ID));
        }
    }
}
