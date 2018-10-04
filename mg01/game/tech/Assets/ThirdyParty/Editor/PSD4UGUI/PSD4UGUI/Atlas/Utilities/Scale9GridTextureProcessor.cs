using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class Scale9GridTextureProcessor
    {
        /// <summary>
        /// 按九宫格生成最小图片
        /// </summary>
        /// <param name="source"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        /// <param name="transFrame"></param>
        /// <returns></returns>
        public static Texture2D Process(Texture2D source, int top, int right, int bottom, int left, bool transFrame = false)
        {
            int sourceWidth = source.width;
            int sourceHeight = source.height;
            Color32[] sourcePixels = source.GetPixels32();
            int targetWidth = left + 1 + right; //填充区域只留1像素
            int targetHeight = top + 1 + bottom;
            Color32[] targetPixels = new Color32[targetWidth * targetHeight];
            Texture2D target = new Texture2D(targetWidth, targetHeight);
            int pixelIndex = 0;
            for (int i = 0; i < sourceHeight; i++)
            {
                if (i > bottom && i < (sourceHeight - top))
                {
                    continue;
                }
                for (int j = 0; j < sourceWidth; j++)
                {
                    if (j > left && j < (sourceWidth - right))
                    {
                        continue;
                    }
                    var color = sourcePixels[i * sourceWidth + j];
                    if (transFrame && color.a == 0)
                        color.a = 1;    //如果此像素是透明的，置为不透明？
                    targetPixels[pixelIndex++] = color;
                }
            }
            target.SetPixels32(targetPixels);
            target.Apply();
            return target;
        }
    }

}