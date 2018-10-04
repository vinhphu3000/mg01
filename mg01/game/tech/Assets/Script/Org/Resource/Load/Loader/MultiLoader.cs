/* ==============================================================================
 * 混合加载器
 * @author jr.zeng
 * 2017/5/18 19:39:39
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{

    public class MultiLoader : AbstractLoader
    {
        List<AbstractLoader> m_loaders = new List<AbstractLoader>();

        bool tmp_b;
        AbstractLoader tmp_loader;

        public MultiLoader()
        {

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {

            tmp_b = true;
            for (int i = m_loaders.Count-1; i >=0 ; --i)
            {
                tmp_loader = m_loaders[i];
                if (tmp_loader.isDone)
                {
                    LoaderFactory.CloseLoader(tmp_loader);
                    m_loaders.RemoveAt(i);
                }
                else
                {
                    tmp_b = false;
                }
            }

            tmp_loader = null;

            if (tmp_b)
            {
                OnComplete();
            }

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected override LoadReq ExcuteLoad(LoadReq req_)
        {
            LoadReq req = req_;
            AbstractLoader loader = LoaderFactory.GetLoader(req_.type);
            
            req = loader.LoadAsync(req);
            if (loader.isDone)
            {
                LoaderFactory.CloseLoader(loader);
                return req;
            }

            req.OnStart();

            //m_isAsync = isAsync_;
            //m_progress = 0;
            //m_data = null;
            //m_errorStr = null;

            m_isOpen = true;
            m_done = false;
            m_loading = true;

            m_loaders.Add(loader);
            SchUpdate(true);

            //__print("○load start: " + loader.UrlStr);
            NotifyAsynEvt(LOAD_EVT.START, req);

            return req;
        }

        protected override void __Stop()
        {
            if (m_loaders.Count > 0)
            {
                for (int i=0; i < m_loaders.Count; ++i)
                {
                    tmp_loader = m_loaders[i];
                    LoaderFactory.CloseLoader(tmp_loader);
                }
                m_loaders.Clear();
            }
            
            SchUpdate(false);
        }


    }


}