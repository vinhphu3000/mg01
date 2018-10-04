/* ==============================================================================
 * FilledImageOperator
 * @author jr.zeng
 * 2017/7/8 18:00:58
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class FilledImageOperator : MonoBehaviour
    {
        private Vector3 m_scale;
        private Vector2 m_size;
        private Vector2 m_originalSize;

        private Image.FillMethod m_fillMethod;
        private bool m_slicedFilled;
        

        private void Awake()
        {
            m_originalSize = (this.transform as RectTransform).sizeDelta;   //原尺寸
            m_size = m_originalSize;

            KImage image = GetComponentInChildren<KImage>();
            m_fillMethod = image.fillMethod;
            m_slicedFilled = image.slicedFilled;
        }

        public Vector2 Size
        {
            get  { return m_size; }
            set
            {
                m_size = value;
                KImage[] images = GetComponentsInChildren<KImage>();
                foreach (KImage image in images)
                {
                    if (m_fillMethod == Image.FillMethod.Horizontal)
                    {
                        image.fillAmount = m_size.x / m_originalSize.x; //算出百分比
                    }
                    else if (m_fillMethod == Image.FillMethod.Vertical)
                    {
                        image.fillAmount = m_size.y / m_originalSize.y;
                    }
                }
            }
        }

        public Vector3 Scale
        {
            get {  return m_scale; }
            set
            {
                m_scale = value;
                KImage[] images = GetComponentsInChildren<KImage>();
                foreach (KImage image in images)
                {
                    if (m_fillMethod == Image.FillMethod.Horizontal)
                    {
                        image.fillAmount = m_scale.x;
                    }
                    else if (m_fillMethod == Image.FillMethod.Vertical)
                    {
                        image.fillAmount = m_scale.y;
                    }
                }
            }
        }

        /// <summary>
        /// 没用到
        /// </summary>
        public bool SlicedFilled
        {
            get { return m_slicedFilled;  }
            set
            {
                if (m_slicedFilled != value)
                {
                    m_slicedFilled = value;
                    KImage image = GetComponentInChildren<KImage>();       //为什么只设置一个
                    image.slicedFilled = m_slicedFilled;
                }
            }
        }


        public void Align()
        {
            RectTransform[] rects = GetComponentsInChildren<RectTransform>();
            foreach (RectTransform rect in rects)
            {
                KImage image = rect.GetComponent<KImage>();
                if (image)
                {
                    // 加个判断 因为被包了一层 
                    Vector2 position = rect.anchoredPosition;
                    if (image.fillMethod == Image.FillMethod.Horizontal && image.fillOrigin == (int)Image.OriginHorizontal.Right)
                    {
                        position.x = -m_originalSize.x * (1 - m_scale.x);
                    }
                    if (image.fillMethod == Image.FillMethod.Vertical && image.fillOrigin == (int)Image.OriginVertical.Bottom)
                    {
                        position.y = m_originalSize.y * (1 - m_scale.y);
                    }
                    rect.anchoredPosition = position;
                }
            }
        }
        

    }

}