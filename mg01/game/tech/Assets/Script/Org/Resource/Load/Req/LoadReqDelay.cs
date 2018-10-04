/* ==============================================================================
 * LoadReqDelay
 * @author jr.zeng
 * 2017/6/9 10:32:18
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    
    public class LoadReqDelay :LoadReq
    {

        public float delay = 1;
        
        public LoadReqDelay(float delay_) 
        {

            type = LoadReqType.DELAY;

            delay = delay_;
            path = GetType().Name + ": " + delay_;  //纯粹用来显示
        }


    }

}
