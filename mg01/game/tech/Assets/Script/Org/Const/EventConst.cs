/* ==============================================================================
 * CCEventConst
 * @author jr.zeng
 * 2016/7/11 11:09:35
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{
    //公共事件
    public class GL_EVENT
    {
        //进帧
        public const string ENTER_FRAME = "ENTER_FRAME";
        //
        public const string ENTER_FRAME_LATE = "ENTER_FRAME_LATE";
        //GUI进帧
        public const string ENTER_FRAME_GUI = "ENTER_FRAME_GUI";
        
        //通知refer沉默
        public const string REFER_DEACTIVE = "REFER_DEACTIVE";
        //通知refer析构
        public const string REFER_DISPOSE = "REFER_DEACTIVE";
    }

    //键盘事件
    public class KEY_EVENT
    {
        //按下
        public const string PRESS = "KEY_EVENT_PRESS";
        //弹起
        public const string RELEASE = "KEY_EVENT_RELEASE";

    }

    //鼠标事件
    public class MOUSE_EVENT
    {
        //鼠标按下
        public const string PRESS = "MOUSE_EVT_PRESS";
        //鼠标弹起
        public const string RELEASE = "MOUSE_EVT_RELEASE";
        //鼠标移动
        public const string MOVE = "MOUSE_EVT_MOVE";
        //转轮
        public const string WHEEL = "MOUSE_EVT_WHEEL";
    }

    //计时器事件
    public class TIMER_EVENT
    {
        //到达计时时间
        public const string TIMER = "TIMER_EVENT_TIMER";
        //计时完成
        public const string COMPLETE = "TIMER_EVENT_COMPLETE";
    }

    //倒计时时间
    public class COUNT_TIMER_EVENT
    {
        //添加倒计时
        public const string ADD = "COUNT_TIMER_EVENT_ADD";
        //添加倒计时
        public const string REMOVE = "COUNT_TIMER_EVENT_ADD";
        //运行中
        public const string RUNNING = "COUNT_TIMER_EVENT_RUNNING";
        //计时完成
        public const string COMPLETE = "COUNT_TIMER_EVENT_COMPLETE";
    }




}