using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class SliderBuilder : ContainerBuilder
    {
        public const string PARAM_RIGHT2LEFT = "right2Left";
        public const string PARAM_TOP2BOTTOM = "top2Bottom";
        public const string PARAM_BOTTOM2TOP = "bottom2Top";

        public override string Identifier { get { return ComponentType.Slider; } }

        protected override void AddComponent(GameObject go)
        {
            KSlider slider = go.AddComponent<KSlider>();
            ComponentUtil.AddChildComponent<StateImage>(go, "Container_fillArea/Container_fillHolder/Image_fill");
            //slider.AddChildComponent<StateImage>("Container_fillArea/Container_fillHolder/Image_fill");
            if (HasParam(PARAM_RIGHT2LEFT) == true)
            {
                slider.direction = Slider.Direction.RightToLeft;
            }
            else if (HasParam(PARAM_TOP2BOTTOM) == true)
            {
                slider.direction = Slider.Direction.TopToBottom;
            }
            else if (HasParam(PARAM_BOTTOM2TOP) == true)
            {
                slider.direction = Slider.Direction.BottomToTop;
            }
        }

    }

}