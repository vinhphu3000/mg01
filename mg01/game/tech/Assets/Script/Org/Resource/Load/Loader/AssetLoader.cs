/* ==============================================================================
 * 资源加载器
 * @author jr.zeng
 * 2017/5/15 16:47:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class AssetLoader : AbstractLoader
    {

        AssetData m_assetData = null;


        public AssetLoader()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {

            if (m_assetData.isDone)
            {
                //加载完成
                if (m_assetData.asset != null)
                {
                    m_data = m_assetData.asset;
                    OnComplete();
                }
                else
                {
                    OnFail();
                }
            }
            else
            {
                SetProgress(m_assetData.progress);
            }
            
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        protected override void StartLoad()
        {
            //异步请求
            m_assetData = m_assetCache.LoadAsync(m_url, null, m_loadReq.referId);
            if (m_assetData.isDone)
            {
                //加载成功
                m_data = m_assetData.asset;
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
            if (m_assetData == null)
                return;
            SchUpdate(false);
            m_assetData = null;
        }
        


    }
    
}
