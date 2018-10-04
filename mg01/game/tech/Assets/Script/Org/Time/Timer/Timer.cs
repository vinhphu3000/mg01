/* ==============================================================================
 * 计时器
 * @author jr.zeng
 * 2016/7/10 15:51:47
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{
    public class Timer : Subject
    {
        //运行中
        private bool m_running = false;
        //是否补帧
        private bool m_isCompeateOn = false;
        //是否自动更新
        private bool m_isAutoUpdate = true;

        //计时间隔
        private float m_delay = 0;
        //经过时间
        private float m_passTime = 0;

        //当前次数
        private int m_curCount = 0;
        //重复次数
        private int m_repeatCount = 0;

        public Timer(float delay_ = 0.001f, int repeat_ = 0)
        {
            m_delay = delay_;
            m_repeatCount = repeat_;
        }

        public bool Running
        {
            get
            {
                return m_running;
            }
        }

        /// <summary>
        /// 计时间隔
        /// </summary>
        public float Delay
        {
            set { m_delay = value; }
            get { return m_delay; }
        }

        /// <summary>
        /// 当前次数
        /// </summary>
        public int CurCount
        {
            get { return m_curCount; }
        }

        /// <summary>
        /// 重复次数
        /// </summary>
        public int RepeatCount
        {
            set
            {
                m_repeatCount = value;
                if (m_repeatCount <= 0)
                {
                    m_repeatCount = 0;
                    m_curCount = 0;
                }
                else if (m_repeatCount <= m_curCount)
                {//小于当前次数

                }
            }

            get { return m_repeatCount; }
        }

        /// <summary>
        /// 是否补帧
        /// </summary>
        public bool IsCompeateOn
        {
            set { m_isCompeateOn = value; }
            get { return m_isCompeateOn; }
        }

        /// <summary>
        /// 是否
        /// </summary>
        public bool IsAutoUpdate
        {
            get { return m_isAutoUpdate; }
            set
            {
                if (m_isAutoUpdate == value)
                    return;
                m_isAutoUpdate = value;
                if (!m_isAutoUpdate)
                {
                    if (m_running)
                    {
                        CCApp.UnschUpdate(Step);
                    }
                }
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        

        public void Step(float delta_)
        {
            if (!m_running)
                return;

            m_passTime += delta_;
            if (m_passTime < m_delay)
                return;

            while (m_passTime >= m_delay)
            {
                if (m_isCompeateOn)
                {   //补帧
                    NotifyWithEvent(TIMER_EVENT.TIMER, m_delay);
                }
                else
                {
                    NotifyWithEvent(TIMER_EVENT.TIMER, m_passTime);
                }

                m_passTime -= m_delay;

                if (m_repeatCount > 0)
                {
                    ++m_curCount;
                    if (m_curCount >= m_repeatCount)
                    {
                        Reset();
                        NotifyWithEvent(TIMER_EVENT.COMPLETE);
                    }
                }

                if (!m_isCompeateOn)
                {
                    //不补帧时,触发完成
                    break;
                }
            }

            m_passTime = 0; //每次触发后, 重置经过时间
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public void Start()
        {
            if (m_running)
                return;
            m_running = true;

            if (m_isAutoUpdate)
            {
                CCApp.SchUpdate(Step);
            }
        }

        public void Stop()
        {
            if (!m_running)
                return;
            m_running = false;

            if (m_isAutoUpdate)
            {
                CCApp.UnschUpdate(Step);
            }
        }

        public void Reset()
        {
            Stop();
            m_curCount = 0;
        }


    }
}