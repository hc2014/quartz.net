using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace Scheduler
{
    [DisallowConcurrentExecution]
    public abstract class JobBase: IJob
    {

        public delegate void MyCallBackFun();
        public string Name => throw new NotImplementedException();

        public void Execute(IJobExecutionContext context)
        {
            this.DoExecute(context);
        }

        public abstract void DoExecute(IJobExecutionContext context);

        //public IJobDetail GetJobDetail()
        //{
        //    IJobDetail job = JobBuilder.Create<Class32>().WithIdentity(this.JobName, null).Build();
        //    return job;
        //}
        public abstract IJobDetail GetJobDetail(IScheduler scheduler);


        //public abstract MyCallBackFun CallBackFun();


    }
}
