/* ==============================================================================
 * ScaleBy
 * @author jr.zeng
 * 2016/11/1 11:20:05
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class ScaleBy : ScaleTo
    {


        protected Vector3 m_diff = Vector3.one;

        public ScaleBy()
        {

        }

        public override void InitWithScale(float duration_, UnityEngine.Vector3 scale_)
        {
            base.InitWithScale(duration_, scale_);

            m_diff = m_to;
        }


        public override void InitWithScale(float duration_, float scale_)
        {
            base.InitWithScale(duration_, scale_);

            m_diff = m_to;
        }

        protected override void OnStart()
        {
            m_from = value;

            m_to = m_from + m_diff;

            if (m_use2d)
            {
                m_to.z = m_from.z;
            }

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public ScaleBy Create(float duration_, Vector3 scale_)
        {
            ScaleBy action = new ScaleBy();
            action.InitWithScale(duration_, scale_);
            return action;
        }

        static public ScaleBy Create(float duration_, float scale_)
        {
            ScaleBy action = new ScaleBy();
            action.InitWithScale(duration_, scale_);
            return action;
        }

        static public ScaleBy Create2(float duration_, Vector2 scale_)
        {
            ScaleBy action = new ScaleBy();
            action.m_use2d = true;
            action.InitWithScale(duration_, scale_);
            return action;
        }

        static public ScaleBy Create2(float duration_, float scale_)
        {
            ScaleBy action = new ScaleBy();
            action.m_use2d = true;
            action.InitWithScale(duration_, scale_);
            return action;
        }

    }


}