using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public enum JobDispatchUnit
    {
        /// <summary>
        /// 按分
        /// </summary>
        Minute,

        /// <summary>
        /// 按天
        /// </summary>
        Day,

        /// <summary>
        /// 按周
        /// </summary>
        Week,

        /// <summary>
        /// 按月
        /// </summary>
        Month,

        /// <summary>
        /// 按年
        /// </summary>
        Year,

        /// <summary>
        /// 按秒
        /// </summary>
        Second,

        /// <summary>
        /// 按小时
        /// </summary>
        Hour
    }

    /// <summary>
    /// 周信息
    /// </summary>
    public enum JobDispatchWeek
    {
        /// <summary>
        /// 周日
        /// </summary>
        SUN,

        /// <summary>
        /// 周一
        /// </summary>
        MON,

        /// <summary>
        /// 周二
        /// </summary>
        TUE,

        /// <summary>
        /// 周三
        /// </summary>
        WED,

        /// <summary>
        /// 周四
        /// </summary>
        THU,

        /// <summary>
        /// 周五
        /// </summary>
        FRI,

        /// <summary>
        /// 周六
        /// </summary>
        SAT
    }

    /// <summary>
    /// 月信息
    /// </summary>
    public enum JobDispatchMonth
    {
        /// <summary>
        /// 一月
        /// </summary>
        JAN,

        /// <summary>
        /// 二月
        /// </summary>
        FEB,

        /// <summary>
        /// 三月
        /// </summary>
        MAR,

        /// <summary>
        /// 四月
        /// </summary>
        APR,

        /// <summary>
        /// 五月
        /// </summary>
        MAY,

        /// <summary>
        /// 六月
        /// </summary>
        JUN,

        /// <summary>
        /// 七月
        /// </summary>
        JUL,

        /// <summary>
        /// 八月
        /// </summary>
        AUG,

        /// <summary>
        /// 九月
        /// </summary>
        SEP,

        /// <summary>
        /// 十月
        /// </summary>
        OCT,

        /// <summary>
        /// 十一月
        /// </summary>
        NOV,

        /// <summary>
        /// 十二月
        /// </summary>
        DEC
    }

    /// <summary>
    /// 按月调度的时候 调度类型
    /// </summary>
    public enum JobDispatchMonthType
    {
        /// <summary>
        /// 绝对时间， 如按照本月具体的哪一天
        /// </summary>
        Absolute,

        /// <summary>
        /// 相对时间， 如本月第一个星期二，最后一个星期日等
        /// </summary>
        Relative
    }

    /// <summary>
    /// 按月调度，相对时间
    /// </summary>
    public enum JobDispatchMonthRelative : byte
    {
        /// <summary>
        /// 第一个
        /// </summary>
        One = 1,

        /// <summary>
        /// 第二个
        /// </summary>
        Two = 2,

        /// <summary>
        /// 第三个
        /// </summary>
        Three = 3,

        /// <summary>
        /// 第四个
        /// </summary>
        Four = 4,

        /// <summary>
        /// 最后一个
        /// </summary>
        Last = 5
    }

    /// <summary>
    /// 按月调度时，相对时间单位
    /// </summary>
    public enum JobDispatchMonthRelativeUnit
    {
        /// <summary>
        /// 周日
        /// </summary>
        SUN,

        /// <summary>
        /// 周一
        /// </summary>
        MON,

        /// <summary>
        /// 周二
        /// </summary>
        TUE,

        /// <summary>
        /// 周三
        /// </summary>
        WED,

        /// <summary>
        /// 周四
        /// </summary>
        THU,

        /// <summary>
        /// 周五
        /// </summary>
        FRI,

        /// <summary>
        /// 周六
        /// </summary>
        SAT,

        /// <summary>
        /// 天
        /// </summary>
        DAY
    }

    /// <summary>
    /// 不管按什么调度方法，具体到某一天时候的策略
    /// </summary>
    public enum JobDispatchDayType
    {
        /// <summary>
        /// 执行一次
        /// </summary>
        Single,

        /// <summary>
        /// 执行多次
        /// </summary>
        Multiple
    }

    /// <summary>
    /// 某天执行多次的时候，执行间隔类型
    /// </summary>
    public enum JobDispatchDayUnit
    {
        /// <summary>
        /// 小时(1到24)
        /// </summary>
        Hour,

        /// <summary>
        /// 分钟(1到60)
        /// </summary>
        Minute,

        /// <summary>
        /// 秒(10到60)
        /// </summary>
        Second
    }

    /// <summary>
    /// 作业调度信息
    /// </summary>
    [Serializable]
    public class JobSchedulObj
    {
        /// <summary>
        /// 作业调度的起始日期(必须) 格式：2009-01-01
        /// </summary>

        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 作业调度结束时间，如果没有，则不送或送空
        /// </summary>

        public DateTime EndDate { get; set; }

        /// <summary>
        /// 作业调度单位(默认为天)
        /// </summary>
        public JobDispatchUnit Unit { get; set; }

        /// <summary>
        /// 作业调度频率 如每1天，每两周等(默认为1 即每天，每周，每月)
        /// </summary>

        public int Frequency { get; set; }

        /// <summary>
        /// 作业调度是否需要结束日期？ 如果不需要的话，则无限循环下去（默认不需要）
        /// </summary>
        public bool IsScheduleEndDate { get; set; }

        /// <summary>
        /// 按周调度时， WeekDay的集合(按周的时候有效，默认全部)
        /// </summary>
        public List<JobDispatchWeek> WeekList { get; set; }

        /// <summary>
        /// 以逗号隔开的周的字符串
        /// </summary>
        public string WeekArray { get; set; }

        /// <summary>
        /// 按月调度时，月份的集合(按月的时候有效，默认全部十二个月)
        /// </summary>
        public List<JobDispatchMonth> MonthList { get; set; }

        /// <summary>
        /// 以逗号隔开的月份的字符串
        /// </summary>
        public string MonthArray { get; set; }

        /// <summary>
        /// 按月调度类型(按月的时候有效，默认按绝对时间)
        /// </summary>
        public JobDispatchMonthType MonthScheduleType { get; set; }

        /// <summary>
        /// 按月绝对调度时候，明确哪一天(默认为1，但是要注意设置为31日的时候，不要设置在小月)
        /// </summary>
        public int MonthScheduleAbsoluteDay { get; set; }

        /// <summary>
        /// 按月相对调度， 明确第几个（默认第一个） 
        /// </summary>
        public JobDispatchMonthRelative MonthScheduleRelativeNumber { get; set; }

        /// <summary>
        /// 按月相对调度， 第几个星期几， 如第一个星期四，最后一天等（这里不包括工作日休息日等概念，默认星期一）
        /// </summary>
        public JobDispatchMonthRelativeUnit MonthScheduleRelativeUnit { get; set; }

        /// <summary>
        /// 具体到某一天时的调度策略，如执行一次还是按条件执行多次(默认一次)
        /// </summary>
        public JobDispatchDayType DayScheduleType { get; set; }

        /// <summary>
        /// 每天执行一次的时间(默认为绝对时间)
        /// </summary>
        public DateTime DayScheduleSingleTime { get; set; }

        /// <summary>
        /// 每天执行多次的起始时间
        /// </summary>
        public DateTime DayScheduleMultipleBeginTime { get; set; }

        /// <summary>
        /// 每天执行多次的时候，是否需要结束时间(默认为不要)
        /// </summary>
        public DateTime IsDayScheduleMultipleEndTime { get; set; }

        /// <summary>
        /// 每天执行多次的结束时间
        /// </summary>
        public DateTime DayScheduleMultipleEndTime { get; set; }

        /// <summary>
        /// 某天执行多次的间隔单位， 如每隔1个小时，还是每隔10分钟(默认为1)
        /// </summary>
        public JobDispatchDayUnit DayScheduleMultipleUnit { get; set; }

        /// <summary>
        /// 某天多次执行的间隔（默认小时）
        /// </summary>
        public int DayScheduleMultipleFrequency { get; set; }

        /// <summary>
        /// 将调度信息转换成合法的表达式
        /// "0 0 12 ? * WED" - which means "every Wednesday at 12:00 pm". 
        /// 第一个 秒 第二个 分 第三个 小时 第四个 月中的某一天 第五个 月 第六个 周中的某一天 第七个 年(可选项)
        /// </summary>
        /// <returns>Cron调度表达式</returns>
        public string GetCronExpression()
        {
            if (Unit == JobDispatchUnit.Minute) return string.Empty;

            //第一，判断月份
            string month = "*";

            //按照秒
            if (Unit == JobDispatchUnit.Second)
            {
                return string.Format("0/{0} * * * * ?", DayScheduleSingleTime.Second.ToString());
            }
            else if (Unit == JobDispatchUnit.Minute)
            {
                return string.Format("{0} 0/{1} * * * ?", DayScheduleSingleTime.Second.ToString(), DayScheduleSingleTime.Minute.ToString());
            }
            else if (Unit == JobDispatchUnit.Hour)
            {
                return string.Format("{0} {1} 0/{1} * * ?", DayScheduleSingleTime.Second.ToString(), DayScheduleSingleTime.Minute.ToString(),DayScheduleSingleTime.Hour.ToString());
            }
            else if (Unit == JobDispatchUnit.Week)
            {
                return string.Format("{0} {1} {2} ? * {0}", DayScheduleSingleTime.Second.ToString(), DayScheduleSingleTime.Minute.ToString(), DayScheduleSingleTime.Hour.ToString(), DayScheduleSingleTime.DayOfWeek.ToString());
            }



            if (Unit == JobDispatchUnit.Month)
            {
                if (string.IsNullOrEmpty(MonthArray))
                {
                    //列出哪几个月
                    if (MonthList != null && MonthList.Count > 0 && MonthList.Count < 12)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < MonthList.Count; i++)
                        {
                            sb.Append(MonthList[i].ToString());
                            if (i < MonthList.Count - 1)
                            {
                                sb.Append(",");
                            }
                        }

                        if (sb.Length > 0)
                        {
                            month = sb.ToString();
                        }
                    }
                }
                else
                {
                    month = MonthArray;
                }
            }

            //第二，判断Day-of-Week，如果是按周，则会选择哪几天，如果是按月，而且是相对时间的话，也可能会使用到星期
            string dayOfWeek = "*";
            if (Unit == JobDispatchUnit.Week)
            {
                if (string.IsNullOrEmpty(WeekArray))
                {
                    if (WeekList != null && WeekList.Count > 0 && WeekList.Count < 7)
                    {
                        //列出一周中明确的几天
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < WeekList.Count; i++)
                        {
                            sb.Append(WeekList[i].ToString());
                            if (i < WeekList.Count - 1)
                            {
                                sb.Append(",");
                            }
                        }

                        if (sb.Length > 0)
                        {
                            dayOfWeek = sb.ToString();
                        }
                    }
                }
                else
                {
                    dayOfWeek = WeekArray;
                }
            }
            else if (Unit == JobDispatchUnit.Month)
            {
                if (MonthScheduleType == JobDispatchMonthType.Relative)
                {
                    if (MonthScheduleRelativeUnit != JobDispatchMonthRelativeUnit.DAY)
                    {
                        if (MonthScheduleRelativeNumber != JobDispatchMonthRelative.Last)
                        {
                            //第几个星期几(6#3)
                            dayOfWeek = MonthScheduleRelativeUnit.ToString() + "#" + ((byte)MonthScheduleRelativeNumber).ToString();
                        }
                        else
                        {
                            //最后一个星期几(6L)
                            dayOfWeek = MonthScheduleRelativeUnit.ToString() + "L";
                        }
                    }
                }
            }

            //第三 判断Day-of-Month
            string dayOfMonth = "*";
            if (Unit == JobDispatchUnit.Month)
            {
                if (MonthScheduleType == JobDispatchMonthType.Absolute)
                {
                    dayOfMonth = MonthScheduleAbsoluteDay.ToString();
                    if (int.Parse(dayOfMonth) == 0)
                    {
                        dayOfMonth = "1";
                    }
                }
                else if (MonthScheduleType == JobDispatchMonthType.Relative)
                {
                    if (MonthScheduleRelativeUnit == JobDispatchMonthRelativeUnit.DAY)
                    {
                        dayOfMonth = "L";//最后一天
                    }
                }
            }

            //第四，时间判断
            string seconds = "*", minutes = "*", hours = "*";
            if (DayScheduleType == JobDispatchDayType.Single)
            {
                hours = DayScheduleSingleTime.Hour.ToString();
                minutes = DayScheduleSingleTime.Minute.ToString();
                seconds = DayScheduleSingleTime.Second.ToString();
            }
            else if (DayScheduleType == JobDispatchDayType.Multiple)
            {
                //每天执行多次,暂时不考虑
            }

            if (!dayOfWeek.Equals("?") && !dayOfMonth.Equals("?"))
            {
                if (dayOfWeek.Equals("*"))
                {
                    dayOfWeek = "?";
                }
                else if (dayOfMonth.Equals("*"))
                {
                    dayOfMonth = "?";
                }
            }

            // 频率(当频率大于1时的控制，在JobRunner中)
            if (Frequency == 0)
            {
                Frequency = 1;
            }

            return string.Format("{0} {1} {2} {3} {4} {5}", seconds, minutes, hours, dayOfMonth, month, dayOfWeek);
        }
    }
}
