using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class LabelBuilder : ComponentBuilder
    {
        public const string PARAM_STATIC = "static";

        
        public override string Identifier { get { return ComponentType.Label; } }
        
        public override void Build(GameObject go, bool applyDeferred = true)
        {
            RefreshParamList(go);
            if (HasParam(PARAM_STATIC) == false)
            {
                //go.AddComponent<StateText>(); //不需要全部加StateImage
            }
        }
        

    }


}
