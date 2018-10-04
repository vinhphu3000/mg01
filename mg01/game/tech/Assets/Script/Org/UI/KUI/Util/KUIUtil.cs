/* ==============================================================================
 * KUIUtil
 * @author jr.zeng
 * 2017/7/31 14:46:56
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{

    public class KuiUtil
    {


        /// <summary>
        /// 获取中心边距
        /// </summary>
        /// <returns></returns>
        public static Padding CalcPivotPadding(RectTransform rectTrans)
        {
            Vector2 pivot = rectTrans.pivot;
            Vector2 size = rectTrans.sizeDelta;
            return CalcPivotPadding(pivot, size);
        }

        public static Padding CalcPivotPadding(Vector2 pivot_, Vector2 size_)
        {
            Padding result = new Padding();
            result.left = pivot_.x * size_.x;
            result.right = (1 - pivot_.x) * size_.x;
            result.top = (1 - pivot_.y) * size_.y;
            result.bottom = pivot_.y * size_.y;
            return result;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽PSD4UGUI∽-★-∽--------∽-★-∽------∽-★-∽--------//

        private static Vector3[] s_Corners = new Vector3[4];


        static float Round(float value)
        {
            return Mathf.Floor(0.5f + value);
        }



        private static bool ShouldDoIntSnapping(RectTransform rect)
        {
            Canvas componentInParent = rect.gameObject.GetComponentInParent<Canvas>();
            return componentInParent != null && componentInParent.renderMode != RenderMode.WorldSpace;
        }

        private static Vector3 GetRectReferenceCorner(RectTransform gui, bool worldSpace)
        {
            if (!worldSpace)
            {
                return new Vector3(gui.rect.min.x, gui.rect.min.y, 0) + gui.transform.localPosition;
            }
            Transform transform = gui.transform;
            gui.GetWorldCorners(s_Corners);
            if (transform.parent)
            {
                return transform.parent.InverseTransformPoint(s_Corners[0]);
            }
            return s_Corners[0];
        }


        //-------∽-★-∽------∽-★-∽Pivot(需要消化)∽-★-∽------∽-★-∽--------//


        public static void SetPivotSmart(RectTransform rect, Vector2 pivot, bool smart)
        {
            SetPivotSmart(rect, pivot.x, 0, smart, true);
            SetPivotSmart(rect, pivot.y, 1, smart, true);
        }
        public static void SetPivotSmart(RectTransform rect, float value, int axis, bool smart, bool parentSpace)
        {
            Vector3 rectReferenceCorner = GetRectReferenceCorner(rect, !parentSpace);
            Vector2 pivot = rect.pivot;
            pivot[axis] = value;
            rect.pivot = pivot;
            if (smart)
            {
                Vector3 rectReferenceCorner2 = GetRectReferenceCorner(rect, !parentSpace);
                Vector3 v = rectReferenceCorner2 - rectReferenceCorner;
                rect.anchoredPosition -= new Vector2(v.x, v.y);
                Vector3 position = rect.transform.position;
                position.z -= v.z;
                rect.transform.position = position;
            }
        }

        /// <summary>
        /// 计算矩形中心坐标
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="pivot"></param>
        /// <returns></returns>
        static public Vector2 CalcPivotPos(Rect rect, Vector2 pivot)
        {
            return new Vector2(rect.x + rect.width * pivot.x, rect.y + rect.height * pivot.y);
        }

        /// <summary>
        /// 根据中点计算矩形
        /// </summary>
        /// <param name="pivotPos">中点坐标</param>
        /// <param name="pivot"></param>
        /// <param name="size_"></param>
        /// <returns></returns>
        static public Rect CalcRectByPivot(Vector2 pivotPos, Vector2 pivot, Size size_)
        {
            return new Rect(pivotPos.x - size_.width * pivot.x,
                pivotPos.y - size_.height * pivot.y,
                size_.width,
                size_.height);
        }


        //-------∽-★-∽------∽-★-∽Anchor(需要消化)∽-★-∽------∽-★-∽--------//


        public static void SetAnchorSmart(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax)
        {
            var oAnchorMin = rect.anchorMin;
            //set x first
            if (oAnchorMin.x > anchorMax.x)
            {
                SetAnchorSmart(rect, anchorMin.x, 0, false, true);
                SetAnchorSmart(rect, anchorMax.x, 0, true, true);
            }
            else//another case: oAnchorMax.x < anchorMin.x
            {
                SetAnchorSmart(rect, anchorMax.x, 0, true, true);
                SetAnchorSmart(rect, anchorMin.x, 0, false, true);
            }

            if (oAnchorMin.y > anchorMax.y)
            {
                SetAnchorSmart(rect, anchorMin.y, 1, false, true);
                SetAnchorSmart(rect, anchorMax.y, 1, true, true);
            }
            else//another case: oAnchorMax.y < anchorMin.y
            {
                SetAnchorSmart(rect, anchorMax.y, 1, true, true);
                SetAnchorSmart(rect, anchorMin.y, 1, false, true);
            }
        }

        public static void SetAnchorSmart(RectTransform rect, float valueX, float valueY, bool isMax, bool smart)
        {
            SetAnchorSmart(rect, valueX, 0, isMax, smart);
            SetAnchorSmart(rect, valueY, 1, isMax, smart);
        }

        public static void SetAnchorSmart(RectTransform rect, float value, int axis, bool isMax, bool smart)
        {
            RectTransform rectTransform = null;
            if (rect.transform.parent == null)
            {
                smart = false;
            }
            else
            {
                rectTransform = rect.transform.parent.GetComponent<RectTransform>();
                if (rectTransform == null)
                {
                    smart = false;
                }
            }

            //always clamp new value
            {
                value = Mathf.Clamp01(value);
            }

            //always ensure Max is Larger than Min
            if (isMax)
            {
                value = Mathf.Max(value, rect.anchorMin[axis]);
            }
            else
            {
                value = Mathf.Min(value, rect.anchorMax[axis]);
            }

            float num = 0f;
            float num2 = 0f;
            if (smart)
            {
                float num3 = (!isMax) ? rect.anchorMin[axis] : rect.anchorMax[axis];
                num = (value - num3) * rectTransform.rect.size[axis];
                float num4 = 0f;
                if (ShouldDoIntSnapping(rect))
                {
                    num4 = Mathf.Round(num) - num;
                }
                num += num4;

                value += num4 / rectTransform.rect.size[axis];
                if (Mathf.Abs(Round(value * 1000f) - value * 1000f) < 0.1f)
                {
                    value = Round(value * 1000f) * 0.001f;
                }

                {
                    value = Mathf.Clamp01(value);
                }

                if (isMax)
                {
                    value = Mathf.Max(value, rect.anchorMin[axis]);
                }
                else
                {
                    value = Mathf.Min(value, rect.anchorMax[axis]);
                }

                num2 = ((!isMax) ? (num * (1f - rect.pivot[axis])) : (num * rect.pivot[axis]));
            }
            if (isMax)
            {
                Vector2 anchorMax = rect.anchorMax;
                anchorMax[axis] = value;
                rect.anchorMax = anchorMax;
                Vector2 anchorMin = rect.anchorMin;
                rect.anchorMin = anchorMin;
            }
            else
            {
                Vector2 anchorMin2 = rect.anchorMin;
                anchorMin2[axis] = value;
                rect.anchorMin = anchorMin2;
                Vector2 anchorMax2 = rect.anchorMax;
                rect.anchorMax = anchorMax2;
            }
            if (smart)
            {
                Vector2 anchoredPosition = rect.anchoredPosition;
                float num5 = anchoredPosition[axis];
                anchoredPosition[axis] = num5 - num2;
                rect.anchoredPosition = anchoredPosition;

                Vector2 sizeDelta = rect.sizeDelta;
                num5 = sizeDelta[axis];
                sizeDelta[axis] = num5 + num * (float)((!isMax) ? 1 : -1);
                rect.sizeDelta = sizeDelta;
            }
        }




    }

}