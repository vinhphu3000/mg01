/* ==============================================================================
 * LoadReqLevel
 * @author jr.zeng
 * 2017/6/9 10:10:50
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    

    public class LoadReqLevel : LoadReq
    {
        public bool isAdditive = false;

        public LoadReqLevel()
        {
            type = LoadReqType.LEVEL;

        }

        public LoadReqLevel(string url_, bool isAdditive_) : this()
        {
            path = url_;
            isAdditive = isAdditive_;
        }
    }

}
