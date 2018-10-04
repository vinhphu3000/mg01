/* ==============================================================================
 * DelayLoader
 * @author jr.zeng
 * 2017/6/9 10:38:21
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class DelayLoader : AbstractLoader
    {

        LoadReqDelay m_loadReqDelay;

        float m_delay = 0;
        float m_time = 0;

        public DelayLoader()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {

            m_time += (float)delta_;
            if(m_time >= m_delay)
            {
                OnComplete();
            }
            else
            {
                SetProgress(m_time / m_delay);
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        protected override void StartLoad()
        {
            m_loadReqDelay = m_loadReq as LoadReqDelay;
            m_delay = m_loadReqDelay.delay;
            m_time = 0;

            m_loadReq.OnStart();
            SchUpdate(true);
        }

        protected override void __Stop()
        {
            SchUpdate(false);
        }

        protected override void __Close()
        {

            m_loadReqDelay = null;
        }

    }


}
