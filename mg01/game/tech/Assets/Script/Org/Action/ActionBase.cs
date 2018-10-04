/* ==============================================================================
 * 缓动基类
 * @author jr.zeng
 * 2016/10/26 10:33:13
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class ActionBase
    {

        protected GameObject m_target;

        protected bool m_inited = false;
        protected bool m_isDone = false;

        public ActionBase()
        {

        }

        virtual public void StartWithTarget(GameObject target_)
        {
            EnsureInited();

            Reset();

            m_target = target_;
        }


        virtual public void Step(float dt_)
        {

        }

        /** 
         * Called once per frame. time a value between 0 and 1.
         * For example:
         * - 0 Means that the action just started.
         * - 0.5 Means that the action is in the middle.
         * - 1 Means that the action is over.
         *
         * @param time A value between 0 and 1.
         */
        virtual public void Progress(float progress_)
        {

        }


        //动作完成
        protected void Done()
        {
            if (m_isDone)
                return;
            m_isDone = true;
            OnDone();
        }

        virtual protected void OnDone()
        {

        }


        /// <summary>
        /// 重置缓动的状态, 与Start对应
        /// </summary>
        public void Reset()
        {
            if (m_target == null)
                return;

            OnReset();

            m_target = null;
            m_isDone = false;
        }

        virtual protected void OnReset()
        {

        }

        //确保已经初始化
        protected bool EnsureInited()
        {
            if (!m_inited)
            {
                Log.Assert("缓动还没初始化", this);
                return false;
            }

            return true;
        }

        virtual public bool isDone { get { return m_isDone; } }
        virtual public bool Running { get { return m_target != null; } }
        public GameObject Target    { get { return m_target; } }

        virtual public float Duration
        {
            get { return 0; }
            set { }
        }

        virtual protected void OnClear()
        {

        }

        public void Clear()
        {
            if (!m_inited)
                return;
            m_inited = false;

            OnClear();

            m_target = null;
            m_isDone = false;
        }


    }


}