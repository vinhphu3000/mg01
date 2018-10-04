/* ==============================================================================
 * KToggleGroup
 * @author jr.zeng
 * 2017/8/26 15:07:58
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;



namespace mg.org.KUI
{
    public class KToggleGroup : KContainer
    {

        KComponentEvent<KToggleGroup, int, bool> m_onValueChange;
        public KComponentEvent<KToggleGroup, int, bool> onValueChange
        {
            get { return KComponentEvent<KToggleGroup, int, bool>.GetEvent(ref m_onValueChange); }
        }

        KComponentEvent<KToggleGroup, int, bool> m_onReqChange;
        public KComponentEvent<KToggleGroup, int, bool> onReqChange
        {
            get { return KComponentEvent<KToggleGroup, int, bool>.GetEvent(ref m_onReqChange); }
        }
        

        List<KToggle> m_toggleList = new List<KToggle>();

        //能否取消
        bool m_allowSwitchOff = false;
        //能否多选
        bool m_allowMultiple = false;
        //需要请求改变
        bool m_needReqChange = false;

        bool m_ingoreInvoke = false;

        List<int> m_selectList = new List<int>();

        int m_lastIndex = -1;

       
        protected override void OnEnable()
        {
            base.OnEnable();

        }

        protected override void __Initialize()
        {
            m_toggleList.Clear();
            GetComponentsInChildren<KToggle>(false, m_toggleList);

            KToggle toggle;
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                toggle = m_toggleList[i];
                toggle.needReqChange = true;    //需要申请
                toggle.onReqChange.AddListener(OnToggleReqChanged);
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
            }

            InitByToggles();
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        //根据当前子项初始化
        void InitByToggles()
        {
            m_selectList.Clear();

            KToggle toggle;
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                toggle = m_toggleList[i];
                if (toggle.isOn)
                {
                    m_selectList.Add(i);

                    if (m_lastIndex < 0)
                        m_lastIndex = i;
                }
            }

            CheckMultipleLegal();
        }

        //检测多选合法
        void CheckMultipleLegal()
        {
            if (m_allowMultiple)
                return;

            if (m_selectList.Count <= 1)
                return;

            //不能多选,但当前多选了,只保留第一个

            bool b = false;

            KToggle toggle;
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                toggle = m_toggleList[i];
                if (toggle.isOn)
                {
                    if(!b)
                    {
                        //第一个合法
                        b = true;
                        m_lastIndex = i;
                    }
                    else
                    {
                        //之后的置false
                        toggle.Select(false, true);
                        RemoveFromSelect(i);
                    }
                }
            }
        }

        void OnToggleReqChanged(KToggle target_, bool isOn_)
        {
            if(m_needReqChange)
            {
                int index = m_toggleList.IndexOf(target_);

                onReqChange.Invoke(this, index, isOn_);
                LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.REQ_VALUE_CHANGE, index+1, isOn_);  //lua从1开始
                return;
            }
            
            if (!m_allowSwitchOff && !m_allowMultiple)
            {
                //不允许取消 and 不为多选
                if (isOn_ == false)
                    return;
            }

            target_.Select(isOn_);
        }



        void OnToggleValueChanged(KToggle target_, bool isOn_)
        {
            int lastIndex = m_lastIndex;

            if (isOn_)
            {
                if (!m_allowMultiple)
                {
                    //不允许多选,取消上一个
                    if(m_lastIndex >= 0)
                    {
                        KToggle last = m_toggleList[m_lastIndex];
                        last.Select(false, true);
                        RemoveFromSelect(m_lastIndex);
                        m_lastIndex = -1;
                    }
                }

                m_lastIndex = m_toggleList.IndexOf(target_);
                m_selectList.Add(m_lastIndex);

                if (!m_ingoreInvoke)
                {
                    onValueChange.Invoke(this, m_lastIndex, isOn_);
                    LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.VALUE_CHANGE, m_lastIndex+1, isOn_);     //lua从1开始
                }
            }
            else
            {
                //取消
                int index = m_toggleList.IndexOf(target_);
                RemoveFromSelect(index);

                if (m_lastIndex == index)
                {
                    m_lastIndex = -1;
                }

                if (m_allowMultiple)
                {
                    if (m_selectList.Count > 0)
                    {
                        //多选,还有选中的
                        m_lastIndex = m_selectList[m_selectList.Count - 1];
                    }
                }

                if (!m_ingoreInvoke)
                {
                    onValueChange.Invoke(this, index, isOn_);
                    LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.VALUE_CHANGE, index+1, isOn_);   //lua从1开始
                }
            }
            
        }

        void RemoveFromSelect(int index_)
        {
            if (!m_selectList.Remove(index_))
            {
                Debug.Assert(false, "没有在选中列表:" + index_);
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 能否取消
        /// </summary>
        public bool allowSwitchOff
        {
            get { return m_allowSwitchOff; }
            set { m_allowSwitchOff = value; }
        }

        /// <summary>
        /// 能否多选
        /// </summary>
        public bool allowMultiple
        {
            get { return m_allowMultiple; }
            set
            {
                if (m_allowMultiple == value)
                    return;
                m_allowMultiple = value;
                if (!value)
                    CheckMultipleLegal();   //不能多选,检测合法性
            }
        }


        /// <summary>
        /// 需要请求改变
        /// </summary>
        public bool needReqChange
        {
            get { return m_needReqChange; }
            set { m_needReqChange = value; }
        }

        /// <summary>
        /// 获取Toggle
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public KToggle GetToggle(int index)
        {
            if (index >= 0 && index < m_toggleList.Count)
                return m_toggleList[index];
            return null;
        }


        /// <summary>
        /// 当前选中的序号
        /// </summary>
        public int index
        {
            get { return m_lastIndex; }
        }

        /// <summary>
        /// 单选框数量
        /// </summary>
        public int toggleNum
        {
            get { return m_toggleList.Count; }
        }

        /// <summary>
        /// 指定序号是否选中
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool isSelected(int index = 0)
        {
            KToggle toggle = m_toggleList[index];
            return toggle.isOn;
        }

        /// <summary>
        /// 根据序号选中
        /// </summary>
        /// <param name="index"></param>
        /// <param name="b_"></param>
        /// <param name="ingoreInvoke_">不派事件</param>
        public void Select(int index, bool b_, bool ingoreInvoke_ = false)
        {
            KToggle toggle = m_toggleList[index];
            if (toggle.isOn == b_)
                return;

            m_ingoreInvoke = ingoreInvoke_;
            toggle.isOn = b_;
            m_ingoreInvoke = false;
        }
        

        /// <summary>
        /// 取消全部
        /// </summary>
        /// <param name="ingoreInvoke_"></param>
        public void CancelAll(bool ingoreInvoke_ = false)
        {
            if (m_selectList.Count == 0)
                return;

            if (!m_allowSwitchOff && !m_allowMultiple)
            {
                //单选且不允许取消
                Log.Warn("单选且不允许取消");
                return;
            }

            KToggle toggle;
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                toggle = m_toggleList[i];
                if (toggle.isOn)
                {
                    toggle.Select(false, ingoreInvoke_);
                }
            }

            m_selectList.Clear();
            m_lastIndex = -1;
        }


    }

}