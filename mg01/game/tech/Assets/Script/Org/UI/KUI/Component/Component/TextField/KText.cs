/* ==============================================================================
 * K文本框
 * @author jr.zeng
 * 2017/7/6 14:13:16
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
    public class KText : Text
    {
        //public static Func<int, string> GetLanguage;

        public int langId = -1;

        public KText()
        {

        }

        ////在Prefab资源加载进内存反序列化后即执行
        //public void OnAfterDeserialize()
        //{
        //    //if(GetLanguage != null && langId != -1)
        //    //{
        //    //    this.text = GetLanguage(this.langId);
        //    //}
        //}

        //public void OnBeforeSerialize()
        //{
        //    //left blank
        //}


        //public float GetPreferredHeight()
        //{
        //    RectTransform rect = gameObject.GetComponent<RectTransform>();
        //    float h = LayoutUtility.GetPreferredHeight(rect);
        //    return h;
        //}

        //public float GetPreferredWidth()
        //{
        //    RectTransform rect = gameObject.GetComponent<RectTransform>();
        //    return LayoutUtility.GetPreferredWidth(rect);
        //}


        //public float GetPreferredHeight()
        //{
        //    Vector2 extents = rectTransform.rect.size;
        //    var settings = GetGenerationSettings(extents);
        //    return cachedTextGeneratorForLayout.GetPreferredHeight(text, settings);
        //}
        //public float GetPreferredWidth()
        //{
        //    Vector2 extents = rectTransform.rect.size;
        //    var settings = GetGenerationSettings(extents);
        //    return cachedTextGeneratorForLayout.GetPreferredWidth(text, settings);
        //}

        /// <summary>
        /// 获取文本高度
        /// </summary>
        /// <returns></returns>
        public float GetPreferredHeight()
        {
            return preferredHeight;
        }

        /// <summary>
        /// 获取文本宽度
        /// </summary>
        /// <returns></returns>
        public float GetPreferredWidth()
        {
            return preferredWidth;
        }

    }



}