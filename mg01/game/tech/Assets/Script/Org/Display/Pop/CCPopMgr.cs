/* ==============================================================================
 * 窗口管理基类
 * @author jr.zeng
 * 2017/2/28 11:12:02
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class CCPopMgr : CCModule
    {

        protected Dictionary<string, IPop> m_id2pop = new Dictionary<string, IPop>();

        protected int m_stackMax = 0;   //最大存栈数量
        protected List<IPop> m_openList = new List<IPop>();
        protected List<IPop> m_closeList = new List<IPop>();

        public CCPopMgr()
        {

        }

        protected override void __Setup(params object[] params_)
        {


        }


        protected override void __Clear()
        {

            DelAllPops();
        }

        protected override void SetupEvent()
        {

        }

        protected override void ClearEvent()
        {

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        protected virtual void DestroyPop(IPop pop_)
        {
            pop_.DestroyRemove();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽层级管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 添加到层级
        /// </summary>
        /// <param name="pop_"></param>
        protected virtual void AddToLayer(IPop pop_)
        {
            
        }

        //窗口对齐
       

        //-------∽-★-∽------∽-★-∽--------∽-★-∽窗口操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="popId_"></param>
        /// <param name="showObjs_"></param>
        public IPop Show(string popId_, object showObjs_, params object[] params_)
        {
            IPop pop = GetPop(popId_);
            if (pop != null)
            {
                pop.Show(showObjs_);
                return pop;
            }

            pop = CreatePop(popId_);    //创建窗口
            if (pop == null)
                return null;

            AddPop(pop);    //添加到缓存
            pop.Show(showObjs_, params_);

            return pop;
        }

        public IPop Show(string popId_)
        {
            return Show(popId_, null);
        }


        /// <summary>
        /// 打开或关闭
        /// </summary>
        /// <param name="popId_"></param>
        /// <param name="showObjs_"></param>
        public void ShowOrClose(string popId_, object showObjs_, params object[] params_)
        {
            if (PopIsOpen(popId_))
            {
                Close(popId_);
            }
            else
            {
                Show(popId_, showObjs_, params_);
            }
        }

        public void ShowOrClose(string popId_)
        {
            ShowOrClose(popId_, null);
        }

        //-------∽-★-∽窗口弹出∽-★-∽--------//

        public void Pop(IPop pop_)
        {
            string pop_id = pop_.popID;

            Log.Info("Pop Open: " + pop_id, this);

            AddToOpen(pop_);
            AddToLayer(pop_);

            NotifyWithEvent(POP_EVT.POP_OPEN, pop_id);

            //淡入动画
        }

        //-------∽-★-∽窗口关闭∽-★-∽--------//

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="popId_"></param>
        /// <param name="force_">强制关闭</param>
        public void Close(string popId_, bool force_ = false)
        {
            IPop pop = GetPopOpened(popId_);
            if (pop == null)
                return;

            if (force_)
            {
                //直接关闭
                ExcuteClose(pop);
                return;
            }

            //淡出动画 -> ExcuteClose
            ExcuteClose(pop);
        }

        //执行关闭
        protected void ExcuteClose(IPop pop)
        {
            if (!pop.isOpen)
                return;

            string pop_id = pop.popID;

            DestroyPop(pop);
            AddToClose(pop);

            if (m_openList.Count == 0)
            {
                //没有开启的窗口了
                NotifyWithEvent(POP_EVT.POP_CLOSE_ALL);
            }

            Log.Info("Pop Close: " + pop_id, this);
            NotifyWithEvent(POP_EVT.POP_CLOSE, pop_id);
        }



        /// <summary>
        /// 关闭全部窗口
        /// </summary>
        public void CloseAll()
        {
            int len = m_openList.Count;
            if (len == 0)
                return;

            IPop pop;

            for (int i = 0; i < len; i++)
            {
                pop = m_openList[0];
                Close(pop.popID, true);
            }
        }


        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽窗口管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //添加窗口
        protected void AddPop(IPop pop_)
        {
            string pop_id = pop_.popID;
            if (!m_id2pop.ContainsKey(pop_id))
            {
                m_id2pop[pop_id] = pop_;
                pop_.Retain(this);
            }
            else
            {
                if (m_id2pop[pop_id] != pop_)
                {
                    Log.Assert(false, "repeat pop: " + pop_id, this);
                    return;
                }
            }


        }

        //移除窗口
        protected void DelPop(string popId_)
        {
            if (!m_id2pop.ContainsKey(popId_))
                return;

            IPop pop = m_id2pop[popId_];
            m_id2pop.Remove(popId_);

            RemoveFromStack(pop);

            pop.DestroyRemove();
            pop.Release(this);
            //GameObjUtil.Delete(pop);
        }

        //移除所有窗口
        protected void DelAllPops()
        {
            if (m_id2pop.Count == 0)
                return;

            ClearStack();   //清空堆栈

            IPop[] pops = DicUtil.ToValues<string, IPop>(m_id2pop);
            m_id2pop.Clear();

            IPop pop;
            for (int i = 0; i < pops.Length; ++i)
            {
                pop = pops[i];
                pop.DestroyRemove();
                pop.Release(this);
                //GameObjUtil.Delete( pop );
            }

        }

        public IPop GetPop(string popId_)
        {
            if (m_id2pop.ContainsKey(popId_))
                return m_id2pop[popId_];
            return null;
        }

        /// <summary>
        /// 获取已打开的窗口
        /// </summary>
        /// <param name="popId_"></param>
        /// <returns></returns>
        public IPop GetPopOpened(string popId_)
        {
            IPop pop = GetPop(popId_);
            if (pop != null)
            {
                if (pop.isOpen)
                    return pop;
            }

            return null;
        }

        /// <summary>
        /// 窗口是否打开
        /// </summary>
        /// <param name="popId_"></param>
        /// <returns></returns>
        public bool PopIsOpen(string popId_)
        {
            IPop pop = GetPop(popId_);
            if (pop == null)
                return false;
            return pop.isOpen;
        }

       

        //-------∽-★-∽------∽-★-∽--------∽-★-∽堆栈管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //添加到开启栈
        protected void AddToOpen(IPop pop_)
        {
            //放到栈头
            m_openList.Remove(pop_);
            m_openList.Add(pop_);   

            if (NeedStack(pop_))
            {
                //需要缓存,从关闭列表移除
                m_closeList.Remove(pop_);
            }
        }


        //添加到关闭栈
        protected void AddToClose(IPop pop_)
        {
            m_openList.Remove(pop_);    //从开启栈移除

            if (NeedStack(pop_))
            {

                //放到栈尾
                m_closeList.Remove(pop_);
                m_closeList.Add(pop_); 

                if (m_closeList.Count > m_stackMax)
                {
                    //大于最大堆栈数量，删除栈头
                    DelPop(m_closeList[0].popID);
                }
            }
            else if (pop_.lifeType == POP_LIFE.WEAK)
            {
                //弱引用, 直接删除
                DelPop(pop_.popID);
            }


        }

        //从堆栈移除
        protected virtual void RemoveFromStack(IPop pop_)
        {
            m_openList.Remove(pop_);    //从开启栈移除

            if (NeedStack(pop_))
            {
                //从关闭栈移除
                m_closeList.Remove(pop_);
            }

        }

        //清空堆栈
        protected void ClearStack()
        {
            m_openList.Clear();
            m_closeList.Clear();
        }

        //是否需要存栈
        protected bool NeedStack(IPop pop_)
        {
            if (pop_.lifeType == POP_LIFE.STACK)
                return true;
            return false;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽窗口预制相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

       
        //创建窗口
        protected IPop CreatePop(string popId_)
        {
            string url = CC_POP_ID.GetPrefebPath(popId_);
            if (string.IsNullOrEmpty(url))
            {
                Log.Warn("窗口未注册: " + popId_);
                return null;
            }

            IPop pop = ClassUtil.New(popId_) as IPop;
            pop.popID = popId_;
            return pop;
        }


      

    }

}