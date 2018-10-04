/* ==============================================================================
 * CCUtil
 * @author jr.zeng
 * 2016/10/12 18:17:03
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using mg.org.Actions;

namespace mg.org
{


    public class cc
    {

        static public Vector2 p(float x_, float y_)
        {
            return new Vector2(x_, y_);
        }

        static public Vector2 p(Vector2 p_)
        {
            return new Vector2(p_.x, p_.y);
        }

        static public Vector3 p3(float x_ , float y_ , float z_)
        {
            return new Vector3(x_, y_, z_);
        }

        static public Vector3 p3(Vector3 p_)
        {
            return new Vector3(p_.x, p_.y, p_.z);
        }



        static public Vector2 size(float x_, float y_ )
        {
            return new Vector2(x_, y_);
        }

        static public Rect rect(float x_, float y_, float width_, float height_ )
        {
            return new Rect(x_, y_, width_, height_);
        }

        static public Color c3b(int r_, int g_, int b_)
        {
            float f = 1f / 255f;
            return new Color(r_ * f, g_ * f, b_ * f);
        }

        static public Color c4b(int r_, int g_, int b_, int a_)
        {
            float f = 1f / 255f;
            return new Color(r_ * f, g_ * f, b_ * f, a_ * f);
        }

        static public Color c3f(float r_, float g_, float b_)
        {
            return new Color(r_, g_, b_);
        }

        static public Color c4f(float r_, float g_, float b_, float a_)
        {
            return new Color(r_, g_, b_, a_);
        }


        static public Vector2 vec2(float x_ = 0, float y_ = 0)
        {
            return new Vector2(x_, y_);
        }

        static public Vector3 vec3(float x_, float y_ , float z_ )
        {
            return new Vector3(x_, y_, z_);
        }

        static public Vector4 vec4(float x_, float y_ , float z_, float w_ )
        {
            return new Vector4(x_, y_, z_, w_);
        }

        static public Quaternion quaternion(float x_ , float y_ , float z_ , float w_ )
        {
            return new Quaternion(x_, y_, z_, w_);
        }


        
        


    }

}