/* ==============================================================================
 * UIConst
 * @author jr.zeng
 * 2016/10/12 17:59:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    public class UIConst
    {
        //ui容器默认名称
        static public string UI_CONR_NAME = "Container"; 

    }

    public class UI_NAME
    {
        static public string CONTAINER = "Container";
        static public string SPRITE = "Sprite";
        static public string TEXTURE = "Texture"; 

    }

    /// <summary>
    /// 镜头层
    /// </summary>
    public class CAMERA_LAYER
    {
        static public int Default = 0;
        static public int TransparentFX = 1;
        static public int IgnoreRaycast = 2;
        static public int Water = 4;
        static public int UI = 5;
    }


    /// <summary>
    /// 视图方向
    /// </summary>
    public enum CC_VIEW_DIR
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 视图方向
    /// </summary>
    public enum CC_LIST_SHOW_TP
    {
        //显示全部
        SHOW_ALL,
        //忽略空项
        IGNORE_NULL,
    }

    /// <summary>
    /// 水平方向
    /// </summary>
    public enum CC_DIR_X
    {
        RIGHT = 1,
        LEFT = -1,
    }


}