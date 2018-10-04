/* ==============================================================================
 * KImage
 * @author jr.zeng
 * 2017/6/19 10:20:58
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{


    public class KImage : Image
    {
        
        public KImage()
        {

        }


        virtual public void Initialize()
        {
            
        }

        /// <summary>
        /// 从网络下载
        /// </summary>
        /// <param name="imgUrl_"></param>
        public void LoadFromNet(string imgUrl_)
        {
            IEnumerator routine = _LoadFromNet(this, imgUrl_);
            StartCoroutine(routine);
        }

        IEnumerator _LoadFromNet(Image img_, string imgUrl_)
        {
            WWW w = new WWW(imgUrl_);

            yield return w;

            Texture2D tex = w.texture;
            String ctype = w.responseHeaders["CONTENT-TYPE"].ToLower();
            if (ctype.IndexOf("jpg") != -1 || ctype.IndexOf("jpeg") != -1 || ctype.IndexOf("png") != -1 || ctype.IndexOf("gif") != -1)
            {
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                img_.sprite = spr;
            }
        }

    }

}