using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using UnityEngine;
using Object = UnityEngine.Object;

using LitJson;
using mg.org.KUI;


namespace Edit.PSD4UGUI
{

    //TODO: ListCreator，ListBuilder 改为使用KListView

    public class ListCreator : ContainerCreator
    {
        
        /// <summary>
        /// 使用ListPageable(基本不用)
        /// </summary>
        public const string PARAM_PAGEABLE = "pageable";
        public static Regex PATTERN_PAGEABLE = new Regex(PARAM_PAGEABLE);

        /// <summary>
        /// 
        /// </summary>
        public const string PARAM_SHRINK = "shrink";
        public static Regex PATTERN_SHRINK = new Regex(PARAM_SHRINK);


        public override string Identifier { get { return ComponentType.List; } }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = base.Create(data, parent);
            HideItem(go);
            HideTemplate(go);
            return go;
        }

        private void HideItem(GameObject go)
        {
            HideChild(go, "item");
        }

        private void HideTemplate(GameObject go)
        {
            HideChild(go, "template");
        }

        private void HideChild(GameObject go, string name)
        {
            GameObject itemGo = FuzzySearchChild(go, name);
            if (itemGo != null)
            {
                itemGo.SetActive(false);
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);

            //都是延时创建
            //if (HasParam(data, PATTERN_PAGEABLE) == true)
            //{
            //    AddBuildHelper(go, PARAM_PAGEABLE);
            //}

            //if (HasParam(data, PATTERN_SHRINK) == true)
            //{
            //    AddBuildHelper(go, PARAM_SHRINK);
            //}
            
            KListView listView = go.AddComponent<KListView>();

        }

      
    }

}