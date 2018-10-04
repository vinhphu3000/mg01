/* ==============================================================================
 * Disposal
 * @author jr.zeng
 * 2016/12/14 10:07:23
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    public class Disposal : BaseObject, System.IDisposable
    {

        protected bool m_disposed = false;

        public Disposal()
        {

        }



        //供程序员显式调用的Dispose方法
        public void Dispose()
        {
            //if (Application.isPlaying)
            //{
            //    if (m_ref_cnt > 0)
            //        Log.Error("还在被引用", this);
            //}

            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            System.GC.SuppressFinalize(this);
        }

        //protected的Dispose方法，保证不会被外部调用。
        //传入bool值disposing以确定是否释放托管资源
        public virtual void Dispose(bool disposing_)
        {
            if (m_disposed)
                return;
            m_disposed = true;

            Log.Debug("Ref Dispose: " + disposing_, this);

            if (disposing_)
            {
                //TODO:在这里加入清理"托管资源"的代码，应该是xxx.Dispose();

            }
            else
            {

            }

            //TODO:在这里加入清理"非托管资源"的代码
            __Dispose(disposing_);

            NotifyDispose();
        }


        /// <summary>
        /// 析构函数
        /// </summary>
        /// <param name="disposing_">是否主动dispose</param>
        virtual protected void __Dispose(bool disposing_)
        {

        }



        protected bool IsDisposed(bool alarm_ = false)
        {
            if (m_disposed && alarm_)
                Log.Assert("对象已销毁", this);
            return m_disposed;
        }
    }

}