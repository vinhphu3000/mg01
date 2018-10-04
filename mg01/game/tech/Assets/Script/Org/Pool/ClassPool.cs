/* ==============================================================================
 * ClassPool2
 * @author jr.zeng
 * 2018/9/25 10:34:59
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    
    public class ClassPool2<T> where T : class
    {
        protected Stack<T> m_objArr = new Stack<T>();

        public int capacity = 0;   //最大容量

        //预分配数量
        int m_chunk = 5;

        public ClassPool2()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public virtual T Pop()
        {
            if (m_objArr.Count > 0)
            {
                return m_objArr.Pop();
            }

            T obj;
            if (m_chunk > 1)
            {
                //预分配
                AllocChunk(m_chunk);
                obj = m_objArr.Pop();
            }
            else
            {
                obj = Activator.CreateInstance<T>();
            }
            return obj;
        }

        //回收对象
        public virtual void Push(T obj_)
        {
            if (m_objArr.Contains(obj_))
                Log.Assert("重复添加", this);

            m_objArr.Push(obj_);

            if (capacity > 0)
                CheckCapacity();
        }


        public int RemainCount
        {
            get { return m_objArr.Count; }
        }

        

        //清空对象池
        public void Clear()
        {
            m_objArr.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 检测最大容量
        /// </summary>
        protected void CheckCapacity()
        {
            if (m_objArr.Count <= capacity)
                return;

            int cutNum = Mathf.FloorToInt(capacity * 0.3f);   //砍掉3分一
            if (cutNum <= 0)
                return;

#if UNITY_EDITOR
            if (cutNum < 3)
            {
                Log.Warn("这么少量就别搞了", this);
            }
            Log.Debug(string.Format("砍掉{0}个", cutNum), this);
#endif
            for (int i = 0; i < cutNum; ++i)
            {
                m_objArr.Pop();
            }
        }


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
        public void AllocChunk(int num_, bool isAdd_ = false)
        {
            if (!isAdd_ && m_objArr.Count >= num_)
                //已经有那么多了
                return;

            T obj;
            int len = isAdd_ ? num_ : num_ - m_objArr.Count;
            for (int i = 0; i < len; ++i)
            {
                obj = Activator.CreateInstance<T>();
                m_objArr.Push(obj);
            }
        }




    }
}