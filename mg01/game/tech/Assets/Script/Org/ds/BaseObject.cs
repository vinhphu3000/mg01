/* ==============================================================================
 * Baseobj
 * @author jr.zeng
 * 2016/12/16 11:24:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{

    public class BaseObject : IRefer
    {

        //对象唯一id
        int __objId = 0;
        //类型名称
        string __typeName;
        //对象名称
        protected string m_name;

        public BaseObject()
        {
            __objId = AllocUtil.GetObjId();
            __typeName = GetType().Name;

            m_name = __typeName;
        }
        
        //public static implicit operator bool(BaseObj exists)
        //{
        //    return exists != null;
        //}

        /// <summary>
        /// 对象id
        /// </summary>
        public int obj_id  { get { return __objId; } }

        /// <summary>
        /// 类型名称
        /// </summary>
        public String TypeName { get { return __typeName; } }

        /// <summary>
        /// 对象名称
        /// </summary>
        virtual public String name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        
        public virtual string ReferId
        {
            get { return __typeName + "#CS#" + __objId; }
        }

        public void NotifyDeactive()
        {
            Refer.NotifyDeactive(this);
        }

        public void NotifyDispose()
        {
            Refer.NotifyDispose(this);
        }


        //不能加这个，不然lua绑定会判断为bool
        //public static implicit operator bool(Baseobj exists)
        //{
        //    return exists != null;
        //}

    }


}