/* ==============================================================================
 * KScrollPage
 * @author jr.zeng
 * 2017/7/19 17:05:57
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
    public class KScrollPage : KContainer, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public enum KScrollPageDirection
        {
            TopToBottom,
            LeftToRight
        }


        float m_bounce = 0.1f;  //移动系数
        public float bounce
        {
            get { return m_bounce; }
            set { m_bounce = value; }
        }

        bool m_createDisplyerByCoroutine = false;

        bool m_moving;
        public bool isMoving { get { return m_moving; } }
        
        public bool m_dragging { get; protected set; }
        public bool isDragging { get { return m_dragging; } }

        bool m_draggable = true;
        public bool draggable
        {
            get { return m_draggable; }
            set { m_draggable = value; }
        }

        KScrollPageDirection m_direction = KScrollPageDirection.LeftToRight;
        public KScrollPageDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }


        RectTransform m_pageTrans;
        
        Vector2 m_pointerStartPosition;
        Vector2 m_pointerEndPosition;

        RectTransform m_contentTrans;
        Vector2 m_contentStartPosition;
        Vector2 m_contentEndPosition;

        KScrollPageArrow m_pageArrow;
        KScrollPageDisplayer m_pageDisplayer;

        int m_evtId_indexChange = AllocUtil.getEvtId();

        int m_totalPage;
        int m_page;
        KComponentEvent<KScrollPage> _onPageChanged;
        public KComponentEvent<KScrollPage> onPageChanged { get { return KComponentEvent<KScrollPage>.GetEvent(ref _onPageChanged); } }

        public KComponentEvent<int, int> _onPageIndexChange;
        public KComponentEvent<int, int> onPageIndexChange { get { return KComponentEvent<int, int>.GetEvent(ref _onPageIndexChange); } }


       

        protected void Update()
        {
            Move();
        }

        override protected void __Initialize()
        {

            GameObject arrowGo = GameObjUtil.FuzzySearchChild(gameObject, "arrow");
            if (arrowGo != null)
                m_pageArrow = ComponentUtil.EnsureComponent < KScrollPageArrow > (arrowGo);

            GameObject displayer = GameObjUtil.FuzzySearchChild(gameObject, "pager");
            if (displayer != null)
                m_pageDisplayer = ComponentUtil.EnsureComponent< KScrollPageDisplayer >(displayer);

            m_pageTrans = GetChildComponent<RectTransform>("Image_mask");

            GameObject contentGo = GameObjUtil.FuzzySearchChild(m_pageTrans.gameObject, "content");
            m_contentTrans = contentGo.GetComponent<RectTransform>();
        }

        
        void RefreshPageArrow()
        {
            if (m_pageArrow != null)
            {
                m_pageArrow.Refresh(Page, m_totalPage);
            }
        }

        void RefreshPageDisplayer()
        {
            if (m_pageDisplayer != null)
            {
                m_pageDisplayer.SetPage(Page);
            }
        }

        void RefreshPageableList()
        {
            KListPageable pageable = m_contentTrans.gameObject.GetComponent<KListPageable>();
            if (pageable != null)
            {
                pageable.Page = Page;
            }
        }


        //内容对象的位置
        Vector2 GetContentPosition()
        {
            return m_contentTrans.anchoredPosition;
        }
        void SetContentPosition(Vector2 value)
        {
            if (m_contentTrans.anchoredPosition == value)
                return;

            m_contentTrans.anchoredPosition = value;
        }
        


        public RectTransform pageTrans
        {
            get { return m_pageTrans; }
            set { m_pageTrans = value; }
        }

        //单页的尺寸
        Vector2 GetPageSize()
        {
            return m_pageTrans.sizeDelta;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Page相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void SetPagerGap(int gap)
        {
            if (m_pageDisplayer != null)
            {
                m_pageDisplayer.Gap = gap;
                if (m_totalPage > 0)
                {
                    m_pageDisplayer.SetTotalPage(m_totalPage, m_pageTrans, m_createDisplyerByCoroutine);
                }
            }
        }

        public int TotalPage
        {
            get { return m_totalPage; }
            set
            {
                value = Mathf.Max(1, value);
                if (m_totalPage == value)
                    return;
                m_totalPage = value;

                if (m_pageDisplayer != null)
                    m_pageDisplayer.SetTotalPage(m_totalPage, m_pageTrans, m_createDisplyerByCoroutine);

                if (m_pageArrow != null)
                    m_pageArrow.Refresh(m_page, m_totalPage);
            }
        }

        public int Page
        {
            get { return m_page; }
            set
            {
                value = Mathf.Clamp(value, 0, m_totalPage - 1);
                if (m_page == value)
                    return;
                m_page = value;

                RefreshPageArrow();
                RefreshPageDisplayer();
                RefreshPageableList();
                onPageChanged.Invoke(this);

                KComponentEvent<int, int>.InvokeEvent(onPageIndexChange, m_evtId_indexChange, m_page);
                KEvtCenter4Lua.AddUIListEvent(m_evtId_indexChange, m_page);
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Drag相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        public void OnBeginDrag(PointerEventData evtData)    //内部事件
        {
           
            if (evtData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            if (m_draggable == false)
                return;

            Vector2 pointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_pageTrans, evtData.position, evtData.pressEventCamera, out pointerPosition) == false)
            {
                return;
            }

            m_pointerStartPosition = pointerPosition;   //鼠标初始位置
            m_contentStartPosition = m_contentTrans.anchoredPosition;   //content的初始位置
            m_dragging = true;
        }

        public void OnDrag(PointerEventData evtData)     //内部事件
        {
            if (evtData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            if (!m_draggable)
                return;
            
            Vector2 pointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_pageTrans, evtData.position, evtData.pressEventCamera, out pointerPosition) == false)
            {
                return;
            }

            Vector2 delta;
            if (m_direction == KScrollPageDirection.LeftToRight)
            {
                delta = new Vector2((pointerPosition - m_pointerStartPosition).x, 0);
            }
            else
            {
                delta = new Vector2(0, (pointerPosition - m_pointerStartPosition).y);
            }

            Vector2 contentPosition = m_contentStartPosition + delta;   //新的位置
            SetContentPosition(contentPosition);

            //拖曳的时候也应该更新页码的吧
            //TODO
        }

        public void OnEndDrag(PointerEventData evtData)
        {
            if (evtData.button != PointerEventData.InputButton.Left)
                return;

            if (!draggable)
                return;

            if (!m_dragging)
                return;

            m_dragging = false;
            Vector2 pointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_pageTrans, evtData.position, evtData.pressEventCamera, out pointerPosition);
            m_pointerEndPosition = pointerPosition;
            StartMove();
        }

        void StartMove()
        {
            m_moving = true;

            Vector2 viewSize = GetPageSize();   //整个容器的尺寸就为一页

            //int orention;
            int offsetPage;
            if (m_direction == KScrollPageDirection.LeftToRight)
            {
                //orention = _pointerEndPosition.x < _pointerStartPosition.x ? 1 : -1;
                offsetPage = Mathf.RoundToInt((m_pointerStartPosition.x - m_pointerEndPosition.x) / viewSize.x);    //接近取整
                float x = Mathf.Clamp(-(m_page + offsetPage) * viewSize.x, -(m_totalPage - 1) * viewSize.x, 0);
                m_contentEndPosition = new Vector2(x, m_contentEndPosition.y);
            }
            else
            {
                //orention = _pointerEndPosition.y < _pointerStartPosition.y ? -1 : 1;
                offsetPage = Mathf.RoundToInt((m_pointerEndPosition.y - m_pointerStartPosition.y) / viewSize.y);
                float y = Mathf.Clamp((m_page + offsetPage) * viewSize.y, 0, (m_totalPage - 1) * viewSize.y);
                m_contentEndPosition = new Vector2(m_contentEndPosition.x, y);
            }
        }

        void Move()
        {
            if (m_moving )
            {
                Vector2 delta = (m_contentEndPosition - GetContentPosition()) * m_bounce;
                if (delta.magnitude < 1.0f)
                {
                    SetContentPosition(m_contentEndPosition);
                    StopMove();
                }
                else
                {
                    SetContentPosition(GetContentPosition() + delta);
                }
            }
        }

        void StopMove()
        {
            m_moving = false;
            int page;
            if (m_direction == KScrollPageDirection.LeftToRight)
            {
                page = -Mathf.RoundToInt(GetContentPosition().x / GetPageSize().x);
            }
            else
            {
                page = Mathf.RoundToInt(GetContentPosition().y / GetPageSize().y);
            }
            Page = page;
        }

      


    }


}