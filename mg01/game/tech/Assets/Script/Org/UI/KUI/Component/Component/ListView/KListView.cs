/* ==============================================================================
 * KListView
 * @author jr.zeng
 * 2017/9/1 14:14:59
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
    public class KListView : KContainer
    {
        //数据更新回调
        KComponentEvent<GameObject, int, object> m_onDataChanged;
        public new KComponentEvent<GameObject, int, object> onDataChanged { get { return KComponentEvent<GameObject, int, object>.GetEvent(ref m_onDataChanged); } }
        //数据移除回调
        KComponentEvent<GameObject, int> m_onDataRemoved;
        public new KComponentEvent<GameObject, int> onDataRemoved { get { return KComponentEvent<GameObject, int>.GetEvent(ref m_onDataRemoved); } }


        //布局参数
        protected LayoutParam m_layoutParam = new LayoutParam();
        List<LayoutItem> m_layoutItems = new List<LayoutItem>();

        //列表项容器
        protected GameObject m_contentGo;

        //item模板,一般名为“Container_item”的
        protected GameObject m_itemTemp;
        //protected Vector2    m_itemTempSize;

        
        //item列表
        List<GameObject> m_itemPool = new List<GameObject>();
        protected Dictionary<int, GameObject> m_index2item = new Dictionary<int, GameObject>();

        //视图类型, 必须是KListViewItem
        Type m_itemViewType = null;
        Dictionary<GameObject, KListViewItem> m_item2view = new Dictionary<GameObject, KListViewItem>();

        //数据列表
        protected IList m_dataList;
        protected int m_dataLenLast = 0;
        protected Dictionary<GameObject, object> m_item2data = new Dictionary<GameObject, object>();

        //生成纯序列时用
        protected List<int> tmp_seq;   


        protected override void OnDestroy()
        {
            ClearItemViews(true);
            ClearList();
            m_itemPool.Clear();
            

            base.OnDestroy();
            
        }

      

        protected override void __Initialize()
        {

            m_contentGo = gameObject;

            m_itemTemp = GameObjUtil.FuzzySearchChild(m_contentGo, "item");  //只查找一层
            m_itemTemp.SetActive(false);    //隐藏掉模板

            RectTransform rectTrans = m_itemTemp.GetComponent<RectTransform>();

            m_layoutParam.itemSize = rectTrans.sizeDelta;
            m_layoutParam.pivot = rectTrans.pivot;
        }


        /// <summary>
        /// 根据序号获取列表项
        /// </summary>
        /// <param name="index_"></param>
        /// <returns></returns>
        public virtual GameObject GetItemByIndex(int index_)
        {
            if (m_index2item.ContainsKey(index_))
                return m_index2item[index_];
            return null;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽layout相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 布局参数
        /// </summary>
        public LayoutParam layoutParam
        {
            get {  return m_layoutParam; }
        }


        //初始化布局参数(lua用)
        public void InitLayoutParam(LayoutDirection dir_,
                int divNum_,
                float itemSizeX_, float itemSizeY_,
                float itemGapX_, float itemGapY_,
                float originX_, float originY_,
                float pivotX_, float pivotY_,
                float paddingL_, float paddingT_, float paddingR_, float paddingB_)
        {

            if (itemSizeX_ <=0 || itemSizeY_ <= 0)
            {
                itemSizeX_ = m_layoutParam.itemSize.x;
                itemSizeY_ = m_layoutParam.itemSize.y;
            }

            if (pivotX_ < 0 || pivotY_ < 0)
            {
                pivotX_ = m_layoutParam.pivot.x;
                pivotY_ = m_layoutParam.pivot.y;
            }
            

            m_layoutParam.Init(
                dir_, 
                divNum_, 
                itemSizeX_, itemSizeY_, 
                itemGapX_, itemGapY_, 
                originX_, originY_, 
                pivotX_, pivotY_, 
                paddingL_, paddingT_, paddingR_, paddingB_);
  
        }

        //初始化布列表
        protected void GenLayoutItems(int len_)
        {

            LayoutUtil.CreateLayoutItems(m_layoutParam, len_, m_layoutItems);

        }

        //清空布局列表
        protected void ClearLayoutItems()
        {

            LayoutUtil.RecyleLayoutItems(m_layoutItems);
        }

        protected LayoutItem GetLayoutItem(int index_)
        {
            LayoutItem layouItem = m_layoutItems[index_];
            return layouItem;
        }


        protected Vector2 LayItem(int index_, GameObject item_)
        {
            LayoutItem layouItem = m_layoutItems[index_];

            RectTransform rectTrans = item_.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = layouItem.pos;
            return layouItem.pos;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        public virtual void ShowList(IList dataList_)
        {
            m_dataList = dataList_;
            m_item2data.Clear();    //清除原来的数据

            GenLayoutItems(m_dataList.Count);

            __ShowList();

            m_dataLenLast = m_dataList.Count;
        }

        public void ShowLen(int len_)
        {
            if (tmp_seq == null)
                tmp_seq = new List<int>();

            ShowList(ListUtil.GenSequence(len_, 1, 1, tmp_seq));
        }


        //显示列表
        protected virtual void __ShowList()
        {
            
            GameObject item;

            int index = 0;
            int len = m_dataList.Count;
            for (int i = 0; i < len; ++i)
            {
                index = i;
                item = CreateItem(index);

                SetItemData(item, index, m_dataList[i]);
            }

            ++index;
            if (m_dataLenLast > index)
            {
                //隐藏剩余项
                for (int i = m_dataLenLast - 1; i >= index; --i)
                {
                    RecyleItem(i);
                }
            }

        }


        public void ClearList()
        {
            if (m_dataList == null)
                return;
            m_dataList = null;
            m_dataLenLast = 0;

            int[] toDel = DicUtil.ToKeys(m_index2item);

            int index;
            for (int i = 0; i < toDel.Length; ++i)
            {
                index = toDel[i];
                RecyleItem(index);
            }

            m_index2item.Clear();
            m_item2data.Clear();

            ClearItemViews();
            ClearLayoutItems();

            __ClearList();
        }

        protected virtual void __ClearList()
        {


        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽列表项相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //创建列表项
        protected GameObject CreateItem(int index_)
        {
            if (m_index2item.ContainsKey(index_))
                return m_index2item[index_];

            GameObject item;

            if (m_itemPool.Count > 0)
            {
                item = ListUtil.Pop(m_itemPool);  //先从池里拿
            }
            else
            {
                item = GameObjUtil.Instantiate(m_itemTemp);
                GameObjUtil.ChangeParent(item, m_contentGo);
            }

            m_index2item[index_] = item;
            item.SetActive(true);
            item.name = "item(" + index_ + ")";

            LayItem(index_, item);
            return item;
        }



        //回收列表项
        protected void RecyleItem(int index_)
        {
            if (!m_index2item.ContainsKey(index_))
                return;

            GameObject item = m_index2item[index_];

            ClearItemData(item, index_);    //清除数据
            
            m_index2item.Remove(index_);

            m_itemPool.Add(item);
            item.SetActive(false);
        }

       

        //为item设置data
        protected void SetItemData(GameObject item, int index_, object data_)
        {
            if(m_item2data.ContainsKey(item))
            {
                if (m_item2data[item] == data_) 
                    //数据一样，不重复设置
                    return;
            }
            
            m_item2data[item] = data_;

            KComponentEvent<GameObject, int, object>.InvokeEvent(m_onDataChanged, item, index_, data_);
            
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.LIST_ITEM_DATA, item, index_+1);    //lua从1开始

            KListViewItem view = CreateItemView(item, index_);
            if (view != null)
            {
                view.Index = index_;
                view.Show(data_);
            }
        }

        //清除item的data
        void ClearItemData(GameObject item, int index_)
        {
            if (!m_item2data.ContainsKey(item))
                return;
            m_item2data.Remove(item);

            KComponentEvent<GameObject, int>.InvokeEvent(m_onDataRemoved, item, index_);

            KListViewItem view = CreateItemView(item, index_);
            if (view != null)
            {
                view.Index = -1;
                view.Destroy();
            }
        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ItemView∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 列表项视图类型
        /// </summary>
        public Type itemViewType
        {
            get  { return m_itemViewType;  }
            set
            {
                m_itemViewType = value;
#if UNITY_EDITOR
                if (!ClassUtil.IsParentType(m_itemViewType, typeof(KListViewItem)))
                    Log.Assert(string.Format("必须是{0}的子类", typeof(KListViewItem).Name));
#endif
            }
        }

        //创建列表视图
        KListViewItem CreateItemView(GameObject item_, int index_)
        {
            if (m_itemViewType == null)
                return null;

            if (m_item2view.ContainsKey(item_))
                return m_item2view[item_];
            
            KListViewItem view = ClassUtil.New(m_itemViewType) as KListViewItem;
            view.ShowGameObjectEx(item_);

            m_item2view[item_] = view;
            view.Retain(this);

            return view;
        }

        //清除所有列表视图
        //@del_ 是否删除
        void ClearItemViews(bool del_ = false)
        {
            if (m_item2view.Count == 0)
                return;


            KListViewItem view;
            foreach (var kvp in m_item2view)
            {
                view = kvp.Value;
                view.Destroy();

                if (del_)
                {
                    view.Release(this);
                }
            }

            if (del_)
            {
                m_item2view.Clear();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ListViewItem∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 列表项基类
        /// </summary>
        public class KListViewItem: KUIAbs
        {
            protected int m_index = -1;

            public KListViewItem()
            {

            }
            
            /// <summary>
            /// 列表项序号
            /// </summary>
            public int Index
            {
                get { return m_index; }
                set { m_index = value; }
            }

            protected override void __Destroy()
            {


            }


        }
    }

}