/* ==============================================================================
 * KListItem
 * @author jr.zeng
 * 2017/7/19 19:24:07
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{
    public class KListItem : KContainer, IPointerClickHandler
    {
        int m_index = 0;

        bool m_selected = false;

        object m_data = null;

        //数据改变回调
        public Action<KListItem, object> onDataChange;    

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data_"></param>
        public void SetData(object data_)
        {
            if (m_data == data_)
                return;
            m_data = data_;

            __OnData();

            if (onDataChange != null)
                onDataChange(this, data_);
        }


        protected virtual void __OnData()
        {


        }

        public virtual int Index
        {
            get { return m_index; }
            set
            {
                m_index = value;
            }
        }


        public virtual bool IsSelected
        {
            get { return m_selected; }
            set
            {
                m_selected = value;
            }
        }

        
        //public KList List
        //{
        //    get { return this.transform.parent.GetComponent<KList>(); }
        //}


        //-------∽-★-∽------∽-★-∽--------∽-★-∽点击事件∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected KComponentEvent<KListItem, PointerEventData> m_onClick;
        public KComponentEvent<KListItem, PointerEventData> onClick
        {
            get { return KComponentEvent<KListItem, PointerEventData>.GetEvent(ref m_onClick); }
        }

        /// <summary>
        /// 点击事件(内部冒泡事件)
        /// 这个要改
        /// </summary>
        /// <param name="evtData"></param>
        public virtual void OnPointerClick(PointerEventData evtData)
        {
            if (evtData.dragging == true)
            {
                return;
            }

            this.IsSelected = true;
            KComponentEvent<KListItem, PointerEventData>.InvokeEvent(m_onClick, this, evtData);
        }

    }

}