/* ==============================================================================
 * 窗口常量
 * @author jr.zeng
 * 2016/9/26 18:26:41
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{


    /// <summary>
    /// 窗口层级ID
    /// </summary>
    public class POP_LAYER_IDX
    {
        static public int LAYER_NONE = 0;       //场景
        static public int LAYER_SCENE = 1;      //场景
        static public int LAYER_MAIN_UI = 2;    //主UI
        static public int LAYER_POP_1 = 3;      //窗口层1
        static public int LAYER_POP_2 = 4;      //窗口层2
        static public int LAYER_POP_3 = 5;      //窗口层3

        static public int LAYER_LOADING = 40;   //加载条
        static public int LAYER_TIPS = 50;      //tips
        static public int LAYER_TOP = 99;       //顶层

        static public int[] ALL_LAYERS = new int[]{
            LAYER_SCENE,
            LAYER_MAIN_UI,
            LAYER_POP_1,
            LAYER_POP_2,
            LAYER_POP_3,
            LAYER_LOADING,
            LAYER_TIPS,
            LAYER_TOP
        };

    }

    /// <summary>
    /// 窗口对齐方式
    /// </summary>
    public enum POP_ALIGN
    {
        //无对齐
        NONE = 0,
        //居中
        CENTER,
        //上居中
        CENTER_TOP,
        //左居中
        LEFT_CENTER,
        //左下
        LEFT_BOTTOM,
        //右居中
        RIGHT_CENTER,
        //右下
        RIGHT_BOTTOM,
    }

    /// <summary>
    /// 窗口生命周期
    /// </summary>
    public enum POP_LIFE
    {
        //存栈,适当时会释放
        STACK       = 1,
        //永远存在
        FOREVER     = 2,
        //用完立刻释放
        WEAK        = 3,
    }

    /// <summary>
    /// 窗口id
    /// </summary>
    public class CC_POP_ID
    {
        static public string INVALID = "invalid";


        static Dictionary<string, string> __pop2prefeb = new Dictionary<string, string>()
        {
            {CC_POP_ID.INVALID, "none"},
        };


        public static void AddPrefebPath(Dictionary<string, string> dic_)
        {
            foreach (var kvp in dic_)
            {
                __pop2prefeb[kvp.Key] = kvp.Value;
            }
        }

        public static string GetPrefebPath(string popId_)
        {
            if (__pop2prefeb.ContainsKey(popId_))
                return __pop2prefeb[popId_];
            return null;
        }

    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽事件相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public class POP_EVT
    {
        //窗口打开
       public const  string POP_OPEN       = "POP_EVT_OPEN";
        //窗口关闭
       public const  string POP_CLOSE      = "POP_EVT_CLOSE";       
        //所有面板窗口
       public const  string POP_CLOSE_ALL  = "POP_CLOSE_ALL";   
        
        //窗口淡入开始
       public const  string POP_IN_BEGIN   = "POP_EVT_IN_BEGIN";      
        //窗口淡入完成
      public  const  string POP_IN_FINISH  = "POP_EVT_IN_BEGIN";
        //窗口淡出开始
       public const  string POP_OUT_BEGIN  = "POP_EVT_OUT_BEGIN";    
        //窗口淡出完成
       public const  string POP_OUT_FINISH = "POP_EVT_OUT_FINISH";    
        
        
        //点击按钮事件
        public const  string POP_CLICK_BTN  = "POP_EVT_CLICK_BTN";
    }

}
