/* ==============================================================================
 * 视图抽象类2D
 * @author jr.zeng
 * 2016/12/29 10:17:14
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class ImageAbs2D : ImageAbs
    {
        

        public ImageAbs2D()
        {

        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Transform∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void SetPosition(Vector2 pos_)
        {
            DisplayUtil.SetPos2(this.gameObject, pos_);
        }
        
        public Vector2 position
        {
            get { return transform.localPosition; }
        }

    }


}