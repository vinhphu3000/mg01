/* ==============================================================================
 * RectTransformOperator
 * @author jr.zeng
 * 2017/7/8 18:06:34
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    /// <summary>
    /// 放置在StateImage和StateText节点上，用于操作states上的RectTransform相关属性
    /// </summary>
    public class RectTransformOperator : MonoBehaviour
    {

        private Vector2 m_originalSize;
        

        private void Awake()
        {
            m_originalSize = (this.transform as RectTransform).sizeDelta;
        }


        public Vector2 Size
        {
            get
            {
                RectTransform rect = GetComponent<RectTransform>();
                return rect.sizeDelta;
            }
            set
            {
                RectTransform[] rects = GetComponentsInChildren<RectTransform>();
                foreach (RectTransform rect in rects)
                {
                    rect.sizeDelta = value;
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                RectTransform rect = GetComponent<RectTransform>();
                return rect.localScale;
            }
            set
            {
                RectTransform[] rects = GetComponentsInChildren<RectTransform>();
                foreach (RectTransform rect in rects)
                {
                    rect.localScale = value;
                }
            }
        }

        /// <summary>
        /// 旋转值，参数为0-360
        /// </summary>
        public float RotationZ
        {
            get
            {
                RectTransform rect = GetComponent<RectTransform>();
                return rect.localRotation.eulerAngles.z;
            }
            set
            {
                RectTransform[] rects = GetComponentsInChildren<RectTransform>();
                foreach (RectTransform rect in rects)
                {
                    rect.localRotation = Quaternion.AngleAxis(value, Vector3.forward);
                }
            }
        }

        public void Align()
        {
            //left blank
        }

    }

}