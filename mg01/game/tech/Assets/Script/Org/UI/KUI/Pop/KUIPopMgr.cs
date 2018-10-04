/* ==============================================================================
 * KUIPopMgr
 * @author jr.zeng
 * 2017/6/19 17:17:48
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KUIPopMgr : CCPopMgr
    {

        int m_canvasNumMax = 20;    //画布渲染上限
        int m_canvasDisStart = 5;   //窗口的默认间距
        int m_canvasDisPer = 15;    //窗口的默认间距
        int m_canvasDisMax = 0;   //最大渲染距离

        bool m_sortPopCalled = false;   //延时排序

        Dictionary<int, bool> m_trash = new Dictionary<int, bool>();

        public KUIPopMgr()
        {
            //最大堆栈数量
            m_stackMax = 5;
            //
            m_canvasDisMax = m_canvasDisStart + m_canvasNumMax * m_canvasDisPer;
            KUIApp.UICamera.farClipPlane = m_canvasDisMax;  //设置镜头的最大渲染距离


        }

        override protected void __Setup(params object[] params_)
        {


        }

        override protected void __Clear()
        {

            base.__Clear();

            m_trash.Clear();


        }

        override protected void SetupEvent()
        {
           
        }

        override protected void ClearEvent()
        {

            if (m_sortPopCalled)
            {
                m_sortPopCalled = false;
                TimerMgr.inst.RemoveCallDelay(OnSortPop);
                //CCApp.Detach(GL_EVENT.ENTER_FRAME_LATE, OnSortPop);
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        override protected void DestroyPop(IPop pop_)
        {
            KUIPop pop = pop_ as KUIPop;

            pop.Destroy();
            AddToTrash(pop);
        }

        protected override void RemoveFromStack(IPop pop_)
        {
            base.RemoveFromStack(pop_);
            
            RemoveFromTrash(pop_ as KUIPop);
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽窗口管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        new public KUIPop GetPop(string popId_)
        {
            if (m_id2pop.ContainsKey(popId_))
                return m_id2pop[popId_] as KUIPop;
            return null;
        }

        new public KUIPop GetPopOpened(string popId_)
        {
            KUIPop pop = GetPop(popId_);
            if (pop != null)
            {
                if (pop.isOpen)
                    return pop;
            }

            return null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽层级管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        override protected void AddToLayer(IPop pop_)
        {
            KUIPop pop = pop_ as KUIPop;
            pop.popTime = DateUtil.TimeFromStart;  //记录开启时间

            RemoveFromTrash(pop);

            if (!m_sortPopCalled)
            {
                m_sortPopCalled = true;
                TimerMgr.inst.CallDelay(OnSortPop);
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽渲染排序∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //窗口渲染排序
        void SortAllPop()
        {

            List<IPop> popList = ListUtil.Clone(m_openList);

            ListUtil.SortOn(popList, new string[] { "layerIdx", "popTime" }, SortOption.DESCENDING);

            KUIPop pop;
            int dis;
            int len = popList.Count;
            for (int i=0; i< len;++i)
            {
                pop = popList[i] as KUIPop;
                dis = m_canvasDisStart + m_canvasDisPer * i;    //镜头距离

                pop.sortingLayer = pop.layerIdx;
                pop.sortingOrder = len - i;
                pop.planeDistance = dis;
            }

            m_sortPopCalled = false;
        }
        
        

        void OnSortPop(object obj_)
        {
            //TimerMgr.Inst.RemoveCallDelay(OnSortPop);
            //CCApp.Detach(GL_EVENT.ENTER_FRAME_LATE, OnSortPop);

            if (m_sortPopCalled)
            {
                SortAllPop();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽垃圾桶∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void AddToTrash(KUIPop pop_)
        {

            int slot = 0;

            foreach (var kvp in m_trash)
            {
                if (!kvp.Value)
                {
                    slot = kvp.Key;
                    break;
                }
            }

            if (slot == 0)
                slot = m_trash.Count + 1;
            m_trash[slot] = true;

            pop_.trashSlot = slot;
            int dis = m_canvasDisMax + slot * m_canvasDisPer;
            pop_.sortingLayer = POP_LAYER_IDX.LAYER_NONE; //重置layer，然后会阻挡鼠标事件
            pop_.sortingOrder = 0;      //不置0会阻挡鼠标事件
            pop_.planeDistance = dis;
            Log.Debug("AddToTrash: "+ slot, this);
        }

        void RemoveFromTrash(KUIPop pop_)
        {
            if (pop_.trashSlot <= 0)
                return;

            int slot = pop_.trashSlot;
            m_trash[slot] = false;

            Log.Debug("RemoveFromTrash:"+ slot, this);
        }


    }
}