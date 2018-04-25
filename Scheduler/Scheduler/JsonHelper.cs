using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Scheduler
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将指定对象序列化成XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T t)
        {
            using (var sw = new StringWriter())
            {
                try
                {
                    var xz = new XmlSerializer(typeof(T));
                    xz.Serialize(sw, t);
                    return sw.ToString();
                }
                catch (Exception e)
                {
                    //LogHelper.Log(e.Message + e.StackTrace);
                }

                return "";
            }
        }

        public static T XmlDeserialize<T>(string s)
        {
            using (var sr = new StringReader(s))
            {
                try
                {
                    var xz = new XmlSerializer(typeof(T));
                    return (T)xz.Deserialize(sr);
                }
                catch (Exception e)
                {
                    //LogHelper.Log("======XML文件内容格式如下============：");
                    //LogHelper.Log(s);
                    //LogHelper.Log("可以使用如下语句进行查询：");
                    //LogHelper.Log("SELECT * FROM TCRT_OBJECT A WHERE o_type=10 and dbms_lob.instr(o_body,'改为关键词查询',1,1)>0");
                    //LogHelper.Log(e.Message + e.StackTrace);
                }
            }

            return default(T);
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }

        /// <summary>
        /// 将对象序列为Json数据格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            var result = JsonConvert.SerializeObject(obj);
            return result;
        }

        /// <summary>
        /// 将Json数据格式的字符串反序列化为一个对象
        /// </summary>
        /// <returns></returns>
        public static T Deserialize<T>(string josnString)
        {
            try
            {
                if (string.IsNullOrEmpty(josnString))
                {
                    josnString = string.Empty;
                }
                var obj = JsonConvert.DeserializeObject<T>(josnString);
                return obj;
            }
            catch (Exception e)
            {
                string s = e.InnerException != null ? e.InnerException.ToString() : e.ToString();

                if (s.Contains("Could not cast or convert from"))
                {
                    s = "数据类型格式错误，请检查输入项。";
                }

                throw new Exception(s);
            }
        }
    }
}
