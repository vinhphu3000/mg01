using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class AtlasOptimizer
    {
        /// <summary>
        /// 优化图集
        /// 1.可强制设置正方形Atlas
        /// 2.若出现超过一半面积为空的情况，可以删除空的部分
        /// </summary>
        public static Texture2D Optimize(Texture2D atlas, Rect[] rects, bool forceSquare = false)
        {
            Rect rect = GetAtlasContentRect(rects);
            if (rect.width <= 0.5f)
            {
                atlas = CreateResizedAtlas(atlas, 0.5f, 1.0f, rects);
            }
            if (rect.height <= 0.5f)
            {
                atlas = CreateResizedAtlas(atlas, 1.0f, 0.5f, rects);
            }

            if (forceSquare == true)
            {
                if (atlas.width > atlas.height)
                {
                    atlas = CreateResizedAtlas(atlas, 1.0f, 2.0f, rects);
                }
                else if (atlas.width < atlas.height)
                {
                    atlas = CreateResizedAtlas(atlas, 2.0f, 1.0f, rects);
                }
            }
            return atlas;
        }

        private static Texture2D CreateResizedAtlas(Texture2D atlas, float xScale, float yScale, Rect[] rects)
        {
            int width = (int)(atlas.width * xScale);
            int height = (int)(atlas.height * yScale);
            Texture2D result = new Texture2D(width, height);
            result.name = atlas.name;
            int pixelWidth = width > atlas.width ? atlas.width : width;
            int pixelHeight = height > atlas.height ? atlas.height : height;
            var dw = (width - pixelWidth);
            var dh = (height - pixelHeight);
            var emptyColor = new Color[dw * dh];
            if (emptyColor.Length > 0)
            {
                for (var i = 1; i < emptyColor.Length; ++i)
                {
                    emptyColor[i] = new Color(0, 0, 0, 0);
                }
                result.SetPixels(pixelWidth, pixelHeight, dw, dh, emptyColor);
            }
            result.SetPixels(0, 0, pixelWidth, pixelHeight, atlas.GetPixels(0, 0, pixelWidth, pixelHeight));
            result.Apply();
            for (int i = 0; i < rects.Length; i++)
            {
                Rect rect = rects[i];
                rects[i] = new Rect(rect.xMin / xScale, rect.yMin / yScale, rect.width / xScale, rect.height / yScale);
            }
            return result;
        }

        //获取矩形外框
        private static Rect GetAtlasContentRect(Rect[] rects)
        {
            Rect result = new Rect(0, 0, 0, 0);
            foreach (Rect rect in rects)
            {
                if (rect.xMin < result.xMin)
                {
                    result.xMin = rect.xMin;
                }
                if (rect.yMin < result.yMin)
                {
                    result.yMin = rect.yMin;
                }
                if (rect.xMax > result.xMax)
                {
                    result.xMax = rect.xMax;
                }
                if (rect.yMax > result.yMax)
                {
                    result.yMax = rect.yMax;
                }
            }
            return result;
        }
    }

}