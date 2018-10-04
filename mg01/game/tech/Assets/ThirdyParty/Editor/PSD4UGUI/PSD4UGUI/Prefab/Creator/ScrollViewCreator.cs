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

    public class ScrollViewCreator : ContainerCreator
    {
        /// <summary>
        /// 滑动方向为水平
        /// </summary>
        public const string PARAM_H = "horizontal";
        public static Regex PATTERN_H = new Regex(PARAM_H);

        /// <summary>
        /// 滚动输入框
        /// </summary>
        public static Regex PATTERN_ADAPT_INPUT = new Regex("inputAdapt");

        public override string Identifier  {  get { return ComponentType.ScrollView;  }  }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = CreateGameObject(parent, data);
            CreateChildren(go, data);

            Restructure(go);

            ApplyGameObjectParam(go, data);


            return go;
        }
        
        void Restructure(GameObject go)
        {
            GameObject mask = FuzzySearchChild(go, "mask");
            RectTransform maskRect = mask.GetComponent<RectTransform>();

            GameObject content = FuzzySearchChild(go, "content");
            RectTransform rectTransform = content.GetComponent<RectTransform>();
            Vector3 pos = rectTransform.position;

            AttachToParent(content, mask);   //手动把content放到mask里面
            rectTransform.position = pos;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);

            if (HasParam(data, PATTERN_STATIC) == false)
            {

                KScrollView scrollView = go.AddComponent<KScrollView>();
                
                if (HasParam(data, PATTERN_H) == true)
                {
                    scrollView.direction = KScrollView.ScrollDir.horizontal;
                }
                else
                {
                    scrollView.direction = KScrollView.ScrollDir.vertical;
                }

                if (HasParam(data, PATTERN_ADAPT_INPUT))
                {
                    //AdaptInputField(go);
                }

                //scrollView.Initialize();

                GameObject mask = FuzzySearchChild(go, "mask");
                GameObject content = FuzzySearchChild(mask, "content");
                GameObject item = FuzzySearchChild(content, "item", true);
                if (item != null)
                {
                    //有列表项
                    KListViewScroll list = go.AddComponent<KListViewScroll>();
                }

            }

        }

        void AdaptInputField(GameObject go)
        {
            GameObject content = FuzzySearchChild(go, "content");
            var input = content.GetComponentInChildren<InputField>();
            if (input != null)
            {
                var scrollText = go.AddComponent<ScrollText>();
                scrollText.ScrollRectRef = go.GetComponent<ScrollRect>();
                scrollText.InputTextRef = input;
            }
        }
    }

}