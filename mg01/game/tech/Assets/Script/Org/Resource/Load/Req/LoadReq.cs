/* ==============================================================================
 * 加载请求
 * @author jr.zeng
 * 2017/5/15 16:51:55
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class LoadReq
    {
        public LoadReqType type = LoadReqType.DEFAULT;

        //资源路径
        public string path = "";    //具有代表性的路径

        //完成回调( 目的是能加载完成后能精确回调 )
        public CALLBACK_LoadReq on_complete;    
        

        //资源数据
        public Object data;
        //加载进度
        public float progress = 0;
        //是否完成
        public bool done = false;
        //错误码
        public string error_str = null;
        //用户数据
        public object userData = null;

        //public bool fromPool = false;   //想做成可回收
        
        //引用者
        protected string m_referId = null;

        public LoadReq()
        {

        }

        public LoadReq(string path_, CALLBACK_LoadReq on_complete_ = null)
        {
            path = path_;
            on_complete = on_complete_;
        }

        /// <summary>
        /// 资源地址
        /// </summary>
        public virtual string url
        {
            get { return path; }
        }

        /// <summary>
        /// 加载开始
        /// </summary>
        public virtual void OnStart()
        {
           LoadMgr.AddReq(this);
           
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        public virtual void OnComplete()
        {
            LoadMgr.RemoveReq(this);

            if (on_complete != null)
                on_complete(this);
            
        }

        /// <summary>
        /// 销毁请求(完成或中断都可能触发销毁)
        /// </summary>
        public virtual void OnDestroy()
        {
            LoadMgr.RemoveReq(this);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽引用者∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //暂时没用??
        public virtual void SetRefer(object refer_)
        {
            m_referId = Refer.Format(refer_);

        }

        public string referId
        {
            get { return m_referId; }
        }
        


    }


}
