using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{
    public class MaskBuilder : ContainerBuilder
    {

        public override string Identifier { get { return ComponentType.Image_mask; } }

        public override void Build(GameObject go, bool applyDeferred = true)
        {
            BuildChildren(go);
        }


    }

}