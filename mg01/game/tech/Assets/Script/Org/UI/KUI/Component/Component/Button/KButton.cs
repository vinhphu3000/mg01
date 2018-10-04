/* ==============================================================================
 * KButton
 * @author jr.zeng
 * 2017/6/19 11:19:29
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KButton : Button
    {


        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件注册相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //点击
        KComponentEvent<KButton> m_onClick;
        public new KComponentEvent<KButton> onClick { get { return KComponentEvent<KButton>.GetEvent(ref m_onClick); } }
        //鼠标按下
        KComponentEvent<KButton, PointerEventData> m_onPointerDown;
        public KComponentEvent<KButton, PointerEventData> onPointerDown { get { return KComponentEvent<KButton, PointerEventData>.GetEvent(ref m_onPointerDown); } }
        //鼠标弹起
        KComponentEvent<KButton, PointerEventData> m_onPointerUp;
        public KComponentEvent<KButton, PointerEventData> onPointerUp { get { return KComponentEvent<KButton, PointerEventData>.GetEvent(ref m_onPointerUp); } }
        //鼠标进入
        KComponentEvent<KButton, PointerEventData> m_onPointerEnter;
        public KComponentEvent<KButton, PointerEventData> onPointerEnter { get { return KComponentEvent<KButton, PointerEventData>.GetEvent(ref m_onPointerEnter); } }
        //鼠标离开
        KComponentEvent<KButton, PointerEventData> m_onPointerExit;
        public KComponentEvent<KButton, PointerEventData> onPointerExit { get { return KComponentEvent<KButton, PointerEventData>.GetEvent(ref m_onPointerExit); } }
       

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_onClick != null)          m_onClick.RemoveAllListeners();
            if (m_onPointerDown != null)    m_onPointerDown.RemoveAllListeners();
            if (m_onPointerUp != null)      m_onPointerUp.RemoveAllListeners();
            if (m_onPointerEnter != null)   m_onPointerEnter.RemoveAllListeners();
            if (m_onPointerExit != null)    m_onPointerExit.RemoveAllListeners();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
            //base.OnPointerClick(eventData); //屏蔽基类的
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            KComponentEvent<KButton>.InvokeEvent(m_onClick, this);
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_CLICK);

            if (m_isPassPoint)
            {
                PassPointEvent(eventData, ExecuteEvents.submitHandler);
                PassPointEvent(eventData, ExecuteEvents.pointerClickHandler);
            }

//#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR))
            if (Input.touchCount >= (eventData.pointerId + 1))
            {
                m_pointId = eventData.pointerId;
            }
            else
            {
                m_pressure = 0;
                m_maximumPossiblePressure = 0;
            }
//#endif
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            base.OnPointerDown(eventData);

            KComponentEvent<KButton, PointerEventData>.InvokeEvent(m_onPointerDown, this, eventData);
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_DOWN, eventData.pressPosition.x, eventData.pressPosition.y);

            if (m_isPassPoint)
            {
                PassPointEvent(eventData, ExecuteEvents.pointerDownHandler);
            }

//#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR))
            if (Input.touchCount >= (eventData.pointerId + 1))
            {
                m_pointId = eventData.pointerId;
            }
            else
            {
                m_pressure = 0;
                m_maximumPossiblePressure = 0;
            }
//#endif
        }

        /// <summary>
        /// 鼠标弹起
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            base.OnPointerUp(eventData);

            KComponentEvent<KButton, PointerEventData>.InvokeEvent(m_onPointerUp, this, eventData);
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_UP, eventData.pressPosition.x, eventData.pressPosition.y);
            
            
            if (m_isPassPoint)
            {
                PassPointEvent(eventData, ExecuteEvents.pointerUpHandler);
            }
        }
        
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            KComponentEvent<KButton, PointerEventData>.InvokeEvent(m_onPointerEnter, this, eventData);

            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_ENTER);

            if (m_isPassPoint)
            {
                PassPointEvent(eventData, ExecuteEvents.pointerEnterHandler);
            }

//#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR))
            if (Input.touchCount >= (eventData.pointerId + 1))
            {
                m_pointId = eventData.pointerId;
            }
            else
            {
                m_pressure = 0;
                m_maximumPossiblePressure = 0;
            }
//#endif
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            KComponentEvent<KButton, PointerEventData>.InvokeEvent(m_onPointerExit, this, eventData);

            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_EXIT);

            if (m_isPassPoint)
            {
                PassPointEvent(eventData, ExecuteEvents.pointerExitHandler);
            }
        }

        /// <summary>
        /// 状态转换, 可以在这里换图片
        /// </summary>
        /// <param name="state"></param>
        /// <param name="instant"></param>
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            //状态转换
            //StateChangeableFacade.DoStateTransition(this, (int)state, instant);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件穿透∽-★-∽--------∽-★-∽------∽-★-∽--------//

        bool m_isPassPoint = false;

        /// <summary>
        /// 是否穿透鼠标事件
        /// </summary>
        public bool isPassPoint
        {
            get { return m_isPassPoint; }
            set { m_isPassPoint = value; }
        }

        public void PassPointEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.ExecuteHierarchy(results[i].gameObject, data, function);
                    break;
                }
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽IKContainer∽-★-∽--------∽-★-∽------∽-★-∽--------//

       
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽压力相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //压力
        float m_pressure;
        float m_maximumPossiblePressure;
        int m_pointId;


        public float pressure
        {
            get
            {
//#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR))
            if (Input.touchCount > (m_pointId))
            {
                try
                {
                    var touch = Input.GetTouch(m_pointId);
                    return touch.pressure;
                }
                catch (Exception e){
                        return 0;
                }
            }
            else
            {
                return 0;
            }
//#else
                //return 0;
//#endif
            }
        }

        public float maximumPossiblePressure
        {
            get
            {
//#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR))
            if (Input.touchCount > (m_pointId))
            {
                try
                {
                    var touch = Input.GetTouch(m_pointId);
                    return touch.maximumPossiblePressure;
                }
                catch (Exception e)
                {
                   return 0;
                }
            }
            else
            {
                return 0;
            }
//#else
//                return 0;
//#endif
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽其他∽-★-∽--------∽-★-∽------∽-★-∽--------//

       

        //public StateText Label
        //{
        //    get
        //    {
        //        for (int i = 0; i < transform.childCount; i++)
        //        {
        //            Transform child = transform.GetChild(i);
        //            StateText label = child.GetComponent<StateText>();
        //            if (label != null)
        //            {
        //                return label;
        //            }
        //        }
        //        return null;
        //    }
        //}

    }

}