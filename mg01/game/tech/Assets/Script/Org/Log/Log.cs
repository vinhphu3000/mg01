/* ==============================================================================
 * 日志
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public class LOG_LV
    {
        //调试
        public const int DEBUG = 0;
        //信息
        public const int INFO = 1;
        //警告
        public const int WARN = 2;
        //错误
        public const int ERROR = 3;
        //致命
        public const int FATAL = 4;
        //崩溃
        public const int ASSET = 5;

        public static Dictionary<int, string> LV2NAME = new Dictionary<int, string>()
        {
            {DEBUG, "DEBUG"},
            {INFO, "INFO"},
            {WARN,
#if UNITY_EDITOR
                "<color=yellow>" +
#endif
                "WARN"
#if UNITY_EDITOR
                 + "</color>"
#endif
            },
            {ERROR, "ERROR"},
            {FATAL, "FATAL"},
            {ASSET, "ASSET"},
        };

    }
    
    public class Log
    {

        //当前日志等级
        static public int log_lv = LOG_LV.DEBUG;

        static string time_format = "yy/MM/dd HH:mm:ss.fff";

        static Log()
        {

        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="message">打印的内容</param> 
        /// <param name="context">触发打印的对象</param>
        static public void Print(object message, object context = null)
        {
            if (context != null)
            {
                if (context is Object)
                {
                    UnityEngine.Debug.Log(message, context as Object);
                }
                else
                {
                    message = String.Format("[{0}] {1}", context.GetType().Name, message);
                    UnityEngine.Debug.Log(message);
                }
            }
            else
            {
                UnityEngine.Debug.Log(message);
            }
        }


        /// <summary>
        /// 打印调试
        /// </summary>
        /// <param name="message">打印的内容</param> 
        /// <param name="context">触发打印的对象</param>
        static public void Debug(object message, object context = null)
        {
            __LogInDetail(LOG_LV.DEBUG, message, context);
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="message">打印的内容</param> 
        /// <param name="context">触发打印的对象</param>
        static public void Info(object message, object context = null)
        {
            __LogInDetail(LOG_LV.INFO, message, context);
        }

        /// <summary>
        /// 打印警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static public void Warn(object message, object context = null)
        {
            __LogInDetail(LOG_LV.WARN, message, context);
        }


        /// <summary>
        /// 打印错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static public void Error(object message, object context = null)
        {
            __LogInDetail(LOG_LV.ERROR, message, context);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static public void Fatal(object message, object context = null)
        {
            __LogInDetail(LOG_LV.FATAL, message, context);
        }


        /// <summary>
        /// Assert
        /// </summary>
        /// <param name="condition_"></param>
        /// <param name="message_"></param>
        /// <param name="context_"></param>
        static public void Assert(bool condition_, object message_ = null, object context_ = null)
        {
            if (condition_)
                return;

            if (message_ == null)
                message_ = "";

            __LogInDetail(LOG_LV.ASSET, message_, context_);
        }

        static public void Assert(string message_, object context_ = null)
        {
            Assert(false, message_, context_);
        }


        static private void __LogInDetail(int level, object message, object context = null)
        {
            if (level < Log.log_lv)
                //等级不足
                return;

            //因为日志很耗时, 所以先暂停StopWatch
            //StopWatch.PauseST();

            string logName = LOG_LV.LV2NAME[level];
            string content; ;

            string time = DateTime.Now.ToString(time_format);

            if (context != null)
            {
                //有打印目标
                if (context is Object)
                {
                    //是unity对象
                    content = String.Format("[{0}][{1}] {2}", logName, time, message);
                    
                    if (level == LOG_LV.ASSET || level == LOG_LV.ERROR)
                        UnityEngine.Debug.Assert(false, content, context as Object);
                    else if(level == LOG_LV.WARN)
                        UnityEngine.Debug.LogWarning(content, context as Object);
                    else
                        UnityEngine.Debug.Log(content, context as Object);
                }
                else
                {
                    string tar;

                    if (context is Type)
                    {
                        tar = (context as Type).Name;
                    }
                    else if (context is string)
                    {
                        tar = context as string;
                    }
                    else
                    {
                        tar = context.GetType().Name;
                    }

                    content = String.Format("[{0}][{1}][{2}] {3}", logName, time, tar, message);

                    if (level == LOG_LV.ASSET || level == LOG_LV.ERROR)
                        UnityEngine.Debug.Assert(false, content);
                    //else if (level == LOG_LV.WARN)
                    //    UnityEngine.Debug.LogWarning(content);
                    else
                        UnityEngine.Debug.Log(content);
                }
            }
            else
            {
                content = String.Format("[{0}][{1}] {2}", logName, time, message);

                if (level == LOG_LV.ASSET || level == LOG_LV.ERROR)
                    UnityEngine.Debug.Assert(false, content);
                //else if (level == LOG_LV.WARN)
                //    UnityEngine.Debug.LogWarning(content);
                else
                    UnityEngine.Debug.Log(content);
            }
            
            //StopWatch.ResumeST();

        }


    }
}