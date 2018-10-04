/* ==============================================================================
 * 码表
 * @author jr.zeng
 * 2017/1/4 17:44:31
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace mg.org
{

    public class StopWatch
    {

        bool m_running;

        System.Diagnostics.Stopwatch m_sw = new System.Diagnostics.Stopwatch();

        public void Start()
        {
            //__sw = new Stopwatch();
            //__sw.Reset();
            m_sw.Start();
            m_running = true;
        }


        /// <summary>
        /// 停止耗时记录,并返回消耗的时间
        /// </summary>
        /// <param name="log_"></param>
        /// <returns></returns>
        public double Stop(bool log_ = true)
        {
            if (!m_running)
                return 0;
            m_running = false;

            m_sw.Stop();
            double ms = m_sw.Elapsed.TotalMilliseconds;  //  总毫秒数
            m_sw.Reset();

            if (log_)
                Log.Debug("本次耗时:" + ms + " ms");
            return ms;
        }

        public void Pause()
        {
            if (!m_running)
                return;
            m_sw.Stop();
        }

        public void Resume()
        {
            if (!m_running)
                return;
            m_sw.Start();
        }

        public double Milliseconds
        {
            get
            {
                if (m_running)
                {
                    //m_sw.Stop();
                    double ms = m_sw.Elapsed.TotalMilliseconds;  //  总毫秒数
                    //m_sw.Start();
                    return ms;
                }
                else
                {
                    return 0;
                }
            }
        }
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        static StopWatch __sw = new StopWatch();

        static public void StartST()
        {
            __sw.Start();
        }

        static public void PauseST()
        {
            __sw.Pause();
        }

        static public void ResumeST()
        {
            __sw.Resume();
        }

        /// <summary>
        /// 停止耗时记录,并返回消耗的时间
        /// </summary>
        /// <param name="log_"></param>
        /// <returns></returns>
        static public double StopST(bool log_ = true)
        {
            return __sw.Stop(log_);
        }



    }

}