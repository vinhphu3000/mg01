/* ==============================================================================
 * ButtonCreator
 * @author jr.zeng
 * 2017/7/31 18:57:36
 * ==============================================================================*/

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

    public class ButtonCreator : ContainerCreator
    {
        /// <summary>
        /// shrink按钮
        /// "param":"shrink|pivot0005"
        /// </summary>
        public const string PARAM_SHRINK = "shrink";
        public static Regex PATTERN_SHRINK = new Regex(PARAM_SHRINK);

        /// <summary>
        /// 重置轴点
        /// </summary>
        public const string PARAM_PIVOT_RESET = "pivotReset";
        

        public override string Identifier  { get { return ComponentType.Button; }  }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = base.Create(data, parent);
            JsonData childrenData = data["children"];
            for (int i = 0; i < childrenData.Count; i++)
            {
                JsonData childData = childrenData[i];
                GameObject child = go.transform.GetChild(childrenData.Count - 1 - i).gameObject;    //从末尾开始
                string type = (string)childData["type"];
                if (type == "Label")
                {
                    //按钮标签文本的对齐方式为居中对齐
                    ChangeLabelAlignment(child, childData);
                }
            }
            return go;
        }


        private void ChangeLabelAlignment(GameObject go, JsonData data)
        {
            
            if (go.transform.childCount == 0)
            {
                //只有一态的情况
                ChangeLabelStateAlignment(go, data["normal"]);
            }
            else
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject stateGo = go.transform.GetChild(i).gameObject;
                    ChangeLabelStateAlignment(stateGo, data[stateGo.name]);
                }
            }
        }

        private void ChangeLabelStateAlignment(GameObject stateGo, JsonData stateData)
        {
            Text t = stateGo.GetComponent<Text>();
            if (stateData.Keys.Contains("alignment") == false)  
            {
                //不有对齐方式,居中对齐
                t.alignment = TextAnchor.MiddleCenter;
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);
            

            if (HasParam(data, PATTERN_SHRINK) == true)
            {
                //SHRINK按钮
                go.AddComponent<KButtonShrinkable>();
            }
            else
            {
                go.AddComponent<KButton>();
            }


            bool shouldResetPivot = true;   //需要重置轴点

            if (HasParam(data, PATTERN_ANCHOR))
            {
                //有锚点参数
                string param = GetParam(data, PATTERN_ANCHOR);
                if (param.Contains(PARAM_CENTER) && param.Contains(PARAM_MIDDLE))
                {
                    //居中
                    shouldResetPivot = false;
                }
            }

            if (shouldResetPivot && !HasParam(data, PATTERN_PIVOT))
            {
                //没有轴点参数
                KuiUtil.SetPivotSmart(go.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), true);
            }


        }

       

    

    }

}