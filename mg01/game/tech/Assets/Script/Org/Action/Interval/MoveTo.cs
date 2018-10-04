/* ==============================================================================
 * 动作_MoveTo
 * @author jr.zeng
 * 2016/10/26 15:37:24
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class MoveTo : ActionIntervalTrans
    {

        //protected bool m_isWorldSpace = false; //是否世界坐标

        protected Vector3 m_from;
        protected Vector3 m_to;

        public MoveTo()
        {

        }

        virtual public void InitWithPos(float duration_, Vector3 to_)
        {
            InitWithDuration(duration_);

            m_to = to_;
            m_use2d = false;
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
            get
            {
                //return m_isWorldSpace ? m_transform.position : m_transform.localPosition;
                return m_transform.localPosition;
            }
            set
            {
                m_transform.localPosition = value;
                //if (m_isWorldSpace)
                //{
                //    m_transform.position = value;
                //}
                //else
                //{
                //    m_transform.localPosition = value;
                //}
            }
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public MoveTo Create(float duration_, Vector3 to_)
        {
            MoveTo action = new MoveTo();
            action.InitWithPos(duration_, to_);
            
            action.m_use2d = false;

            return action;
        }


        static public MoveTo Create2(float duration_, Vector2 to_)
        {
            MoveTo action = new MoveTo();
            action.InitWithPos(duration_, to_);
            
            action.m_use2d = true;

            return action;
        }



    }



    //此动作只是为了提取出Transform
    public class ActionIntervalTrans : ActionInterval
    {

        protected bool m_use2d = false; //使用2d坐标

        protected Transform m_transform;

        public ActionIntervalTrans()
        {

        }


        public override void StartWithTarget(GameObject target_)
        {
            base.StartWithTarget(target_);

            m_transform = target_.transform;

            OnStart();

        }

        protected virtual void OnStart()
        {
           
        }

        protected override void OnClear()
        {
            base.OnClear();

            m_transform = null;
        }


    }

}