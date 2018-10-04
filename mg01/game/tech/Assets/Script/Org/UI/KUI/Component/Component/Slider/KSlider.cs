/* ==============================================================================
 * KSlider
 * @author jr.zeng
 * 2017/7/10 10:19:08
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
    public class KSlider : Slider
    {

        //手柄
        public const string CHILD_NAME_HANDLE = "Container_handleArea/Container_handleHolder";
        //点击区域
        public const string CHILD_NAME_FILL = "Container_fillArea/Container_fillHolder";



        KComponentEvent<KSlider, float> m_onValueChanged;
        public new KComponentEvent<KSlider, float> onValueChanged
        {
            get { return KComponentEvent<KSlider, float>.GetEvent(ref m_onValueChanged); }
        }
        

        StateImage m_fillImage;
        RectTransform m_fillImageRect;

        // 默认关
        bool m_slicedFilled = true; 

        protected override void Awake()
        {
            base.Awake();
            this.transition = Transition.None;

            Initialize();
        }

        void Initialize()
        {
            //这结构要重新规划

            //fillRect  
            RectTransform fillHolderRect = KComponentUtil.NeedChildComponent<RectTransform>(gameObject, "Container_fillArea/Container_fillHolder");     
            this.fillRect = fillHolderRect;
            fillHolderRect.sizeDelta = new Vector2(0, 0);

            //根据进度填充的那张图片
            m_fillImage = KComponentUtil.NeedChildComponent<StateImage>(gameObject, "Container_fillArea/Container_fillHolder/Image_fill");
            if (m_fillImage)
            {
                m_fillImageRect = m_fillImage.gameObject.GetComponent<RectTransform>();
                m_fillImage.SlicedFilled = m_slicedFilled;
            }

            //handleRect
            GameObject handleGo = KComponentUtil.NeedChild(gameObject, "Container_handleArea/Container_handleHolder");
            if (handleGo != null)
            {
                RectTransform handleHolderRect = handleGo.GetComponent<RectTransform>();
                this.handleRect = handleHolderRect;
            }
            

            RefreshFillImage(this.value);

            base.onValueChanged.AddListener(onInternalValueChanged);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        //刷新填充图片
        void RefreshFillImage(float value)
        {
            if (!m_fillImage)
                return;

            if (this.direction == Direction.LeftToRight || this.direction == Direction.RightToLeft)
            {
                m_fillImage.Scale = new Vector2(value, 1);
            }
            else if (this.direction == Direction.TopToBottom || this.direction == Direction.BottomToTop)
            {
                m_fillImage.Scale = new Vector2(1, value);
            }
            m_fillImage.Align();
        }
        


        void onInternalValueChanged(float value)
        {
            RefreshFillImage(value);
            
            KComponentEvent<KSlider, float>.InvokeEvent(m_onValueChanged, this, value);
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.VALUE_CHANGE, value);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {

            base.OnPointerDown(eventData);
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_DOWN, value);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.POINTER_UP, value);
        }


        //protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
        //{
        //    StateChangeableFacade.DoStateTransition(this, (int)state, instant);
        //}

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public bool SlicedFilled
        {
            get { return m_slicedFilled; }
            set
            {
                if (m_slicedFilled == value)
                    return;
                m_slicedFilled = value;

                if (m_fillImage != null)
                {
                    RefreshFillImage(1);
                    m_fillImage.SlicedFilled = value;
                }
            }
        }



    }

}