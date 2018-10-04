/* ==============================================================================
 * 动画接口类
 * @author jr.zeng
 * 2016/11/10 15:14:22
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{


    public interface IAni
    {

        void Play();
        void Stop();


        void GotoAndPlay(int frame_);
        void GotoAndStop(int frame_);

        bool IsPlaying { get; }

        int CurrentFrame { get; }
        int TotalFrame { get; }
    }


}