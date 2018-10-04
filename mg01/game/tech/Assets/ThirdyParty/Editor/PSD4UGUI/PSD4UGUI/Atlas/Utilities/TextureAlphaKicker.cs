using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class TextureAlphaKicker
    {
        public const float ALPHA_THRESHOLD = 0.0f;

        public static Texture2D Kick(Texture2D source, out Rect rect, out Vector4 padding)
        {
            //若是按照OpenGL的坐标零点在左下角，此处的top和bottom应该反过来
            int top = GetPixelTop(source);  //查找第一个有像素的行
            int left = GetPixelLeft(source);
            int right = GetPixelRight(source);
            int bottom = GetPixelBottom(source);
            Color[] pixels = source.GetPixels(left, top, right - left, bottom - top);
            Texture2D texture = new Texture2D(right - left, bottom - top);  //去掉四边透明像素
            texture.name = source.name;
            texture.SetPixels(pixels);
            texture.Apply();
            if (pixels.Length == 0 || texture.width == 0 || texture.height == 0)
            {
                throw new Exception("图片处理出错：" + source.name);
            }
            rect = new Rect(left, top, right - left, bottom - top);
            padding = new Vector4(left, top, source.width - right, source.height - bottom); //边距
            return texture;
        }

        private static int GetPixelLeft(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    if (texture.GetPixel(i, j).a > ALPHA_THRESHOLD)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        private static int GetPixelRight(Texture2D texture)
        {
            for (int i = texture.width - 1; i >= 0; i--)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    if (texture.GetPixel(i, j).a > ALPHA_THRESHOLD)
                    {
                        return Mathf.Min(texture.width, i + 1);
                    }
                }
            }
            return texture.width;
        }

        private static int GetPixelTop(Texture2D texture)
        {
            for (int i = 0; i < texture.height - 1; i++)
            {
                for (int j = 0; j < texture.width; j++)
                {
                    if (texture.GetPixel(j, i).a > ALPHA_THRESHOLD)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        private static int GetPixelBottom(Texture2D texture)
        {
            for (int i = texture.height - 1; i >= 0; i--)
            {
                for (int j = 0; j < texture.width; j++)
                {
                    if (texture.GetPixel(j, i).a > ALPHA_THRESHOLD)
                    {
                        return Mathf.Min(texture.height, i + 1);
                    }
                }
            }
            return 0;
        }

    }

}