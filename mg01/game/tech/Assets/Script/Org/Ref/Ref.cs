/* ==============================================================================
 * 引用计数
 * @author jr.zeng
 * 2016/12/9 15:43:32
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class Ref : Disposal, IRef
    {
    
        int m_ref_cnt = 0;
        HashSet<string> m_refHash = new HashSet<string>();

        AutoRelease m_autoPool = null;


        //断点打在析构函数, 第二次调式必然死机
        ~Ref()
        {
            Dispose(false);
        }

        public Ref()
        {

        }



        /// <summary>
        /// 引用+1
        /// </summary>
        public bool Retain(object refer_)
        {
            if (IsDisposed(true))
                return false;


            if (refer_ != null)
            {
                string referId = Refer.Format(refer_);
                if (m_refHash.Contains(referId))
                    //已被它引用
                    return false;
                m_refHash.Add(referId);
                Refer.AttachDispose(referId, OnReferDispose);   //监听引用者销毁
            }
            
            m_ref_cnt++;
            if (m_ref_cnt == 1)
            {
                
            }

            return true;
        }

        /// <summary>
        /// 引用-1
        /// </summary>
        public bool Release(object refer_)
        {
            if (IsDisposed(true))
                return false;

            if (refer_ != null)
            {
                string referId = Refer.Format(refer_);
                if (!m_refHash.Contains(referId))
                    //没被它引用
                    return false;
                m_refHash.Remove(referId);
                Refer.DetachDispose(referId, OnReferDispose);
            }
            
            if (m_ref_cnt > 0)
            {
                m_ref_cnt--;
                if (m_ref_cnt == 0)
                {
                    __OnRelease();
                }
            }
            else
            {
                Log.Assert("错误的引用计数", this);
            }
            return true;
        }


        public void OnReferDispose(object refer_)
        {
#if UNITY_EDITOR
            Log.Debug("<color=magenta>检测到引用者析构: " + Refer.Format(refer_)+"</color>", this);
#endif
            Release(refer_);
        }


        /// <summary>
        /// 持有计数
        /// </summary>
        public int RefCount
        {
            get { return m_ref_cnt; }
        }


        void ClearAllRefers()
        {

            m_ref_cnt = 0;

            if (m_refHash.Count > 0)
            {
                foreach (string kvp in m_refHash)
                {
                    Refer.DetachDispose(kvp, OnReferDispose);
                }
                m_refHash.Clear();
            }
        }
          

        protected virtual void __OnRelease()
        {
            Dispose();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽AutoRelease∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 自动释放
        /// </summary>
        public virtual void AutoRelease()
        {
            AutoRelease(CCApp.autoRelease);
        }

        public void AutoRelease(AutoRelease pool_)
        {
            if (IsDisposed(true))
                return;

            if (m_autoPool != null)
                return;
            m_autoPool = pool_;
            m_autoPool.Add(this);
        }

        void ClearAutoRelease()
        {
            if (m_autoPool == null)
                return;
            m_autoPool.Remove(this);    //从池里移除
            m_autoPool = null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Dispose模式∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing_">false为析构销毁</param>
        override public void Dispose(bool disposing_)
        {
            if (m_disposed)
                return;
            m_disposed = true;

#if UNITY_EDITOR
            Log.Debug("对象销毁:" + (disposing_ ? "正常 " : "析构 ") + name, this);
#endif

            ClearAutoRelease();
            ClearAllRefers();

            __Dispose(disposing_);

            NotifyDispose();
        }

    }

}