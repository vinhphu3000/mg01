/* ==============================================================================
 * 声音管理
 * @author jr.zeng
 * 2016/8/26 14:47:16
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class SoundMgr: CCModule
    {

        //绑定的gameobject
        GameObject m_bindObj = null;


        AudioSource m_audioBgm;
        AudioSource m_audioOneShot;//并行audio
        AudioSource m_audioUi;     //ui用阻断方式


        List<AudioSource> m_effAudioPool = new List<AudioSource>();


        string m_bgmPath;

        //开关
        bool m_totalOn = true;
        bool m_bgmOn = true;
        bool m_effOn = true;
        //音量
        float m_totalVolume = 1;
        float m_bgmVolume = 1;
        float m_effVolume = 1;

        public SoundMgr(GameObject go_)
        {
            m_bindObj = go_ ?? CCApp.root;

        }


        protected override void __Setup(params object[] params_)
        {
            m_audioBgm = m_bindObj.AddComponent<AudioSource>();
            m_audioOneShot = m_bindObj.AddComponent<AudioSource>();
            m_audioUi = m_bindObj.AddComponent<AudioSource>();

        }

        protected override void __Clear()
        {

            UnityEngine.Object.Destroy(m_audioBgm);
            UnityEngine.Object.Destroy(m_audioOneShot);
            UnityEngine.Object.Destroy(m_audioUi);

            m_bgmPath = null;
        }


        protected override void SetupEvent()
        {

        }

        protected override void ClearEvent()
        {

        }
        


        //-------∽-★-∽------∽-★-∽--------∽-★-∽声音开关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 总开关
        /// </summary>
        public bool totalOn
        {
            get { return m_totalOn; }
            set { SetTotalOn(value); }
        }

        /// <summary>
        /// 背景音乐开关
        /// </summary>
        public bool bgmOn
        {
            get { return m_bgmOn; }
            set { SetBgmOn(value); }
        }


        /// <summary>
        /// 音效开关
        /// </summary>
        public bool effOn
        {
            get { return m_effOn; }
            set { SetEffOn(value); }
        }


        //背景音乐实际上是否开启
        bool isBgmOnReally { get { return m_totalOn && m_bgmOn; } }


        //背景音乐实际上是否开启
        bool isEffOnReally { get { return m_totalOn && m_effOn; } }

        

        void SetTotalOn(bool b_, bool force_=false)
        {
            if (m_totalOn == b_ && !force_)
                return;
            m_totalOn = b_;

            SetBgmOn(m_bgmOn, true);
            SetEffOn(m_effOn, true);
        }
        

        void SetBgmOn(bool b_, bool force_ = false)
        {
            if (m_bgmOn == b_ && !force_)
                return;
            m_bgmOn = b_;

            if (isBgmOnReally)
            {
                if (m_bgmPath != null)
                {
                    PlayBgm(m_bgmPath);
                }
            }
            else
            {
                StopBgm();
            }
        }


        void SetEffOn(bool b_, bool force_ = false)
        {
            if (m_effOn == b_ && !force_)
                return;
            m_effOn = b_;

            if (!isEffOnReally)
            {
                StopAllEff();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽音量相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 总音量
        /// </summary>
        public float totalVolume
        {
            get { return m_totalVolume; }
            set { SetTotalVolume(value); }
        }

        /// <summary>
        /// 背景音量
        /// </summary>
        public float bgmVolume
        {
            get { return m_bgmVolume; }
            set { SetBgmVolume(value); }
        }

        /// <summary>
        /// 音效音量
        /// </summary>
        public float effVolume
        {
            get { return m_effVolume; }
            set { SetEffVolume(value); }
        }


        void SetTotalVolume(float value_, bool force_=false)
        {
            value_ = Mathf.Clamp(value_, 0, 1);
            if (m_totalVolume == value_ && !force_)
                return;
            m_totalVolume = value_;
            RefrshBgmVolume();
            RefrshEffVolume();
        }
      
        void SetBgmVolume(float value_, bool force_ = false)
        {
            value_ = Mathf.Clamp(value_, 0, 1);
            if (m_bgmVolume == value_ && !force_)
                return;
            m_bgmVolume = value_;
            RefrshBgmVolume();
        }
        
        void SetEffVolume(float value_, bool force_ = false)
        {
            if (m_effVolume == value_ && !force_)
                return;
            m_effVolume = value_;
            RefrshEffVolume();
        }


        void RefrshBgmVolume()
        {
            m_audioBgm.volume = m_bgmVolume * m_totalVolume;
        }


        void RefrshEffVolume()
        {
            m_audioUi.volume = m_effVolume * m_totalVolume;
            m_audioOneShot.volume = m_effVolume * m_totalVolume;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽BGM∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="path_"></param>
        public void PlayBgm(string path_)
        {
            m_bgmPath = path_;

            if (!isBgmOnReally)
                return;
            
            SoundCache.me.LoadSound(m_audioBgm, path_, SoundPlayType.BGM, 1);
            m_audioBgm.volume = m_bgmVolume * m_totalVolume;
        }

        public void StopBgm()
        {
            m_audioBgm.Stop();
            SoundCache.me.StopLoad(m_audioBgm);
        }

        public bool isBgmPlaying
        {
            get { return m_audioBgm.isPlaying; }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽音效∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="path_"></param>
        /// <param name="isLoop_"></param>
        /// <param name="volume_"></param>
        /// <param name="pinch_"></param>
        /// <returns></returns>
        public void PlayOneShot(string path_, float volume_ = 1)
        {
            if (!isEffOnReally)
                return;

            float volume = volume_ * m_totalVolume * m_effVolume;

            SoundCache.me.LoadSound(m_audioOneShot, path_, SoundPlayType.OneShot, volume);
        }

        /// <summary>
        /// 播放ui音效
        /// </summary>
        /// <param name="path_"></param>
        /// <param name="volume_"></param>
        public void PlayUi(string path_, float volume_ = 1)
        {
            if (!isEffOnReally)
                return;

            float volume = volume_ * m_totalVolume * m_effVolume;

            SoundCache.me.LoadSound(m_audioUi, path_, SoundPlayType.Once, volume);
        }


      
        /// <summary>
        /// 停止所有音效
        /// </summary>
        public void StopAllEff()
        {
            m_audioOneShot.Stop();
            m_audioOneShot.enabled = false; //调用playOneShot用这种方式停止
            m_audioOneShot.enabled = true;
            m_audioOneShot.clip = null;

            m_audioUi.Stop();
            m_audioUi.enabled = false;
            m_audioUi.enabled = true;
            m_audioUi.clip = null;

        }

        

    }

}