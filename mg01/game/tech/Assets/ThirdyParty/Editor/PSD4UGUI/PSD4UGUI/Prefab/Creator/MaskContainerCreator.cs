using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using LitJson;

using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;



namespace Edit.PSD4UGUI
{

    public class MaskContainerCreator : ContainerCreator
    {

        public override string Identifier {  get  { return ComponentType.MaskContainer;  }  }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = base.Create(data, parent);
            Restructure(go);
            return go;
        }


        private void Restructure(GameObject go)
        {
            GameObject mask = FuzzySearchChild(go, "mask");
            RectTransform maskRect = mask.GetComponent<RectTransform>();
            GameObject content = FuzzySearchChild(go, "content");
            RectTransform rectTransform = content.GetComponent<RectTransform>();
            Vector3 pos = rectTransform.position;
            AttachToParent(content, mask);  //手动把content放到mask里面
            rectTransform.position = pos;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);
        }


    }

}