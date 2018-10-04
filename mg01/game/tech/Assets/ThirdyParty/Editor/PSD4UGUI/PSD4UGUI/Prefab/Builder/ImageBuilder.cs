/* ==============================================================================
 * ImageBuilder
 * @author jr.zeng
 * 2017/8/1 17:16:05
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ImageBuilder : ComponentBuilder
    {
        public const string PARAM_STATIC = "static";
        public const string NO_TEX = "noTex";


        public override string Identifier { get { return ComponentType.Image; } }

        public override void Build(GameObject go, bool applyDeferred = true)
        {
            RefreshParamList(go);

            if (HasParam(PARAM_STATIC) == false && HasParam(NO_TEX) == false)
            {
                //除了静态，全都加StateImage..
                //go.AddComponent<StateImage>();    //不需要全部加StateImage了
            }
        }

    }


}