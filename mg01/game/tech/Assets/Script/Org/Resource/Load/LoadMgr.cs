/* ==============================================================================
 * LoadMgrNew
 * @author jr.zeng
 * 2017/5/24 16:25:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public class LoadMgr
    {

        static QueueLoader m_mainLoader;
        static CALLBACK_0 m_mainLoadBack = null;

        static AbstractLoader m_subLoader;

        static IPop m_loadView;//当前加载条

        static LoadMgr()
        {

            m_mainLoader = new QueueLoader();
            m_mainLoader.evt_enabled = true;

            m_subLoader = new MultiLoader();
            
        }



        static public void Setup()
        {

            SetupEvent();
        }

        static public void Clear()
        {
            ClearEvent();

            StopMainLoad();

            m_subLoader.Close();

            RemoveAllReq();
        }

        static void SetupEvent()
        {
            m_mainLoader.Attach(LOAD_EVT.COMPLETE, onMainLoadEvt, null);
            Refer.AttachDeactive(onReferDeactive); //监听所有的deactive

        }

        static void ClearEvent()
        {
            m_mainLoader.Detach(LOAD_EVT.COMPLETE, onMainLoadEvt);
            Refer.DetachDeactive(onReferDeactive);

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static void onReferDeactive(object refer_)
        {
            RemoveRefer(refer_);
        }


        static void onMainLoadEvt(object evt_)
        {
            SubjectEvent e = evt_ as SubjectEvent;
            switch (e.type)
            {
                case LOAD_EVT.COMPLETE:
                    //加载完成
                    if (m_loadView == null)
                    {
                        //没有加载条, 直接完成
                        OnMainLoadComplete();
                    }

                    break;
                case LOAD_EVT.VIEW_COMPLETE:
                    
                    OnMainLoadComplete();

                    break;
            }

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽主加载∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public void DoMainLoad(LoadReq req_, CALLBACK_0 onComplete_, string viewTp_)
        {

            m_mainLoader.LoadAsync(req_);

            if (m_mainLoader.Loading)
            {
                ShowLoadView(viewTp_);
                m_mainLoadBack = onComplete_;
            }
            else
            {
                if (onComplete_ != null)
                    onComplete_();
            }

        }

        static public void StopMainLoad()
        {
            ClearLoadView();

            m_mainLoader.Close();
            m_mainLoadBack = null;
        }


        //加载完成返回
        static void OnMainLoadComplete()
        {
            CALLBACK_0 back = m_mainLoadBack;
            StopMainLoad();
            if (back != null)
            {
                back();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽后台加载∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="req_"></param>
        /// <param name="onComplete_"></param>
        /// <returns></returns>
        static public LoadReq LoadAsync(LoadReq req_, CALLBACK_LoadReq onComplete_ = null)
        {
            if (onComplete_ != null)
            {
                req_.on_complete -= onComplete_;
                req_.on_complete += onComplete_;
            }

            LoadReq req = m_subLoader.LoadAsync(req_);
            return req;
        }
        
        
        static public void StopAsync(CALLBACK_LoadReq onComplete_)
        {
            if (m_reqHash.Count == 0)
                return;

            foreach (var req in m_reqHash)
            {
                req.on_complete -= onComplete_;
            }
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载条∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static IPop ShowLoadView(string type_)
        {
            if (string.IsNullOrEmpty(type_))
            {
                ClearLoadView();
                return null;
            }

            IPop view = KUI.KUIApp.PopMgr.Show(type_, m_mainLoader);
            //ImgAbs2 view = CreateLoadView(type_);

            if (m_loadView == view)
                return view;

            ClearLoadView();
            
            m_loadView = view;
            if (m_loadView != null)
            {
                m_loadView.Attach(LOAD_EVT.VIEW_COMPLETE, onMainLoadEvt, null);
            }
            
            return null;
        }

        static void ClearLoadView()
        {
            if (m_loadView == null)
                return;
            //m_loadView.Destroy();
            KUI.KUIApp.PopMgr.Close(m_loadView.popID);
            m_loadView = null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载引用管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        static HashSet<LoadReq> m_reqHash = new HashSet<LoadReq>(); //处于加载中的所有加载请求

        static public void AddReq(LoadReq req_)
        {
            if (m_reqHash.Contains(req_))
                return;
            m_reqHash.Add(req_);

            //Log.Debug("AddReq:" + (req_.url ?? req_.GetType().Name));
        }

        static public void RemoveReq(LoadReq req_)
        {
            if (!m_reqHash.Contains(req_))
                return;
            m_reqHash.Remove(req_);

            //Log.Debug("RemoveReq:" + (req_.url ?? req_.GetType().Name));
        }

        static void RemoveAllReq()
        {
            if (m_reqHash.Count == 0)
                return;
            m_reqHash.Clear();
        }

        /// <summary>
        /// 移除加载中的引用者
        /// </summary>
        /// <param name="refer_"></param>
        static public void RemoveRefer(object refer_)
        {
            if (m_reqHash.Count == 0)
                //这字典大部分时间应该会空
                return;

            string referId = Refer.Format(refer_);
            foreach (var req in m_reqHash)
            {
                if (req.referId == referId)
                {
                    req.on_complete = null;     //通过移除回调来实现
                }
            }
        }

    }



}
