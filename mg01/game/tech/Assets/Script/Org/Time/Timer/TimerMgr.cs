/* ==============================================================================
 * TimerMgr
 * @author jr.zeng
 * 2016/7/11 17:14:17
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{
    public class TimerMgr
    {

        private int m_obj_id = 0;

        private Dictionary<int, TimerHandler> m_id2hdl;
        private List<int> m_delKeys_id2hdl;
        private int m_hdlNum = 0;

        private Dictionary<CALLBACK_1, int> m_fun2id;
        

        private bool m_isOpen = false;
        private bool m_invalid = false;

        private bool m_isAutoUpdate = true;

        public TimerMgr()
        {
            m_id2hdl = new Dictionary<int, TimerHandler>();
            m_fun2id = new Dictionary<CALLBACK_1, int>();

            m_delKeys_id2hdl = new List<int>();
        }

        //因为要实例化, 所以建一个单例
        static public TimerMgr inst
        {
            get  {  return InstUtil.Get<TimerMgr>(); }
        }

        public void Setup()
        {
            if (m_isOpen) return;
            m_isOpen = true;


            SetupEvent();
        }

        public void Clear()
        {
            if (!m_isOpen) return;
            m_isOpen = false;


            ClearEvent();
        }

        private void SetupEvent()
        {
            if (m_isAutoUpdate)
            {
                CCApp.SchUpdate(Step);
            }

        }

        private void ClearEvent()
        {

            CCApp.UnschUpdate(Step);

        }

        /// <summary>
        /// 是否自动更新
        /// </summary>
        public bool IsAutoUpdate
        {
            get { return m_isAutoUpdate; }
            set
            {
                if (m_isAutoUpdate == value)
                    return;
                m_isAutoUpdate = value;
                if (m_isOpen)
                {
                    if (value)
                    {
                        CCApp.SchUpdate(Step);
                    }
                    else
                    {
                        CCApp.UnschUpdate(Step);
                    }
                }
            }
        }

        private int AllocObjId()
        {
            ++m_obj_id;
            return m_obj_id;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

     
        public void Step(float dt_)
        {
            if (m_hdlNum == 0)
                return;

            m_invalid = true;

            TimerHandler hdl;
            foreach (KeyValuePair<int, TimerHandler> kvp in m_id2hdl)
            {
                hdl = kvp.Value;
                if (hdl.willDel)
                {
                    //将被删除
                    continue;
                }

                hdl.time -= dt_;
                if (hdl.time <= 0)
                {
                    --hdl.repeat;
                    if (hdl.repeat == 0)
                    {
                        //时间到了
                        hdl.willDel = true;
                        m_delKeys_id2hdl.Add(kvp.Key);
                    }
                    else
                    {
                        hdl.time = hdl.delay;
                    }

                    hdl.func(hdl.arg);

                    if (!m_invalid)
                    {
                        //在回调里被清空了
                        break;
                    }
                }
            }

            m_invalid = false;

            if (m_delKeys_id2hdl.Count > 0)
            {
                for (int i = 0; i < m_delKeys_id2hdl.Count; ++i)
                {
                    ClearTimeOut(m_delKeys_id2hdl[i]);
                }
                m_delKeys_id2hdl.Clear();
            }

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //-------∽-★-∽------∽-★-∽--------∽-★-∽SetTimeOut∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 延时调用
        /// </summary>
        /// <param name="fun_"></param>
        /// <param name="delay_"></param>
        /// <param name="repeat_">-1时无限循环</param>
        /// <returns>唯一序号</returns>
        public int SetTimeOut(CALLBACK_1 fun_, float delay_ = 0, int repeat_ = 1)
        {
            return SetTimeOutWithArg(fun_, null, delay_, repeat_);
        }

        /// <summary>
        /// 延时调用(带回调参数)
        /// </summary>
        /// <param name="fun_"></param>
        /// <param name="arg_"></param>
        /// <param name="delay_"></param>
        /// <param name="repeat_"></param>
        /// <returns></returns>
        public int SetTimeOutWithArg(CALLBACK_1 fun_, object arg_, float delay_ = 0, int repeat_ = 1)
        {
            int obj_id = AllocObjId();

            if (delay_ <= 0)
                delay_ = 0.001f;

            TimerHandler hdl = new TimerHandler(fun_, arg_);
            hdl.delay = delay_;
            hdl.time = delay_;
            hdl.repeat = repeat_;

            m_id2hdl[obj_id] = hdl;
            ++m_hdlNum;

            return obj_id;
        }

        public int ClearTimeOut(int objId_)
        {
            if (!m_id2hdl.ContainsKey(objId_))
            {
                return 0;
            }
            
            TimerHandler hdl = m_id2hdl[objId_];
            if (m_invalid)
            {
                //锁定中
                hdl.willDel = true;
                m_delKeys_id2hdl.Add(objId_);
                return 0;
            }

            m_id2hdl.Remove(objId_);
            
            if (m_fun2id.ContainsKey(hdl.func))
            {
                m_fun2id.Remove(hdl.func);
            }

            --m_hdlNum;
            return 0;
        }


        public void ClearAllTimeOut()
        {
            if (m_hdlNum == 0)
                return;
            m_hdlNum = 0;

            m_invalid = false;
            m_id2hdl.Clear();
            m_delKeys_id2hdl.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽CallDelay∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void CallDelay(CALLBACK_1 fun_, float delay_ = 0, int repeat_ = 1)
        {
            CallDelayWithArg(fun_, null, delay_, repeat_);
        }

        public void CallDelayWithArg(CALLBACK_1 fun_, object arg_, float delay_ = 0, int repeat_ = 1)
        {
            RemoveCallDelay(fun_);

            int obj_id = SetTimeOutWithArg(fun_, arg_, delay_, repeat_);
            m_fun2id[fun_] = obj_id;
        }

        public void RemoveCallDelay(CALLBACK_1 fun_)
        {
            if (!m_fun2id.ContainsKey(fun_))
                return;

            int id = m_fun2id[fun_];
            ClearTimeOut(id);
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽ScheduleUpdate∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 定时回调
        /// </summary>
        /// <param name="callback_"></param>
        /// <param name="time_">0时:每帧触发</param>
        public void ScheduleUpdate(CALLBACK_1 callback_, float time_=0)
        {
            CallDelay(callback_, time_, -1);
        }

        public void UnscheduleUpdate(CALLBACK_1 callback_)
        {
            RemoveCallDelay(callback_);
        }

        public void ScheduleUpdateOrNot(CALLBACK_1 callback_, bool b_)
        {
            if (b_)
                ScheduleUpdate(callback_);
            else
                UnscheduleUpdate(callback_);
        }

    }


    class TimerHandler
    {
        //回调函数
        public CALLBACK_1 func;
        //回调参数
        public object arg;

        public float delay = 0;
        public float time = 0;
        public int repeat = 0;

        public bool willDel = false;

        public TimerHandler(CALLBACK_1 func_, object arg_ = null)
        {
            func = func_;
            arg = arg_;
        }


    }

}