/* ==============================================================================
 * Refer
 * @author jr.zeng
 * 2017/6/4 14:31:17
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{


    //目前用于：
    //1. ImageAbs解决go托管问题
    //2. AssetCache引用计数
    //3. Subject不用手动Detach
        

    public class Refer //: IRefer   //不做基类使用了
    {
        //string m_referId;

        //public Refer(string referId_)
        //{
        //    m_referId = referId_;
        //}

        ///// <summary>
        ///// 引用者id
        ///// </summary>
        //public string ReferId
        //{
        //    get { return m_referId; }
        //}

        //public void NotifyDeactive()
        //{
        //    NotifyDeactive(this);
        //}
        //public void NotifyDispose()
        //{
        //    NotifyDispose(this);
        //}


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取引用id
        /// </summary>
        /// <param name="refer_"></param>
        /// <returns></returns>
        static public string Format(object refer_)
        {
            if (refer_ == null)
                return null;
            
            string str;
            if (refer_ is string)
            {
                str = refer_ as string;
            }
            else if (refer_ is IRefer)
            {
                str = (refer_ as IRefer).ReferId;
            }
            else
            {
                Log.Assert("不支持的Refer: " + refer_);
                return null;
            }

            if (string.IsNullOrEmpty(str))
                return null;

            return str;
        }

        static public bool Assert(object refer_)
        {
            if (refer_ is string)
            {
                return true;
            }
            else if (refer_ is IRefer)
            {
                return true;
            }

            Log.Assert("不正确的Refer: " + refer_);
            return false;
        }


        /// <summary>
        /// 转换为引用id
        /// </summary>
        /// <param name="refer_"></param>
        /// <returns></returns>
        static public string TransID(UnityEngine.Object refer_)
        {
            return refer_.GetType().Name + "#U3D#" + refer_.GetInstanceID();
        }


        static public void Dump()
        {
            //TODO
        }

        //static ISubject __subDeactive = new Subject();
        static ISubject __subDeactive = new Notifer();
        //static ISubject __subDispose = new Subject();
        static ISubject __subDispose = new Notifer();

        static public void ClearNotify()
        {
            __subDeactive.DetachAll();
            __subDispose.DetachAll();
        }

        //-------∽-★-∽------∽-★-∽NotifyDeactive∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 通知沉默
        /// </summary>
        /// <param name="refer_"></param>
        static public void NotifyDeactive(object refer_)
        {
            string referId = Format(refer_);
            if( __subDeactive.Notify(referId, referId) )
            {
                //通知完移除全部监听
                __subDeactive.DetachByType(referId);
            }

            __subDeactive.Notify(GL_EVENT.REFER_DEACTIVE, refer_);
        }

        static public void AttachDeactive(object refer_, CALLBACK_1 callback_)
        {
            string referId = Format(refer_);
            __subDeactive.Attach(referId, callback_, null);
        }

        static public void DetachDeactive(object refer_, CALLBACK_1 callback_)
        {
            string referId = Format(refer_);
            __subDeactive.Detach(referId, callback_);
        }

        static public void AttachDeactive(CALLBACK_1 callback_)
        {
            __subDeactive.Attach(GL_EVENT.REFER_DEACTIVE, callback_, null);
        }
        static public void DetachDeactive(CALLBACK_1 callback_)
        {
            __subDeactive.Detach(GL_EVENT.REFER_DEACTIVE, callback_);
        }

        //-------∽-★-∽------∽-★-∽NotifyDispose∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 通知析构
        /// </summary>
        /// <param name="refer_"></param>
        static public void NotifyDispose(object refer_)
        {
            string referId = Format(refer_);
            if (__subDispose.Notify(referId, referId) )
            {
                //通知完移除全部监听
                __subDispose.DetachByType(referId);
            }

            //__subDeactive.Notify(GL_EVENT.REFER_DISPOSE, refer_);
        }

        /// <summary>
        /// 监听析构，(暂时是retain用到)
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="callback_"></param>
        static public void AttachDispose(object refer_, CALLBACK_1 callback_)
        {
            string referId = Format(refer_);
            __subDispose.Attach(referId, callback_, null);
        }

        static public void DetachDispose(object refer_, CALLBACK_1 callback_)
        {
            string referId = Format(refer_);
            __subDispose.Detach(referId, callback_);
        }

        static public void AttachDispose(CALLBACK_1 callback_)
        {
            __subDeactive.Attach(GL_EVENT.REFER_DISPOSE, callback_, null);
        }

        static public void DetachDispose(CALLBACK_1 callback_)
        {
            __subDeactive.Detach(GL_EVENT.REFER_DISPOSE, callback_);
        }


    }
}