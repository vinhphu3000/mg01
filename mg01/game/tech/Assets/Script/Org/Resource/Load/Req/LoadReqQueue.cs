/* ==============================================================================
 * 加载请求_串行
 * @author jr.zeng
 * 2017/5/16 19:53:39
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class LoadReqQueue : LoadReq
    {
        //请求列表
        public LoadReq[] load_reqs;

        public LoadReqQueue()
        {
            type = LoadReqType.QUEUE;
        }

        public LoadReqQueue(LoadReq[] reqs_, CALLBACK_LoadReq on_complete_ = null) : this()
        {
            load_reqs = reqs_;
            on_complete = on_complete_;
        }
        
        

        /// <summary>
        /// 加载开始处理
        /// (会递归所有子项)
        /// </summary>
        public override void OnStart()
        {
            LoadMgr.AddReq(this);

            if (load_reqs != null)
            {
                foreach (var v in load_reqs)
                    v.OnStart();
            }
        }

        public override void OnDestroy()
        {
            LoadMgr.RemoveReq(this);

            if (load_reqs != null)
            {
                foreach (var v in load_reqs)
                    v.OnDestroy();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽引用者∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void SetRefer(object refer_)
        {
            string id = Refer.Format(refer_);
            m_referId = id;

            if (load_reqs != null)
            {
                foreach (var v in load_reqs)
                {
                    if (v.referId == null)
                    {
                        //还没有引用者
                        v.SetRefer(id);
                    }
                }
            }
        }

    }


}

