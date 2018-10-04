/* ==============================================================================
 * AsyncLog
 * @author jr.zeng
 * 2017/6/16 15:07:02
 * ==============================================================================*/

using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using ThreadSafeCollections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace mg.org
{
    public class AsyncLog
    {
        
        static StringBuilder sb;
        string filepath;
        LockFreeQueue<Info> lfq;
        Thread logThread;
        ManualResetEvent _eventExit = new ManualResetEvent(false);
        AutoResetEvent _eventFlush = new AutoResetEvent(false);

        public AsyncLog(string outputFile)
        {
            sb = new StringBuilder(1024);
            filepath = outputFile;
            lfq = new LockFreeQueue<Info>();
            logThread = new Thread(new ThreadStart(RunLog));
            logThread.Start();
        }

        public void state()
        {
            Debug.Log("ThreadState:" + logThread.ThreadState.ToString());
        }


        public void push(Info item)
        {
            lfq.Enqueue(item);
        }

        public void flush()
        {
            _eventFlush.Set();
        }

        public void close()
        {
            lfq.Enqueue(new Info("AsyncLog close"));

            _eventFlush.Set();
            _eventExit.Set();
        }

        void RunLog()
        {
            using (StreamWriter sw = File.AppendText(filepath))
            {
                bool has_msg = false;
                while (true)
                {
                    //等待500毫秒Flush一次
                    _eventFlush.WaitOne(1);
                    Info item;
                    while (true)
                    {
                        lfq.Dequeue(out item);
                        if (item != null)
                        {
                            sw.WriteLine(item.ToString());
                            has_msg = true;
                        }
                        else
                        {
                            if (has_msg)
                            {
                                sw.Flush();
                                has_msg = false;
                            }
                            break;
                        }
                    }
                    if (_eventExit.WaitOne(1))
                    {
                        while (lfq.Dequeue(out item))
                        {
                            sw.WriteLine(item.ToString());
                            has_msg = true;
                        }
                        if (has_msg)
                            sw.Flush();
                        break;
                    }
                }
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Info∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static readonly Regex LineRegex = new Regex(@"([\r\n]+)", RegexOptions.Singleline);
        static readonly Regex DateTimeRegex = new Regex(@"^\d+.\d+.\d+\s+\d+.\d+.\d+.\s+((am|pm)\s+)?(.+)$", RegexOptions.Singleline);

        public class Info
        {
            public object[] Lines;

            public Info(params object[] lines)
            {
                Lines = lines;
            }

            override public string ToString()
            {
                try
                {
                    var line = LineRegex.Replace(Lines[0].ToString(), "\r\n|\t");
                    var match = DateTimeRegex.Match(line);

                    if (match.Success)
                        line = match.Groups[3].Value;

                    sb.AppendFormat("{0}\r\n", line); //首行

                    for (var i = 1; i < Lines.Length; ++i)  //从第二行开始
                    {
                        var other = Lines[i].ToString();
                        sb.AppendFormat("|\t{0}\r\n", LineRegex.Replace(other, "\r\n|\t"));
                    }

                    string ret = sb.ToString();
                    sb.Remove(0, sb.Length);
                    return ret;
                }
                catch (System.Exception e)
                {
                    return "Log ToString Error" + e;
                }
            }
        }


    }
}