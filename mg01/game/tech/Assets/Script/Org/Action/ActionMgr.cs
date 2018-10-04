/* ==============================================================================
 * ActionMgr
 * @author jr.zeng
 * 2016/10/26 16:32:44
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using mg.org.Actions;

namespace mg.org
{

    public class ActionMgr : CCModule
    {

        static public ActionMgr inst
        {
            get { return InstUtil.Get<ActionMgr>(); }
        }

        //是否自动Update
        public bool isAutoUpdate = true;

        private bool m_invalid = false;

        private List<ActionBase> m_actions = new List<ActionBase>();
        private List<ActionBase> m_delList = new List<ActionBase>();


        public ActionMgr()
        {

        }


        protected override void __Setup(params object[] params_)
        {
        
        }

        protected override void __Clear()
        {
            StopAllActions();
        }

        protected override void SetupEvent()
        {
           
        }

        protected override void ClearEvent()
        {

        }

       

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

       
        override public void Step(float dt_)
        {

            if (m_actions.Count > 0)
            {
                m_invalid = true;

                ActionBase action;
                for (int i = 0; i < m_actions.Count; ++i)
                {
                    action = m_actions[i];

                    if (action.Running)
                    {
                        action.Step(dt_);
                    }

                    if (!action.Running || action.isDone)
                    {
                        if (m_actions.Count == 0)
                        {
                            //列表已清空, 为了支持在action中清空lite
                            break;
                        }
                        else
                        {
                            m_delList.Add(action);
                            action.Clear();
                        }
                    }
                }

                m_invalid = false;
            }

            if (m_delList.Count > 0)
            {
                for (int i = 0, len = m_delList.Count; i < len; ++i)
                {
                    m_actions.Remove(m_delList[i]);
                }

                m_delList.Clear();

                if (m_actions.Count == 0)
                {
                    UnscheduleUpdate();
                }
            }

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public ActionBase Run(GameObject target_, ActionBase action_)
        {
            action_.StartWithTarget(target_);
            m_actions.Add(action_);

            if (m_actions.Count == 1)
            {
                if (isAutoUpdate)
                    ScheduleUpdate();
            }


            return action_;
        }
        
       
        public ActionBase Stop(ActionBase action_)
        {
            if (m_invalid)
            {
                m_delList.Add(action_);
            }
            else
            {
                if (m_actions.Remove(action_))
                {
                    if (m_actions.Count == 0)
                    {
                        UnscheduleUpdate();
                    }
                }
            }

            action_.Clear();
            return null;
        }


        public void Stop(GameObject target_)
        {
            if (m_actions.Count == 0)
                return;

            ActionBase action;
            for (int i = m_actions.Count-1; i>=0; --i)
            {
                action = m_actions[i];
                if (action.Target == target_)
                {
                    if (m_invalid)
                    {
                        m_delList.Add(action);
                    }
                    else
                    {
                        m_actions.RemoveAt(i);
                    }
                    action.Clear();
                }
            }

            if (m_actions.Count == 0)
            {
                UnscheduleUpdate();
            }
        }

       
        public void StopAllActions()
        {
            if (m_actions.Count == 0)
                return;

            ActionBase action;
            for (int i = 0, len = m_actions.Count; i < len; ++i)
            {
                action = m_actions[i];
                action.Clear();
            }

            m_actions.Clear();
            m_delList.Clear();
            UnscheduleUpdate();
        }

    }


}