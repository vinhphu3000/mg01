/* ==============================================================================
 * BaseAni
 * @author jr.zeng
 * 2016/12/15 11:13:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{

    public class BaseAni : ImageAbs2D
    {
        //自动更新
        public bool isAutoSchedule = true;

        protected string m_resId;
        protected string m_resName;


        protected bool m_playing = false;

        protected int m_curFrame = 0;
        protected int m_totalFrame = 0;

        public BaseAni()
        {

        }

        public override void Show(object showObj_, params object[] params_)
        {
            string resId = (string)showObj_;
            string resName = (string)params_[0];
            ShowWithRes(resId, resName);
        }

        public void ShowWithRes(string resId_, string resName_)
        {
            if (m_resId == resId_ && m_resName == resName_)
                return;
            m_resId = resId_;
            ShowWithRes( resName_ );
        }

        public void ShowWithRes(string resName_)
        {
            if (m_resName == resName_)
                return;
            m_resName = resName_;

            LoadResource();
            ShowAnimation();

            SetupEvent();
            m_isOpen = true;

            Play();
        }

        //加载资源
        virtual protected void LoadResource()
        {
            
        }


        //显示动画
        virtual protected void ShowAnimation()
        {

        }

        virtual protected void ClearAnimation()
        {

        }


        virtual public void Play()
        {
            m_playing = true;
        }

        virtual public void Stop()
        {
            m_playing = false;
        }

        virtual public void GotoAndPlay(int frame_)
        {

        }

        virtual public void GotoAndStop(int frame_)
        {

        }

        public bool IsPlaying
        {
            get { return m_playing; }
        }

        virtual public int CurrentFrame
        {
            get { return m_curFrame; }
        }

        virtual public int TotalFrame
        {
            get { return m_totalFrame; }
        }


        protected override void __Destroy()
        {

            ClearAnimation();

            m_resName = null;
        }
    }

}