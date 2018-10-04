/* ==============================================================================
 * KScrollView
 * @author jr.zeng
 * 2017/7/19 11:28:39
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
    public class KScrollView : KContainer
    {
        /// <summary>
        /// 滚动方向
        /// </summary>
        public enum ScrollDir
        {
            horizontal,
            vertical
        }

        //遮罩
        public const string CHILD_NAME_MASK = "Image_mask";
        //内容容器
        public const string CHILD_NAME_CONTENT = "content";
        //箭头容器
        public const string CHILD_NAME_ARROW = "Container_arrow";
        


        KComponentEvent<KScrollView, Vector2> m_onValueChanged;
        public new KComponentEvent<KScrollView, Vector2> onValueChanged { get { return KComponentEvent<KScrollView, Vector2>.GetEvent(ref m_onValueChanged); } }
        
        bool m_scrollable = true;
        ScrollDir m_direction = ScrollDir.vertical;

        [SerializeField]
        ScrollRect m_scrollRect;
        [SerializeField]
        RectTransform m_contentTrans;
        [SerializeField]
        RectTransform m_maskTrans;

        [SerializeField]
        KScrollViewArrow m_scrollArrow;

        Vector2 m_point = new Vector2();

        protected override void OnEnable()
        {
            base.OnEnable();
            RefreshScrollRectSetting();

            m_point = Vector2.zero;
        }


        protected override void __Initialize()
        {

            m_scrollable = true;

            m_maskTrans = GetChildComponent<RectTransform>("Image_mask");    //遮罩

            GameObject contentGo = GameObjUtil.FuzzySearchChild(m_maskTrans.gameObject, "content");
            m_contentTrans = contentGo.GetComponent<RectTransform>();
            m_contentTrans.anchoredPosition = Vector2.zero;
            m_contentTrans.sizeDelta = new Vector2(m_maskTrans.rect.width, m_maskTrans.rect.height);

            m_scrollRect = ComponentUtil.EnsureComponent<ScrollRect>(this.gameObject);
            RefreshScrollRectSetting();
            m_scrollRect.viewport = m_maskTrans;
            m_scrollRect.content = m_contentTrans;

            if (GetChild("Container_arrow") != null)
            {
                //有箭头
                m_scrollArrow = AddChildComponent<KScrollViewArrow>("Container_arrow");
            }

            m_scrollRect.onValueChanged.AddListener(OnInternalValueChanged);
        }

        void RefreshScrollRectSetting()
        {
            if (m_scrollRect == null)
                return;

            if ( m_scrollable)
            {
                m_scrollRect.horizontal = (m_direction == KScrollView.ScrollDir.vertical) ? false : true;
                m_scrollRect.vertical = (m_direction == KScrollView.ScrollDir.vertical) ? true : false;
            }
            else
            {
                m_scrollRect.horizontal = false;
                m_scrollRect.vertical = false;
            }
        }

        /// <summary>
        /// 滚动方向
        /// </summary>
        public ScrollDir direction
        {
            get { return m_direction; }
            set
            {
                m_direction = value;
                RefreshScrollRectSetting();
            }
        }


        //public ScrollRect ScrollRect
        //{
        //    get { return m_scrollRect; }
        //}

        public RectTransform contentTrans
        {
            get { return m_contentTrans; }
        }

        public RectTransform maskTrans
        {
            get { return m_maskTrans; }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <param name="position">滚到顶部为1,滚到底部为0</param>
        void OnInternalValueChanged(Vector2 position)
        {
            if (m_scrollArrow != null)
            {  
                //更新箭头
                m_scrollArrow.Refresh(position, this);
            }
            
            if (m_point.x != position.x || m_point.y != position.y)
            {
                m_point.x = position.x;
                m_point.y = position.y;

                KComponentEvent<KScrollView, Vector2>.InvokeEvent(m_onValueChanged, this, position);
                LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.VALUE_CHANGE, position.x, position.y);
            }
        }
        

        public void ResetContentPosition()
        {
            StopMovement();
            m_contentTrans.localPosition = Vector2.zero;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 设置是否可以滚动
        /// </summary>
        /// <param name="value"></param>
        public void SetScrollable(bool value)
        {
            m_scrollable = value;
            RefreshScrollRectSetting();
        }

        /// <summary>
        /// 内容长度超出遮罩长度,需要滚动
        /// </summary>
        /// <returns></returns>
        public bool NeedScroll()
        {
            if (m_direction == KScrollView.ScrollDir.vertical)
            {
                return m_contentTrans.rect.height > m_maskTrans.rect.height; 
            }

            return m_contentTrans.rect.width > m_maskTrans.rect.width;
        }

        /// <summary>
        /// 停止滑动
        /// </summary>
        public void StopMovement()
        {
            m_scrollRect.StopMovement();
        }

        /// <summary>
        /// 跳转到顶部
        /// </summary>
        public void JumpToTop()
        {
            StopMovement();
            m_contentTrans.anchoredPosition = Vector2.zero;

        }

        /// <summary>
        /// 跳转到底部
        /// </summary>
        public void JumpToBottom()
        {
            StopMovement();

            Vector2 pos = m_contentTrans.anchoredPosition;

            if (direction == KScrollView.ScrollDir.vertical)
            {
                pos.y = m_contentTrans.sizeDelta.y - m_maskTrans.sizeDelta.y;
            }
            else
            {
                pos.x = -(m_contentTrans.sizeDelta.x - m_maskTrans.sizeDelta.x);
            }

            m_contentTrans.anchoredPosition = pos;
        }


    }
}