/* ==============================================================================
 * KProgressBar
 * @author jr.zeng
 * 2017/7/10 9:51:56
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

    public class KProgressBar : KContainer
    {
        //bar名称
        public const string CHILD_NAME_BAR = "Image_bar";

        private StateImage m_bar;
        private float m_value = 1.0f;   //0~1
        private bool m_slicedFilled = false;

        protected override void Awake()
        {
            base.Awake();

            // 默认开启
            SlicedFilled = true;
        }

        protected override void __Initialize()
        {
            m_bar = ComponentUtil.EnsureChildComponent<StateImage>(gameObject, "Image_bar");

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//
        /// <summary>
        /// 0~100
        /// </summary>
        public int percent
        {
            get { return Mathf.RoundToInt(m_value * 100.0f); }
            set
            {
                this.value = (float)value / 100.0f;
            }
        }

        /// <summary>
        /// 0~1
        /// </summary>
        public float value
        {
            get { return m_value; }
            set
            {
                if (m_value == value)
                    return;
                m_value = Mathf.Clamp01(value);

                if (m_bar != null)
                {
                    m_bar.Scale = new Vector3(m_value, 1.0f, 1.0f);
                    m_bar.Align();
                }
            }
        }

        public bool SlicedFilled
        {
            get {  return m_slicedFilled;  }
            set
            {
                if (m_slicedFilled == value)
                    return;
                 m_slicedFilled = value;

                if (m_bar != null)
                    m_bar.SlicedFilled = value;
            }
        }



    }

}