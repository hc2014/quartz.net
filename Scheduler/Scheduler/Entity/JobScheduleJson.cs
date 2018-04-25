using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class JobScheduleJson
    {
        /// <summary>
        /// 作业调度的起始日期  格式：YYYY-MM-DD
        /// </summary>

        public string BeginDate
        {
            get { return BegDate1; }
            set
            {
                BegDate1 = value.Trim().ToLower().Replace("yyyy", DateTime.Now.Year.ToString()).Replace("mm", DateTime.Now.Month.ToString())
                    .Replace("dd", DateTime.Now.Day.ToString());
            }
        }
        private string BegDate1;
        /// <summary>
        /// 作业调度结束日期，如果没有，则不送或送空
        /// </summary>

        public string EndDate { set; get; }

        /// <summary>
        /// 作业调度单位(默认为天) S=分 D=天 W=周 M=月
        /// </summary>

        public string Unit { set; get; }
        /// <summary>
        /// 界面设置的调度单位
        /// </summary>
        public string SUnit { set; get; }

        /// <summary>
        /// 作业调度频率 如每1天，每2周（虽然是整型数，但也必须以字符串送）
        /// </summary>

        public string Frequency { set; get; }

        /// <summary>
        /// 按周调度时， WeekDay的集合，如周一和周三
        /// 注意：周日到周六按照英文缩写来，不要送数字，中间用逗号隔开
        /// SUN MON TUE WED THU FRI SAT(从周日到周六)
        /// 如果全部7天都选中的话，这个参数可以不送
        /// </summary>

        public string WeekList { set; get; }

        /// <summary>
        /// 按月调度时，月份的集合，如一月，五月等
        /// 关于月份，也不要送数字，送英文，格式如下：
        /// JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC
        /// </summary>

        public string MonthList { set; get; }

        /// <summary>
        /// 按月调度类型 1=按绝对时间 2=按相对时间 默认为1 
        /// </summary>

        public string MonthScheduleType { set; get; }

        /// <summary>
        /// 按月绝对调度时候，明确为哪一天(默认为1，但是要注意设置为31日的时候，不要设置在小月)
        /// </summary>

        public string MonthScheduleAbsoluteDay { set; get; }

        /// <summary>
        /// 按月相对调度， 明确第几个（默认第一个） 1， 2， 3， 4，5最后
        /// </summary>

        public string MonthScheduleRelativeNumber { set; get; }

        /// <summary>
        /// 按月相对调度， 第几个星期几， 如第一个星期四，最后一天等（这里不包括工作日休息日等概念，默认星期一）
        /// 同样：这里的星期与上面是一样的，如果是最后一天，则这个天送 "DAY"
        /// </summary>

        public string MonthScheduleRelativeUnit { set; get; }

        /// <summary>
        /// 具体到某一天时的调度策略，如执行一次还是按条件执行多次(默认一次) 一次送 1 多次送 2 
        /// </summary>

        public string DayScheduleType { set; get; }

        /// <summary>
        /// 每天执行一次的时间(默认为绝对时间) 可以送完整的日期时间格式，也可以只送时间，注意格式如下：
        /// 2009-09-07 12:23:09 或 12:23:09
        /// </summary>
        public string DayScheduleSingleTime { set; get; }

        

        /// <summary>
        /// 月频、周频、半年、年频的指定执行日期
        /// </summary>
        public List<string> ExcuteDateCon { get; set; }

        /// <summary>
        /// 转换成调度计划
        /// </summary>
        /// <returns></returns>
        public JobSchedulObj ToJobSchedule()
        {
            JobSchedulObj jobSchedule = new JobSchedulObj();

            jobSchedule.BeginDate = DateTime.Parse(BeginDate);
            if (string.IsNullOrEmpty(EndDate))
            {
                jobSchedule.IsScheduleEndDate = false;
            }
            else
            {
                jobSchedule.IsScheduleEndDate = true;
                jobSchedule.EndDate = DateTime.Parse(EndDate);
            }

            if (Unit.Equals("D"))
            {
                jobSchedule.Unit = JobDispatchUnit.Day;
            }
            else if (Unit.Equals("W"))
            {
                jobSchedule.Unit = JobDispatchUnit.Week;
            }
            else if (Unit.Equals("M"))
            {
                jobSchedule.Unit = JobDispatchUnit.Month;
            }

            else if (Unit.Equals("MI"))
            {
                jobSchedule.Unit = JobDispatchUnit.Minute;
            }
            else if (Unit.Equals("Y"))
            {
                jobSchedule.Unit = JobDispatchUnit.Year;
            }
            else if (Unit.Equals("S"))
            {
                jobSchedule.Unit = JobDispatchUnit.Second;
            }

            if (string.IsNullOrEmpty(Frequency))
            {
                Frequency = "1";
            }

            jobSchedule.Frequency = int.Parse(Frequency);

            jobSchedule.WeekArray = WeekList;

            jobSchedule.MonthArray = MonthList;

            //按月
            if (!string.IsNullOrEmpty(this.MonthScheduleType))
            {
                if (this.MonthScheduleType.Equals("1"))
                {
                    jobSchedule.MonthScheduleType = JobDispatchMonthType.Absolute;
                    jobSchedule.MonthScheduleAbsoluteDay = int.Parse(MonthScheduleAbsoluteDay);
                }
                else if (this.MonthScheduleType.Equals("2"))
                {
                    jobSchedule.MonthScheduleType = JobDispatchMonthType.Relative;

                    //
                    if (this.MonthScheduleRelativeNumber.Equals("1"))
                    {
                        jobSchedule.MonthScheduleRelativeNumber = JobDispatchMonthRelative.One;
                    }
                    else if (MonthScheduleRelativeNumber.Equals("2"))
                    {
                        jobSchedule.MonthScheduleRelativeNumber = JobDispatchMonthRelative.Two;
                    }
                    else if (MonthScheduleRelativeNumber.Equals("3"))
                    {
                        jobSchedule.MonthScheduleRelativeNumber = JobDispatchMonthRelative.Three;
                    }
                    else if (MonthScheduleRelativeNumber.Equals("4"))
                    {
                        jobSchedule.MonthScheduleRelativeNumber = JobDispatchMonthRelative.Four;
                    }
                    else if (MonthScheduleRelativeNumber.Equals("5"))
                    {
                        jobSchedule.MonthScheduleRelativeNumber = JobDispatchMonthRelative.Last;
                    }

                    if (MonthScheduleRelativeUnit.Equals("SUN"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.SUN;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("MON"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.MON;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("TUE"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.TUE;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("WED"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.WED;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("THU"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.THU;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("FRI"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.FRI;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("SAT"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.SAT;
                    }
                    else if (MonthScheduleRelativeUnit.Equals("DAY"))
                    {
                        jobSchedule.MonthScheduleRelativeUnit = JobDispatchMonthRelativeUnit.DAY;
                    }
                }
            }

            // 默认都是一次
            jobSchedule.DayScheduleType = JobDispatchDayType.Single;

            jobSchedule.DayScheduleSingleTime = DateTime.Parse(DayScheduleSingleTime);

            return jobSchedule;
        }
        /// <summary>
        /// 返回调度的时序格式化字符串
        /// 目前，可分按天、按月、按季度、按年调度
        /// </summary>
        /// <param name="BegDate">job开始日期</param>
        /// <param name="Unit">调度类型：D,天；M：月,Y:年；Q:季度，周：W，半年：P</param>
        /// <param name="SingleTime">执行的具体时间</param>
        /// <param name="EndDate">job结束日期</param>
        /// <returns></returns>
        public static string ToJsonJobSchedule(string BegDate, string Unit,
            string SingleTime,
            string EndDate = "", List<string> ExctueDateCon = null)
        {
            string json = string.Empty;

            JobScheduleJson JobSchedule = new JobScheduleJson();
            JobSchedule.BeginDate = BegDate;

            if (string.IsNullOrEmpty(EndDate) || EndDate == "")
            {
                JobSchedule.EndDate = string.Empty;
            }
            else
            {
                JobSchedule.EndDate = EndDate;
            }
            JobSchedule.SUnit = Unit;
            if (Unit == "W") { JobSchedule.Unit = Unit; } else JobSchedule.Unit = "D";

            JobSchedule.Frequency = "1";
            JobSchedule.ExcuteDateCon = ExctueDateCon;
            if (Unit == "W")
            {
                if (ExctueDateCon != null)
                {
                    JobSchedule.WeekList = string.Join<string>(",", ExctueDateCon);
                }
            }


            // 默认都是一次
            JobSchedule.DayScheduleSingleTime = SingleTime;

            return JsonHelper.Serialize<JobScheduleJson>(JobSchedule);
        }
    }
}
