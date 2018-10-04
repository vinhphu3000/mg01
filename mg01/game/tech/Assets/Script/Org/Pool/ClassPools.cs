/* ==============================================================================
 * 类对象池的池
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace mg.org
{
    public class ClassPools
    {

        private static ClassPools __me;
        public static ClassPools me
        {   
            get
            {
                if (__me == null)//需要高频调用的, 不使用InstUtil
                    __me = new ClassPools();
                return __me;
            }
        }

        private Dictionary<Type, TypePool> m_type2pool = new Dictionary<Type, TypePool>();
        private Dictionary<Type, object> m_t2pool = new Dictionary<Type, object>();
        private Dictionary<string, BasePool> m_id2pool = new Dictionary<string, BasePool>();

        public ClassPools()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public T Pop<T>() where T : class
        {
            Type type = typeof(T);
            TypePool pool = CreatePool(type);
            return pool.Pop() as T;
        }

        //
        public object Pop(Type type_)
        {
            BasePool pool = CreatePool(type_);
            return pool.Pop();
        }

        public object Pop(string id_)
        {
            BasePool pool = GetPool(id_);
            return pool.Pop();
        }

        //回收对象
        public void Push(object obj_)
        {
            Type type = obj_.GetType();
            BasePool pool = GetPool(type);
            pool.Push(obj_);
        }

        public void Push(string id_, object obj_)
        {
            BasePool pool = GetPool(id_);
            pool.Push(obj_);
        }



        //清空全部空闲
        //public void ClearAllIdles(Type type_)
        //{
        //    BasePool pool = GetPool(type_);
        //    if (pool != null)
        //    {
        //        pool.ClearAllIdles();
        //    }
        //}

        //public void ClearAllIdles()
        //{
        //    if (m_type2pool.Count == 0)
        //        return;

        //    foreach (KeyValuePair<Type, ClassPool> kvp in m_type2pool)
        //    {
        //        ClassPool v = kvp.Value;
        //        v.ClearAllIdles();
        //    }
        //}

        //清空对象池
        public void Clear()
        {
            if (m_type2pool.Count > 0)
            {
                foreach (KeyValuePair<Type, TypePool> kvp in m_type2pool)
                {
                    TypePool v = kvp.Value;
                    v.Clear();
                }
                m_type2pool.Clear();
            }
            

            if (m_t2pool.Count > 0)
            {

                foreach (KeyValuePair<Type, object> kvp in m_t2pool)
                {
                    ClassUtil.CallMethod(kvp.Value, "Clear");
                }
                m_t2pool.Clear();
            }


            if (m_id2pool.Count > 0)
            {

                foreach (KeyValuePair<string, BasePool> kvp in m_id2pool)
                {
                    BasePool v = kvp.Value;
                    v.Clear();
                }
                m_id2pool.Clear();
            }

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //-------∽-★-∽------∽-★-∽--------∽-★-∽pool相关∽-★-∽--------∽-★-∽------∽-★-∽--------


        //-------∽-★-∽Type∽-★-∽--------//


        //创建对象池
        public TypePool CreatePool(Type type_)
        {
            if (m_type2pool.ContainsKey(type_))
                return m_type2pool[type_];
            TypePool pool = new TypePool(type_);
            m_type2pool[type_] = pool;
            return pool;
        }

        //获取对象池
        public TypePool GetPool(Type type_)
        {
            TypePool pool;
            if (m_type2pool.TryGetValue(type_, out pool))
                return pool;
            return null;
        }

        //清空对象池
        public void ClearPool(Type type_)
        {
            BasePool pool = GetPool(type_);
            if (pool != null)
            {
                pool.Clear();
            }
        }
        


        //-------∽-★-∽<T>∽-★-∽--------//
        

        public ClassPool2<T> CreatePool<T>() where T:class
        {
            Type t = typeof(T);
            if (m_t2pool.ContainsKey(t))
                return m_t2pool[t] as ClassPool2<T>;
            ClassPool2<T> pool = new ClassPool2<T>();
            m_t2pool[t] = pool;
            return pool;
        }


        public ClassPool2<T> GetPool<T>() where T : class
        {
            Type t = typeof(T);
            object pool;
            if (m_t2pool.TryGetValue(t, out pool))
                return pool as ClassPool2<T>;
            return null;
        }


        public void ClearPool<T>() where T : class
        {
            ClassPool2<T> pool = GetPool<T>();
            if (pool != null)
            {
                pool.Clear();
            }
        }


        //-------∽-★-∽id∽-★-∽--------//
        
        public BasePool CreatePool(string id_)
        {
            if (m_id2pool.ContainsKey(id_))
                return m_id2pool[id_];
            BasePool pool = new BasePool();     //创建基本池
            m_id2pool[id_] = pool;
            return pool;
        }


        public BasePool CreatePool(string id_, Type type_)
        {
            if (m_id2pool.ContainsKey(id_))
                return m_id2pool[id_];
            TypePool pool = new TypePool(type_);
            m_id2pool[id_] = pool;
            return pool;                                                                        
        }

        public BasePool CreatePool<T>(string id_)
        {
            return CreatePool(id_, typeof(T));
        }
        


        public BasePool GetPool(string id_)
        {
            BasePool pool;
            if (m_id2pool.TryGetValue(id_, out pool))
                return pool;
            return null;
        }


      





    }
}