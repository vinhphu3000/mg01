using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ScrollViewBuilder : ContainerBuilder
    {

        public const string PARAM_STATIC = "static";
        public const string PARAM_H = "horizontal";

        public override string Identifier { get { return ComponentType.ScrollView; } }


        protected override void AddComponent(GameObject go)
        {
            if (HasParam(PARAM_STATIC) == false)    //为啥?
            {
                KScrollView scrollView = go.AddComponent<KScrollView>();
                if (HasParam(PARAM_H) == true)
                {
                    scrollView.direction = KScrollView.ScrollDir.horizontal;
                }
                else
                {
                    scrollView.direction = KScrollView.ScrollDir.vertical;
                }
                scrollView.Initialize();
            }
        }


    }
}