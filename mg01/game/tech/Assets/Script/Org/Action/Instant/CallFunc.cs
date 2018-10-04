/* ==============================================================================
 * 回调动作
 * @author jr.zeng
 * 2016/10/28 11:30:25
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{


    public class CallFunc : ActionInstant
    {

        CALLBACK_GO m_func = null;

        bool m_called = false;

        public CallFunc()
        {

        }

        public void InitWithFunc(CALLBACK_GO func_)
        {
            Init();

            m_func = func_;
        }

        protected override void OnReset()
        {
            base.OnReset();
            m_called = false;
        }


        protected override void OnDone()
        {
            base.OnDone();

            if (!m_called)
            {
                m_called = true;

                if (m_func != null)
                {
                    m_func(m_target);
                }
            }
        }


        protected override void OnClear()
        {
            base.OnClear();

            m_func = null;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public CallFunc Create(CALLBACK_GO func_)
        {
            CallFunc action = new CallFunc();
            action.InitWithFunc(func_);
            return action;
        }
    }

}