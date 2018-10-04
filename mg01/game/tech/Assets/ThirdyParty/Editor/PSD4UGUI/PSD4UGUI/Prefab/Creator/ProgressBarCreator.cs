using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using LitJson;

using UnityEngine;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ProgressBarCreator : ContainerCreator
    {
        /// <summary>
        /// 从右向左
        /// </summary>
        public const string PARAM_RIGHT2LEFT = "right2Left";
        public static Regex PATTERN_RIGHT2LEFT = new Regex(PARAM_RIGHT2LEFT);

        public override string Identifier {  get {  return ComponentType.ProgressBar;  }  }

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);


            KProgressBar bar = go.AddComponent<KProgressBar>();
            bar.AddChildComponent<StateImage>("Image_bar");     //因为不会统一加了， 所以这里要单独加

            //if (HasParam(data, PATTERN_RIGHT2LEFT) == true)
            //{
            //    AddBuildHelper(go, PARAM_RIGHT2LEFT);
            //}
        }



    }

}