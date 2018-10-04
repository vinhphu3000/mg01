/* ==============================================================================
 * MathUtil
 * @author jr.zeng
 * 2016/8/5 16:42:54
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    public class MathUtil
    {

        


        //-------∽-★-∽------∽-★-∽--------∽-★-∽随机数相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static float __randSeed;    //随机种子
        static Random __rand = new Random();

        static public float Random(float min_ = 0, float max_ = 1)
        {
            float num = (float)__rand.NextDouble(); //返回一个大于或等于 0.0 且小于 1.0 的随机浮点数
            return min_ + num * max_;
        }

        static public int RandomInt(int min_, int max_)
        {
            return __rand.Next(min_, max_); //返回一个大于或等于 min 且小于 max 的任意整数
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽16进制相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public int HexToDecimal(char ch)
        {
            switch (ch)
            {
                case '0': return 0x0;
                case '1': return 0x1;
                case '2': return 0x2;
                case '3': return 0x3;
                case '4': return 0x4;
                case '5': return 0x5;
                case '6': return 0x6;
                case '7': return 0x7;
                case '8': return 0x8;
                case '9': return 0x9;
                case 'a':
                case 'A': return 0xA;
                case 'b':
                case 'B': return 0xB;
                case 'c':
                case 'C': return 0xC;
                case 'd':
                case 'D': return 0xD;
                case 'e':
                case 'E': return 0xE;
                case 'f':
                case 'F': return 0xF;
            }
            return 0xF;
        }

        /// <summary>
        /// Convert a single 0-15 value into its hex representation.
        /// It's coded because int.ToString(format) syntax doesn't seem to be supported by Unity's Flash. It just silently crashes.
        /// </summary>

        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public char DecimalToHexChar(int num)
        {
            if (num > 15) return 'F';
            if (num < 10) return (char)('0' + num);
            return (char)('A' + num - 10);
        }


        /// <summary>
        /// Convert a decimal value to its hex representation.
        /// It's coded because num.ToString("X6") syntax doesn't seem to be supported by Unity's Flash. It just silently crashes.
        /// string.Format("{0,6:X}", num).Replace(' ', '0') doesn't work either. It returns the format string, not the formatted value.
        /// </summary>
        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        static public string DecimalToHex24(int num)
        {
            num &= 0xFFFFFF;
#if UNITY_FLASH
		StringBuilder sb = new StringBuilder();
		sb.Append(DecimalToHexChar((num >> 20) & 0xF));
		sb.Append(DecimalToHexChar((num >> 16) & 0xF));
		sb.Append(DecimalToHexChar((num >> 12) & 0xF));
		sb.Append(DecimalToHexChar((num >> 8) & 0xF));
		sb.Append(DecimalToHexChar((num >> 4) & 0xF));
		sb.Append(DecimalToHexChar(num & 0xF));
		return sb.ToString();
#else
            return num.ToString("X6");
#endif
        }


    }



}
