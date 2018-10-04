/* ==============================================================================
 * 串行加载器
 * @author jr.zeng
 * 2017/5/16 11:29:58
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public class QueueLoader : AbstractLoader
    {
        AbstractLoader m_loader;

        LoadReqQueue m_loadReqQue;

        int m_load_num = 0;
        int m_load_idx = 0;

        public QueueLoader()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        override protected void Step(float delta_)
        {
            CheckLoaded();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected override LoadReq ExcuteLoad(LoadReq req_)
        {
            if (m_loadReq == req_)
                return m_loadReq;

            m_loadReqQue = req_ as LoadReqQueue;

            //关闭当前加载
            Close();

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
            m_load_idx = -1;
            LoadNext();

            if(!isDone)
            {
                m_loadReq.OnStart();
                SchUpdate(true);
            }
        }

        void LoadNext()
        {
            m_load_idx++;
            if (m_load_idx >= m_load_num)
            {
                OnComplete();
            }
            else
            {
                LoadOne();
            }
        }

        void LoadOne()
        {

            LoadReq req_ = m_loadReqQue.load_reqs[m_load_idx];

            if (m_loader == null || m_loader.Req.type != req_.type)
            {
                LoaderFactory.CloseLoader(m_loader);
                m_loader = LoaderFactory.GetLoader(req_.type);
            }

            m_loader.LoadAsync(req_);

            CheckLoaded();
        }

        //检测加载进度
        private void CheckLoaded()
        {
            if (m_loader.isDone)
            {
                //if (m_loader.ErrorStr != null)
                //{
                //    m_errorStr = m_loader.ErrorStr;
                //}
                //else
                //{
                    //if (m_data == null)
                        //m_data = m_loader.Data;
                //}

                if (m_data == null)
                    m_data = m_loader.Data;
                LoadNext();
            }
            else
            {
                CheckProgress();
            }
        }

        private void CheckProgress()
        {
            float percent = m_load_idx + m_loader.Progress; //例如 2+0.4
            percent = percent / m_load_num;
            SetProgress(percent);
            //Log.Debug("Progress: " + m_progress, this);
        }

        protected override void __Stop()
        {
            LoaderFactory.CloseLoader(m_loader);
            m_loader = null;
            SchUpdate(false);
        }
        
        protected override void __Close()
        {

            m_loadReqQue = null;
        }

    }
}
