/* ==============================================================================
 * 进度数据
 * @author jr.zeng
 * 2017/12/6 17:53:08
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class ProgData : IProgress
    {
        

        public virtual bool isDone { get; set; }
        public virtual float progress { get; set; }


    }

}