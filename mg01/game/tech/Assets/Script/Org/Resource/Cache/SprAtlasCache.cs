/* ==============================================================================
 * 图集缓存
 * @author jr.zeng
 * 2017/11/1 16:28:12
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace mg.org
{

    public class SprAtlasCache : CCModule
    {
        static SprAtlasCache __me;
        public static SprAtlasCache me
        {
            get
            {
                if (__me == null)
                    __me = new SprAtlasCache();
                return __me;
            }
        }
        
        AssetCache m_assetCache = AssetCache.me;
        
        ClassPool2<LoadItem> m_itemPool = ClassPools.me.CreatePool<LoadItem>();
        List<LoadItem> m_itemQueue = new List<LoadItem>();
        Dictionary<Image, LoadItem> m_image2item = new Dictionary<Image, LoadItem>();

        Dictionary<string, HashSet<Image> > m_ref2imgHash = new Dictionary<string, HashSet<Image> >();
        Dictionary<Image, string> m_image2ref = new Dictionary<Image, string>();

        public SprAtlasCache()
        {

        }

        override protected void __Setup(params object[] params_)
        {


        }

        override protected void __Clear()
        {
            //m_assetCache.Release(this);   //自己不持有引用
            m_itemQueue.Clear();
            m_itemPool.Clear();
            m_image2item.Clear();

            m_image2ref.Clear();
            m_ref2imgHash.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public static string FormatPath(string url_)
        {
            string url = string.Format("SpriteAtlas/{0}/{1}", url_, url_);
            url = url.ToLower();
            return url;
        }

       

        /// <summary>
        /// 释放图集引用
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        public void ReleaseSprite(  string url_, object refer_)
        {
            url_ = FormatPath(url_);
            m_assetCache.ReleaseByUrl(refer_, url_);
        }

        /// <summary>
        /// 卸载图集
        /// </summary>
        /// <param name="url_"></param>
        public void UnloadSprite(string url_)
        {
            url_ = FormatPath(url_);
            m_assetCache.UnloadAsset(url_);
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="image_"></param>
        /// <param name="url_"></param>
        /// <param name="spriteName"></param>
        /// <param name="refer_"></param>
        /// <param name="nativeSize_">重置图片尺寸</param>
        public void LoadSprite(object refer_, Image image_, string url_ , string spriteName, bool nativeSize_=false)
        {
            string real_url_ = FormatPath(url_);

            if (m_assetCache.HasAsset(real_url_) )
            {
                //资源已加载
                SetSprite(refer_, image_, url_, spriteName, nativeSize_);
                return;
            }

            Refer.Assert(refer_);
            string referId = Refer.Format(refer_);
            RetainImage(image_, referId);

            image_.sprite = null;   //设置为透明图片

            AddLoadItem(referId, image_,  real_url_, spriteName, nativeSize_);

            m_assetCache.LoadAsync(real_url_, OnLoaded, refer_); //用refer_作引用者,直到它deactive才释放

            //(这里有bug, 如果先来异步再来同步，异步回来会顶掉正确的那个)
        }

        /// <summary>
        /// 设置图片(同步加载)
        /// </summary>
        /// <param name="image_">是否需要保持引用,等go失效时设置为移除引用？</param>
        /// <param name="url_"></param>
        /// <param name="spriteName"></param>
        /// <param name="refer_"></param>
        /// <param name="nativeSize_">重置尺寸</param>
        public void SetSprite(object refer_, Image image_, string url_, string spriteName,  bool nativeSize_=false)
        {
            string real_url_ = FormatPath(url_);

            if (m_itemQueue.Count > 0 && m_image2item.ContainsKey(image_))
            {
                //image在加载队列中,从队列移除
                LoadItem item = m_image2item[image_];
                RemoveLoadItem(item);
            }

            Refer.Assert(refer_);
            string referId = Refer.Format(refer_);
            RetainImage(image_, referId);

            __SetSprite(referId, image_, real_url_, spriteName, nativeSize_);
        }

        void __SetSprite(object refer_, Image image_, string real_url_, string spriteName, bool nativeSize_)
        {
            
            SprAtlas atlas = m_assetCache.LoadSync(real_url_, refer_) as SprAtlas;
            if (atlas == null)
            {
                //如果找不到资源，已经报错了，这里不需要报
                return;
            }

            if (string.IsNullOrEmpty(spriteName))
            {
                //传空时,取图集名称
                spriteName = atlas.file_name;
            }

            Sprite sprite = atlas.GetSprite(spriteName);
            if (sprite == null)
            {
                Log.Error("图集缺少图片：" + real_url_ + ", " + spriteName);
                return;
            }

            image_.sprite = null;
            image_.sprite = sprite;

            if (nativeSize_)
                image_.SetNativeSize();

            Vector4 border = sprite.border;
            if (border.x > 0 || border.y > 0 || border.z > 0 || border.w > 0)
            {
                //九宫格
                image_.type = Image.Type.Sliced;
            }
            else
            {
                image_.type = Image.Type.Simple;
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽引用相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        void RetainImage(Image image_, string referId_)
        {
            if (m_image2ref.ContainsKey(image_) )
            {
                if (m_image2ref[image_] == referId_)    //已经是这个引用者
                    return;
            }

            m_image2ref[image_] = referId_;

            HashSet<Image> imageHash;
            if (!m_ref2imgHash.TryGetValue(referId_, out imageHash))
            {
                imageHash = new HashSet<Image>();
                m_ref2imgHash[referId_] = imageHash;
                
                Refer.AttachDeactive(referId_, OnDeactive); //监听沉默
            }
            
            imageHash.Add(image_);
        }


        void ReleaseBy(string referId_)
        {
            if (!m_ref2imgHash.ContainsKey(referId_))
                return;
            
            HashSet<Image> imageHash = m_ref2imgHash[referId_];
            m_ref2imgHash.Remove(referId_);

            Image image;
            var enumerator = imageHash.GetEnumerator();
            while (enumerator.MoveNext())
            {
                image = enumerator.Current;
                image.sprite = null;

                m_image2ref.Remove(image);
            }

            if (m_itemQueue.Count > 0)
            {
                string referId = referId_ as string;
                for (int i = m_itemQueue.Count - 1; i >= 0; --i)
                {
                    if (m_itemQueue[i].referId == referId)
                    {
                        RemoveLoadItem(m_itemQueue[i]);     //移除加载项
                    }
                }
            }


            Refer.DetachDeactive(referId_, OnDeactive);
        }


        void OnDeactive(object referId_)
        {
            ReleaseBy((string)referId_);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void OnLoaded(object data_)
        {
            AssetData data = data_ as AssetData;

            if (m_itemQueue.Count > 0)
            {
                LoadItem item;
                for (int i = m_itemQueue.Count - 1; i >= 0; --i)
                {
                    item = m_itemQueue[i];
                    if (item.url == data.url)
                    {
                        __SetSprite(item.referId, item.image, item.url, item.spriteName, item.nativeSize);
                        RemoveLoadItem(item);
                    }
                }
            }

        }
        
        LoadItem AddLoadItem(string referId_,  Image image_,  string url_, string spriteName_,  bool nativeSize_)
        {
            LoadItem item = null;

            if (m_itemQueue.Count > 0 && m_image2item.ContainsKey(image_))
                item = m_image2item[image_];  //加载项已存在

            if (item == null)
            {
                item = m_itemPool.Pop();
                m_itemQueue.Add(item);
                m_image2item[image_] = item;
            }

            item.Init(referId_,  image_,  url_, spriteName_, nativeSize_);
            
            return item;
        }

        
        void RemoveLoadItem(LoadItem item_)
        {
           
            m_image2item.Remove(item_.image);

            item_.Clear();

            if (m_itemQueue.Remove(item_))
            {
                 m_itemPool.Push(item_);
            }
               
        }


        class LoadItem
        {
            public string url;
            public string spriteName;
            public Image image;
            public string referId;
            public bool nativeSize;

            public LoadItem() { }

            public void Init(string referId_,  Image image_,  string url_,  string spriteName_,  bool nativeSize_)
            {
                referId = referId_;
                image = image_;
                url = url_;
                spriteName = spriteName_;
                nativeSize = nativeSize_;
            }

            public void Clear()
            {
                url = null;
                spriteName = null;
                image = null;
                referId = null;
            }
            
        }




    }
}