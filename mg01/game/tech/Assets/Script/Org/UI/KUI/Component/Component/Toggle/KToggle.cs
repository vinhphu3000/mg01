/* ==============================================================================
 * KToggle
 * @author jr.zeng
 * 2017/7/13 11:27:42
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{

    public class KToggle : Toggle
    {
        //勾选go
        public const string CHILD_NAME_MARK = "checkmark";
        //不勾选go
        public const string CHILD_NAME_BACK = "back"; 
        

        /// <summary>
        /// 覆盖基类的事件定义
        /// </summary>
        public new KComponentEvent<KToggle, bool> onValueChanged = new KComponentEvent<KToggle, bool>();

        /// <summary>
        /// 请求改变
        /// </summary>
        public KComponentEvent<KToggle, bool> onReqChange = new KComponentEvent<KToggle, bool>();


        //on时显示的对象
        GameObject m_checkmarkGo;
        //off时显示的对象
        GameObject m_backGo;

        //需要请求改变
        bool m_needReqChange = false;
        //忽略这次变化
        bool m_ignoreChange = false;

        //是点击触发的
        bool m_pointerTriggered = false;
        //这次不派发事件
        bool m_ignoreInvoke = false;   


        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            RefreshMask(this.isOn);
        }

        void Initialize()
        {
            base.onValueChanged.AddListener(OnValueChanged);

            m_checkmarkGo = GameObjUtil.FuzzySearchChild(this.gameObject, "checkmark");
            m_backGo = GameObjUtil.FuzzySearchChild(this.gameObject, "back");
        }
        


        //刷新go能见
        void RefreshMask(bool isOn)
        {
            if (m_checkmarkGo != null)
                m_checkmarkGo.SetActive(isOn);

            if (m_backGo != null)
                m_backGo.SetActive(!isOn);
        }


        //当前是按下
        bool CurrentIsOn()
        {
            if (m_checkmarkGo != null)
            {
                return m_checkmarkGo.activeSelf;
            }
            else
            {
                return false;
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// React to clicks.
        /// </summary>
        override public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !IsInteractable())
                return;

            m_pointerTriggered = true;
            isOn = !isOn;
            m_pointerTriggered = false;
        }


        void OnValueChanged(bool isOn_)
        {
            bool pointerTriggered = m_pointerTriggered;
            m_pointerTriggered = false;

            if (m_ignoreChange)
                return;
            
            if (m_needReqChange && pointerTriggered)
            {
                //点击触发才进来
                bool cur = CurrentIsOn();
                SelectIgnoreChange(cur); //置回去, 因为基类的isOn已经变了

                onReqChange.Invoke(this, isOn_);
                LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.REQ_VALUE_CHANGE, isOn_);

                return;
            }

            RefreshMask(isOn_);

            if (!m_ignoreInvoke)
            {
                onValueChanged.Invoke(this, isOn_);
                LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.VALUE_CHANGE, isOn_);
            }
        }


        //忽略所有改变
        void SelectIgnoreChange(bool isOn_)
        {
            if (this.isOn == isOn_)
                return;
            m_ignoreChange = true;
            this.isOn = isOn_;
            m_ignoreChange = false;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//



        /// <summary>
        /// 需要请求改变
        /// </summary>
        public bool needReqChange
        {
            get { return m_needReqChange; }
            set { m_needReqChange = value; }
        }

        

        /// <summary>
        /// 选中,且不派发事件
        /// </summary>
        /// <param name="isOn"></param>
        /// <param name="ingoreInvoke">不派发事件</param>
        public void Select(bool isOn_, bool ingoreInvoke_ = false)
        {
            if (this.isOn == isOn_)
                return;

            m_ignoreInvoke = ingoreInvoke_;
            this.isOn = isOn_;
            m_ignoreInvoke = false;
        }
        
        

    }


}