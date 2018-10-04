using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{
    public class ButtonBuilder : ContainerBuilder
    {
        //带缩放动画
        public const string PARAM_SHRINK = "shrink";
        //重置锚点
        public const string PARAM_PIVOT_RESET = "pivotReset";

        public override string Identifier { get { return ComponentType.Button; } }


        protected override void AddComponent(GameObject go)
        {
            //if (HasParam(PARAM_SHRINK) == true)
            //{
            //    go.AddComponent<KButtonShrinkable>();
            //}
            //else
            //{
            //    go.AddComponent<KButton>();
            //}

            //if (HasParam(PARAM_PIVOT_RESET))
            //{
            //    KuiUtil.SetPivotSmart(go.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), true);
            //}
        }


    }

}