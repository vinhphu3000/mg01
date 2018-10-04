/* ==============================================================================
 * IRaycastable
 * @author jr.zeng
 * 2017/7/8 18:24:12
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    interface IRaycastable : ICanvasRaycastFilter
    {

        /// <summary>
        /// 是否可以参加输入交互
        /// </summary>
        bool Raycast { get; set; }
    }


}