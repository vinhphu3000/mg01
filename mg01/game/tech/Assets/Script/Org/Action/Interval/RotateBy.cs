/* ==============================================================================
 * RotateBy
 * @author jr.zeng
 * 2016/11/1 11:19:54
 * ==============================================================================*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class RotateBy : RotateTo
    {
        protected Vector3 m_diff = Vector3.one;

        public RotateBy()
        {

        }


        public override void InitWithRotation(float duration_, Vector3 to_)
        {
            base.InitWithRotation(duration_, to_);

            m_diff = to_;
        }


        protected override void OnStart()
        {
            m_from = value.eulerAngles;

            m_to = m_from + m_diff;

            if (m_use2d)
            {
                m_to.x = m_from.x;
                m_to.y = m_from.y;
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public RotateBy Create(float duration_, Vector3 to_)
        {
            RotateBy action = new RotateBy();
            action.m_use2d = false;
            action.InitWithRotation(duration_, to_);

            return action;
        }


        static public RotateBy Create2(float duration_, float angle_)
        {
            RotateBy action = new RotateBy();
            action.m_use2d = true;
            action.InitWithRotation(duration_, new Vector3(0, 0, angle_));

            return action;
        }
        
    }

}