/* ==============================================================================
 * 几何工具
 * @author jr.zeng
 * 2017/1/9 17:06:12
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class GeomUtil
    {

        /// <summary>
        /// 转换到本地坐标系
        /// </summary>
        /// <param name="pos_"></param>
        /// <param name="from_"></param>
        /// <param name="to_"></param>
        /// <returns></returns>
        static public Vector3 ConvertToLocalSpace(Vector3 pos_, GameObject from_, GameObject to_)
        {
            if (from_ == to_)
            {
                return pos_;
            }

            Vector3 worldPos = from_.transform.TransformPoint(pos_);    //转至世界坐标
            Vector3 localPos = to_.transform.InverseTransformPoint(worldPos);   //转到本地坐标
            return localPos;
        }

        /// <summary>
        /// 转换到本地坐标系
        /// </summary>
        /// <param name="rect_"></param>
        /// <param name="from_"></param>
        /// <param name="to_"></param>
        /// <returns></returns>
        static public Rect ConvertToLocalSpace(Rect rect_, GameObject from_, GameObject to_)
        {
            if (from_ == to_)
            {
                return rect_;
            }

            Vector2 leftDown = rect_.min;
            Vector2 rightUp = rect_.max;
            leftDown = ConvertToLocalSpace(leftDown, from_, to_);
            rightUp = ConvertToLocalSpace(rightUp, from_, to_);
            Rect rect = new Rect();
            rect.min = leftDown;
            rect.max = rightUp;
            return rect;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽角度相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 把角度调整为0~360
        /// </summary>
        /// <param name="angle_"></param>
        /// <returns></returns>
        static public float ClampAngle360(float angle_)
        {
            var rot = angle_;
            while (rot < 0)
                rot += 360;
            while (rot > 360)
                rot -= 360;
            return rot;
        }

        /// <summary>
        /// 把角度调整为-180~180
        /// </summary>
        /// <param name="angle_"></param>
        /// <returns></returns>
        static public float ClampAngle180(float angle_)
        {
            var rot = angle_;
            while (rot < -180)
                rot += 360;
            while (rot > 180)
                rot -= 360;
            return rot;
        }

    }

}

