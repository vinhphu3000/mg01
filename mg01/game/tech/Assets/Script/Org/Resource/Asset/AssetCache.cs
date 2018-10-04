/* ==============================================================================
 * 资源缓存
 * @author jr.zeng
 * 2017/5/15 19:44:33
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace mg.org
{



    public class AssetCache : CCModule
    {

        static AssetCache __me;
        public static AssetCache me
        {
            get
            {
                if (__me != null)
                    return __me;

#if USE_BUNDLE
                __me = new bundle.AssetCacheBdl();
#else
                __me = new AssetCacheRss();
#endif

                return __me;
            }
        }


        //protected Type m_dataType = typeof(AssetData);
        //protected BasePool m_dataPool = new BasePool();

        Stack<HashSet<string>> m_hashPool = new Stack<HashSet<string>>();

        protected Dictionary<string, AssetData> m_url2data = new Dictionary<string, AssetData>();


        //引用者->url
        protected Dictionary<string, HashSet<string>> m_refer2urls = new Dictionary<string, HashSet<string>>();


        public AssetCache()
        {
            m_notifier = new Subject();   //独立观察者
        }


        override protected void __Setup(params object[] params_)
        {
            base.__Setup();

        }

        override protected void __Clear()
        {
            base.__Clear();

            ClearAllAssets();
        }

        protected override void SetupEvent()
        {
            base.SetupEvent();

            Refer.AttachDeactive(onReferDeactive); //监听所有的deactive
        }

        protected override void ClearEvent()
        {
            base.ClearEvent();

            Refer.DetachDeactive(onReferDeactive);

        }

        void onReferDeactive(object refer_)
        {
            ReleaseByRefer(refer_);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽资源加载∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        /// <returns></returns>
        public virtual Object LoadSync(string url_, object refer_)
        {
            url_ = url_.ToLower();
            return null;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        /// <param name="onComplete_"></param>
        /// <returns></returns>
        public virtual AssetData LoadAsync(string url_, CALLBACK_1 onComplete_, object refer_)
        {
            url_ = url_.ToLower();
            return null;
        }
        

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        /// <returns></returns>
        public virtual Object LoadSync_Level(string url_, object refer_)
        {
            url_ = url_.ToLower();
            return null;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        /// <param name="onComplete_"></param>
        /// <returns></returns>
        public virtual AssetData LoadAsync_Level(string url_, CALLBACK_1 onComplete_, object refer_)
        {
            url_ = url_.ToLower();
            return null;
        }

        /// <summary>
        /// 移除异步加载监听
        /// </summary>
        /// <param name="onComplete_"></param>
        public virtual void StopAsync(CALLBACK_1 onComplete_)
        {

        }

        /// <summary>
        /// 检测是否加载完毕
        /// </summary>
        /// <param name="url_"></param>
        /// <returns></returns>
        internal virtual bool __CheckLoaded(string url_)
        {
            return false;
        }

        /// <summary>
        /// 获取加载进度
        /// </summary>
        /// <param name="url_"></param>
        /// <returns></returns>
        internal virtual float __GetProgress(string url_)
        {
            AssetData data = GetData(url_);
            if (data == null)
                return 0;
            if (data.asset != null)
                return 1;
            return 0;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //删除一个data
        protected virtual void DeleteData(AssetData data_)
        {
            //m_dataPool.Push(data_);
        }

        //创建数据
        protected virtual AssetData CreateData(string url_)
        {
            url_ = url_.ToLower();

            AssetData data;
            if (m_url2data.TryGetValue(url_, out data))
                return data;

            data = new AssetData();
            data.Init(url_);
            data.active_time = DateUtil.TimeFromStart;

            m_url2data[url_] = data;
            return data;
        }


        //获取数据
        protected AssetData GetData(string url_)
        {
            url_ = url_.ToLower();
            if (!m_url2data.ContainsKey(url_))
                return null;
            return m_url2data[url_];
        }


        //卸载数据
        protected virtual void UnloadData(AssetData data_)
        {

            if (data_.RefCount > 0)
            {
                Log.Warn("要卸载的资源还在被引用: " + data_.url, this);
            }

            string url = data_.url;
            Object asset = data_.asset;

            data_.Clear();
            DeleteData(data_);

            if (asset)
            {
                //资源已加载
                UnloadAsset(url, asset);
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽资源管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="url_"></param>
        /// <param name="asset_"></param>
        public AssetData AddAsset(string url_, Object asset_)
        {
            Log.Assert(asset_ != null);

            url_ = url_.ToLower();
            AssetData data = CreateData(url_);
            if (data.asset)
            {
                if (data.asset != asset_)
                {
                    Log.Warn("重复添加资源: " + data.url, this);
                    UnloadAsset(data.url, asset_);  //卸载掉新资源
                }
                return data;
            }

            data.asset = asset_;
            data.active_time = DateUtil.TimeFromStart;

            //Object.Destroy(asset_); //Destroying assets is not permitted to avoid data loss.
            //Resources.UnloadAsset(asset_);

            return data;
        }

        /// <summary>
        /// 移除资源
        /// </summary>
        /// <param name="url_"></param>
        /// <returns></returns>
        public bool UnloadAsset(string url_)
        {
            url_ = url_.ToLower();
            if (!m_url2data.ContainsKey(url_))
                return false;

            AssetData data = m_url2data[url_];
            m_url2data.Remove(url_);

            UnloadData(data);   //卸载资源

            return true;
        }


        //卸载资源
        protected virtual void UnloadAsset(string url_, Object asset_)
        {
            if (asset_ is GameObject)
            {
                //prefab不能卸载
                return;
            }
            else if (asset_ is IAsset)
            {
                (asset_ as IAsset).Unload();
            }
            else
            {
                Resources.UnloadAsset(asset_);
            }

        }


        protected virtual void OnAssetLoaded(string url_, Object asset_)
        {
            if (asset_ is IAsset)
            {
                (asset_ as IAsset).OnLoaded();
            }
           
        }

        /// <summary>
        /// 卸载空闲资源
        /// </summary>
        public void UnloadAssetsUnused()
        {


        }

        /// <summary>
        /// 清空所有资源
        /// </summary>
        public void ClearAllAssets()
        {
            if (m_url2data.Count == 0)
                return;

            AssetData data;
            var enumerator = m_url2data.GetEnumerator();
            while (enumerator.MoveNext())
            {
                data = enumerator.Current.Value;
                UnloadData(data);   //卸载资源
            }
            enumerator.Dispose();

            m_url2data.Clear();
            m_refer2urls.Clear();

        }


        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="url_"></param>
        /// <returns></returns>
        public Object GetAsset(string url_)
        {
            url_ = url_.ToLower();
            if (!m_url2data.ContainsKey(url_))
                return null;

            AssetData data = m_url2data[url_];
            data.active_time = DateUtil.TimeFromStart;  //刷新时间
            return data.asset;
        }

        /// <summary>
        /// 资源已加载
        /// </summary>
        /// <param name="url_"></param>
        /// <returns></returns>
        public bool HasAsset(string url_)
        {
            url_ = url_.ToLower();
            if (!m_url2data.ContainsKey(url_))
                return false;
            return m_url2data[url_].asset != null;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽引用计数∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 持有资源引用
        /// </summary>
        /// <param name="url_"></param>
        /// <param name="refer_"></param>
        public void RetainByUrl(object refer_, string url_)
        {
            url_ = url_.ToLower();
            AssetData data = CreateData(url_);
            data.Retain(refer_);
            data.active_time = DateUtil.TimeFromStart;  //刷新时间

        }

        /// <summary>
        /// 释放引用的指定资源
        /// </summary>
        /// <param name="url_"></param>
        /// <param name="refer_"></param>
        public void ReleaseByUrl(object refer_, string url_)
        {
            url_ = url_.ToLower();
            if (!m_url2data.ContainsKey(url_))
                return;

            AssetData data = m_url2data[url_];
            if (data.Release(refer_))
            {
                data.active_time = DateUtil.TimeFromStart;  //刷新时间
            }

            //DetachBy_Type_Refer(url_, refer_)

        }

        /// <summary>
        /// 释放此引用者的所有引用
        /// </summary>
        /// <param name="referId_"></param>
        public void ReleaseByRefer(object refer_)
        {
            string referId = Refer.Format(refer_);
            if (referId == null)
                return;

            HashSet<string> hash;
            if (m_refer2urls.TryGetValue(referId, out hash))
            {
                foreach (string url in hash)
                {
                    AssetData data = m_url2data[url];
                    if (data.refHash.Remove(referId))
                    {
                        data.active_time = DateUtil.TimeFromStart;  //刷新时间
                    }
                }

                hash.Clear();
                m_hashPool.Push(hash);
                m_refer2urls.Remove(referId);
            }

            
            //DetachByRefer(refer_)
        }


        //-------∽-★-∽------∽-★-∽引用管理∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 添加引用记录
        /// </summary>
        /// <param name="referId_"></param>
        /// <param name="url_"></param>
        /// <returns></returns>
        internal bool __AddRefer(string referId_, string url_)
        {
            HashSet<string> hash;
            if (!m_refer2urls.TryGetValue(referId_, out hash))
            {
                hash = m_hashPool.Count > 0 ? m_hashPool.Pop() : new HashSet<string>();
                m_refer2urls[referId_] = hash;
            }

            return hash.Add(url_);
        }

        /// <summary>
        /// 移除引用记录
        /// </summary>
        /// <param name="referId_"></param>
        /// <param name="url_"></param>
        /// <returns></returns>
        internal bool __RemoveRefer(string referId_, string url_)
        {
            HashSet<string> hash;
            if (!m_refer2urls.TryGetValue(referId_, out hash))
                return false;

            if (hash.Remove(url_))
            {
                if (hash.Count == 0)
                {
                    m_hashPool.Push(hash);
                    m_refer2urls.Remove(referId_);
                }
                return true;
            }

            return false;
        }
        

        /// <summary>
        /// 移除指定资源的所有引用记录
        /// </summary>
        /// <param name="data_"></param>
        /// <returns></returns>
        internal bool __RemoveReferByData(AssetData data_)
        {
            if (data_.RefCount == 0)
                return false;

            HashSet<string> refHash = data_.refHash;
            foreach (var referId in refHash)
            {
                __RemoveRefer(referId, data_.url);
            }
            return true;
        }
        



    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽AssetData∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public class AssetData : IProgress, IRef
    {
        //public string type = "default";

        protected AssetCache m_cache;

        public HashSet<string> refHash = new HashSet<string>();
        //public Dictionary<string, string> refHash = new Dictionary<string, string>();

        public string url;
        //激活的时间
        public float active_time = 0;

        public Object asset;

        protected bool m_loading = false;
        

        public AssetData()
        {

        }

        public void Init(string url_)
        {
            url = url_;
            m_cache = AssetCache.me;
        }

        public virtual void Clear()
        {
            m_cache.__RemoveReferByData(this);
            refHash.Clear();

            url = null;
            asset = null;
            m_cache = null;
        }

        public float progress
        {
            get { return m_cache.__GetProgress(url); }
        }

        public bool isDone
        {
            get { return m_cache.__CheckLoaded(url); }
        }

        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽引用相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public bool Retain(object refer_)
        {
            string referId = Refer.Format(refer_);
            if (referId == null)
                return false;

            if (refHash.Add(referId))
            {
                m_cache.__AddRefer(referId, url);
                return true;
            }
            return false;
        }

        public bool Release(object refer_)
        {
            string referId = Refer.Format(refer_);
            if (referId == null)
                return false;

            if (refHash.Remove(referId))
            {
                m_cache.__RemoveRefer(referId, url);
                return true;
            }
            return false;
        }


        public int RefCount
        {
            get { return refHash.Count; }
        }

    }

}
