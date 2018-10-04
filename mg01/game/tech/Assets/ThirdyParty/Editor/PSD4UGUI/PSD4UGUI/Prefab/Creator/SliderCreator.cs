using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using LitJson;

using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class SliderCreator : ContainerCreator
    {

        /// <summary>
        /// 滑动方向
        /// </summary>
        public static Regex PATTERN_DIRECTION = new Regex(@"(left2Right)|(right2Left)|(top2Bottom)|(bottom2Top)");

        public const string PARAM_RIGHT2LEFT = "right2Left";
        public const string PARAM_TOP2BOTTOM = "top2Bottom";
        public const string PARAM_BOTTOM2TOP = "bottom2Top";

        Vector3 HorizontalVec3 = new Vector3();
        Vector3 VerticalVec3 = new Vector3();

        GameObject m_handleGo;

        public override string Identifier  {  get { return ComponentType.Slider;  }  }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = CreateGameObject(parent, data);
            CreateChildren(go, data); 

            Restructure(go);

            // 判断方向
            GameObject handle = FuzzySearchChild(go, "handle");
            if (handle != null)
            {
                var dir = "left2Right";
                GameObject handleArea = FuzzySearchChild(go, "Container_handleArea");
                GameObject handleHolder = FuzzySearchChild(handleArea, "Container_handleHolder");
                if (HasParam(data, PATTERN_DIRECTION) == true)
                {
                    dir = GetParam(data, PATTERN_DIRECTION);
                }

                if (dir == "bottom2Top" || dir == "top2Bottom")
                {
                    //handleHolder.transform.localPosition = new Vector3(VerticalVec3.x, VerticalVec3.y, VerticalVec3.z);

                    handleHolder.transform.localPosition = new Vector3(VerticalVec3.x, 0, VerticalVec3.z);
                    if(m_handleGo)//要对handle居中,而不是Container_handleArea zjr_20170830
                        m_handleGo.transform.localPosition = new Vector3(m_handleGo.transform.localPosition.x, VerticalVec3.y, m_handleGo.transform.localPosition.z);
                }
                else
                {
                    //handleHolder.transform.localPosition = new Vector3(HorizontalVec3.x, HorizontalVec3.y, HorizontalVec3.z);

                    handleHolder.transform.localPosition = new Vector3(0, HorizontalVec3.y, HorizontalVec3.z);
                    if (m_handleGo)//要对handle居中,而不是Container_handleArea zjr_20170830
                        m_handleGo.transform.localPosition = new Vector3(HorizontalVec3.x, m_handleGo.transform.localPosition.y, m_handleGo.transform.localPosition.z);
                }
            }

            ApplyGameObjectParam(go, data);

            return go;
        }

        void Restructure(GameObject go)
        {
            GameObject fill = FuzzySearchChild(go, "fill");
            RectTransform fillRect = fill.GetComponent<RectTransform>();
            int depth = fillRect.GetSiblingIndex();
            GameObject fillArea = CreateGameObject(go, "Container_fillArea", fillRect.anchoredPosition, fillRect.sizeDelta);
            fillArea.transform.SetSiblingIndex(depth);  //深度设为fill的原深度
            GameObject fillHolder = CreateGameObject(fillArea, "Container_fillHolder", Vector2.zero, fillRect.sizeDelta);
            AttachToParent(fill, fillHolder);

            GameObject handle = FuzzySearchChild(go, "handle");
            if (handle != null)
            {
                m_handleGo = handle;

                RectTransform handleRect = handle.GetComponent<RectTransform>();
                depth = handleRect.GetSiblingIndex();
                RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
                GameObject handleArea = CreateGameObject(go, "Container_handleArea", fillAreaRect.anchoredPosition, fillAreaRect.sizeDelta);
                RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
                handleAreaRect.SetSiblingIndex(depth);  //深度设为handle的原深度
                GameObject handleHolder = CreateGameObject(handleArea, "Container_handleHolder", handleRect.anchoredPosition, handleRect.sizeDelta);
                AttachToParent(handle, handleHolder);
                Vector3 handleAreaPosition = handleAreaRect.localPosition;


                // 方向的计算 其实算的是中间位置的坐标
                HorizontalVec3.x = -handleRect.sizeDelta.x * 0.5f;
                HorizontalVec3.y = (handleRect.sizeDelta.y - handleAreaRect.sizeDelta.y) * 0.5f;
                HorizontalVec3.z = handleAreaPosition.z;

                VerticalVec3.x = (handleAreaRect.sizeDelta.x - handleRect.sizeDelta.x) * 0.5f;
                VerticalVec3.y = handleRect.sizeDelta.y * 0.5f;
                VerticalVec3.z = handleAreaPosition.z;
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);

            KSlider slider = go.AddComponent<KSlider>();
            ComponentUtil.AddChildComponent<StateImage>(go, "Container_fillArea/Container_fillHolder/Image_fill");
            //slider.AddChildComponent<StateImage>("Container_fillArea/Container_fillHolder/Image_fill");
            
            if (HasParam(data, PATTERN_DIRECTION) == true)
            {
                string param = GetParam(data, PATTERN_DIRECTION);
                if (param == PARAM_RIGHT2LEFT)
                {
                    slider.direction = Slider.Direction.RightToLeft;
                }
                else if (param == PARAM_TOP2BOTTOM )
                {
                    slider.direction = Slider.Direction.TopToBottom;
                }
                else if (param == PARAM_BOTTOM2TOP )
                {
                    slider.direction = Slider.Direction.BottomToTop;
                }
            }
        }


    }
}

