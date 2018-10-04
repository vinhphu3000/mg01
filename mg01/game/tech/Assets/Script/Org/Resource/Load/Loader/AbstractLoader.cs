/* ==============================================================================
 * 抽象加载器
 * @author jr.zeng
 * 2017/5/13 16:01:39
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{

    public abstract class AbstractLoader : Subject
    {
        protected AssetCache m_assetCache = AssetCache.me;
        
        // 启用事件
        public bool evt_enabled = false;
        
        //加载请求
        protected LoadReq m_loadReq = null;

        //加载路径
        protected string m_url = null;
        protected string m_url_str = null;

        //加载回来的数据
        protected Object m_data = null;
        //进度( 0到1，0表示任何数据都没有开始下载，1表示下载完毕 )
        protected float m_progress = 0;
        //当前错误内容
        protected string m_errorStr = null;


        protected bool m_isOpen = false;
        //加载中
        protected bool m_loading = false;
        //是否完成
        protected bool m_done = false;

        private bool m_isSchUpdte = false;


        public AbstractLoader()
        {

        }

        /// <summary>
        ///加载中 
        /// </summary>
        public bool Loading { get { return m_loading; } }

        /// <summary>
        /// 当前进度(0~1)
        /// </summary>
        public float Progress { get { return m_progress; } }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string ErrorStr { get { return m_errorStr; } }

        /// <summary>
        /// 数据
        /// </summary>
        public Object Data { get { return m_data; } }

        /// <summary>
        /// 已完成
        /// </summary>
        public bool isDone { get { return m_done; } }
        
        public string Url { get { return m_url; } }

        public string UrlStr { get { return m_url_str; } }

        public LoadReq Req { get { return m_loadReq; } }

        protected virtual void __print(string content_)
        {
            //Log.Info(content_, this);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //设置当前进度
        protected void SetProgress(float progress_)
        {
            if (m_progress == progress_)
                return;
            m_progress = progress_;

            if (m_loadReq != null)
                m_loadReq.progress = progress_;

            //Log.Debug("load progress: " + progress_);
            NotifyAsynEvt(LOAD_EVT.PROGRESS, m_progress);
        }

        //派发异步事件
        protected bool NotifyAsynEvt(string type_, object data_ = null)
        {
            //m_isAsyn = true;
            if (evt_enabled)
            {
                //异步加载, 才派发事件
                return NotifyWithEvent(type_, data_);
            }
            return false;
        }


        protected void SchUpdate(bool b_)
        {
            if (m_isSchUpdte == b_)
                return;
            m_isSchUpdte = b_;
            CCApp.SchUpdateOrNot(Step, b_);
        }


        protected virtual void Step(float delta_)
        {
            
        }


        //加载完成处理
        protected void OnComplete()
        {
            if(m_url_str != null)
                __print("●load complete: " + m_url_str);

            Stop();
            SetProgress(1);

            m_done = true;

            if (m_loadReq != null)
            {
                m_loadReq.done = true;
                m_loadReq.data = m_data;

                LoadReq req = m_loadReq;    //先保存引用, 因为在派发事件时, 可能会关闭加载

                req.OnComplete();
                

                //派发事件要放在最后, 因为有可能在这里重启加载
                NotifyAsynEvt(LOAD_EVT.COMPLETE, req);
            }
            else
            {
                NotifyAsynEvt(LOAD_EVT.COMPLETE);
            }

        }


        //加载失败处理
        protected void OnFail()
        {
            if (m_errorStr == null)
                m_errorStr = "load fail:" + m_url_str;

            __print("×" + m_errorStr);
            
            Stop();
            SetProgress(1);

            m_data = null;
            m_done = true;


            if (m_loadReq != null)
            {
                m_loadReq.done = true;
                m_loadReq.error_str = m_errorStr;   //记录错误码

                LoadReq req = m_loadReq;    //先保存引用, 因为在派发事件时, 可能会关闭加载

                //失败时,不回调完成
                //req.OnComplete();

                NotifyAsynEvt(LOAD_EVT.FAIL, req);
            }
            else
            {
                NotifyAsynEvt(LOAD_EVT.FAIL);
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path_"></param>
        /// <param name="on_complete_"></param>
        /// <returns></returns>
        public LoadReq LoadAsync(string path_, CALLBACK_LoadReq on_complete_ = null)
        {
            LoadReq req = new LoadReq(path_, on_complete_);
            return LoadAsync(req);
        }

        public LoadReq LoadAsync(LoadReq req_)
        {
            return ExcuteLoad(req_);
        }
        

        protected virtual LoadReq ExcuteLoad(LoadReq req_)
        {
            if (m_loadReq == req_)
                return m_loadReq;

            //关闭当前加载
            Close();


            m_loadReq = req_;
            m_url = m_loadReq.url;
            m_url_str = m_url;
            
            m_progress = 0;
            m_data = null;
            m_errorStr = null;

            m_isOpen = true;
            m_loading = true;
            m_done = false;


            __print("○load start: " + m_url_str);

            NotifyAsynEvt(LOAD_EVT.START, m_loadReq);
            
            if(m_assetCache.HasAsset(m_url))
            {
                //已加载,立刻完成
                m_data = m_assetCache.GetAsset(m_url);
                OnComplete();
            }
            else
            {
                StartLoad();
            }

            return m_loadReq;
        }

        //开始加载
        protected virtual void StartLoad()
        {

            
        }


        /// <summary>
        /// 停止加载
        /// </summary>
        public void Stop()
        {
            if (!m_loading)
                return;
            m_loading = false;
            __Stop();
        }
        
        protected virtual void __Stop()
        {
            

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (!m_isOpen)
                return;
            m_isOpen = false;

            Stop();

            __Close();

            if (m_loadReq != null)
            {
                m_loadReq.OnDestroy();//销毁
                m_loadReq = null;
            }

            m_data = null;
            m_errorStr = null;
            m_progress = 0;
            m_done = false;
        }

        protected virtual void __Close()
        {


        }
        
    }

}