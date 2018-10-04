/* ==============================================================================
 * Notifer
 * @author jr.zeng
 * 2017/10/10 10:06:20
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;



namespace mg.org
{

    /// <summary>
    /// 优点：效率高
    /// 缺点：事件派发时切断监听还是会被调用；不支持refer
    /// </summary>
    public class Notifer : ISubject
    {

        static ClassPool2<SubjectEvent> __evtPool = ClassPools.me.CreatePool<SubjectEvent>();

        Dictionary<string, CALLBACK_1> m_id2fun = new Dictionary<string, CALLBACK_1>();

        public Notifer()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件监听∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void Attach(string type_, CALLBACK_1 callback_, object refer_ = null)
        {
            if (callback_ == null)
                return;

            if (!m_id2fun.ContainsKey(type_))
            {
                m_id2fun[type_] = callback_;
            }
            else
            {
                m_id2fun[type_] -= callback_;
                m_id2fun[type_] += callback_;
            }
        }


        public void Detach(string type_, CALLBACK_1 callback_)
        {
            if (callback_ == null)
                return;

            if (m_id2fun.ContainsKey(type_))
            {
                m_id2fun[type_] -= callback_;

                if (m_id2fun[type_] == null)
                    m_id2fun.Remove(type_);
            }
        }

        public void DetachByType(string type_)
        {
            if (m_id2fun.ContainsKey(type_))
            {
                m_id2fun[type_] = null;
                m_id2fun.Remove(type_);
            }
        }


        public bool HasAttach(string type_)
        {
            return m_id2fun.ContainsKey(type_);
        }

        public void DetachAll()
        {
            m_id2fun.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件派发∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public bool Notify(string type_, object data_)
        {
            if (!m_id2fun.ContainsKey(type_))
                return false;
            m_id2fun[type_](data_);
            return true;
        }

        public bool NotifyEvent(SubjectEvent evt_)
        {
            if (!m_id2fun.ContainsKey(evt_.type))
                return false;
            m_id2fun[evt_.type](evt_);
            return true;
        }

        public bool NotifyWithEvent(string type_, object data_ = null)
        {
            if (!m_id2fun.ContainsKey(type_))
                return false;

            //SubjectEvent evt = new SubjectEvent(type_, data_);
            SubjectEvent evt = __evtPool.Pop();
            evt.type = type_;
            evt.data = data_;

            bool b = NotifyEvent(evt);
            evt.Clear();

            __evtPool.Push(evt);

            return b;
        }


    }
}