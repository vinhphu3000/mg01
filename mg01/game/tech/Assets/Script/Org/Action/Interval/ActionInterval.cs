/* ==============================================================================
 * 时序动作
 * @author jr.zeng
 * 2016/10/26 11:41:03
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class ActionInterval : ActionBase
    {

        protected float m_duration = 1;
        protected float m_perDelta = 1;

        protected float m_elapsed = 0;

        protected float m_progress = 0;	//当前进度

        public ActionInterval()
        {

        }

        public void InitWithDuration(float duration_)
        {
            Duration = duration_;

            m_inited = true;
        }

        protected override void OnReset()
        {
            m_elapsed = 0;
            m_progress = 0;
        }

        public override void Step(float dt_)
        {

            m_elapsed += dt_;

            float progress = 1;
            if (m_duration > 0)
            {
                progress = m_elapsed * m_perDelta;
            }
            
            Progress(progress);

        }


        public override void Progress(float progress_)
        {
            m_progress = Mathf.Clamp01(progress_);

            OnProgress(m_progress);

            if (m_progress >= 1)
            {
                Done();
            }
        }


        virtual protected void OnProgress(float progress_)
        {

        }

        override public float Duration
        {
            get  { return m_duration; }
            set
            {
                m_duration = value;
                if (m_duration > 0)
                {
                    m_perDelta = Mathf.Abs(1f / m_duration) * Mathf.Sign(m_perDelta);
                }
                else
                {
                    m_duration = 0;
                    m_perDelta = float.MaxValue;
                }

            }
        }

        public float Elapsed        { get { return m_elapsed; } }

      
        protected override void OnClear()
        {
            m_elapsed = 0;
            m_progress = 0;

        }


    }

}