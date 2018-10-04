/* ==============================================================================
 * InstUtil
 * @author jr.zeng
 * 2016/9/18 12:15:32
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{
 

    public class InstUtil
    {

        private static Dictionary<Type, object> __tp2inst = new Dictionary<Type, object>();

        /// <summary>
        /// 获取单例, 有一定消耗, 尽量在低频调用时使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T Get<T>() where T : class, new()
        {
            Type t = typeof(T);
            if (__tp2inst.ContainsKey(t))
                return __tp2inst[t] as T;

            T obj = new T();
            __tp2inst[t] = obj;
            return obj;
        }

        /// <summary>
        /// 获取单例, 有一定消耗, 尽量在低频调用时使用
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        static public object Get(Type type_)
        {
            if (__tp2inst.ContainsKey(type_))
                return __tp2inst[type_];

            object obj = ClassUtil.New(type_);
            if (obj != null)
            {
                __tp2inst[type_] = obj;
            }
            return obj;
        }

        /// <summary>
        /// 获取单例, 有一定消耗, 尽量在低频调用时使用
        /// (不要频繁调用)
        /// </summary>
        /// <param name="typeName_"></param>
        /// <returns></returns>
        static public object Get(string typeName_)
        {
            Type type = Type.GetType(typeName_, true);
            return Get(type);
        }

        /// <summary>
        /// 添加单例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inst_"></param>
        /// <returns></returns>
        static public T Add<T>(T inst_) where T : class, new()
        {
            Type t = typeof(T);
            if (__tp2inst.ContainsKey(t))
            {
                Log.Assert(false, "already has Inst:" + t.Name);
                return __tp2inst[t] as T;
            }

            __tp2inst[t] = inst_;
            return inst_;
        }

        /// <summary>
        /// 移除单例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T Remove<T>() where T : class, new()
        {
            Type t = typeof(T);
            if (!__tp2inst.ContainsKey(t))
                return default(T);

            T obj = __tp2inst[t] as T;
            __tp2inst.Remove(t);
            return obj;
        }

        /// <summary>
        /// 移除所有单例
        /// </summary>
        static public void RemoveAll()
        {
            __tp2inst.Clear();
        }

    }


   

}