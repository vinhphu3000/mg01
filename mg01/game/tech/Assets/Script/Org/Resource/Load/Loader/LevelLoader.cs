/* ==============================================================================
 * 场景加载器
 * @author jr.zeng
 * 2017/6/9 10:01:13
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class LevelLoader : AbstractLoader
    {
        LoadReqLevel m_loadReqLevel = null;
        LevelMgr.LevelData m_levelData = null;

        public LevelLoader()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {

            if (m_levelData.isDone)
            {
                //加载完成
                OnComplete();
            }
            else
            {
                SetProgress(m_levelData.progress);
            }

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        protected override void StartLoad()
        {

            m_loadReqLevel = m_loadReq as LoadReqLevel;

            //异步请求
            m_levelData = LevelMgr.me.LoadAsync(m_url, m_loadReqLevel.isAdditive, null, m_loadReq.referId);    //如果已经加载完成, 返回空
            if (m_levelData.isDone)
            {
                //加载成功
                OnComplete();
            }
            else
            {
                m_loadReq.OnStart();
                SchUpdate(true);
            }
        }

        protected override void __Stop()
        {
            if (m_levelData == null)
                return;
            SchUpdate(false);
            m_levelData = null;
        }

        protected override void __Close()
        {

            m_loadReqLevel = null;
        }


    }


}
