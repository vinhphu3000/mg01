/* ==============================================================================
 * 动画常量
 * @author jr.zeng
 * 2016/11/18 16:18:33
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{

    /// <summary>
    /// 动画类型
    /// </summary>
    public enum AniTp
    {
        none = 0,
        //spine骨骼
        spine,
        //粒子
        particle,
    }



    /// <summary>
    /// 动画事件
    /// </summary>
    public class AniEvt
    {
        //进帧
        static public string ENTER_FRAME = "AniEvt_ENTER_FRAME";
        //帧事件
        static public string FRAME_EVENT = "AniEvt_FRAME_EVENT";
        //播放完毕
        static public string PLAY_COMPLETE = "AniEvt_PLAY_COMPLETE";
        //动画销毁
        static public string DESTROY = "AniEvt_DESTROY";
    }


}