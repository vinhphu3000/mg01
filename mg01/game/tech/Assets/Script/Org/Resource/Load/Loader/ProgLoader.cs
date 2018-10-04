/* ==============================================================================
 * 外部进度加载器
 * @author jr.zeng
 * 2017/12/7 9:34:34
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{
    public class ProgLoader : AbstractLoader
    {

        LoadReqProg m_loadReqProg;
        IProgress m_iProg;

        public ProgLoader()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {
            if (m_iProg.isDone)
            {
                OnComplete();
            }
            else
            {
                SetProgress(m_iProg.progress);
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected override void StartLoad()
        {
            m_loadReqProg = m_loadReq as LoadReqProg;
            m_iProg = m_loadReqProg.iProg;

            m_loadReq.OnStart();
            SchUpdate(true);

            if (m_loadReqProg.onStartLoad != null)
                m_loadReqProg.onStartLoad(m_iProg);

        }

        protected override void __Stop()
        {
            SchUpdate(false);
        }

        protected override void __Close()
        {

            m_loadReqProg = null;
            m_iProg = null;
        }


    }

}