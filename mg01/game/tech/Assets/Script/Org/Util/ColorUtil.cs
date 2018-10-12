/* ==============================================================================
 * ColorUtil
 * @author jr.zeng
 * 2016/10/20 19:00:45
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{


    public class C3B
    {
        static public Color WHITE = cc.c3b(255, 255, 255);
        static public Color YELLOW = cc.c3b(255, 255, 0);
        static public Color GREEN = cc.c3b(0, 255, 0);
        static public Color BLUE = cc.c3b(0, 0, 255);
        static public Color RED = cc.c3b(255, 0, 0);
        static public Color MAGENTA = cc.c3b(255, 0, 255);
        static public Color BLACK = cc.c3b(0, 0, 0);
        static public Color ORANGE = cc.c3b(255, 127, 0);
        static public Color GRAY = cc.c3b(166, 166, 166);
    }



    public class ColorUtil
    {

        static float c_factor = 1f / 255f;

        /// <summary>
        /// 转换为Color
        /// </summary>
        /// <param name="color_"></param>
        /// <returns></returns>
        public static Color ColorToC3B(object color_)
        {
            Color color;
            if (color_ is string)
            {
                color = ParseColor24(color_ as string);
            }
            else if (color_ is int)
            {
                string str = Convert.ToString((int)color_, 16);
                color = ParseColor24(str);
            }
            else if (color_ is Color)
            {
                color = (Color)color_;
            }
            else
            {
                color = new Color(1,1,1);
            }

            return color;
        }

        public static Color ColorToC4B(object color_)
        {
            Color color;
            if (color_ is string)
            {
                color = ParseColor32(color_ as string, 0);
            }
            else if (color_ is int)
            {
                string str = Convert.ToString((int)color_, 16);
                color = ParseColor32(str, 0);
            }
            else if (color_ is Color)
            {
                color = (Color)color_;
            }
            else
            {
                color = new Color(1, 1, 1, 1);
            }

            return color;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="color_"></param>
        /// <returns></returns>
        static public string ColorToString(object color_)
        {
            string str;
            if (color_ is string)
            {
                //"FF0000"
                str = color_ as string;
            }
            else if (color_ is int)
            {
                str = Convert.ToString((int)color_, 16);
            }
            else if (color_ is Color)
            {
                Color color = (Color)color_;
                str = EncodeColor24(color);
            }
            else
            {
                str = "FFFFFF";
            }

            return str;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽颜色与字符串 转换∽-★-∽--------∽-★-∽------∽-★-∽--------//
        

        /// <summary>
        /// Parse a RrGgBb color encoded in the string.
        /// </summary>
        [System.Diagnostics.DebuggerHidden]         //输出的日志不打印此方法的堆栈
        [System.Diagnostics.DebuggerStepThrough]    //单步调试代码时不要进入此方法
        static public Color ParseColor24(string text, int offset = 0)
        {
            int r = (MathUtil.HexToDecimal(text[offset]) << 4) | MathUtil.HexToDecimal(text[offset + 1]);
            int g = (MathUtil.HexToDecimal(text[offset + 2]) << 4) | MathUtil.HexToDecimal(text[offset + 3]);
            int b = (MathUtil.HexToDecimal(text[offset + 4]) << 4) | MathUtil.HexToDecimal(text[offset + 5]);
            return new Color(c_factor * r, c_factor * g, c_factor * b);
        }

        /// <summary>
        /// Parse a RrGgBbAa color encoded in the string.
        /// </summary>
        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public Color ParseColor32(string text, int offset)
        {
            int r = (MathUtil.HexToDecimal(text[offset]) << 4) | MathUtil.HexToDecimal(text[offset + 1]);
            int g = (MathUtil.HexToDecimal(text[offset + 2]) << 4) | MathUtil.HexToDecimal(text[offset + 3]);
            int b = (MathUtil.HexToDecimal(text[offset + 4]) << 4) | MathUtil.HexToDecimal(text[offset + 5]);
            int a = (MathUtil.HexToDecimal(text[offset + 6]) << 4) | MathUtil.HexToDecimal(text[offset + 7]);
            return new Color(c_factor * r, c_factor * g, c_factor * b, c_factor * a);
        }


        /// <summary>
        /// Convert the specified color to RGBA32 integer format.
        /// </summary>
        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public int ColorToInt(Color c)
        {
            int retVal = 0;
            retVal |= Mathf.RoundToInt(c.r * 255f) << 24;
            retVal |= Mathf.RoundToInt(c.g * 255f) << 16;
            retVal |= Mathf.RoundToInt(c.b * 255f) << 8;
            retVal |= Mathf.RoundToInt(c.a * 255f);
            return retVal;
        }



        /// <summary>
        /// The reverse of ParseColor24 -- encodes a color in RrGgBb format.
        /// </summary>
        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public string EncodeColor24(Color c)
        {
            int i = 0xFFFFFF & (ColorToInt(c) >> 8);
            return MathUtil.DecimalToHex24(i);
        }

        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public string EncodeColor32(Color c)
        {
            int i = 0xFFFFFF & ColorToInt(c);
            return MathUtil.DecimalToHex24(i);
        }


    }
}