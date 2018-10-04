/* ==============================================================================
 * LoadReqProgress
 * @author jr.zeng
 * 2017/12/7 9:43:44
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class LoadReqProg : LoadReq
    {

        public IProgress iProg;
        public Action<IProgress> onStartLoad;

        public LoadReqProg(IProgress progData_, Action<IProgress> onStartLoad_=null)
        {
            type = LoadReqType.PROGRESS;

            path = GetType().Name;  //纯粹用来显示

            iProg = progData_;
            onStartLoad = onStartLoad_;
        }

       

    }
}