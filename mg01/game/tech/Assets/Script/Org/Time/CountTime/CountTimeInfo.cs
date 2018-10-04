/* ==============================================================================
 * CountTimeInfo
 * @author jr.zeng
 * 2016/7/11 10:46:33
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{
    public class CountTimeInfo
    {

        private object m_id = null;

        private float m_cdTime = 0;
        private float m_passTime = 0;

        public CountTimeInfo(object id_, float cdTime_)
        {
            m_id = id_;
            m_cdTime = cdTime_;
        }

        public float PassTime
        {
            get { return m_passTime; }
        }

        public object Id
        {
            get { return m_id; }
        }


        public float CdTime
        {
            get { return m_cdTime; }
            set { m_cdTime = value; }
        }


        public float RemainTime
        {
            get
            {
                float remain = m_cdTime - m_passTime;
                //remain = Math.Max(0, remain);
                return remain;
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public void Update(float dt_)
        {
            m_passTime += dt_;
        }

        public void Reset()
        {
            m_passTime = 0;
        }
    }
}