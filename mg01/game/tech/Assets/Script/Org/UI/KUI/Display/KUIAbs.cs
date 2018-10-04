/* ==============================================================================
 * KUIAbs
 * @author jr.zeng
 * 2017/6/21 16:08:31
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KUIAbs : ImageAbs2D
    {
        public KUIAbs()
        {

        }


        protected override void __ShowGameObject()
        {
            base.__ShowGameObject();

            m_gameObject.layer = CAMERA_LAYER.UI;   //默认ui层

        }



    }
}
