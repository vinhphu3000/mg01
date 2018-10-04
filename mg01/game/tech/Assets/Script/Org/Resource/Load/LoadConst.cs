/* ==============================================================================
 * 加载常量
 * @author jr.zeng
 * 2016/8/22 14:28:52
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    /// <summary>
    /// 加载模式
    /// </summary>
    public enum LoadMode
    {
        //-无效
        Invalid = 0,
        //本地GET
        LocalGet = 1,
        //网络GET
        WebGet,
        //网络POST
        WebPost,
    }

  
    /// <summary>
    /// 加载条类型
    /// </summary>
    partial class LOAD_VIEW_TYPE
    {
        static public string NONE = "";

    }

    public enum LoadReqType
    {
        DEFAULT = 0,
        QUEUE = 1,      //串行加载
        BATCH = 2,      //并行加载
        LEVEL = 3,      //场景加载
        DELAY = 4,      //延时
        PROGRESS = 5,   //外部进度
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽事件相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

    /// <summary>
    /// 加载事件
    /// </summary>
    public class LOAD_EVT
    {
        //加载开始
        public const string START = "LOAD_START";
        //加载进度
        public const string PROGRESS = "LOAD_PROGRESS";
        //加载完成
        public const string COMPLETE = "LOAD_COMPLETE";
        //加载失败
        public const string FAIL = "LOAD_FAIL";

        //加载条完成
        public const string VIEW_COMPLETE = "LOAD_VIEW_COMPLETE";
    }



}
