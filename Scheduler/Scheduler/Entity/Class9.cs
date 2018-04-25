using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Quartz;

namespace Scheduler
{

    public class Class9 : JobBase
    {

        public string EntityName { get; set; }
        public string JobName { get; set; }
        public string JOB_ID { get; set; }

        public Class9()
        {
            EntityName = "Class9";
            JOB_ID = "3928";
        }

        public Class9(string jobId):this()
        {
            JOB_ID = jobId;
        }


        public override IJobDetail GetJobDetail(IScheduler scheduler)
        {
            Action<IScheduler> ac = new
                 Action<IScheduler>(TestCallFun);
            //Action<IScheduler> ac1 = (s) => TestCallFun(s);

            JobDataMap jobDataMap = new JobDataMap();
            KeyValuePair<string, object> kv = new KeyValuePair<string, object>("successCallBackFun", ac);
            jobDataMap.Add(kv);


            //var jobName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            IJobDetail job = JobBuilder.Create<Class9>().WithIdentity(new JobKey(JOB_ID)).SetJobData(jobDataMap).Build();
            return job;
        }

        public override void DoExecute(IJobExecutionContext context)
        {
            JobEntity.UpdateJobRunDate(JOB_ID);

            LogHelper.Log(string.Format("{0}执行:{1}", JOB_ID, DateTime.Now + Environment.NewLine));

            //模拟执行
            //Thread.Sleep(5000);
        }


        public void TestCallFun(IScheduler scheduler)
        {

            ConcurrentQueue<JobEntity> CalcTaskQueue = new ConcurrentQueue<JobEntity>();

            //业务代码执行完成后 来执行
            //检查所有依赖当前任务的 其他任务
            string sql = "select * from ttask_job where job_state='Y' and instr(parent_job_list," + JOB_ID + ")>0";
            CalcTaskQueue = JobEntity.GetList(sql);
            foreach (var item in CalcTaskQueue)
            {
                bool isOk = false;

                //假设该任务还依赖其他任务
                var otherJobList = item.PARENT_JOB_LIST.Where(r => r != JOB_ID).ToList();

                if (otherJobList.Count() > 0)
                {
                    sql = "select * from ttask_job where job_state='Y' and job_id in(" + string.Join(",", otherJobList) + ")";
                    var ds = DbHelperSQL.Query(sql);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        isOk = ds.Tables[0].AsEnumerable().ToList().Where(r => r["JOB_RUN_DATE"].ToString() != DateTime.Now.ToString("yyyy-MM-dd")).Count() <= 0;
                    }

                }
                else
                {
                    isOk = true;
                }

                if (isOk)
                {
                    scheduler.ResumeJob(new JobKey(item.JobId.ToString()));

                    //JobEntity obj = new JobEntity();
                    //CalcTaskQueue.TryDequeue(out obj);

                    //JobBase jobObj = (JobBase)Assembly.Load("Scheduler").CreateInstance("Scheduler." + obj.JobTypeEntity);

                    //var jobName = jobObj.GetType().FullName;
                    ////如果存在就先删除
                    //context.Scheduler.DeleteJob(new JobKey(jobName));

                    ////2、创建一个任务
                    //IJobDetail job = jobObj.GetJobDetail();


                    //ITrigger trigger = TriggerBuilder.Create()
                    //        .WithIdentity("trigger_" + obj.JobId, null)
                    //        .WithCronSchedule("0/3 * * * * ?")
                    //        .Build();

                    //context.Scheduler.ScheduleJob(job, trigger);
                }

            }

            LogHelper.Log("测试执行成功后的回调方法"+ DateTime.Now + Environment.NewLine);
        }
    }
}
