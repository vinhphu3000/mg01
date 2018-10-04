/* ==============================================================================
 * 时间工具
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Time = UnityEngine.Time;

namespace mg.org
{
    public class DateUtil
    {

        /// <summary>
        /// TimeFromStart
        /// 启动到现在的时间
        /// </summary>
        static public float TimeFromStart
        {
            get { return Time.realtimeSinceStartup; }
        }

        /// <summary>
        /// 获取当前时间戳(毫秒)
        /// </summary>
        static public long TimeST_ms
        {
            get { return DateTime.Now.Ticks; }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽耗时检测∽-★-∽--------∽-★-∽------∽-★-∽--------//


    }

}