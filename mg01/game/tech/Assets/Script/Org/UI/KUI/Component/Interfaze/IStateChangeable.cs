/* ==============================================================================
 * IStateChangeable
 * @author jr.zeng
 * 2017/7/8 18:25:01
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{

    interface IStateChangeable
    {
        string State { get; set; }

    }

}