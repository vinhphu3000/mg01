/* ==============================================================================
 * IProgress
 * @author jr.zeng
 * 2017/9/15 14:37:16
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public interface IProgress
    {

        bool isDone { get; }
        float progress { get; }



    }
    }