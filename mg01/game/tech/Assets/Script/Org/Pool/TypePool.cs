/* ==============================================================================
 * 类对象池
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace mg.org
{

    public class TypePool : BasePool
    {

        //对象类型
        private Type m_objType;
        //预分配数量
        int m_chunk = 5;

        public TypePool(Type type_)
        {
            m_objType = type_;

        }

        public Type objType
        {
            get { return m_objType; }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        

        override public object Pop()
        {
            object obj = null;
            if (m_objArr.Count > 0)
            {
                obj = m_objArr.Pop();
            }

            if (obj != null)
                return obj;

            if (m_chunk > 1)
            {
                //预分配
                AllocChunk(m_chunk);
                obj = m_objArr.Pop();
            }
            else
            {
                obj = Activator.CreateInstance(m_objType);
            }

            return obj;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public int chunk
        {
            get { return m_chunk; }
            set
            {
                if (capacity > 0 && value > capacity)
                    Log.Assert("预分配数量不要大于容量上限");
                m_chunk = chunk;
            }
        }

        /// <summary>
        /// 预分配
        /// </summary>
        /// <param name="num_"></param>
        /// <param name="isAdd_">是否增量</param>
        public void AllocChunk(int num_, bool isAdd_=false)
        {
            if (!isAdd_ && m_objArr.Count >= num_)
                //已经有那么多了
                return;

            object obj;
            int len = isAdd_ ? num_ : num_ - m_objArr.Count;
            for (int i=0; i<len; ++i)
            {
                obj = Activator.CreateInstance(m_objType);
                m_objArr.Push(obj);
            }
        }
        
      

       


    }
}