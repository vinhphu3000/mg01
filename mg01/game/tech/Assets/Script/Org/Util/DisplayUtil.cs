/* ==============================================================================
 * DisplayUtil
 * @author jr.zeng
 * 2016/6/12 15:19:18
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace mg.org
{
    public class DisplayUtil
    {


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Position相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
       
        static public void SetPos(GameObject obj_, float x_, float y_, float z_)
        {
            Transform trans = obj_.transform;
            trans.localPosition = new Vector3(x_, y_, z_);
        }
        static public void SetPos2(GameObject go_, float x_, float y_)
        {
            Transform trans = go_.transform;
            trans.localPosition = new Vector3(x_, y_, trans.localPosition.z);
        }

        static public void SetPosX(GameObject go_, float x_)
        {
            SetPos2(go_, x_, go_.transform.localPosition.y);
        }

        static public void SetPosY(GameObject go_, float y_)
        {
            SetPos2(go_, go_.transform.localPosition.x, y_);
        }

        static public void SetPosZ(GameObject go_, float z_)
        {
            SetPos(go_, go_.transform.localPosition.x, go_.transform.localPosition.y, z_);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Scale相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        static public void SetScale(GameObject obj_, float scaleX_, float scaleY_, float scaleZ_)
        {
            obj_.transform.localScale = new Vector3(scaleX_, scaleY_, scaleZ_);
        }

        static public void SetScale2(GameObject go_, float scale_)
        {
            SetScale2(go_, scale_, scale_);
        }

        static public void SetScale2(GameObject go_, float scaleX_, float scaleY_)
        {
            SetScale(go_, scaleX_, scaleY_, go_.transform.localScale.z);
        }
        

    


    }

}