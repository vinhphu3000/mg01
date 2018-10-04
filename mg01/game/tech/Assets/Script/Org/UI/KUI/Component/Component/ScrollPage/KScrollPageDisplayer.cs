/* ==============================================================================
 * KScrollPageDisplayer
 * @author jr.zeng
 * 2017/7/19 17:37:05
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    public class KScrollPageDisplayer : KContainer
    {
        private KList m_list;

        private int m_page = 0;
        private int m_gap = 0;

       
        
        override protected void __Initialize()
        {
            m_list = GetComponent<KList>();

            m_list.SetDirection(KList.KListDirection.TopToBottom, 1);
            m_list.SetGap(m_gap, 0);    //Y方向间距为0
        }

        public void SetTotalPage(int totalPage, RectTransform anchoredRect, bool createByCoroutine)
        {
            m_list.RemoveAll();

            object[] fakeDatas = new object[totalPage];
            int coroutineCreateCount = createByCoroutine == true ? 1 : 0;
            
            m_list.SetDataList<KScrollPageDisplayerItem>(fakeDatas, coroutineCreateCount);
            m_list[m_page].IsSelected = true;

            RectTransform rect = GetComponent<RectTransform>();
            float width;
            if (createByCoroutine == true)
            {
                width = rect.sizeDelta.x * totalPage + m_gap * (totalPage - 1);
            }
            else
            {
                width = rect.sizeDelta.x;
            }

            float x = anchoredRect.anchoredPosition.x + (anchoredRect.sizeDelta.x - width) * 0.5f;
            rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
        }

        public void SetPage(int value)
        {
            if (m_page != value)
                return;

            m_list[m_page].IsSelected = false;
            m_page = value;
            m_list[m_page].IsSelected = true;
        }

        public int Gap
        {
            get { return m_gap; }
            set
            {
                if (m_gap == value)
                    return;
                m_gap = value;
                m_list.SetGap(value, 0);
            }
        }

        public class KScrollPageDisplayerItem : KListItem, ICanvasRaycastFilter
        {
            private StateImage m_spotlightImage; //选中图片

            protected override void Awake()
            {
                CreateChildren();
            }

            private void CreateChildren()
            {
                m_spotlightImage = GetChildComponent<StateImage>("Image_spotlight");
                m_spotlightImage.Visible = false;
            }

            public override bool IsSelected
            {
                get { return base.IsSelected;  }
                set
                {
                    base.IsSelected = value;
                    m_spotlightImage.Visible = value;
                }
            }

            public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
            {
                return false;
            }


        }

    }

}