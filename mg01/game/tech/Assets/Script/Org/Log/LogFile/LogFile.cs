/* ==============================================================================
 * 日志文件
 * @author jr.zeng
 * 2017/6/16 15:22:15
 * ==============================================================================*/

using System;
using System.IO;
using System.Diagnostics;

using UnityEngine;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

namespace mg.org
{
    public class LogFile
    {
        static string fold_name = "Log";   //文件夹路径

        static AsyncLog __asyncLog = null;

        static LogFile()
        {

        }
        
        public static void Run()
        {
            CreateLogFile();
        }

        public static void Close()
        {
            CloseLogFile();
        }

        // 以下都是设备上的路径
        static string HomePath
        {
            get
            {
#if UNITY_EDITOR
                return System.IO.Path.GetFullPath(Application.dataPath + "/..");
#else
				return Application.persistentDataPath;
#endif
            }
        }

        //日志文件路径(真机上才用)
        static string LogPath { get { return HomePath + "/log.txt"; } }
        //备份文件路径
        static string BackPath { get { return HomePath + "/log.bak"; } }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽文件创建∽-★-∽--------∽-★-∽------∽-★-∽--------//



        //创建日志文件
        static void CreateLogFile()
        {

            if (__asyncLog != null)
                return;

//#if !UNITY_STANDALONE
#if UNITY_STANDALONE
            //windows
            if (!Directory.Exists(fold_name))
                Directory.CreateDirectory(fold_name);   //创建文件夹

            var filePath = fold_name + "/Log " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
#else
			var	filePath = LogPath;
            
			try
			{
				if( File.Exists( filePath ) )   
				{
                    //有上次的日志
					if( File.Exists( BackPath ) )
						File.Delete( BackPath );
					File.Move( filePath, BackPath );   //备份 
				}
			}
			catch
			{

			}
#endif
            __asyncLog = new AsyncLog(filePath);

            Application.logMessageReceived += UnityLogCallback; //监听unitys的log
            
            Debug.Log("start log file: " + filePath);
        }

        static void UnityLogCallback(string condition, string stackTrace, LogType type)
        {

            if (type == LogType.Warning)
                return;

            try
            {
                condition = "|"+type+"| -> " + condition;   //|Log| -> [2017-10-10 17:50:20.857] [debug][env>UnityTest]  asasas  
                Write(condition, stackTrace);
                //Write(type, condition, stackTrace);
                //if (type == LogType.Error || type == LogType.Exception)
                //{
                //    if ((ErrorEvent != null) && needToUpload)
                //    {
                //        ErrorEvent(condition, stackTrace, type);
                //        needToUpload = false;
                //    }
                //}
            }
            catch
            {

            }
        }

        static void CloseLogFile()
        {
            if (__asyncLog == null)
                return;

            Application.logMessageReceived -= UnityLogCallback;
            __asyncLog.close();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽文件操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public static void Flush()
        {
            if (__asyncLog != null)
            {
                __asyncLog.flush();
            }
        }

        public static void Write(string message_)
        {
            if (__asyncLog != null)
            {
                __asyncLog.push(new AsyncLog.Info(message_));
            }
        }

        public static void Write(params object[] lines)
        {
            if (__asyncLog != null)
            {
                __asyncLog.push(new AsyncLog.Info(lines));
            }
        }

      

    }

}