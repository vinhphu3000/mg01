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
    public class ContainerCreator : ComponentCreator
    {
        /// <summary>
        /// 生成时隐藏子元素，运行时立即Build
        /// 注意hide和hideChildren的区别，当对容器应用hide的时候，当其SetActive(true)时，子元素会同时SetActive(true),反之hideChildren不会
        /// </summary>
        public const string PARAM_HIDE_CHILDREN = "hideChildren";
        public static Regex PATTERN_HIDE_CHILDREN = new Regex(PARAM_HIDE_CHILDREN);

        /// <summary>
        /// 生成时隐藏，运行时首次SetActive(true)时Build
        /// </summary>
        public const string PARAM_DEFERRED = "deferred";
        public static Regex PATTERN_DEFERRED = new Regex(PARAM_DEFERRED);

        /// <summary>
        ///将所有子元素居中对齐。会改变子元素的anchor和pivot参数
        /// </summary>
        public const string PARAM_CENTER_CHILDREN = "centerChildren";
        public static Regex PATTERN_CENTER = new Regex(PARAM_CENTER_CHILDREN);

        /// <summary>
        /// Canvas组, 一般用于统一透明度
        /// </summary>
        public const string PARAM_CANVAS_GROUP = "canvasGroup";
        public static Regex PATTERN_CANVAS_GROUP = new Regex(PARAM_CANVAS_GROUP, RegexOptions.IgnoreCase);

        /// <summary>
        /// Canvas
        /// </summary>
        public const string PARAM_CANVAS = "subCanvas";
        public static Regex PATTERN_CANVAS = new Regex(PARAM_CANVAS, RegexOptions.IgnoreCase);

        /// <summary>
        /// graphicRaycast
        /// </summary>
        public const string PARAM_GRAPHIC_RAYCAST = "graphicRaycast";
        public static Regex PATTERN_GRAPHIC_RAYCAST = new Regex(PARAM_GRAPHIC_RAYCAST, RegexOptions.IgnoreCase);


        //-------∽-★-∽LayoutGroup参数∽-★-∽--------//
        /// <summary>
        /// 
        /// </summary>
        public const string PARAM_LAYOUT_GROUP = @"(v|h)Layout(Upper|Middle|Lower)?(Left|Center|Right)?";
        public static Regex PATTERN_LAYOUT_GROUP = new Regex(PARAM_LAYOUT_GROUP, RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public const string PARAM_LAYOUT_ELE = @"Le(Ignore)";
        public static Regex PATTERN_LAYOUT_ELE = new Regex(PARAM_LAYOUT_ELE, RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public const string PARAM_SIZE_FITTER = "sizeFitter";
        public static Regex PATTERN_SIZE_FITTER = new Regex(PARAM_SIZE_FITTER, RegexOptions.IgnoreCase);


        static readonly Dictionary<string, Dictionary<string, TextAnchor>> AnchorMap = new Dictionary<string, Dictionary<string, TextAnchor>> {
            {"upper", new Dictionary<string, TextAnchor>
                {
                    { "left", TextAnchor.UpperLeft } ,
                    { "center", TextAnchor.UpperCenter } ,
                    { "right", TextAnchor.UpperLeft }
                }
            },
            {"middle", new Dictionary<string, TextAnchor>
                {
                    { "left", TextAnchor.MiddleLeft } ,
                    { "center", TextAnchor.MiddleCenter } ,
                    { "right", TextAnchor.MiddleLeft }
                }
            },
            {"lower", new Dictionary<string, TextAnchor>
                {
                    { "left", TextAnchor.LowerLeft } ,
                    { "center", TextAnchor.LowerCenter } ,
                    { "right", TextAnchor.LowerLeft }
                }
            },
        };

        public override string Identifier { get { return ComponentType.Container; }  }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = CreateGameObject(parent, data);
            CreateChildren(go, data);
            ApplyGameObjectParam(go, data);
            return go;
        }


        protected virtual void CreateChildren(GameObject go, JsonData data)
        {
            JsonData childrenData = data["children"];
            for (int i = childrenData.Count - 1; i >= 0; i--)
            {
                CreateChild(go, childrenData[i]);
            }
        }


        protected GameObject CreateChild(GameObject parent, JsonData data)
        {
            string type = (string)data["type"];
            string name = (string)data["name"];
            ComponentCreator creator = ComponentCreatorFactory.GetCustomCreator(type, name);    //先从自定义里取
            if (creator == null)
                creator = ComponentCreatorFactory.GetTypeCreator(type);
            return creator.Create(data, parent);    //在这里递归调用creator
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);

            if (HasParam(data, PATTERN_HIDE) == true)
            {
                //隐藏自己
                go.SetActive(false);
            }

            if (HasParam(data, PATTERN_HIDE_CHILDREN) == true)
            {
                //隐藏孩子
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    go.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            if (HasParam(data, PATTERN_DEFERRED) == true)
            {
                //延后实例化
                go.SetActive(false);
                //AddBuildHelper(go, PARAM_DEFERRED);
            }

            if (HasParam(data, PATTERN_CENTER) == true)
            {
                //把孩子全部居中
                CenterChildrenTransform(go);
            }

            if (HasParam(data, PATTERN_CANVAS_GROUP) == true)
            {
                //添加CanvasGroup
                go.AddComponent<CanvasGroup>();
            }

            if (HasParam(data, PATTERN_CANVAS) == true)
            {
                //添加Canvas
                go.AddComponent<Canvas>();
                if (HasParam(data, PATTERN_GRAPHIC_RAYCAST) == true)
                {
                    go.AddComponent<GraphicRaycaster>();    //有这个才接收鼠标事件
                }
            }


            if (HasParam(data, PATTERN_LAYOUT_ELE) == true) //自动布局的原子
            {
                LayoutElement le = go.AddComponent<LayoutElement>();
                string param = GetParam(data, PATTERN_LAYOUT_ELE);
                Match m = PATTERN_LAYOUT_ELE.Match(param);
                string styleSetting = m.Groups[1].Value;
                if (styleSetting.ToLower() == "ignore") //忽略布局
                {
                    le.ignoreLayout = true;
                }
            }
            if (HasParam(data, PATTERN_LAYOUT_GROUP) == true)    //自动布局
            {
                string param = GetParam(data, PATTERN_LAYOUT_GROUP);
                Match m = PATTERN_LAYOUT_GROUP.Match(param);
                string type = m.Groups[1].Value;
                string verticalParam = m.Groups[2].Value;
                string horizontalParam = m.Groups[3].Value;
                HorizontalOrVerticalLayoutGroup groupCom = null;
                bool isVertical = false;
                if (type.Contains("v"))
                {
                    groupCom = go.AddComponent<VerticalLayoutGroup>();
                    isVertical = true;
                }
                else if (type.Contains("h"))
                {
                    groupCom = go.AddComponent<HorizontalLayoutGroup>();
                }
                if (groupCom != null)
                {
                    groupCom.childForceExpandHeight = false;
                    groupCom.childForceExpandWidth = false;
                    GameObject layoutHintBase = SearchChildEndWith(go, "layoutHint");
                    GameObject layoutHintNext = SearchChildEndWith(go, "layoutHintNext");
                    if (layoutHintBase != null)
                    {
                        Rect rtBase = layoutHintBase.GetComponent<RectTransform>().rect;
                        Vector2 posBase = layoutHintBase.GetComponent<RectTransform>().anchoredPosition;
                        Rect rtNext = layoutHintNext.GetComponent<RectTransform>().rect;
                        Vector2 posNext = layoutHintNext.GetComponent<RectTransform>().anchoredPosition;
                        float space = 0.0f;
                        if (isVertical)
                        {
                            space = posBase.y - rtBase.height - posNext.y;
                        }
                        else
                        {
                            space = posNext.x - (posBase.x + rtBase.width);
                        }
                        groupCom.spacing = space;
                        GameObject.DestroyImmediate(layoutHintBase);
                        GameObject.DestroyImmediate(layoutHintNext);
                    }
                    if (!string.IsNullOrEmpty(verticalParam) && !string.IsNullOrEmpty(horizontalParam))
                    {
                        verticalParam = verticalParam.ToLower();
                        horizontalParam = horizontalParam.ToLower();
                        try
                        {
                            TextAnchor ac = AnchorMap[verticalParam][horizontalParam];
                            groupCom.childAlignment = ac;
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                    }
                    if (HasParam(data, PATTERN_SIZE_FITTER) == true)
                    {
                        ContentSizeFitter fitter = go.AddComponent<ContentSizeFitter>();
                        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    }
                }
            }

        }

        //居中孩子
        private void CenterChildrenTransform(GameObject go)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                RectTransform rectTransform = go.transform.GetChild(i).GetComponent<RectTransform>();
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            }
        }



    }

}