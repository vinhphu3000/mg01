/* ==============================================================================
 * KUIConst
 * @author jr.zeng
 * 2017/6/21 17:51:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KUIConst
    {
        //设计分辨率
        public static Vector2 DESIGN_WIN_SIZE = new Vector2(1920, 1080);


        

    }


    public class KUI_EVT
    {
        //鼠标按下
        public const string POINTER_DOWN = "POINTER_DOWN";
        //鼠标弹起
        public const string POINTER_UP = "POINTER_UP";
        //鼠标进入
        public const string POINTER_ENTER = "POINTER_ENTER";
        //鼠标离开
        public const string POINTER_EXIT = "POINTER_EXIT";
        //鼠标移动
        public const string POINTER_MOVE = "POINTER_MOVE";
        //鼠标点击
        public const string POINTER_CLICK = "POINTER_CLICK";
        //数值改变
        public const string VALUE_CHANGE = "VALUE_CHANGE";
        //列表项数据改变
        public const string LIST_ITEM_DATA = "LIST_ITEM_DATA";
        //编辑完成
        public const string EDIT_END = "EDIT_END";
        //请求数值改变
        public const string REQ_VALUE_CHANGE = "REQ_VALUE_CHANGE";
    }



}