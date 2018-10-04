/* ==============================================================================
 * 倒计时器
 * @author jr.zeng
 * 2016/7/11 11:03:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{
    public class CountTimer : Subject
    {
        //id2信息
        private Dictionary<object, CountTimeInfo> m_id2info = new Dictionary<object, CountTimeInfo>();
        //字典删除列表
        private List<object> m_delArr_id2info = new List<object>();

        private int m_infoNum = 0;

        private float m_nfyTime = 0;
        //通知间隔
        private float m_nfyDelay = 1;

        private bool m_isAutoUpdate = true;

        public CountTimer()
        {

        }

        public void Clear()
        {
            if (m_infoNum == 0)
                return;
            m_infoNum = 0;
            m_id2info.Clear();
        }

        private void SetupEvent()
        {
            CCApp.SchUpdate(Step);
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
                if (!m_isAutoUpdate)
                {
                    if (m_infoNum > 0)
                    {
                        ClearEvent();
                    }
                }
            }
        }

        public bool Has(object id_)
        {
            return m_id2info.ContainsKey(id_);
        }

        public CountTimeInfo GetTimeInfo(object id_)
        {
            if (!m_id2info.ContainsKey(id_))
            {
                return null;
            }

            return m_id2info[id_];
        }

        /// <summary>
        /// 获取经过时间
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public float GetPassTime(object id_)
        {
            if (!m_id2info.ContainsKey(id_))
            {
                return 0;
            }

            return m_id2info[id_].PassTime;
        }

        /// <summary>
        /// 获取剩余时间
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public float GetRemainTime(object id_)
        {
            if (!m_id2info.ContainsKey(id_))
            {
                return 0;
            }

            return m_id2info[id_].RemainTime;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        public void Step(float dt_)
        {
            if (m_infoNum == 0)
                return;

            bool notify = false;

            m_nfyTime -= dt_;
            if (m_nfyTime <= 0)
            {
                m_nfyTime = m_nfyDelay;
                notify = true;
            }

            float remain = 0;
            foreach (CountTimeInfo info in m_id2info.Values)
            {

                info.Update(dt_);
                remain = info.RemainTime;
                if (remain <= 0)
                {
                    //时间到了
                    NotifyWithInfo(COUNT_TIMER_EVENT.RUNNING, info);
                    NotifyWithInfo(COUNT_TIMER_EVENT.COMPLETE, info);

                    m_delArr_id2info.Add(info.Id);
                }
                else
                {
                    if (notify)
                    {
                        NotifyWithInfo(COUNT_TIMER_EVENT.RUNNING, info);
                    }
                }
            }

            if (m_delArr_id2info.Count > 0)
            {
                for (int i = 0; i < m_delArr_id2info.Count; ++i)
                {
                    RemoveTime(m_delArr_id2info[i]);
                }
                m_delArr_id2info.Clear();
            }

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 添加倒计时
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="time_"></param>
        public void AddTime(object id_, float time_)
        {
            if (time_ <= 0)
            {
                RemoveTime(id_);
                return;
            }

            CountTimeInfo info;
            if (m_id2info.ContainsKey(id_))
            {
                info = m_id2info[id_];
                info.CdTime = time_;
            }
            else
            {
                info = new CountTimeInfo(id_, time_);
                AddInfo(info);
            }
        }

        private void AddInfo(CountTimeInfo info_)
        {
            if (m_id2info.ContainsKey(info_.Id))
                return;

            m_id2info[info_.Id] = info_;
            ++m_infoNum;

            if (m_infoNum == 1)
            {
                m_nfyTime = m_nfyDelay;

                if (m_isAutoUpdate)
                {
                    SetupEvent();
                }
            }

            NotifyWithInfo(COUNT_TIMER_EVENT.ADD, info_);
        }

        /// <summary>
        /// 移除倒计时
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public bool RemoveTime(object id_)
        {
            if (!m_id2info.ContainsKey(id_))
                return false;

            CountTimeInfo info = m_id2info[id_];
            m_id2info.Remove(id_);
            --m_infoNum;

            if (m_infoNum == 0)
            {
                if (m_isAutoUpdate)
                {
                    ClearEvent();
                }
            }

            NotifyWithInfo(COUNT_TIMER_EVENT.REMOVE, info);
            return true;
        }

        private void NotifyWithInfo(string type_, CountTimeInfo info_)
        {
            NotifyWithEvent(type_, info_);
        }



    }
}