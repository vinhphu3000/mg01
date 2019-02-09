/* ==============================================================================
 * KUIPop
 * @author jr.zeng
 * 2017/6/21 15:19:13
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KUIPop : KUIAbs, IPop
    {

        protected string m_popId = CC_POP_ID.INVALID;
        //生命周期
        protected POP_LIFE m_lifeType = POP_LIFE.STACK;
        //层级id
        protected int m_layerIdx = POP_LAYER_IDX.LAYER_SCENE;
       

        protected Canvas m_canvas;
        protected int m_sortingLayer = 0;
        protected int m_sortingOrder = 0;
        protected int m_planeDistance = 999;

        //打开窗口的时间戳(PopMgr用)
        public float popTime = 0;
        //(PopMgr用)
        public int trashSlot = 0;


        public KUIPop()
        {

        }

        public override void Show(object showObj_, params object[] params_)
        {
            __Show(showObj_, params_);
            
            SetupEvent();
            Pop();
            m_isOpen = true; //开启时访问为false, 用以区分首次打开,因此不支持在show的时候close
        }

        protected void Pop()
        {
            KUIApp.PopMgr.Pop(this);
        }

        //关闭窗口
        public void Close()
        {
            KUIApp.PopMgr.Close(m_popId);
        }

        /// <summary>
        /// 窗口id
        /// </summary>
        public string popID {
            get { return m_popId; }
            set { m_popId = value; }
        }
        /// <summary>
        /// 窗口层级
        /// </summary>
        public int layerIdx { get { return m_layerIdx; } }
        /// <summary>
        /// 生命周期
        /// </summary>
        public POP_LIFE lifeType { get { return m_lifeType; } }


        protected override void __Destroy()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Canvas∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //public Canvas Canvas { get { return m_canvas; } }

        /// <summary>
        /// 镜头距离
        /// </summary>
        public int planeDistance
        {
            get { return m_planeDistance; }
            set
            {
                m_planeDistance = value;
                if (m_canvas)
                    m_canvas.planeDistance = value;
            }
        }

        /// <summary>
        /// 排序层
        /// </summary>
        public int sortingLayer
        {
            get { return m_sortingLayer; }
            set
            {
                m_sortingLayer = value;
                if (m_canvas)
                    m_canvas.sortingLayerName = m_sortingLayer.ToString();
            }
        }

        /// <summary>
        /// 排序号
        /// </summary>
        public int sortingOrder
        {
            get { return m_sortingOrder; }
            set
            {
                m_sortingOrder = value;
                if (m_canvas)
                    m_canvas.sortingOrder = value;
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽GameObject∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected override void ShowGameObject()
        {
            if (m_gameObject)
                return;
            string path = CC_POP_ID.GetPrefebPath(m_popId);
            ShowGameObject(CC_RES_ID.KUI_PREFEB_UI, path);
        }

        protected override void __ShowGameObject()
        {
            base.__ShowGameObject();

            GameObjUtil.ChangeParent(m_gameObject, KUIApp.UILayer);
            m_gameObject.SetActive(true);

            m_canvas = m_gameObject.GetComponent<Canvas>();
            m_canvas.worldCamera = KUIApp.UICamera;
            m_canvas.sortingLayerName = m_sortingLayer.ToString();
            m_canvas.sortingOrder = m_sortingOrder;
            m_canvas.planeDistance = m_planeDistance;
        }


        protected override void __ClearGameObject()
        {
            base.__ClearGameObject();

            m_canvas = null;
        }

    }

}