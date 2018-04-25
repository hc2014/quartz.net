using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        IScheduler scheduler;
        //调度器工厂
        ISchedulerFactory factory;

        private static  ConcurrentQueue<JobEntity> CalcTaskQueue = new  ConcurrentQueue<JobEntity>();


        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            //1、创建一个调度器
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();
            scheduler.Start();
            


            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    JobEntity obj = new JobEntity();
                    bool isOk = CalcTaskQueue.TryDequeue(out obj);
                    if (isOk)
                    {
                        JobBase jobObj = (JobBase)Assembly.Load("Scheduler").CreateInstance("Scheduler." + obj.JobTypeEntity);

                        //Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                        // object[] parameters = new object[1];
                        //parameters[0] = obj.JobId.ToString();
                        //JobBase jobObj = (JobBase)assembly.CreateInstance("Scheduler."+ obj.JobTypeEntity, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);// 创建类的实例


                        var jobName = jobObj.GetType().FullName;
                        //如果存在就先删除
                        //scheduler.DeleteJob(new JobKey(jobName));


                        //2、创建一个任务
                        IJobDetail job = jobObj.GetJobDetail(scheduler);


                        //JobBase.MyCallBackFun f = new
                        //JobBase.MyCallBackFun(jobObj.CallBackFun());


                        string cronExpression = obj.Schedule.GetCronExpression();
                        //var cheduleSingleTime = obj.Schedule.DayScheduleSingleTime;


                        //DateTime begTime = Convert.ToDateTime(obj.BegDate + " " + obj.JobTime);
                        //DateTime endTime = Convert.ToDateTime(obj.EndDate + " " + obj.JobTime);


                        DateTime begTime = new DateTime(2018, 4, 24, 13, 43, 0);
                        DateTime endTime = new DateTime(2019, 4, 24, 13, 43, 0);


                        //3、创建一个触发器
                        ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity("trigger_" + obj.JobId, null).StartAt(begTime).EndAt(endTime)
                            .WithCronSchedule("0/5 * * * * ?")
                            .Build();

                        //根据设置的时间来推算出 最近5次的执行时间  默认是0时区的时间，所以需要转换成本地时间（东八区）
                        IList<DateTimeOffset> dates1 = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, 5);
                        //获取下一次执行时间
                        var date1 = TimeZoneInfo.ConvertTimeFromUtc(dates1[0].DateTime, TimeZoneInfo.Local).ToString();


                        //MyTriggerListener myTriggerListener = new MyTriggerListener();
                        //myTriggerListener.Name = "触发器监听";

                        MyJobListener myJobListener = new MyJobListener();
                        myJobListener.Name = "任务监听器";

                        //scheduler.ListenerManager.AddTriggerListener(myJobListener);
                        scheduler.ListenerManager.AddJobListener(myJobListener);


                        //4、将任务与触发器添加到调度器中
                        scheduler.ScheduleJob(job, trigger);
                        //5、开始执行
                        scheduler.Start();


                        if (obj.PARENT_JOB_LIST.Count > 0 && obj.PARENT_JOB_LIST.First() != "")
                        {
                            scheduler.PauseJob(job.Key);
                        }


                        var kesy = scheduler.GetJobKeys(null);
                    }
                }
            });
           
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "select * from ttask_job where job_state='Y' and job_id='3928'";
            CalcTaskQueue = JobEntity.GetList(sql);
        }
        

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            TaskForm tf = new TaskForm();
            tf.Show();
            this.Hide();
        }
    }

}
