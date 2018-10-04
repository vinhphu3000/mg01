/* ==============================================================================
 * 基本对象池
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mg.org
{
    public class BasePool : IPool
    {
        protected Stack<object> m_objArr = new Stack<object>();

        public int capacity = 0;   //最大容量


        public BasePool()
        {


        }

        public int RemainCount
        {
            get { return m_objArr.Count; }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public virtual object Pop()
        {
            if (m_objArr.Count == 0)
                return null;
            object obj = m_objArr.Pop();
            return obj;
        }

        //public T Pop<T>() where T : class
        //{
        //    return Pop() as T;
        //}


        //回收对象
        public virtual void Push(object obj_)
        {
            if (m_objArr.Contains(obj_))
                Log.Assert("重复添加", this);

            m_objArr.Push(obj_);

            if (capacity > 0)
                CheckCapacity();
        }



        //清空全部空闲
        public void ClearAllIdles()
        {
            Clear();
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


    }

}