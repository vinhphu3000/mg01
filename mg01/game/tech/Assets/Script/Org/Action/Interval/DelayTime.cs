/* ==============================================================================
 * 延时动作
 * @author jr.zeng
 * 2016/10/28 10:32:31
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class DelayTime : ActionInterval
    {

        static public DelayTime Create(float duration_)
        {
            DelayTime action = new DelayTime();
            action.InitWithDuration(duration_);
            return action;
        }

        public DelayTime()
        {

        }



    }


}