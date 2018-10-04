/* ==============================================================================
 * 动作_ScaleTo
 * @author jr.zeng
 * 2016/10/27 11:12:01
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class ScaleTo : ActionIntervalTrans
    {

        protected Vector3 m_from = Vector3.one;
        protected Vector3 m_to = Vector3.one;


        public ScaleTo()
        {

        }

        virtual public void InitWithScale(float duration_, Vector3 scale_)
        {
            InitWithDuration(duration_);

            m_to = scale_;
        }

        virtual public void InitWithScale(float duration_, float scale_)
        {
            InitWithDuration(duration_);

            m_to.Set(scale_, scale_, scale_);
        }


        protected override void OnStart()
        {

            m_from = value;

            if (m_use2d)
            {
                m_to.z = m_from.z;
            }
            
        }


        protected override void OnProgress(float progress_)
        {

            value = m_from * (1f - progress_) + m_to * progress_;

        }


        public Vector3 value 
        {
            get { return m_transform.localScale; } 
            set 
            {
                m_transform.localScale = value;
            }
        }




        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public ScaleTo Create(float duration_, Vector3 scale_)
        {
            ScaleTo action = new ScaleTo();
            action.InitWithScale(duration_, scale_);
            return action;
        }

        static public ScaleTo Create(float duration_, float scale_)
        {
            ScaleTo action = new ScaleTo();
            action.InitWithScale(duration_, scale_);
            return action;
        }

        static public ScaleTo Create2(float duration_, Vector2 scale_)
        {
            ScaleTo action = new ScaleTo();
            action.m_use2d = true;
            action.InitWithScale(duration_, scale_);

            return action;
        }

        static public ScaleTo Create2(float duration_, float scale_)
        {
            ScaleTo action = new ScaleTo();
            action.m_use2d = true;
            action.InitWithScale(duration_, scale_);

            return action;
        }


    }


}