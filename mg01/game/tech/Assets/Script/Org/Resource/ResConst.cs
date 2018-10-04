/* ==============================================================================
 * 资源常量
 * @author jr.zeng
 * 2016/6/15 16:25:10
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{
    public class ResConst
    {
        public const string RES_SEPARATOR = "@";

        static public string GenResUID(string resId_, string resName_ = null)
        {
            return resId_ + RES_SEPARATOR + (resName_ != null ? resName_ : "");
        }
    }



    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        //-无效
        INVALID = 0,
        //二进制文件
        BINARY = 1,
        //文本
        TEXT,
        //图片
        IMAGE,
        //声音
        SOUND,
        //材质
        MATERIAL,
        //AssetBundle
        BUNDLE,
        //预设
        PREFAB,
        //Unity.Object
        OBJECT,

        //NGUI
        NGUI = 100,
        //字体
        NGUI_FONT,

    }


    /// <summary>
    /// 文件后缀
    /// </summary>
    public class ResSuffix
    {
        public const string NULL = null;
        public const string NONE = "";
        public const string BYTES = ".bytes";
        //文本
        public const string XML = ".xml";
        public const string TXT = ".txt";
        public const string LUA = ".lua";
        public const string PLIST = ".plist";
        public const string ATLAS = ".atlas";
        public const string JSON = ".json";
        public const string EXPORT_JSON = ".ExportJson";
        //图片
        public const string PNG = ".png";
        public const string JPG = ".jpg";
        public const string PKM = ".pkm";
        public const string PVR = ".pvr.ccz";
        public const string TGA = ".tga";
        //3d
        public const string C3B = "c3b";
        //字体
        public const string FNT = ".fnt";
        //音效
        public const string MP3 = ".mp3";
        public const string WAV = ".wav";
        public const string WMV = ".wmv";
        public const string CAF = ".caf";
        public const string AIFC = ".aifc";
        public const string OGG = ".ogg";

        //unity
        public const string BUNDLE = ".abundle";
        public const string UNITY = ".unity3d";
        public const string PREFAB = ".prefab";
        public const string ASSET = ".asset";
    }

    //资源位置
    public enum ResLocation
    {
        //-无效
        INVALID = 0,
        //Resources目录
        RSS = 1,
        //StreamingAssets目录
        STREAM_ASSETS = 2,
        //网络
        WEB = 3,

    }


   



    /// <summary>
    /// 资源id
    /// </summary>
    public class CC_RES_ID
    {
        //无效
        public const string INVALID = "invalid";
        //
        public const string RSS = "rss";
        //音效
        public const string AUDIO = "audio";
        //配置表
        public const string CONFIG = "config";


        //KUI
        public const string KUI_PREFEB_UI = "kui_prefeb_ui";


        //资源注册
        public static ResCfgVo[] RES_REG = new ResCfgVo[]
        {

            new ResCfgVo(CC_RES_ID.RSS, "", ResSuffix.NONE, ResType.OBJECT, ResLocation.RSS  ),     //其实已经不需要ResType与ResLocation了
            //配置表
            new ResCfgVo(CC_RES_ID.CONFIG,  "Config/", ResSuffix.BYTES, ResType.BINARY, ResLocation.RSS),
            //音效
            new ResCfgVo(CC_RES_ID.AUDIO,  "Audio/", ResSuffix.NONE, ResType.SOUND, ResLocation.RSS),
            
            //KUI
            new ResCfgVo(CC_RES_ID.KUI_PREFEB_UI,  "GUI/cn/Prefab/", ResSuffix.NONE, ResType.PREFAB, ResLocation.RSS),

        };

    }

    /// <summary>
    /// 事件
    /// </summary>
    public class RES_EVT
    {

        //加载完成
        public const string LOAD_COMPLETE = "LOAD_COMPLETE";
        //加载异常
        public const string LOAD_EXCEPTION = "LOAD_EXCEPTION";

        //场景加载完成
        public const string LOAD_LEVEL_COMPLETE = "LOAD_LEVEL_COMPLETE";
        public const string LOAD_LEVEL_EXCEPTION = "LOAD_LEVEL_EXCEPTION";
    }



}