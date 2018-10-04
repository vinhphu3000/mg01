/* ==============================================================================
 * 并行动作
 * @author jr.zeng
 * 2016/10/29 9:50:50
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace mg.org.Actions
{

    public class Spawn : ActionInterval
    {

        static public Spawn Create(params ActionBase[] actions_)
        {
            Spawn action = new Spawn();
            action.InitWithActions(actions_);
            return action;
        }

        static public Spawn CreateWithTwoActions(ActionBase a1_, ActionBase a2_)
        {
            Spawn action = new Spawn();
            action.InitWithTwoActions(a1_, a2_);
            return action;
        }


        ActionBase m_action1;
        ActionBase m_action2;

        public Spawn()
        {

        }

        public void InitWithActions(params ActionBase[] actions_)
        {

            int size = actions_.Length;

            if (size == 0)
            {
                InitWithDuration(0);
                return;
            }

            if (size == 1)
            {
                InitWithTwoActions(actions_[0], DelayTime.Create(0));
                return;
            }


            ActionBase prev = actions_[0];

            //从第二个开始
            for (int i = 1, len = actions_.Length - 1; i < len; ++i)
            {
                prev = CreateWithTwoActions(prev, actions_[i]);
            }

            InitWithTwoActions(prev, actions_.Last());
        }

        public void InitWithTwoActions(ActionBase a1_, ActionBase a2_)
        {

            if (a2_ == null)
            {
                a2_ = DelayTime.Create(0);
            }

            float d1 = a1_.Duration;
            float d2 = a2_.Duration;
            float maxTime = Mathf.Max(d1, d2);


            InitWithDuration(maxTime);

            if (d1 > d2)
            {
                m_action1 = a1_;
                m_action2 = Sequence.CreateWithTwoActions(a2_, DelayTime.Create(d1 - d2));

            }
            else
            {
                m_action1 = Sequence.CreateWithTwoActions(a1_, DelayTime.Create(d2 - d1));
                m_action2 = a2_;
            }

        }


        public override void StartWithTarget(GameObject target_)
        {
            base.StartWithTarget(target_);

            m_action1.StartWithTarget(m_target);
            m_action2.StartWithTarget(m_target);
        }

        
        //进度更新
        protected override void OnProgress(float progress_)
        {

            m_action1.Progress(progress_);
            m_action2.Progress(progress_);
        }
       

        protected override void OnReset()
        {
            base.OnReset();

            m_action1.Reset();
            m_action2.Reset();

        }



        protected override void OnClear()
        {
            base.OnClear();

            m_action1.Clear();
            m_action2.Clear();
            m_action1 = null;
            m_action2 = null;
        }

    }


}