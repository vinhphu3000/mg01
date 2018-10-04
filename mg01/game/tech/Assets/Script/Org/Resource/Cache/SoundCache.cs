/* ==============================================================================
 * SoundCache
 * @author jr.zeng
 * 2017/11/14 14:23:05
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{

    public enum SoundPlayType
    {
        //只播一次
        OneShot,
        //只播一次
        Once,
        //循环播放
        Loop,
        //背景音乐
        BGM,

    }


    public class SoundCache : CCModule
    {
        static SoundCache __me;
        public static SoundCache me
        {
            get
            {
                if (__me == null)
                    __me = new SoundCache();
                return __me;
            }
        }


        AssetCache m_assetCache = AssetCache.me;

        ClassPool2<LoadItem> m_itemPool = ClassPools.me.CreatePool<LoadItem>();

        List<LoadItem> m_itemList = new List<LoadItem>();

        public SoundCache()
        {

        }

        override protected void __Setup(params object[] params_)
        {


        }

        override protected void __Clear()
        {

            m_assetCache.ReleaseByRefer(this);
            m_itemList.Clear();
            m_itemPool.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 播放声音(同步加载)
        /// </summary>
        /// <param name="audio_"></param>
        /// <param name="url_"></param>
        /// <param name="playType_"></param>
        /// <param name="volume_"></param>
        public void PlaySound(AudioSource audio_, string url_, SoundPlayType playType_, float volume_)
        {
            url_ = url_.ToLower();
            AudioClip clip = AssetCache.me.LoadSync(url_, this) as AudioClip;
            if (clip == null)
                return;

            switch(playType_)
            {
                case SoundPlayType.Once:
                    //只播放一次，且会打断当前的播放
                    audio_.clip = clip;
                    audio_.loop = false;
                    audio_.volume = volume_;
                    audio_.Play();

                    break;
                case SoundPlayType.Loop:

                    audio_.clip = clip;
                    audio_.loop = true;
                    audio_.volume = volume_;
                    audio_.Play();

                    break;
                case SoundPlayType.BGM:

                    audio_.clip = clip;
                    audio_.loop = true;
                    //audio_.volume = volume_;  //音量由调用者控制
                    audio_.Play();

                    break;
                default:
                    //OneShot，只播放一次，且不会打断当前的播放
                    audio_.PlayOneShot(clip, volume_);
                    
                    break;

            }
           
        }

        /// <summary>
        /// 加载并播放声音(异步加载)
        /// </summary>
        /// <param name="audio_"></param>
        /// <param name="url_"></param>
        /// <param name="playType_"></param>
        /// <param name="volume_"></param>
        public void LoadSound(AudioSource audio_, string url_, SoundPlayType playType_, float volume_ )
        {
            url_ = url_.ToLower();
            if (m_assetCache.HasAsset(url_))
            {
                //如果已经在加载队列 TODO

                PlaySound(audio_, url_, playType_, volume_);
                return;
            }
            
            AddLoadItem(audio_, url_, playType_, volume_);
            m_assetCache.LoadAsync(url_, OnLoaded, this);   //自己是引用者
        }


        public void StopLoad(AudioSource audio_)
        {
            if (m_itemList.Count == 0)
                return;

            LoadItem item;
            for (int i = m_itemList.Count - 1; i >= 0; --i)
            {
                item = m_itemList[i];
                if (item.audio == audio_)
                {
                    RemoveLoadItem(item);
                }
            }
        }


        void OnLoaded(object data_)
        {
            AssetData data = data_ as AssetData;

            if (m_itemList.Count > 0)
            {
                LoadItem item;
                for (int i = m_itemList.Count - 1; i >= 0; --i)
                {
                    item = m_itemList[i];
                    if (item.url == data.url)
                    {
                        AudioSource audio = item.audio;
                        string url = item.url;
                        SoundPlayType playType = item.playType;
                        float volume = item.volume;

                        RemoveLoadItem(item);

                        PlaySound(audio, url, playType, volume);
                    }
                }
            }

        }
        //-------∽-★-∽------∽-★-∽--------∽-★-∽LoadItem∽-★-∽--------∽-★-∽------∽-★-∽--------//

        LoadItem AddLoadItem(AudioSource audio_, string url_, SoundPlayType playType_, float volume_)
        {
            LoadItem item = m_itemPool.Pop();
            item.Init(audio_, url_, playType_, volume_);
            m_itemList.Add(item);
            return item;
        }
        
       
        void RemoveLoadItem(LoadItem item_)
        {
            item_.Clear();

            if (m_itemList.Remove(item_) )
                m_itemPool.Push(item_);
        }


        class LoadItem
        {
            public string url;
            public AudioSource audio;
            
            public SoundPlayType playType = 0;
            public float volume;

            public LoadItem() { }

            public void Init( AudioSource audio_, string url_, SoundPlayType playType_, float volume_)
            {
                audio = audio_;
                url = url_;
                playType = playType_;
                volume = volume_;
                
            }

            public void Clear()
            {
                url = null;
                audio = null;
                playType = 0;
                volume = 1;
            }

        }
    }

}