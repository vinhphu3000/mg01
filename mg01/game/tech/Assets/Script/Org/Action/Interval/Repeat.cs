/* ==============================================================================
 * 重复动作
 * @author jr.zeng
 * 2016/10/29 11:42:25
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{


    public class Repeat : ActionInterval
    {

        ActionInterval m_action;

        uint m_repeatCnt = 0;
        uint m_cnt = 0;

        float m_nextDt;
        float m_dtAmount;

        public Repeat()
        {

        }

        public void InitWithAction(ActionInterval action_, uint repeatCnt_)
        {
            m_action = action_;

            m_repeatCnt = repeatCnt_;
            float duration = m_action.Duration * m_repeatCnt;
            InitWithDuration(duration);

            m_dtAmount = m_action.Duration / m_duration;
            m_nextDt = m_dtAmount;
        }


        public override void StartWithTarget(GameObject target_)
        {
            base.StartWithTarget(target_);

            m_action.StartWithTarget(m_target);
            m_nextDt = m_dtAmount;
        
        }

        
        //进度更新
        protected override void OnProgress(float progress_)
        {

            if (progress_ >= m_nextDt)
            {

                while (progress_ > m_nextDt && m_cnt < m_repeatCnt)
                {

                    m_action.Progress(1.0f);
                    m_cnt++;


                    m_action.Reset();
                    m_action.StartWithTarget(m_target);

                    m_nextDt = m_dtAmount * (m_cnt + 1);
                }

                // fix for issue #1288, incorrect end value of repeat
                if (progress_ >= 1.0f && m_cnt < m_repeatCnt)
                {
                    m_cnt++;
                }

                if (m_cnt == m_repeatCnt)
                {
                    //最后一次
                    m_action.Progress(1.0f);
                    m_action.Reset();
                }
                else
                {
                    m_action.Progress(progress_ - (m_nextDt - m_dtAmount));

                }

            }
            else
            {
                float dt = (progress_ * m_repeatCnt) % 1.0f;
                m_action.Progress(dt);
            }

        
        }

        protected override void OnReset()
        {
            base.OnReset();

            m_action.Reset();
            m_cnt = 0;
        }


        public override bool isDone
        {
            get
            {
                return m_cnt == m_repeatCnt;
            }
        }


        protected override void OnClear()
        {
            base.OnClear();

            m_action.Clear();
            m_action = null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public Repeat Create(ActionInterval action_, uint repeatCnt_ = 1)
        {
            Repeat action = new Repeat();
            action.InitWithAction(action_, repeatCnt_);
            return action;
        }

    }


}