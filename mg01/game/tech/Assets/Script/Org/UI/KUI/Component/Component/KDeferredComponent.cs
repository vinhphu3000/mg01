/* ==============================================================================
 * 不懂
 * @author jr.zeng
 * 2017/8/1 16:52:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{
    public class KDeferredComponent : MonoBehaviour
    {
        protected void Awake()
        {
            Build();
        }

        private void Build()
        {
            //ComponentBuilder builder = ComponentBuilders.GetBuilder(gameObject.name);
            //builder.Build(gameObject, false);
        }

    }
}