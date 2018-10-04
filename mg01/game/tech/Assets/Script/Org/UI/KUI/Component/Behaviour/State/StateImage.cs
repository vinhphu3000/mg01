/* ==============================================================================
 * StateImage
 * @author jr.zeng
 * 2017/7/8 18:45:54
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

    
    public class StateImage : StateChangeable
    {

        float m_alpha = 1;

        public override float Alpha
        {
            get { return m_alpha; }
            set
            {
                m_alpha = Mathf.Clamp01(value);
                for (int i = 0; i < this.StateCount; i++)
                {
                    GetStateComponent<KImage>(i).alpha = m_alpha;
                }
            }
        }

        /// <summary>
        /// 获取当前Image
        /// </summary>
        public KImage CurImage
        {
            get { return GetCurStateComponent<KImage>(); }
        }

        /// <summary>
        /// 是否填充
        /// </summary>
        public bool SlicedFilled
        {
            get  { return GetStateComponent<KImage>(0).slicedFilled;  }
            set
            {

                for (int i = 0; i < this.StateCount; i++)
                {
                    KImage img = GetStateComponent<KImage>(i);
                    img.slicedFilled = value;
                    if (value && (img.fillMethod == Image.FillMethod.Radial360))    //TODO
                    {
                        img.fillMethod = Image.FillMethod.Horizontal;
                    }
                }
            }
        }

        /// <summary>
        /// StateImage所有状态图片应该保持同类型
        /// </summary>
        /// <returns></returns>
        bool HasFilledImage()
        {
            KImage image = GetComponentInChildren<KImage>();
            if (image != null)
                return image.type == Image.Type.Filled ||                       //进度填充
                    (image.slicedFilled && image.type == Image.Type.Sliced);    //九宫格
            return false;
        }


        RectTransformOperator GetRectTransformOperator()
        {
            RectTransformOperator result = this.gameObject.GetComponent<RectTransformOperator>();
            if (result == null)
                result = this.gameObject.AddComponent<RectTransformOperator>();
            return result;
        }

        FilledImageOperator GetFilledImageOperator()
        {
            FilledImageOperator result = this.gameObject.GetComponent<FilledImageOperator>();
            if (result == null)
                result = this.gameObject.AddComponent<FilledImageOperator>();
            return result;
        }



        public Vector2 Size
        {
            get
            {
                if (HasFilledImage() == true)
                {
                    return GetFilledImageOperator().Size;
                }
                return GetRectTransformOperator().Size;
            }
            set
            {
                if (HasFilledImage() == true)
                {
                    GetFilledImageOperator().Size = value;
                }
                else
                {
                    GetRectTransformOperator().Size = value;
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                if (HasFilledImage() == true)
                {
                    return GetFilledImageOperator().Scale;
                }
                return GetRectTransformOperator().Scale;
            }
            set
            {
                if (HasFilledImage() == true)
                {
                    GetFilledImageOperator().Scale = value;
                }
                else
                {
                    GetRectTransformOperator().Scale = value;
                }
            }
        }

        public float RotationZ
        {
            get { return GetRectTransformOperator().RotationZ; }
            set
            {
                GetRectTransformOperator().RotationZ = value;
            }
        }

        public void Align()
        {
            if (HasFilledImage() == true)
            {
                GetFilledImageOperator().Align();
            }
            else
            {
                GetRectTransformOperator().Align();
            }
        }


    }

}