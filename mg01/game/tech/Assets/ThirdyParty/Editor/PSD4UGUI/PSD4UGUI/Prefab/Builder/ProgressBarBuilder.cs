using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ProgressBarBuilder : ContainerBuilder
    {

        public override string Identifier { get { return ComponentType.ProgressBar; } }

        protected override void AddComponent(GameObject go)
        {
            KProgressBar bar = go.AddComponent<KProgressBar>();
            bar.AddChildComponent<StateImage>("Image_bar");     //因为不会统一加了， 所以这里要单独加
        }
    }

}