/* ==============================================================================
 * Ease
 * @author jr.zeng
 * 2016/11/1 17:18:38
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{


    public class Ease : ActionInterval
    {

        public enum Type
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            BounceIn,
            BounceOut,
        }

        /// Whether the tweener will use steeper curves for ease in / out style interpolation.
        public bool steeperCurves = false;

        private Type m_type = Type.Linear;

        ActionInterval m_action;

        public Ease()
        {

        }

        public void InitWithAction(ActionInterval action_, Type type_ = Type.Linear)
        {
            m_action = action_;
            m_type = type_;

            InitWithDuration(m_action.Duration);

        }

        public override void StartWithTarget(GameObject target_)
        {
            base.StartWithTarget(target_);

            m_action.StartWithTarget(m_target);
        }

        public override void Progress(float progress_)
        {
            progress_ = Sample(progress_);
            base.Progress(progress_);
        }


        protected override void OnProgress(float progress_)
        {
            base.OnProgress(progress_);

            m_action.Progress(progress_);
        }

        protected override void OnReset()
        {
            base.OnReset();

            m_action.Reset();
        }


        protected override void OnClear()
        {
            base.OnClear();

            m_action.Clear();
            m_action = null;
        }

        float Sample(float factor)
        {
            // Calculate the sampling value
            float val = Mathf.Clamp01(factor);

            if (m_type == Type.EaseIn)
            {
                val = 1f - Mathf.Sin(0.5f * Mathf.PI * (1f - val));
            }
            else if (m_type == Type.EaseOut)
            {
                val = Mathf.Sin(0.5f * Mathf.PI * val);

                if (steeperCurves)
                {
                    val = 1f - val;
                    val = 1f - val * val;
                }
            }
            else if (m_type == Type.EaseInOut)
            {
                const float pi2 = Mathf.PI * 2f;
                val = val - Mathf.Sin(val * pi2) / pi2;

                if (steeperCurves)
                {
                    val = val * 2f - 1f;
                    float sign = Mathf.Sign(val);
                    val = 1f - Mathf.Abs(val);
                    val = 1f - val * val;
                    val = sign * val * 0.5f + 0.5f;
                }
            }
            else if (m_type == Type.BounceIn)
            {
                val = BounceLogic(val);
            }
            else if (m_type == Type.BounceOut)
            {
                val = 1f - BounceLogic(1f - val);
            }

            return val;
        }


        float BounceLogic(float val)
        {
            if (val < 0.363636f) // 0.363636 = (1/ 2.75)
            {
                val = 7.5685f * val * val;
            }
            else if (val < 0.727272f) // 0.727272 = (2 / 2.75)
            {
                val = 7.5625f * (val -= 0.545454f) * val + 0.75f; // 0.545454f = (1.5 / 2.75) 
            }
            else if (val < 0.909090f) // 0.909090 = (2.5 / 2.75) 
            {
                val = 7.5625f * (val -= 0.818181f) * val + 0.9375f; // 0.818181 = (2.25 / 2.75) 
            }
            else
            {
                val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f; // 0.9545454 = (2.625 / 2.75) 
            }
            return val;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public Ease Create(ActionInterval action_, Type typs_ = Type.Linear)
        {
            Ease action = new Ease();
            action.InitWithAction(action_, typs_);
            return action;
        }


    }


}