/* ==============================================================================
 * MoveBy
 * @author jr.zeng
 * 2016/11/1 11:18:34
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class MoveBy : MoveTo
    {

        protected Vector3 m_diff = Vector3.one;

        public MoveBy()
        {

        }


        public override void InitWithPos(float duration_, UnityEngine.Vector3 to_)
        {
            base.InitWithPos(duration_, to_);

            m_diff = to_;
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


        new static public MoveBy Create(float duration_, Vector3 pos_)
        {
            MoveBy action = new MoveBy();
            action.m_use2d = false;
            action.InitWithPos(duration_, pos_);

            return action;
        }


        new static public MoveBy Create2(float duration_, Vector2 pos_)
        {
            MoveBy action = new MoveBy();
            action.m_use2d = true;
            action.InitWithPos(duration_, pos_);

            return action;
        }

    }


}