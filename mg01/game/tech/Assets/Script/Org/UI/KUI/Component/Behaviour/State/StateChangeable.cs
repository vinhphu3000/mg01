/* ==============================================================================
 * StateChangeable
 * @author jr.zeng
 * 2017/7/8 18:10:04
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    [DisallowMultipleComponent]
    public abstract class StateChangeable : MonoBehaviour, IStateChangeable, IRaycastable
    {
        
        public const string STATE_NORMAL = "normal";    //普通状态的名称

        //当前状态
        protected string m_curState = STATE_NORMAL;

        protected List<KeyValuePair<string, GameObject>> m_stateList;

        [NonSerialized]
        bool m_inited = false;

        bool m_useRaycast = false;

        protected virtual void Awake()
        {
            this.Raycast = true;
        }

        void Initialize()
        {
            if (m_inited == true)
                return;
            m_inited = true;

            m_stateList = new List<KeyValuePair<string, GameObject>>();

            if (this.transform.childCount == 0)
            {
                //只有一态的情况下
                m_stateList.Add(new KeyValuePair<string, GameObject>(STATE_NORMAL, this.gameObject));
            }
            else
            {
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    GameObject child = this.transform.GetChild(i).gameObject;
                    m_stateList.Add(new KeyValuePair<string, GameObject>(child.name, child));
                }
            }
        }

        public bool Visible
        {
            get { return this.gameObject.activeSelf; }
            set  {  this.gameObject.SetActive(value); }
        }


        public virtual float Alpha { get; set; }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽状态管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

            
        public int StateCount
        {
            get
            {
                Initialize();
                return m_stateList.Count;
            }
        }

        public string State
        {
            get {  return m_curState; }
            set
            {
                if (m_curState != value && HasState(value) == true)
                {
                    m_curState = value;
                    ShowCurrentState();
                }
            }
        }

        /// <summary>
        /// 有这个状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool HasState(string state)
        {
            Initialize();
            foreach (KeyValuePair<string, GameObject> kvp in m_stateList)
            {
                if (kvp.Key == state)
                {
                    return true;
                }
            }
            return false;
        }

        //刷新显示
        void ShowCurrentState()
        {
            foreach (KeyValuePair<string, GameObject> kvp in m_stateList)
            {
                if (kvp.Key == m_curState)
                {
                    kvp.Value.SetActive(true);
                }
                else
                {
                    kvp.Value.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 获取当前状态的go
        /// </summary>
        /// <returns></returns>
        public GameObject GetCurStateGo()
        {
            return GetStateGo(this.State);
        }

        /// <summary>
        /// 获取指定状态的go
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public GameObject GetStateGo(string state)
        {
            Initialize();
            foreach (KeyValuePair<string, GameObject> kvp in m_stateList)
            {
                if (kvp.Key == state)
                {
                    return kvp.Value;
                }
            }
            return null;
        }

        public GameObject GetStateGo(int index)
        {
            Initialize();
            if (index < 0 || index >= m_stateList.Count)
            {
                return null;
            }
            return m_stateList[index].Value;
        }

        public T GetCurStateComponent<T>() where T : Component
        {
            return GetStateComponent<T>(this.State);
        }

        public T GetStateComponent<T>(string state) where T : Component
        {
            if (HasState(state) == true)
            {
                return GetStateGo(state).GetComponent<T>();
            }
            return null;
        }

        public T GetStateComponent<T>(int index) where T : Component
        {
            GameObject state = GetStateGo(index);
            if (state != null)
            {
                return state.GetComponent<T>();
            }
            return null;
        }

        public T[] GetAllStateComponent<T>() where T : Component
        {
            T[] result = new T[this.StateCount];
            for (int i = 0; i < this.StateCount; i++)
            {
                result[i] = GetStateComponent<T>(i);
            }
            return result;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Raycast∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return m_useRaycast;
        }

        public bool Raycast
        {
            get { return m_useRaycast; }
            set {  m_useRaycast = value; }
        }




    }

}