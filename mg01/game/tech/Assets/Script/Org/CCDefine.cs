using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace mg.org
{

    public class CCDefine
    {

        //调试
        public const bool DEBUG = true;
        //默认帧频
        public const int FPS_DEFAULT = 60;
        //使用LuA
        public const bool USE_LUA = false;
        //设计的分辨率
        //public static Vector2 DESIGN_WIN_SIZE = cc.size(1334f, 750f);
        
        public const string Platform =
#if UNITY_EDITOR
      "editor";
#elif UNITY_ANDROID
    "android";
#elif UNITY_IOS
    "ios";
#else
    "pc";
#endif
        //是否使用键盘
        public const bool USE_KEYBOARD = (Platform == "editor" || Platform == "pc"); 
    }


    /// <summary>
    /// 屏幕适配模式
    /// </summary>
    public enum ResolutionPolicy
    {
        //拉伸变形，使铺满屏幕
        EXACT_FIT = 0,  
        //按比例放缩，全屏展示不留黑边。 （长宽中小的铺满屏幕，大的超出屏幕）
        NO_BORDER,
        //按比例放缩，全部展示不裁剪。（长宽中大的铺满屏幕，小的留有黑边）
        SHOW_ALL,
        //按比例放缩，宽度铺满屏幕
        FIXED_WIDTH,
        //按比例放缩，高度铺满屏幕
        FIXED_HEIGHT,
    }

    //消息名称
    public class CCMsgName
    {
        //上级父节点改变
        static public string UpperParentChange = "OnMsgUpperParentChange";
    }


   
}