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
    public abstract class ComponentCreator
    {
      

        public const string TYPE = "type";
        public const string NAME = "name";
        public const string PARAM = "param";
        public const string STATE_NORMAL = "normal";
        public const string STATE_DISABLE = "disable";

        //参数例子 "param":"MiddleCenter|shrink|hide|pivot0005"

        /// <summary>
        /// 锚点参数
        /// "param":"MiddleCenter"
        /// </summary>
        public static Regex PATTERN_ANCHOR = new Regex(@"stretch|^(Upper|Middle|Lower)(Left|Center|Right)$|(anchor)(Upper|Middle|Lower|Left|Center|Right)", RegexOptions.IgnoreCase);
        public const string PARAM_UPPER = "upper";
        public const string PARAM_MIDDLE = "middle";
        public const string PARAM_LOWER = "lower";
        public const string PARAM_LEFT = "left";
        public const string PARAM_CENTER = "center";
        public const string PARAM_RIGHT = "right";
        public const string PARAM_STRETCH = "stretch";
        public const string PARAM_PREFIX = "anchor";


        /// <summary>
        /// 轴点参数 
        /// "param":"pivot0005"
        /// </summary>
        public static Regex PATTERN_PIVOT = new Regex(@"(pivot)([\d]{2})([\d]{2})", RegexOptions.IgnoreCase);

        /// <summary>
        /// 不参与业务逻辑的图片或文字，不挂接组件脚本
        /// </summary>
        public const string PARAM_STATIC = "static";
        public static Regex PATTERN_STATIC = new Regex(PARAM_STATIC);

        /// <summary>
        /// 生成时隐藏，运行时立即Build
        /// "param":"hide"
        /// </summary>
        public const string PARAM_HIDE = "hide";
        public static Regex PATTERN_HIDE = new Regex(PARAM_HIDE);
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ComponentCreator∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// creator类型
        /// </summary>
        public abstract string Identifier { get; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public abstract GameObject Create(JsonData data, GameObject parent);



        /// <summary>
        /// 应用作用在GameObject上的参数，如Hide，Deferred等
        /// </summary>
        /// <param name="go"></param>
        /// <param name="data"></param>
        public virtual void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            if (HasParam(data, PATTERN_ANCHOR) == true)
            {
                //锚点
                SetAnchor(go, data);
            }

            if (HasParam(data, PATTERN_PIVOT) == true)
            {
                //轴点
                SetPivot(go, data);
            }
        }

        /// <summary>
        /// 添加builder
        /// </summary>
        /// <param name="go"></param>
        /// <param name="param"></param>
        public void AddBuildHelper(GameObject go, string param)
        {
            BuildHelper helper = go.AddComponent<BuildHelper>();
            helper.param = param;
        }

        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽GameObject相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public GameObject CreateGameObject(GameObject parent, JsonData data)
        {
            string name = data["type"] + "_" + data["name"];
            GameObject go = new GameObject(name);
            go.layer = LayerMask.NameToLayer("UI");
            AttachToParent(go, parent);
            AddTopLeftRectTransform(go, data);
            return go;
        }

        public GameObject CreateGameObject(GameObject parent, string name, Vector2 position, Vector2 size)
        {
            return CreateGameObject(parent, name, position.x, position.y, size.x, size.y);
        }

        public GameObject CreateGameObject(GameObject parent, string name, float x, float y, float width, float height)
        {
            GameObject go = new GameObject(name);
            go.layer = LayerMask.NameToLayer("UI");
            AttachToParent(go, parent);
            AddTopLeftRectTransform(go, x, y, width, height);
            return go;
        }

        /// <summary>
        /// 创建状态Go
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="stateData"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public GameObject CreateStateGameObject(GameObject parent, JsonData stateData, string state)
        {
            GameObject go = new GameObject(state);
            go.layer = LayerMask.NameToLayer("UI");
            AttachToParent(go, parent);
            AddTopLeftRectTransform(go, stateData);
            return go;
        }

        public void AttachToParent(GameObject child, GameObject parent)
        {
            child.transform.SetParent(parent.transform);
            child.transform.localPosition = Vector3.zero;
        }

        public void AddTopLeftRectTransform(GameObject go, JsonData data)
        {
            //坐标系转为左上角
            float x = float.Parse(data["x"].ToString());
            float y = -1 * float.Parse(data["y"].ToString());
            float width = float.Parse(data["width"].ToString());
            float height = float.Parse(data["height"].ToString());
            AddTopLeftRectTransform(go, x, y, width, height);
        }

        public void AddTopLeftRectTransform(GameObject go, RectTransform rect)
        {
            float x = rect.anchoredPosition.x;
            float y = rect.anchoredPosition.y;
            float width = rect.sizeDelta.x;
            float height = rect.sizeDelta.y;
            AddTopLeftRectTransform(go, x, y, width, height);
        }

        private void AddTopLeftRectTransform(GameObject go, float x, float y, float width, float height)
        {
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0, 1);        //轴点默认左上
            rectTransform.anchorMin = new Vector2(0, 1);    //锚点都设置为左上
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.anchoredPosition = new Vector2(x, y);
        }


        /// <summary>
        /// 返回名称中包含特定字段（childName）的子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="childName"></param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns></returns>
        protected GameObject FuzzySearchChild(GameObject go, string childName, bool ignoreCase = false)
        {
            if (ignoreCase)
                childName = childName.ToLower();

            string name;

            for (int i = 0; i < go.transform.childCount; i++)
            {
                name = go.transform.GetChild(i).name;

                if (ignoreCase)
                    name = name.ToLower();

                if (name.Contains(childName) == true)
                {
                    return go.transform.GetChild(i).gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// 搜索指定名称结尾的子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected GameObject SearchChildEndWith(GameObject go, string childName)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).name.EndsWith(childName) == true)
                {
                    return go.transform.GetChild(i).gameObject;
                }
            }
            return null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Color相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public Color GetColor(string hexValue, int alpha)
        {
            int rgb = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            float r = (rgb >> 16) / 255.0f;
            float g = ((rgb >> 8) & 0x00FF) / 255.0f;
            float b = (rgb & 0x0000FF) / 255.0f;
            float a = alpha / 100.0f;
            Color result = new Color(r, g, b, a);
            return result;
        }

        public Color GetColor(JsonData data)
        {
            return GetColor((string)data["color"], (int)data["alpha"]);
        }

        public float GetAlpha(int alpha)
        {
            return (float)alpha / 100.0f;
        }

        public float GetAlpha(JsonData data)
        {
            return GetAlpha((int)data["alpha"]);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Param∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 提取参数中特定模式的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected bool HasParam(JsonData data, Regex pattern)
        {
            if (data.Keys.Contains("param") == false)
            {
                return false;
            }
            string allParam = (string)data["param"];
            string[] paramGroup = allParam.Split('|');
            bool ret = false;
            foreach (string p in paramGroup)
            {
                if (pattern.IsMatch(p))
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        protected string GetParam(JsonData data, Regex pattern)
        {
            if (data.Keys.Contains("param") == false)
            {
                return string.Empty;
            }
            string allParam = (string)data["param"];
            string[] paramGroup = allParam.Split('|');

            foreach (string p in paramGroup)
            {
                if (pattern.IsMatch(p))
                {
                    Match m = pattern.Match(p);
                    return m.Value;
                }
            }
            return string.Empty;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽State∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //只有普通状态
        protected bool HasOnlyNormalState(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (data[key].IsObject == true && key != STATE_NORMAL)
                {
                    return false;
                }
            }
            return true;
        }

        protected void ShowDefaultState(GameObject go)
        {
            if (go.transform.childCount == 0)
            {
                return;
            }

            if (go.transform.FindChild(STATE_NORMAL) != null)
            {
                go.transform.FindChild(STATE_NORMAL).gameObject.SetActive(true);
            }
            else
            {
                go.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Anchor∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void SetAnchor(GameObject go, JsonData stateData)
        {
            string param = GetParam(stateData, PATTERN_ANCHOR);
            param = param.ToLower();
            param = param.Replace(PARAM_PREFIX, "");

            RectTransform rectTransform = go.GetComponent<RectTransform>();
            Vector2 anchorMin = new Vector2();
            Vector2 anchorMax = new Vector2();

            if (param == PARAM_STRETCH)
            {
                anchorMin.Set(0, 0);
                anchorMax.Set(1, 1);
                if (go.transform.parent == null || go.transform.parent.name.Contains("Canvas"))
                {
                    Vector3 oldPos = rectTransform.position;
                    rectTransform.sizeDelta = Vector2.zero;
                    rectTransform.anchorMin = anchorMin;
                    rectTransform.anchorMax = anchorMax;
                    rectTransform.position = oldPos;
                    return;
                }
            }
            else
            {
                //左中右
                if (param.Contains(PARAM_LEFT))
                {
                    anchorMin.x = anchorMax.x = 0;
                }
                else if (param.Contains(PARAM_CENTER))
                {
                    anchorMin.x = anchorMax.x = 0.5f;
                }
                else if (param.Contains(PARAM_RIGHT))
                {
                    anchorMin.x = anchorMax.x = 1;
                }
                else
                {
                    anchorMin.x = 0;
                    anchorMax.x = 1;
                }

                //上中下
                if (param.Contains(PARAM_UPPER))
                {
                    anchorMin.y = anchorMax.y = 1;
                }
                else if (param.Contains(PARAM_MIDDLE))
                {
                    anchorMin.y = anchorMax.y = 0.5f;
                }
                else if (param.Contains(PARAM_LOWER))
                {
                    anchorMin.y = anchorMax.y = 0;
                }
                else
                {
                    anchorMin.y = 0;
                    anchorMax.y = 1;
                }
            }

            if (param.Contains(PARAM_CENTER) && param.Contains(PARAM_MIDDLE))
            {
                KuiUtil.SetPivotSmart(rectTransform, new Vector2(0.5f, 0.5f), true);
            }

            KuiUtil.SetAnchorSmart(rectTransform, anchorMin, anchorMax);

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Pivot∽-★-∽--------∽-★-∽------∽-★-∽--------//

        private void SetPivot(GameObject go, JsonData data)
        {
            var s = GetParam(data, PATTERN_PIVOT);
            Match m = PATTERN_PIVOT.Match(s);
            float pivotX = Int32.Parse(m.Groups[2].Value) / 10.0f;
            pivotX = Mathf.Clamp01(pivotX);
            float pivotY = Int32.Parse(m.Groups[3].Value) / 10.0f;
            pivotY = Mathf.Clamp01(pivotY);
            RectTransform rect = go.GetComponent<RectTransform>();
            KuiUtil.SetPivotSmart(rect, new Vector2(pivotX, pivotY), true);
        }


    }

}