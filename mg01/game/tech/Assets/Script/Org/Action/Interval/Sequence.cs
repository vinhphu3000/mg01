/* ==============================================================================
 * 串行动作
 * @author jr.zeng
 * 2016/10/27 17:22:12
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class Sequence : ActionInterval
    {

        

        ActionBase m_action1;
        ActionBase m_action2;

        ActionBase m_cutAction;

        float m_split;


        public Sequence()
        {

        }


        public void InitWithActions(params ActionBase[] actions_)
        {
            ActionBase prev = actions_[0];

            int len = actions_.Length - 1;
            if (len > 0)
            {
                //至少有两个
                for (int i = 1; i < len; ++i)
                {
                    prev = CreateWithTwoActions(prev, actions_[i]);
                }

                InitWithTwoActions(prev, actions_[len]);
            }
            else if (len == 0)
            {
                //只有一个
                InitWithTwoActions(prev, null);
            }
            else
            {
                Log.Assert("至少传入一个动作", this);
            }
        }

        public void InitWithTwoActions(ActionBase a1_, ActionBase a2_)
        {
            m_action1 = a1_;

            if(a2_ != null)
            {
                m_action2 = a2_;
            }
            else
            {
                m_action2 = DelayTime.Create(0f);
            }

            float duration = m_action1.Duration + m_action2.Duration;

            //duration不能都为0
            //duration = Mathf.Max(0.001f, duration);
            
            InitWithDuration(duration);

            m_split = m_action1.Duration / m_duration;
        }


        protected override void OnProgress(float progress_)
        {
            ActionBase next;
            float new_dt;

            if (progress_ < m_split)
            {
                next = m_action1;
                if (m_split > 0)
                {
                    new_dt = progress_ / m_split;
                }
                else
                {
                    new_dt = 1;
                }
            }
            else
            {
                //m_act_2必需保证被执行
                next = m_action2;
                if (m_split == 1)
                {
                    new_dt = 1;
                }
                else
                {
                    new_dt = (progress_ - m_split) / (1 - m_split);
                }
            }

            if (next == m_action2)
            {
                if(m_cutAction == null)
                {
                    if(!m_action1.Running)
                        m_action1.StartWithTarget(m_target);
                    m_action1.Progress(1);
                }
                else if (m_cutAction == m_action1)
                {
                    m_action1.Progress(1);
                }
            }
            else
            {
                //action1时
                if (m_cutAction == m_action2)
                {
                    m_action2.Progress(0);
                }
            }

            if (m_cutAction == next)
            {
                if (m_cutAction.isDone)
                    return;
            }
            else
            {
                m_cutAction = next;
                if (!m_cutAction.Running)
                    m_cutAction.StartWithTarget(m_target);
            }

            m_cutAction.Progress(new_dt);
        }


        protected override void OnReset()
        {
            base.OnReset();

            m_action1.Reset();
            m_action2.Reset();
            m_cutAction = null;
        }

        protected override void OnClear()
        {
            base.OnClear();

            m_action1.Clear();
            m_action2.Clear();

            m_action1 = null;
            m_action2 = null;
            m_cutAction = null;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public Sequence Create(params ActionBase[] actions_)
        {
            Sequence action = new Sequence();
            action.InitWithActions(actions_);
            return action;
        }


        static public Sequence CreateWithTwoActions(ActionBase a1_, ActionBase a2_)
        {
            Sequence action = new Sequence();
            action.InitWithTwoActions(a1_, a2_);
            return action;
        }
    }


}