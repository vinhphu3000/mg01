/* ==============================================================================
 * 动作_RotateTo
 * @author jr.zeng
 * 2016/10/27 12:04:35
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class RotateTo : ActionIntervalTrans
    {
        public bool quaternionLerp = false;

        protected Vector3 m_from;
        protected Vector3 m_to;

        public RotateTo()
        {

        }


        virtual public void InitWithRotation(float duration_, Vector3 to_)
        {
            InitWithDuration(duration_);

            m_to = to_;
        }

        
        protected override void OnStart()
        {

            m_from = value.eulerAngles;

            if (m_use2d)
            {
                m_to.x = m_from.x;
                m_to.y = m_from.y;
            }

        }

        protected override void OnProgress(float progress_)
        {

            if (quaternionLerp)
            {
                value = Quaternion.Slerp(Quaternion.Euler(m_from), Quaternion.Euler(m_to), progress_);

            }
            else
            {
                value = Quaternion.Euler(new Vector3(
                    Mathf.Lerp(m_from.x, m_to.x, progress_),
                    Mathf.Lerp(m_from.y, m_to.y, progress_),
                    Mathf.Lerp(m_from.z, m_to.z, progress_)) );
            }
            
        }


        public Quaternion value 
        { 
            get { return m_transform.localRotation; } 
            set 
            {
                m_transform.localRotation = value; 
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public RotateTo Create(float duration_, Vector3 to_)
        {
            RotateTo action = new RotateTo();
            action.InitWithRotation(duration_, to_);

            action.m_use2d = false;

            return action;
        }


        static public RotateTo Create2(float duration_, float angle_)
        {
            RotateTo action = new RotateTo();
            action.InitWithRotation(duration_, new Vector3(0, 0, angle_) );

            action.m_use2d = true;

            return action;
        }
        


    }

}