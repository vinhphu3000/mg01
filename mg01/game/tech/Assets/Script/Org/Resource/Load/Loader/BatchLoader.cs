/* ==============================================================================
 * 并行加载器
 * @author jr.zeng
 * 2017/5/17 18:18:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public class BatchLoader : AbstractLoader
    {
        protected LoadReqBatch m_loadReqQue;

        protected List<AbstractLoader> m_loaders = new List<AbstractLoader>();
        protected int m_load_num = 0;


        protected AbstractLoader tmp_loader;
        protected bool tmp_b;
        protected float tmp_progress;

        public BatchLoader()
        {
            //log_enable = false;
        }

        protected AbstractLoader StartLoader(LoadReq req_)
        {
            AbstractLoader loader = LoaderFactory.GetLoader(req_.type);
            loader.LoadAsync(req_);
            return loader;
        }


        protected void CloseLoader(AbstractLoader loader_)
        {
            LoaderFactory.CloseLoader(loader_);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {
            tmp_b = true;
            tmp_progress = 0;
            for (int i = 0; i < m_load_num; ++i)
            {
                tmp_loader = m_loaders[i];
                tmp_progress += tmp_loader.Progress;
                if (!tmp_loader.isDone)
                {
                    tmp_b = false;  //需要全部完成
                }
            }

            tmp_loader = null;

            if (tmp_b)
            {
                m_data = m_loaders[0].Data;   //取第一个
                OnComplete();
            }
            else
            {
                tmp_progress /= m_load_num;
                //Log.Debug(tmp_progress);
                SetProgress(tmp_progress);
            }

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected override LoadReq ExcuteLoad(LoadReq req_)
        {
            if (m_loadReq == req_)
                return m_loadReq;

            //关闭当前加载
            Close();

            m_loadReqQue = req_ as LoadReqBatch;
            m_load_num = m_loadReqQue.load_reqs.Length;
            if (m_load_num == 0)
            {
                Log.Assert("加载列表为空", this);
                return m_loadReqQue;
            }

            m_loadReq = m_loadReqQue;
            m_url = m_loadReqQue.load_reqs[0].url;
            m_url_str = m_url + (m_load_num > 1 ? " <...>" : "");
            
            m_progress = 0;
            m_data = null;
            m_errorStr = null;

            m_isOpen = true;
            m_done = false;
            m_loading = true;

            __print("○load start: " + m_url_str);

            NotifyAsynEvt(LOAD_EVT.START, m_loadReq);

            StartLoad();

            return m_loadReq;
        }


        protected override void StartLoad()
        {
            LoadReq[] load_reqs = m_loadReqQue.load_reqs;
            for (int i = 0; i < m_load_num; ++i)
            {
                AbstractLoader loader = StartLoader(load_reqs[i]);
                m_loaders.Add(loader);
            }

            Step(0);

            if (!isDone)
            {
                m_loadReq.OnStart();
                SchUpdate(true);
            }

        }

        protected override void __Stop()
        {
            if (m_loaders.Count == 0)
                return;

            for (int i = 0; i < m_loaders.Count; ++i)
            {
                tmp_loader = m_loaders[i];
                CloseLoader(tmp_loader);
            }
            tmp_loader = null;
            m_loaders.Clear();

            SchUpdate(false);
        }


        protected override void __Close()
        {

            m_loadReqQue = null;
        }

       


    }
}
