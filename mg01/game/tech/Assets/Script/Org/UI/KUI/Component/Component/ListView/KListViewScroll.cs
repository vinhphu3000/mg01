/* ==============================================================================
 * KListView_支持滚动
 * @author jr.zeng
 * 2017/9/4 9:46:09
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    public class KListViewScroll : KListView
    {

        /// <summary>
        /// 跳转位置类型
        /// </summary>
        public enum JumpPosType
        {
            TOP= 1,
            CENTER = 2,
            BOTTOM = 3,
        }

        protected KScrollView m_scrollView;

        RectTransform m_contentTrans;
        RectTransform m_maskTrans;

        //content的默认尺寸
        Vector2 m_contentSizeDefault;
        //content的实际尺寸
        Vector2 m_contentSize;

        //列表项是否循环利用
        bool m_isRecyle = true;
        //显示区域
        Rect m_dispRect = new Rect();

        int m_begin = 0;
        int m_end = 0;

        static List<int> __addList = new List<int>();    //需要增加的序号
        static List<int> __delList = new List<int>();    //需要删除的序号



        protected override void __Initialize()
        {

            m_scrollView = ComponentUtil.EnsureComponent<KScrollView>(this.gameObject);

            m_maskTrans = m_scrollView.maskTrans;

            m_contentTrans = m_scrollView.contentTrans;
            m_contentSizeDefault = m_contentTrans.sizeDelta;      //content的默认尺寸(实际size不能比这个小)
            m_contentGo = m_contentTrans.gameObject;

            m_itemTemp = GameObjUtil.FuzzySearchChild(m_contentGo, "item");  //只查找一层
            m_itemTemp.SetActive(false);    //隐藏掉模板

            //KListView a1 = gameObject.GetComponent<KListView>();
            //Component a2 = gameObject.GetComponent("mg.org.KUI.KListView");

            RectTransform rectTrans = m_itemTemp.GetComponent<RectTransform>();

            m_layoutParam.itemSize = rectTrans.sizeDelta;
            m_layoutParam.pivot = rectTrans.pivot;
        }
        
        public KScrollView.ScrollDir direction
        {
            get { return m_scrollView.direction; }
            set { m_scrollView.direction = value; }
        }
        
        public RectTransform contentTrans
        {
            get { return m_contentTrans; }
        }

        public RectTransform maskTrans
        {
            get { return m_maskTrans; }
        }

        /// <summary>
        /// 设置是否可以滚动
        /// </summary>
        /// <param name="value"></param>
        public void SetScrollable(bool value)
        {
            m_scrollView.SetScrollable(value);
        }

        public bool NeedScroll()
        {
            return m_scrollView.NeedScroll();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        override public void ShowList(IList dataList_)
        {
            m_dataList = dataList_;
            m_item2data.Clear();    //清除原来的数据

            GenLayoutItems(m_dataList.Count);
            RefreshContentSize();   //修改content尺寸

            __ShowList();

            m_dataLenLast = m_dataList.Count;
        }


        public void ShowList(IList dataList_, int jumpTo_=-1, JumpPosType jumpType_ = JumpPosType.CENTER)
        {
            m_dataList = dataList_;
            m_item2data.Clear();    //清除原来的数据

            GenLayoutItems(m_dataList.Count);
            RefreshContentSize();   //修改content尺寸

            if (jumpTo_ >= 0)
            {
                JumpToIndex(jumpTo_, jumpType_);
            }

            __ShowList();

            m_dataLenLast = m_dataList.Count;
        }


        public void ShowLen(int len_, int jumpTo_=-1, JumpPosType jumpType_ = JumpPosType.CENTER)
        {
            if (tmp_seq == null)
                tmp_seq = new List<int>();

            ShowList(ListUtil.GenSequence(len_, 1, 1, tmp_seq), jumpTo_, jumpType_);
        }

        protected override void __ShowList()
        {
            if (m_isRecyle)
            {
                m_scrollView.onValueChanged.AddListener(OnScrollValueChanged);
                ShowItems(true);
            }
            else
            {
                base.__ShowList();
            }

        }

        protected override void __ClearList()
        {
            base.__ClearList();

            m_scrollView.StopMovement();

            if (m_isRecyle)
            {
                m_scrollView.onValueChanged.RemoveListener(OnScrollValueChanged);
            }
        }

        void OnScrollValueChanged(KScrollView scrollView_, Vector2 position)
        {
            if (!m_isRecyle)
                return;

            //Log.Debug("scroll:" + position, this);
            ShowItems(false);
        }

        //刷新content容器的尺寸
        protected void RefreshContentSize()
        {
            Vector2 size = LayoutUtil.CalcLayoutSize(m_layoutParam, m_dataList.Count);

            size.x = Mathf.Max(size.x, m_contentSizeDefault.x);
            size.y = Mathf.Max(size.y, m_contentSizeDefault.y);

            m_contentSize = size;
            m_contentTrans.sizeDelta = size;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽跳转相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 跳转到顶部
        /// </summary>
        public void JumpToTop()
        {
            m_scrollView.JumpToTop();
        }

        /// <summary>
        /// 跳转到底部
        /// </summary>
        public void JumpToBottom()
        {
            m_scrollView.JumpToBottom(); 
        }

        /// <summary>
        /// 跳转到指定序号
        /// </summary>
        /// <param name="index_"></param>
        /// <param name="posType_"></param>
        /// <returns></returns>
        public bool JumpToIndex(int index_, JumpPosType posType_)
        {
            //if (!m_scrollView.NeedScroll())
            //    //不需要滚动
            //    return false;

            if (index_ < 0 || index_ > m_dataList.Count - 1)
                //超出数据范围
                return false;


            Vector2 contentPos = m_contentTrans.anchoredPosition;
            Vector2 maskSize = m_scrollView.maskTrans.sizeDelta;


            LayoutItem layoutItem = GetLayoutItem(index_);
            Rect rect = layoutItem.rect;
            

            KScrollView.ScrollDir dir = direction;
            if (dir == KScrollView.ScrollDir.vertical)
            {
                float yMin = 0;
                float yMax = m_contentSize.y - maskSize.y;
                float y;
                
                switch(posType_)
                {
                    case JumpPosType.TOP:
                        //列表项位于顶部
                        y = -rect.yMax;    //y是负数
                        break;
                    case JumpPosType.CENTER:
                        //列表项位于中间
                        y = -rect.center.y - maskSize.y * 0.5f;
                        break;
                    default:
                        //列表项位于底部
                        y = -rect.yMin - maskSize.y;
                        break;
                }
                
                contentPos.y = Mathf.Clamp(y, yMin, yMax);
            }
            else
            { 
                float xMin = maskSize.x - m_contentSize.x;
                float xMax = 0;
                float x;

                switch (posType_)
                {
                    case JumpPosType.TOP:
                        //列表项位于顶部
                        x = -rect.xMin;
                        break;
                    case JumpPosType.CENTER:
                        //列表项位于中间
                        x = -rect.center.x + maskSize.x * 0.5f;
                        break;
                    default:
                        //列表项位于底部
                        x = -rect.xMax + maskSize.x;
                        break;
                }
                
                contentPos.x = Mathf.Clamp(x, xMin, xMax);

            }

            m_scrollView.StopMovement(); //停止滑动
            m_contentTrans.anchoredPosition = contentPos;
            return true;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽循环相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        void ShowItems(bool force_)
        {

            Vector2 contentPos = m_contentTrans.anchoredPosition;

            //显示区域
            m_dispRect.Set(
                -contentPos.x,  // >0
                -contentPos.y - m_maskTrans.sizeDelta.y,  // <0 折算到右下
                m_maskTrans.sizeDelta.x,
                m_maskTrans.sizeDelta.y
                );

            Vector2 origin = m_layoutParam.origin;
            Vector2 itemSize = m_layoutParam.itemSize;
            Vector2 gapSize = m_layoutParam.itemGap;

            int begin;
            int end;

            if (direction == KScrollView.ScrollDir.vertical)
            {
                begin = Mathf.FloorToInt(-(m_dispRect.yMax - origin.y) / (itemSize.y + gapSize.y));
                end = Mathf.CeilToInt(-(m_dispRect.yMin - origin.y) / (itemSize.y + gapSize.y));
            }
            else
            {
                begin = Mathf.FloorToInt((m_dispRect.xMin - origin.x) / (itemSize.x + gapSize.x));
                end = Mathf.CeilToInt((m_dispRect.xMax - origin.x) / (itemSize.x + gapSize.x));
            }

            begin = Math.Max(begin, 0);
            end = Math.Min(end, m_dataList.Count - 1);     //不能超过最大行数
            
            ShowItemsFrom(begin, end, force_);
        }

        
        void ShowItemsFrom(int begin_, int end_, bool force_)
        {
            if (m_begin == begin_ && m_end == end_ && !force_)
                return;
            m_begin = begin_;
            m_end = end_;

            __addList.Clear();
            __delList.Clear();

            //记录需要增加的序号
            for (int i = begin_; i <= end_; ++i)
            {
                if (!m_index2item.ContainsKey(i))
                {
                    __addList.Add(i);
                }
            }

            //记录需要移除的序号
            foreach (var kvp in m_index2item)
            {
                if (kvp.Key < begin_ || kvp.Key > end_)
                {
                    __delList.Add(kvp.Key);
                }
            }


            GameObject item;
            int index;
            int delIndex;
            for (int i = 0; i < __addList.Count; ++i)
            {
                index = __addList[i];
                if (__delList.Count > 0)
                {
                    //先从移除列表里面取
                    delIndex = ListUtil.Pop(__delList);
                    item = ChangeItemIndex(delIndex, index);
                }
                else
                {
                    item = CreateItem(index);
                }

                SetItemData(item, index, m_dataList[index]);
            }

            if (__delList.Count > 0)
            {
                //还有要删除的
                for (int i = 0; i < __delList.Count; ++i)
                {
                    index = __delList[i];
                    RecyleItem(index);
                }
            }

            __addList.Clear();
            __delList.Clear();
        }

        //改变某项的序号
        GameObject ChangeItemIndex(int from, int to)
        {
            if (!m_index2item.ContainsKey(from))
            {
                Log.Assert("源序号不存在item", this);
                return null;
            }

            if (m_index2item.ContainsKey(to))
            {
                Log.Assert("目标序号已经存在item", this);
                return null;
            }

            GameObject item = m_index2item[from];
            m_index2item.Remove(from);
            m_index2item[to] = item;
            //item.name = "item(" + to + ")";
            
#if UNITY_EDITOR
            item.name = "item(" + from + " -> " + to + ")";
#endif
            LayItem( to, item);
            return item;
        }


    }


}