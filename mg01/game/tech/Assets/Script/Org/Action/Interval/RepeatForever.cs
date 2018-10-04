/* ==============================================================================
 * RepeatForever
 * @author jr.zeng
 * 2016/11/1 10:03:05
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{


    public class RepeatForever : ActionInterval
    {

        ActionInterval m_action;


        public RepeatForever()
        {

        }


        public void InitWithAction(ActionInterval action_)
        {
            m_action = action_;

            m_inited = true;
        }


        public override void StartWithTarget(GameObject target_)
        {
            base.StartWithTarget(target_);

            m_action.StartWithTarget(m_target);

        }

        public override void Step(float dt_)
        {

            m_action.Step(dt_);

            if (m_action.isDone)
            {
                float duration = m_action.Duration;

                float diff = m_action.Elapsed - duration;
                if (diff > duration)
                    //超过了一个周期
                    diff = diff % duration;

                m_action.StartWithTarget(m_target);
                // to prevent jerk. issue #390, 1247
                m_action.Step(0.0f);
                m_action.Step(diff);
            }
        }


        public override bool isDone
        {
            get { return false; }
        }

        protected override void OnClear()
        {
            base.OnClear();

            m_action.Clear();
            m_action = null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public RepeatForever Create(ActionInterval action_)
        {
            RepeatForever action = new RepeatForever();
            action.InitWithAction(action_);
            return action;
        }

    }


}