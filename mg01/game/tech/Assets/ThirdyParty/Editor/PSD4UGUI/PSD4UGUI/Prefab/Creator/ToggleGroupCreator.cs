using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using LitJson;

using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{

    public class ToggleGroupCreator : ContainerCreator
    {


        public override string Identifier { get { return ComponentType.ToggleGroup; } }

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = base.Create(data, parent);
            HideCheckmark(go);
            return go;
        }        private void HideCheckmark(GameObject go)
        {
            //第一个Toggle.isOn为true
            for (int i = 1; i < go.transform.childCount; i++)   //从第二个开始
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                GameObject checkmark = FuzzySearchChild(child, "checkmark");
                if (checkmark != null)
                {
                    checkmark.SetActive(false);
                }
            }
        }


        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {

            go.AddComponent<KToggleGroup>();
        }


    }

}