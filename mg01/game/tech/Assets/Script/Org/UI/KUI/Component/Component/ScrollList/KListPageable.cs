/* ==============================================================================
 * KListPageable
 * @author jr.zeng
 * 2017/7/24 17:15:46
 * ==============================================================================*/

using System;
using System.Collections;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{
    /// <summary>
    /// 通过页码实现列表项的循环再用
    /// </summary>
    public class KListPageable : KList
    {
        //实际只创建三页Item
        const int REAL_ITEM_PAGE_COUNT = 3; //上一页,当前页,下一页

        Type m_itemType;

        //List展示区域的数据页码
        int m_page = -1;

        //总数据页数
        int m_totalPage;
        public int totalPage { get{ return m_totalPage; }}

        //每页项数
        int m_pageItemCount;
        public int pageItemCount { get { return m_pageItemCount; } }

        IList m_dataList;

        int m_startDataIndex;
        int m_endDataIndex;
        
        protected override int OffsetItemIndex
        {
            get { return m_startDataIndex; }
        }

        //只有第一次设置数据时才需要应用协程创建列表项，以优化列表首次创建所需时间
        bool m_isFirstSetData = false;
        

        override protected void __Initialize()
        {
            base.__Initialize();

            m_isSupportSingle = false;  //不支持单项增加
        }




        //-------∽-★-∽------∽-★-∽--------∽-★-∽设置数据列表∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public override void SetDataList(Type itemType, IList dataList, int coroutineCreateCount = 0, Action onCreateFinished = null)
        {
            //SetDataList(0, itemType, dataList, coroutineCreateCount, onCreateFinished);
            SetDataList(1, itemType, dataList, coroutineCreateCount, onCreateFinished);
        }

        public void SetDataList<T>(int page, IList dataList, int coroutineCreateCount = 0, Action onCreateFinished = null)
        {
            SetDataList(page, typeof(T), dataList, coroutineCreateCount, onCreateFinished);
        }

        public void SetDataList(int page, Type itemType, IList dataList, int coroutineCreateCount = 0, Action onCreateFinished = null)
        {
            if (m_isExecutingCoroutine == true)
            {
                StopAllCoroutines();
            }

            m_itemType = itemType;
            m_dataList = dataList;  //不再支持动态插入数据,所以可以保存数据列表

            //单页数量
            m_pageItemCount = m_leftToRightCount * m_topToBottomCount;
            //总页数  
            m_totalPage = (int)Math.Ceiling((double)m_dataList.Count / m_pageItemCount);
            //从0页开始
            m_page = Mathf.Clamp(page, 0, m_totalPage - 1); 

            //开始的序号
            m_startDataIndex = Mathf.Min(Mathf.Max(0, (m_page - 1) * m_pageItemCount), (m_totalPage - 3) * m_pageItemCount);    //page:0->0, page:1->0, page:2->1
            //结束的序号
            m_endDataIndex = Mathf.Min(m_dataList.Count - 1, (m_page + 2) * m_pageItemCount - 1);

            if (m_isFirstSetData && coroutineCreateCount > 0)
            {
                //启用协程
                StartCoroutine(CreateItemByCoroutine(m_itemType, m_dataList, m_startDataIndex, m_endDataIndex, coroutineCreateCount, null));
            }
            else
            {
                RefreshAllItem();
            }

            m_isFirstSetData = false;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽刷新数据∽-★-∽--------∽-★-∽------∽-★-∽--------//
        void RefreshAllItem()
        {
            int itemCount = Mathf.Max(m_itemList.Count, (m_endDataIndex - m_startDataIndex + 1));
            for (int i = 0; i < itemCount; i++)
            {
                int dataIndex = m_startDataIndex + i;
                if (m_itemList.Count <= i)
                {
                    AddItem(m_itemType, m_dataList[dataIndex], false);
                }
                else
                {
                    RefreshItem(m_itemList[i], dataIndex);
                }
            }
            RefreshAllItemLayout();
        }

        //刷新单项信息
        void RefreshItem(KListItem item, int dataIndex)
        {
            if (dataIndex < m_dataList.Count)
            {
                item.Visible = true;
                item.SetData(m_dataList[dataIndex]);
            }
            else
            {
                item.Visible = false;
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽滚动数据∽-★-∽--------∽-★-∽------∽-★-∽--------//

        
        void ScrollPage(int direction)
        {
            if (direction == 0)
            {
                if (m_itemList.Count < REAL_ITEM_PAGE_COUNT * m_pageItemCount)
                {
                    //不足3页
                    AddOnePage();
                }
            }

            if (direction == 1)
            {
                //下一页
                if (m_page < m_totalPage - 1)
                {
                    //不为最后一页时
                    MoveOnePageNext();
                }
            }
            else if (direction == -1)
            {
                //上一页
                MoveOnePagePrev();
            }
            else
            {
                RefreshAllItem();
            }
        }
        
      
        void AddOnePage()
        {
            int itemCount = Mathf.Min((m_endDataIndex - m_startDataIndex), m_pageItemCount);
            for (int i = 0; i < itemCount; i++)
            {
                int dataIndex = m_startDataIndex + i;
                AddItem(m_itemType, m_dataList[dataIndex], false);
            }
            RefreshAllItemLayout();
        }

        //前滚一页
        void MoveOnePageNext()
        {
            for (int i = 0; i < m_pageItemCount; i++)
            {
                KListItem item = m_itemList[0];
                m_itemList.RemoveAt(0);
                m_itemList.Add(item);
                int dataIndex = m_startDataIndex + m_pageItemCount * 2 + i; //跨两页
                RefreshItem(item, dataIndex);
            }
            RefreshAllItemLayout();
        }

        //后滚一页
        void MoveOnePagePrev()
        {
            for (int i = 0; i < m_pageItemCount; i++)
            {
                KListItem item = m_itemList[m_itemList.Count - 1];
                m_itemList.RemoveAt(m_itemList.Count - 1);
                m_itemList.Insert(i, item);
                int dataIndex = m_startDataIndex + i;
                RefreshItem(item, dataIndex);
            }
            RefreshAllItemLayout();
        }
        

        public int Page
        {
            get { return m_page; }
            set
            {
                if (m_dataList == null)
                {
                    Debug.LogError("需要通过SetDataList接口先设置数据~");
                    return;
                }

                value = Mathf.Clamp(value, 0, m_totalPage-1);
                if (m_page == value)
                    return;
                m_page = value;

                //Log.Debug("m_page = " + m_page, this);

                if (m_isExecutingCoroutine)
                    StopAllCoroutines();

                int oldStartDataIndex = m_startDataIndex;

                //开始的序号, 不能超过倒数第三页
                m_startDataIndex = Mathf.Min( Mathf.Max(0, (m_page - 1) * m_pageItemCount), (m_totalPage - 3) * m_pageItemCount);    //page:0->0, page:1->0, page:2->1
                //结束的序号
                m_endDataIndex = Mathf.Min(m_dataList.Count - 1, (m_page + 2) * m_pageItemCount - 1);
                
                int direction = (m_startDataIndex - oldStartDataIndex) / m_pageItemCount;
                ScrollPage(direction);
            }
        }


    }

}