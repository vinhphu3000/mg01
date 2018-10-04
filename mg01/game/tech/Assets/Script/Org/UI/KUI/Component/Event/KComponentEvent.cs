/* ==============================================================================
 * K控件事件派发器
 * @author jr.zeng
 * 2017/6/16 14:24:30
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    /// <summary>
    /// 
    /// </summary>
    public class KComponentEvent : UnityEvent
    {

        private DelegateList m_delegateList = new DelegateList();

        public KComponentEvent() : base()
        {

        }

        public new void AddListener(UnityAction listener)
        {
            if (m_delegateList.Add(listener) == true)
            {
                base.AddListener(listener);
            }
        }

        public new void RemoveListener(UnityAction listener)
        {
            if (m_delegateList.Remove(listener) == true)
            {
                base.RemoveListener(listener);
            }
        }

        //static

        public static KComponentEvent GetEvent(ref KComponentEvent evt)
        {
            if (evt == null)
                evt = new KComponentEvent();
            return evt;
        }

        public static void InvokeEvent(KComponentEvent evt)
        {
            if (evt != null)
                evt.Invoke();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KComponentEvent<T> : UnityEvent<T>
    {
        private DelegateList m_delegateList = new DelegateList();

        public KComponentEvent() : base()
        {

        }

        public new void AddListener(UnityAction<T> listener)
        {
            if (m_delegateList.Add(listener) == true)
            {
                base.AddListener(listener);
            }
        }

        public new void RemoveListener(UnityAction<T> listener)
        {
            if (m_delegateList.Remove(listener) == true)
            {
                base.RemoveListener(listener);
            }
        }

        //static

        public static KComponentEvent<T> GetEvent(ref KComponentEvent<T> evt)
        {
            if (evt == null)
                evt = new KComponentEvent<T>();
            return evt;
        }

        public static void InvokeEvent(KComponentEvent<T> evt, T t)
        {
            if (evt != null)
                evt.Invoke(t);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class KComponentEvent<T, U> : UnityEvent<T, U>
    {
        private DelegateList m_delegateList = new DelegateList();

        public KComponentEvent() : base()
        {

        }

        public new void AddListener(UnityAction<T, U> listener)
        {
            if (m_delegateList.Add(listener) == true)
            {
                base.AddListener(listener);
            }
        }

        public new void RemoveListener(UnityAction<T, U> listener)
        {
            if (m_delegateList.Remove(listener) == true)
            {
                base.RemoveListener(listener);
            }
        }

        //static

        public static KComponentEvent<T, U> GetEvent(ref KComponentEvent<T, U> evt)
        {
            if (evt == null)
                evt = new KComponentEvent<T, U>();
            return evt;
        }

        public static void InvokeEvent(KComponentEvent<T, U> evt, T t, U u)
        {
            if (evt != null)
                evt.Invoke(t, u);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class KComponentEvent<T, U, V> : UnityEvent<T, U, V>
    {
        private DelegateList m_delegateList = new DelegateList();

        public KComponentEvent() : base()
        {

        }

        public new void AddListener(UnityAction<T, U, V> listener)
        {
            if (m_delegateList.Add(listener) == true)
            {
                base.AddListener(listener);
            }
        }

        public new void RemoveListener(UnityAction<T, U, V> listener)
        {
            if (m_delegateList.Remove(listener) == true)
            {
                base.RemoveListener(listener);
            }
        }

        //static

        public static KComponentEvent<T, U, V> GetEvent(ref KComponentEvent<T, U, V> evt)
        {
            if (evt == null)
                evt = new KComponentEvent<T, U, V>();
            return evt;
        }

        public static void InvokeEvent(KComponentEvent<T, U, V> evt, T t, U u, V v)
        {
            if (evt != null)
                evt.Invoke(t, u, v);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="W"></typeparam>
    public class KComponentEvent<T, U, V, W> : UnityEvent<T, U, V, W>
    {
        private DelegateList m_delegateList = new DelegateList();

        public KComponentEvent() : base()
        {

        }

        public new void AddListener(UnityAction<T, U, V, W> listener)
        {
            if (m_delegateList.Add(listener) == true)
            {
                base.AddListener(listener);
            }
        }

        public new void RemoveListener(UnityAction<T, U, V, W> listener)
        {
            if (m_delegateList.Remove(listener) == true)
            {
                base.RemoveListener(listener);
            }
        }

        //static

        public static KComponentEvent<T, U, V, W> GetEvent(ref KComponentEvent<T, U, V, W> evt)
        {
            if (evt == null)
                evt = new KComponentEvent<T, U, V, W>();
            return evt;
        }

        public static void InvokeEvent(KComponentEvent<T, U, V, W> evt, T t, U u, V v, W w)
        {
            if (evt != null)
                evt.Invoke(t, u, v, w);
        }
    }



    /// <summary>
    /// 
    /// </summary>
    class DelegateList
    {
        private List<Delegate> m_delegates;
        public DelegateList()
        {
            m_delegates = new List<Delegate>();
        }

        public bool Add(Delegate d)
        {
            if (m_delegates.IndexOf(d) > -1)
            {
                Log.Warn(string.Format("重复添加事件监听 Target {0} Method {1}", d.Target, d.Method.Name));
                return false;
            }

            m_delegates.Add(d);
            return true;
        }

        public bool Remove(Delegate d)
        {
            int index = m_delegates.IndexOf(d);
            if (index == -1)
            {
                Log.Warn(string.Format("未添加过该事件监听 Target {0} Method {1}", d.Target, d.Method.Name));
                return false;
            }

            m_delegates.RemoveAt(index);
            return true;
        }


    }

}
