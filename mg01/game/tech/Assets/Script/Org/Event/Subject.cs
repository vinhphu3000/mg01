/* ==============================================================================
 * 观察者
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mg.org
{

    public class Subject : ISubject
    {
        static ClassPool2<SubjectEvent> __evtPool = ClassPools.me.CreatePool<SubjectEvent>();
        static ClassPool2<Observer> __obsPool = ClassPools.me.CreatePool<Observer>();
        static ClassPool2<List<Observer>> __obsArrPool = ClassPools.me.CreatePool<List<Observer>>();
        
        int m_objNum = 0;
        int m_invalid = 0;

        Dictionary<string, List<Observer>> m_id2obsArr = new Dictionary<string, List<Observer>>();
        static Subject()
        {
            //初始化对象池细节
            __obsPool.capacity = 100;
            __obsPool.chunk = 20;
            __obsArrPool.capacity = 100;
            __obsArrPool.chunk = 20;
        }

        public Subject()
        {

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽对象池∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        //创建obs列表
        List<Observer> CreateObsArr(string type_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null)
            {
                arr = __obsArrPool.Pop();
                m_id2obsArr[type_] = arr;
            }
            return arr;
        }

        void RemoveObsArr(string type_)
        {
            List<Observer> obsArr;
            if(m_id2obsArr.TryGetValue(type_, out obsArr) )
            {
                obsArr.Clear();
                m_id2obsArr.Remove(type_);
                __obsArrPool.Push(obsArr);

                Log.Debug("回收obsArr:" + __obsArrPool.RemainCount, this);
            }

        }

        //获取obs列表
        List<Observer> GetObsArr(string type_)
        {
            if (!m_id2obsArr.ContainsKey(type_))
                return null;
            return m_id2obsArr[type_];
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件监听∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="callback_"></param>
        /// <param name="refer_"></param>
        public void Attach(string type_, CALLBACK_1 callback_, object refer_)
        {
            if (callback_ == null)
                return;

            if (HasAttach(type_, callback_))
                //已添加监听
                return;

            List<Observer> arr = CreateObsArr(type_);

            Observer obs = __obsPool.Pop();
            obs.Init(type_, callback_, arr, refer_);

            if(arr.Count > 0 && arr[arr.Count-1] == null)
            {
                arr[arr.Count - 1] = obs;
            }
            else
            {
                arr.Add(obs);
            }

            m_objNum++;
        }
        

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="callback_"></param>
        /// <returns></returns>
        public void Detach(string type_, CALLBACK_1 callback_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null )
                return;
            

            if (arr.Count > 0)
            {
                Observer obs;
                for (int i = arr.Count - 1; i >= 0; --i)
                {
                    obs = arr[i];
                    if (obs != null)
                    {
                        if (obs.func == callback_)
                        {
                            obs.Clear();
                            __obsPool.Push(obs);

                            if (m_invalid > 0)
                            {
                                arr[i] = null;
                            }
                            else
                            {
                                arr.RemoveAt(i);
                            }

                            m_objNum--;
                            //break;    可能会有多个
                        }
                    }
                }
            }


            if(arr.Count == 0)
            {
                RemoveObsArr(type_);
            }
            
        }


        /// <summary>
        /// 按类型移除所有监听
        /// </summary>
        /// <param name="type_"></param>
        public void DetachByType(string type_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null)
                return;
            

            if (arr.Count > 0)
            {
                Observer obs;
                for (int i = arr.Count - 1; i >= 0; --i)
                {
                    obs = arr[i];
                    if (obs != null)
                    {
                        obs.Clear();
                        __obsPool.Push(obs);

                        arr[i] = null;
                        m_objNum--;
                    }
                }
            }
            
            RemoveObsArr(type_);
        }
        

        public void DetachBy_Type_Refer(string type_, object refer_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null)
                return;

            Refer.Assert(refer_);

            if (arr.Count > 0)
            {
                Observer obs;
                for (int i = arr.Count - 1; i >= 0; --i)
                {
                    obs = arr[i];
                    if (obs != null)
                    {
                        if (obs.refer == refer_)
                        {
                            obs.Clear();
                            __obsPool.Push(obs);

                            if (m_invalid > 0)
                            {
                                arr[i] = null;
                            }
                            else
                            {
                                arr.RemoveAt(i);
                            }

                            m_objNum--;
                            //break;    可能会有多个
                        }
                    }
                }
            }


            if (arr.Count == 0)
            {
                RemoveObsArr(type_);
            }
        }

        public void DetachByRefer(string type_, object refer_)
        {
            //TODO
        }
        /// <summary>
        /// 是否已添加监听
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="callback_"></param>
        /// <returns></returns>
        public bool HasAttach(string type_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null)
                return false;
            return arr.Count > 0;
        }

        public bool HasAttach(string type_, CALLBACK_1 callback_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null)
                return false;

            if (arr.Count == 0)
                return false;

            Observer obs;
            for (int i = 0, len = arr.Count; i < len; ++i)
            {
                obs = arr[i];
                if (obs != null)
                {
                    if (obs.func == callback_)
                        return true;
                }
            }
            return false;
        }



      
        //清空观察者
        public void DetachAll()
        {
            if (m_objNum == 0)
                return;
            m_objNum = 0;

            foreach (var kvp in m_id2obsArr)
            {
                if (kvp.Value.Count > 0)
                {
                    foreach (var obs in kvp.Value)
                    {
                        if (obs != null)    //可能刚detach就clear,此时obs会空
                        {
                            obs.Clear();
                            __obsPool.Push(obs);
                        }
                    }
                    kvp.Value.Clear();
                }

                __obsArrPool.Push(kvp.Value);
            }

            m_id2obsArr.Clear();
            m_invalid = 0;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件派发∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //通知
        public bool Notify(string type_, object data_)
        {
            List<Observer> arr = GetObsArr(type_);
            if (arr == null)
                return false;
            return Invoke(type_, arr, data_);
        }

        //通知事件
        public bool NotifyEvent(SubjectEvent evt_)
        {
            evt_.curTarget = this;

            List<Observer> arr = GetObsArr(evt_.type);
            if (arr == null)
                return false;
            bool b = InvokeEvent(evt_.type, arr, evt_);

            evt_.curTarget = null;
            return b;
        }

        //使用事件通知
        public bool NotifyWithEvent(string type_, object data_ = null)
        {
            if (!HasAttach(type_))
                return false;

            //SubjectEvent evt = new SubjectEvent(type_, data_);
            SubjectEvent evt = __evtPool.Pop() as SubjectEvent;
            evt.type = type_;
            evt.data = data_;

            bool b = NotifyEvent(evt);
            evt.Clear();

            __evtPool.Push(evt);

            return b;
        }


        bool Invoke(string type_, List<Observer> objArr_, object data_)
        {
            if (objArr_.Count == 0)
                return false;

            ++m_invalid;
            bool succ = false;
            bool b = false;

            Observer obs;
            int i = 0;
            int len = objArr_.Count; //先取长度, 避免新加入的也接收到事件
            for (; i < len; ++i)
            {
                if (objArr_.Count == 0)
                {
                    //$vec被内存回收
                    b = false;
                    break;
                }

                obs = objArr_[i];
                if (obs != null)
                {
                    if(obs.refer == null) 
                    {
                        obs.func(data_);
                        succ = true;
                    }
                    else
                    {
                        //目标在回调时暂时没有作用
                        obs.func(data_);
                        succ = true;
                    }
                    
                }
                else
                {
                    //已经删除
                    b = true;
                }
            }

            if (m_invalid > 0)
                --m_invalid;

            if (b)
            {
                //需要校正
                Adjust(objArr_);

                if (objArr_.Count == 0)
                {
                    RemoveObsArr(type_);
                }
            }

            return succ;
        }

        bool InvokeEvent(string type_, List<Observer> objArr_, SubjectEvent evt_)
        {
            if (objArr_.Count == 0)
                return false;

            ++m_invalid;

            bool succ = false;
            bool b = false;

            Observer obs;
            int i = 0;
            int len = objArr_.Count;
            for (; i < len; ++i)
            {
                if (objArr_.Count == 0)
                {
                    //$vec被内存回收
                    b = false;
                    break;
                }

                obs = objArr_[i];
                if (obs != null)
                {
                    obs.func(evt_);
                    succ = true;
                }
                else
                {
                    //已经删除
                    b = true;
                }

                if (evt_.isStopped)
                    //停止传递
                    break;
            }


            if (m_invalid > 0)
                --m_invalid;

            if (b)
            {
                //需要校正
                Adjust(objArr_);

                if(objArr_.Count == 0)
                {
                    RemoveObsArr(type_);
                }
            }


            return succ;
        }


        void Adjust(List<Observer> objArr_)
        {
            //倒序删除
            for (int i = objArr_.Count - 1; i >= 0; i--)
            {
                if (objArr_[i] == null)
                {
                    objArr_.RemoveAt(i);
                }
            }
        }


        class Observer
        {
            public List<Observer> parent;

            //事件类型
            public string type;
            //回调函数
            public CALLBACK_1 func;
            //引用者
            public object refer;

            public Observer()
            {

            }

            /// <param name="type_"></param>
            /// <param name="func_"></param>
            /// <param name="parent_">此obs所在的队列</param>
            /// <param name="refer_"></param>
            public void Init(string type_, CALLBACK_1 func_, List<Observer> parent_, object refer_)
            {
                type = type_;
                func = func_;
                parent = parent_;
                refer = refer_;

                if (refer != null)
                {
                    //监听目标沉默
                    Refer.AttachDeactive(refer_, onDeacive);
                }
                //else
                //{
                // Refer.Assert(refer_);
                //}
            }

            public void Clear()
            {
                if (refer != null)
                {
                    Refer.DetachDeactive(refer, onDeacive);
                    refer = null;
                }

                type = null;
                func = null;
                parent = null;
            }


            void onDeacive(object referId_)
            {
                //if((string)referId_ != Refer.Format(refer))
                //{
                //    Debug.Assert(false, "不正确的Refer");
                //}
                
                int index = parent.IndexOf(this);
                if (index >= 0)
                {
                    parent[index] = null;   //因为是用置null的方法卸载obs, 所以需要保存parent
                }
                else
                {
                    Log.Assert("错误的序号", this);
                }

                Clear();
                __obsPool.Push(this);
            }

        }

    }


}