using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class JobEntity
    {
        #region 静态常量
        /// <summary>
        /// 调度单位：分
        /// </summary>
        public const string JobUnitMinute = "S";

        /// <summary>
        /// 调度单位：日
        /// </summary>
        public const string JobUnitDay = "D";

        /// <summary>
        /// 调度单位：周
        /// </summary>
        public const string JobUnitWeek = "W";

        /// <summary>
        /// 调度单位：月
        /// </summary>
        public const string JobUnitMonth = "M";

        /// <summary>
        /// 调度单位：一季度
        /// </summary>
        public const string JobUnitQuarter = "Q";

        /// <summary>
        /// 调度单位：半年
        /// </summary>
        public const string JobUnitHalfOfYear = "P";

        /// <summary>
        /// 调度单位：一年
        /// </summary>
        public const string JobUnitYear = "Y";

        /// <summary>
        /// 作业启用状态
        /// </summary>
        public const string JobStateForRun = "Y";

        /// <summary>
        /// 作业停用状态
        /// </summary>
        public const string JobStateForStop = "N";

        #endregion

        #region 公共属性

        /// <summary>
        /// 作业ID
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 作业描述
        /// </summary>
        public string JobDesc { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public string JobState { get; set; }

        /// <summary>
        /// 作业类型  类型定义见 TaskType 定义\JobSchedule\JobParam.cs
        /// </summary>
        public string JobType { get; set; }


        public string JobTypeEntity
        {
            get
            {
                EntityTypeMapping cv = new EntityTypeMapping();
                Type t = cv.GetType();
                var fileds = t.GetFields();

                var obj = fileds.Where(r => r.GetValue(cv).ToString() == this.JobType).FirstOrDefault();
                if (obj != null && string.IsNullOrEmpty(obj.Name) == false)
                {
                    return obj.Name;
                }
                throw new Exception("没有配置相关的类型");
            }
        }

        /// <summary>
        /// 作业开始执行时间
        /// </summary>
        public string JobTime { get; set; }

        /// <summary>
        /// 作业开始执行日期
        /// </summary>
        //  public string BegDate { get; set; }
        public string BegDate
        {
            get;set;
        }

        /// <summary>
        /// 作业停止执行日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 作业单位 每分、每天、每周、每月
        /// </summary>
        public string JobUnit { get; set; }


        /// <summary>
        /// 作业频率
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// 基准日期偏移
        /// </summary>
        public string DateOff { get; set; }

        /// <summary>
        /// 最近执行日
        /// </summary>
        public string RunDate { get; set; }

        /// <summary>
        /// 下一执行日
        /// </summary>
        public string PlanDate { get; set; }

        /// <summary>
        /// 作业调度参数
        /// </summary>
        public string JobXML { get; set; }

        /// <summary>
        /// 作业参数,调度参数
        /// </summary>
        public string JobParam { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public string UserNo { get; set; }

        public string IsRunning { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 投资监督任务的子类型：全局任务（1），指定计划任务（2）
        /// </summary>
        public string JOB_MTYPE { get; set; }

        /// <summary>
        /// 投资监督任务跟踪任务触发时间
        /// </summary>
        public string TRACKDAYS { get; set; }

        /// <summary>
        /// 通知人员
        /// </summary>
        public string MAILUSERS { get; set; }

        public List<string> PARENT_JOB_LIST { get; set; }


        /// <summary>
        /// 作业类型名称
        /// </summary>
        public string JobTypeName
        {
            get { return JobType; }
        }

        /// <summary>
        /// 作业状态名称
        /// </summary>
        public string JobStateName
        {
            get { return this.JobState == JobStateForRun ? "启用" : "停用"; }
        }

        /// <summary>
        /// 作业单位名称
        /// </summary>
        public string JobUnitName
        {
            get
            {
                string label = string.Empty;
                if (this.JobUnit == JobUnitDay)
                {
                    label = "天";
                }
                else if (this.JobUnit == JobUnitMinute)
                {
                    label = "分";
                }
                else if (this.JobUnit == JobUnitWeek)
                {
                    label = "周";
                }
                else if (this.JobUnit == JobUnitMonth)
                {
                    label = "月";
                }

                return label;
            }
        }

        private JobScheduleJson scheduleJson = null;
        public JobScheduleJson ScheduleJson
        {
            get
            {
                if (this.scheduleJson == null)
                {
                    this.scheduleJson = JsonHelper.Deserialize<JobScheduleJson>(this.JobXML);
                }

                return this.scheduleJson;
            }
        }

        private JobSchedulObj schedule;
        /// <summary>
        /// 作业调度对象
        /// </summary>
        public JobSchedulObj Schedule
        {
            get
            {
                if (this.schedule == null)
                {
                    if (this.ScheduleJson != null)
                    {
                        this.schedule = this.ScheduleJson.ToJobSchedule();
                    }
                }

                return this.schedule;
            }
            set
            {
                this.schedule = value;
            }
        }

        /// <summary>
        /// 用于前台显示计划任务一列
        /// </summary>
        public string ScheduleShortInfo
        {
            get
            {
                return "每" + (this.Frequency > 1 ? this.Frequency.ToString() : string.Empty) + this.JobUnitName;
            }
        }

        /// <summary>
        /// Quartz调度作业名称
        /// </summary>
        public string QuartzJobName
        {
            get { return string.Format("Job_{0}", this.JobId); }
        }

        /// <summary>
        /// Quartz触发器名称
        /// </summary>
        public string QuartzTriggerName
        {
            get { return string.Format("Trigger_{0}", this.JobId); }
        }

        /// <summary>
        /// Quartz分组名称(不同种类的作业放置到不同种类的Group中)
        /// </summary>
        public string QuartzGroupName
        {
            get { return string.Format("Group_{0}", this.JobType); }
        }

        #endregion


        public static void UpdateJobRunDate(string jobId)
        {
            return;
            var sql = string.Format("update ttask_job set job_run_date='{0}' where job_id='{1}'", DateTime.Now.ToString("yyyy-MM-dd"), jobId);

            DbHelperSQL.ExecuteSql(sql);
        }


        public static ConcurrentQueue<JobEntity> GetList(string sql)
        {
            ConcurrentQueue<JobEntity> CalcTaskQueue = new ConcurrentQueue<JobEntity>();

           
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                List<JobEntity> list = new List<JobEntity>();

                dt.AsEnumerable().ToList().ForEach(r =>
                {
                    list.Add(new JobEntity()
                    {
                        JobId = Convert.ToInt32(r["JOB_ID"]),
                        JobName = r["JOB_NAME"].ToString(),
                        JobType = r["JOB_TYPE"].ToString(),
                        JobTime = r["JOB_TIME"].ToString(),
                        BegDate = r["JOB_BEG_DATE"].ToString(),
                        EndDate = r["JOB_END_DATE"].ToString(),
                        JobUnit = r["JOBUNIT"].ToString(),
                        RunDate = r["JOB_RUN_DATE"].ToString(),
                        PlanDate = r["JOB_PLAN_DATE"].ToString(),
                        JobXML = r["JOBXML"].ToString(),
                        JobParam = r["JOBPARAM"].ToString(),
                        IsRunning = r["IS_RUNNING"].ToString(),
                        PARENT_JOB_LIST = r["PARENT_JOB_LIST"].ToString().Split(',').ToList()
                    });
                });

                //list = list.Where(r => Convert.ToDateTime(r.BegDate + " " + r.JobTime) >= DateTime.Now
                //&& Convert.ToDateTime(r.EndDate + " " + r.JobTime) <= DateTime.Now &&Convert.ToDateTime(r.RunDate)<DateTime.Now).ToList();

                var ids = CalcTaskQueue.Select(item => item.JobId).ToList();

                list.ForEach(r =>
                {
                    if (!ids.Contains(r.JobId))
                    {
                        CalcTaskQueue.Enqueue(r);
                    }
                });

            }
            return CalcTaskQueue;
        }
    }
}
