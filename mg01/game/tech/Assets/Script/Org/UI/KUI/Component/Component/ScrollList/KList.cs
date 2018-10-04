/* ==============================================================================
 * KList
 * 不支持循环利用
 * @author jr.zeng
 * 2017/7/19 19:23:55
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;



namespace mg.org.KUI
{
    public class KList : KContainer
    {

        public struct KListPadding
        {
            public int top;
            public int right;
            public int bottom;
            public int left;
        }

        /// <summary>
        /// List Item布局，纵向优先（TopToBottom），横向优先（LeftToRight）
        /// 单列列表（LeftToRight = 1，TopToBottom = in.Max)
        /// 单行列表（LeftToRight = int.Max, TopToBottom = 1)
        /// </summary>
        public enum KListDirection
        {
           
            TopToBottom,
            LeftToRight
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽属性∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        protected List<KListItem> m_itemList;
      
        //名为“Container_item”的item模板
        protected GameObject m_itemTemplate;


        /// <summary>
        /// 是否支持以单个Item插入和删除
        /// </summary>
        protected bool m_isSupportSingle;

        protected bool m_isExecutingCoroutine;

        //列表方向
        public KListDirection m_direction = KListDirection.TopToBottom;
        public KListDirection direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }

        //纵向间距
        protected int m_topToBottomGap = 5;
        public int topToBottomGap { get { return m_topToBottomGap; } }
        //横向间距
        protected int m_leftToRightGap = 5;
        public int leftToRightGap { get { return m_leftToRightGap; } }

        //列数
        protected int m_topToBottomCount = int.MaxValue;
        public int topToBottomCount { get { return m_topToBottomCount; } }
        //行数
        protected int m_leftToRightCount = 1;
        public int leftToRightCount { get { return m_leftToRightCount; } }

        //边距
        public KListPadding m_padding = new KListPadding();
        public KListPadding padding
        {
            get { return m_padding; }
            set { m_padding = value; }
        }

        
        //item的尺寸，其值可以为item模板的真实值，也可以指定值
        protected Vector2 m_itemSize;
        public Vector2 itemSize
        {
            get { return m_itemSize; }
            set { m_itemSize = value; }
        }

        //选中项
        public KListItem m_selectedItem;
        public KListItem selectedItem  {  get { return m_selectedItem; }  }

        public KListItem this[int index]
        {
            get  {  return m_itemList[index];  }
        }


        //用于子类KPageableList中跳页
        protected virtual int OffsetItemIndex
        {
            get {  return 0;  }
        }

        //数据改变回调
        public Action<KListItem, object> onDataChange;   

        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件∽-★-∽--------∽-★-∽------∽-★-∽--------//

        int m_evtId_change = AllocUtil.getEvtId();

        int m_evtId_click = AllocUtil.getEvtId();

        KComponentEvent<int, int> m_onSelectedIndexChanged;
        public KComponentEvent<int, int> onSelectedIndexChanged
        {
            get { return KComponentEvent<int, int>.GetEvent(ref m_onSelectedIndexChanged); }
        }

        KComponentEvent<int, int> m_onItemClick;
        public KComponentEvent<int, int> onItemClick
        {
            get { return KComponentEvent<int, int>.GetEvent(ref m_onItemClick); }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽KList∽-★-∽--------∽-★-∽------∽-★-∽--------//

        

        override protected void __Initialize()
        {
            m_itemList = new List<KListItem>();
            m_isSupportSingle = true;

            m_itemTemplate = GameObjUtil.FuzzySearchChild(gameObject, "item");  //只查找一层
            m_itemTemplate.SetActive(false);    //隐藏掉魔板

            if (m_itemSize == Vector2.zero)
            {
                m_itemSize = m_itemTemplate.GetComponent<RectTransform>().sizeDelta;
            }
        }


        protected string GenerateItemName(int index)
        {
            return m_itemTemplate.name + index;
        }

        void onCreateFinished()
        {
            //TODO 派发事件
        }


        void AddItemEventListener(KListItem item)
        {
            item.onClick.AddListener(OnItemClick);
            
            if (onDataChange != null)
            {
                item.onDataChange -= onDataChange;
                item.onDataChange += onDataChange;
            }
        }

        void RemoveItemEventListener(KListItem item)
        {
            item.onClick.RemoveListener(OnItemClick);
            item.onDataChange = null;
        }

        protected virtual void OnItemClick(KListItem target, PointerEventData evtData)
        {
            int oldSelectedIndex = SelectedIndex;
            KComponentEvent<int, int>.InvokeEvent(m_onItemClick, m_evtId_click, target.Index);

            KEvtCenter4Lua.AddUIListEvent(m_evtId_click, target.Index);

            if (target == m_selectedItem)
            {
                //已经是当前项
                return;
            }

            bool isSelectedIndexChanged = (SelectedIndex != oldSelectedIndex);
            if (isSelectedIndexChanged == true)
            {
                //若在_onItemClick事件中修改了KList的SelectedIndex，则不再调用后续事件处理
                return;
            }

            if (m_selectedItem != null)
            {
                m_selectedItem.IsSelected = false;
                m_selectedItem = null;
            }

            m_selectedItem = target;
            m_selectedItem.IsSelected = true;

            KComponentEvent<int, int>.InvokeEvent(m_onSelectedIndexChanged, m_evtId_change, SelectedIndex);
            KEvtCenter4Lua.AddUIListEvent(m_evtId_change, SelectedIndex);
        }


        public void Reorders(int[] orderArray, bool doLayoutImmediately = true)
        {
            int length = orderArray.Length;
            int itemsCount = GetItemCount();
            if (length > itemsCount)
            {
                Log.Error(GetType().ToString() + "无效的重排序列表");
                return;
            }
            KListItem[] array = new KListItem[length];
            for (int i = 0; i < length; ++i)
            {
                int oldOrder = orderArray[i];
                if (oldOrder < 0 || oldOrder >= length)
                {
                    Log.Error(GetType().ToString() + "无效的重排序下标");
                    return;
                }
                array[orderArray[i]] = m_itemList[i];
            }
            for (int i = 0; i < length; ++i)
            {
                m_itemList[i] = array[i];
            }
            if (doLayoutImmediately == true)
            {
                RefreshAllItemLayout();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽设置数据列表∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 设置数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="coroutineCreateCount"></param>
        /// <param name="onCreateFinished"></param>
        public virtual void SetDataList<T>(IList dataList, int coroutineCreateCount = 0, Action onCreateFinished = null) where T : KListItem
        {
            SetDataList(typeof(T), dataList, coroutineCreateCount, onCreateFinished);
        }

        /// <summary>
        /// 设置数据列表
        /// 可以通过coroutineCreateCount来分帧创建item以优化性能
        /// onCreateFinished为所有item创建结束后回调
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="dataList">由于支持动态添加数据,所以本地不保存dataList</param>
        /// <param name="coroutineCreateCount"></param>
        /// <param name="onCreateFinished"></param>
        public virtual void SetDataList(Type itemType, IList dataList, int coroutineCreateCount = 0, Action onCreateFinished = null)
        {
            if (m_isExecutingCoroutine)
            {
                //异步创建中
                StopAllCoroutines();
            }

            if (coroutineCreateCount == 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    AddItem(itemType, dataList[i], false);
                }
                RefreshAllItemLayout();
            }
            else
            {
                StartCoroutine( CreateItemByCoroutine(itemType, dataList, 0, dataList.Count - 1, coroutineCreateCount, onCreateFinished) );
            }
        }

        protected virtual IEnumerator CreateItemByCoroutine(Type itemType, IList dataList, int startDataIndex, int endDataIndex, int coroutineCreateCount, Action onCreateFinished)
        {
            m_isExecutingCoroutine = true;

            coroutineCreateCount = coroutineCreateCount == 0 ? dataList.Count : coroutineCreateCount;

            int index = 0;
            while (index < endDataIndex)
            {
                index = Mathf.Min(endDataIndex, startDataIndex + coroutineCreateCount);
                for (int i = startDataIndex; i < index; i++)
                {
                    AddItem(itemType, dataList[i], false);
                }
                startDataIndex = index;
                RefreshAllItemLayout();
                yield return null;
            }

            m_isExecutingCoroutine = false;

            if (onCreateFinished != null)
                onCreateFinished();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽添加列表项∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void AddItem<T>(object data, bool doLayoutImmediately = true) where T : KListItem
        {
            AddItemAt<T>(data, m_itemList.Count, doLayoutImmediately);
        }

        protected void AddItem(Type itemType, object data, bool doLayoutImmediately = true)
        {
            AddItemAt(itemType, data, m_itemList.Count, doLayoutImmediately);
        }
        

        public void AddItemAt<T>(object data, int index, bool doLayoutImmediately = true) where T : KListItem
        {
            if (m_isSupportSingle == false)
            {
                Log.Error(GetType().ToString() + ":不支持插入单个项~", this);
                return;
            }

            AddItemAt(typeof(T), data, index, doLayoutImmediately);
        }
        
        protected void AddItemAt(Type itemType, object data, int index, bool doLayoutImmediately = true)
        {
            if (m_isExecutingCoroutine == true)
            {
                Log.Error("有尚未执行完毕的协程~~", this);
                return;
            }

            //要改成循环利用

            GameObject go = GameObjUtil.Instantiate(m_itemTemplate);
            go.transform.SetParent(this.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);


            //BuildItem(go);
            KListItem item = ComponentUtil.EnsureComponent(go, itemType) as KListItem;
            item.Index = index;
            item.name = GenerateItemName(index);
            AddItemEventListener(item);
            m_itemList.Insert(index, item);

            item.SetData(data);   //设置数据
            
            if (doLayoutImmediately)
            {
                RefreshAllItemLayout();
            }
        }
        


        //-------∽-★-∽------∽-★-∽--------∽-★-∽布局相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        protected void RefreshAllItemLayout()
        {
            RectTransform listRect = GetComponent<RectTransform>();
            float right = 0;
            float bottom = 0;
            for (int i = 0; i < m_itemList.Count; i++)
            {
                int index = OffsetItemIndex + i;    //可能有偏移,不是从0开始

                KListItem item = m_itemList[i];
                RectTransform itemRect = item.GetComponent<RectTransform>();
                Vector2 pivot = itemRect.pivot;
                Vector2 position = CalculateItemPosition(index);

                if (item.Visible)
                {
                    //可见时才算入尺寸
                    right = position.x + m_itemSize.x + m_padding.right;
                    bottom = position.y + m_itemSize.y + m_padding.bottom;
                }

                position.x = position.x + m_itemSize.x * pivot.x;
                position.y = position.y + m_itemSize.y * (1 - pivot.y);
                itemRect.anchoredPosition = new Vector2(position.x, -position.y);

                if (item.Index != index)
                {
                    item.Index = index;
                    item.name = GenerateItemName(index);
                }
            }
            listRect.sizeDelta = new Vector2(right, bottom);
        }

        protected Vector2 CalculateItemPosition(int index)
        {
            Vector2 result = Vector2.zero;
            int pageCount = m_leftToRightCount * m_topToBottomCount;
            if (m_direction == KListDirection.LeftToRight)
            {
                result.x = m_padding.left + ((index / pageCount) * m_leftToRightCount + index % m_leftToRightCount) * (m_itemSize.x + m_leftToRightGap);
                result.y = m_padding.top + ((index % pageCount) / m_leftToRightCount) * (m_itemSize.y + m_topToBottomGap);
            }
            else
            {
                //float i = ((index % pageCount) / m_topToBottomCount);   //整形相除会取整
                result.x = m_padding.left + ((index % pageCount) / m_topToBottomCount) * (m_itemSize.x + m_leftToRightGap);
                result.y = m_padding.top + ((index / pageCount) * m_topToBottomCount + index % m_topToBottomCount) * (m_itemSize.y + m_topToBottomGap);
            }
            return result;
        }

       

        //-------∽-★-∽------∽-★-∽--------∽-★-∽移除相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public void RemoveItemAt(int index)
        {
            RemoveItem(index);
            RefreshAllItemLayout();
        }

        void RemoveItem(int index)
        {
            if (m_isSupportSingle == false)
            {
                Log.Error(GetType().ToString() + " 不支持删除Item~~");
                return;
            }

            if (index > m_itemList.Count - 1)
                // 没有这个序号
                return;

            KListItem item = m_itemList[index];
            m_itemList.RemoveAt(index);
            RemoveItemEventListener(item);
            Destroy(item.gameObject);
        }

        public void RemoveItemRange(int start, int count)
        {
            for (int i = start + count - 1; i >= start; i--)
            {
                RemoveItem(i);
            }
            RefreshAllItemLayout();
        }

        public void RemoveAll()
        {
            bool b = m_isSupportSingle;
            m_isSupportSingle = true;

            RemoveItemRange(0, m_itemList.Count);

            m_isSupportSingle = b;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽public∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public KListItem GetItemAt(int index)
        {
            return m_itemList[index];
        }

        public GameObject GetChildAt(int index)
        {
            return m_itemList[index].gameObject;
        }

        public int GetItemCount()
        {
            return m_itemList.Count;
        }


        /// <summary>
        /// 设置边距
        /// </summary>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        public void SetPadding(int top, int right, int bottom, int left)
        {
            KListPadding padding = new KListPadding();
            padding.top = top;
            padding.right = right;
            padding.bottom = bottom;
            padding.left = left;

            m_padding = padding;
        }

        /// <summary>
        /// 设置间距
        /// </summary>
        /// <param name="leftToRightGap"></param>
        /// <param name="topToBottomGap"></param>
        public void SetGap(int leftToRightGap, int topToBottomGap)
        {
            m_topToBottomGap = topToBottomGap;
            m_leftToRightGap = leftToRightGap;
        }

        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="leftToRightCount"></param>
        /// <param name="topToBottomCount"></param>
        public void SetDirection(KListDirection direction, int leftToRightCount, int topToBottomCount)
        {
            m_direction = direction;
            if (m_direction == KListDirection.LeftToRight)
            {
                m_leftToRightCount = leftToRightCount;
                m_topToBottomCount = Mathf.Min(topToBottomCount, int.MaxValue / leftToRightCount);
            }
            else
            {
                m_leftToRightCount = Mathf.Min(leftToRightCount, int.MaxValue / topToBottomCount);
                m_topToBottomCount = topToBottomCount;
            }
        }

        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="count">方向上显示的个数,需要修改</param>
        public void SetDirection(KListDirection direction, int count = int.MaxValue)
        {
            if (direction == KListDirection.LeftToRight)
            {
                SetDirection(direction, count, int.MaxValue);
            }
            else
            {
                SetDirection(direction, int.MaxValue, count);
            }
        }



        public void DoLayout()
        {
            RefreshAllItemLayout();
        }

        /// <summary>
        /// 设置选中序号
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (m_selectedItem == null)
                    return -1;
                return m_selectedItem.Index;
            }

            set
            {
                if (m_selectedItem != null)
                {
                    m_selectedItem.IsSelected = false;  //重置之前的选中
                    m_selectedItem = null;
                }

                if (value >= 0 && value <= m_itemList.Count - 1)
                {
                    m_selectedItem = m_itemList[value];
                    m_selectedItem.IsSelected = true;
                }
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽lua∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //为Lua导出的方法，Item类型固定使用KListItem，dataList为自动生成的一个List，只构造gameObject，不涉及数据
        //回调函数改成派发事件
        public void CreateItems(int itemCount, int coroutineCreateCount = 0)
        {
            List<int> dataList = new List<int>();
            for (int i = 0; i < itemCount; ++i)
            {
                dataList.Add(i);
            }
            SetDataList(typeof(KListItem), dataList, coroutineCreateCount, onCreateFinished);
        }


        //为Lua导出的方法，Item类型固定使用KListItem，数据为空，只构造gameObject
        public void AddItem(bool doLayoutImmediately = true)
        {
            AddItemAt<KListItem>(null, m_itemList.Count, doLayoutImmediately);
        }

        //为Lua导出的方法，Item类型固定使用KListItem，数据为空，只构造gameObject
        public void AddItemAt(int index, bool doLayoutImmediately = true)
        {
            AddItemAt<KListItem>(null, index, doLayoutImmediately);
        }

    }


}