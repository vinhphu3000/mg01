/* ==============================================================================
 * gameobject缓存
 * @author jr.zeng
 * 2017/6/6 15:23:33
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{

    public class GameObjCache : CCModule
    {
        static GameObjCache __me;
        public static GameObjCache me
        {
            get
            {
                if (__me == null)
                    __me = new GameObjCache();
                return __me;
            }
        }


        AssetCache m_assetCache = AssetCache.me;

        ClassPool2<LoadItem> m_itemPool = ClassPools.me.CreatePool<LoadItem>();
        
        Dictionary<string, List<GameObject>> m_url2goList = new Dictionary<string, List<GameObject>>();

        List<LoadItem> m_itemList = new List<LoadItem>();

        public GameObjCache()
        {
            m_notifier = new Subject();  //独立观察者
        }

        override protected void __Setup(params object[] params_)
        {
            base.__Setup();

        }

        override protected void __Clear()
        {
            m_assetCache.ReleaseByRefer(this);
            m_itemList.Clear();
            m_itemPool.Clear();
        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public GameObject LoadSync(string url_)
        {
            url_ = url_.ToLower();
            m_assetCache.LoadSync( url_, this);  //this也做引用者
            return CreateGo(url_, null);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="url_"></param>
        /// <param name="onComplete_"></param>
        /// <param name="refer_">只影响onComplete,不会用来持有资源</param>
        public void LoadAsync(string url_, CALLBACK_GO onComplete_, object refer_)
        {
            url_ = url_.ToLower();
            m_assetCache.RetainByUrl(this, url_);
            //m_assetCache.Retain(refer_, url_);    //引用者只用this

            if (m_assetCache.HasAsset(url_))
            {
                CreateGo(url_, onComplete_);
                return;
            }

            string referId = Refer.Format(refer_);
            AddLoadItem( url_, onComplete_, referId);

            m_assetCache.LoadAsync( url_, OnLoaded, this);  //已自己作为引用者,在ReleaseUnused时才有可能释放
        }
        

        void OnLoaded(object data_)
        {
            AssetData data = data_ as AssetData;

            if(m_itemList.Count > 0)
            {
                LoadItem item;
                for (int i = m_itemList.Count - 1; i >= 0; --i)
                {
                    item = m_itemList[i];
                    if (item.url == data.url)
                    {
                        CreateGo(item.url, item.onComplete);
                        RemoveLoadItem(item);
                    }
                }
                
            }
            
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽gameobject管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public GameObject CreateGo(string url_, CALLBACK_GO onComplete_)
        {
            url_ = url_.ToLower();

            GameObject prefab = m_assetCache.GetAsset(url_) as GameObject;
            if (prefab == null)
            {
                Log.Assert("gameobject未加载:" + url_);
                return null;
            }

            GameObject go = GameObjUtil.CreateGameObj(prefab);

            GameObjUtil.DontDestroyOnLoad(go);  //所有加载出来的go都是默认不销毁

            AddGo(url_, go);

            if (onComplete_ != null)
                onComplete_(go);

            return go;
        }


        void AddGo(string url_, GameObject go_)
        {
            List<GameObject> list;
            if (!m_url2goList.TryGetValue(url_, out list))
            {
                list = new List<GameObject>();
                m_url2goList[url_] = list;
            }

            list.Add(go_);
        }

        /// <summary>
        /// 释放无引用的资源
        /// </summary>
        public void ReleaseUnused()
        {
            if (m_url2goList.Count == 0)
                return;

            List<string> delList = null;
            List<GameObject> goList;

            foreach (var kvp in m_url2goList)
            {
                goList = kvp.Value;

                for (int i = goList.Count - 1; i >= 0; --i)
                {
                    if (goList[i])
                    {
                    }
                    else
                    {
                        //已被destroy
                        goList.RemoveAt(i);
                    }
                }

                if (goList.Count == 0)
                {
                    //全部销毁了, 释放引用
                    if (delList == null)
                        delList = new List<string>();
                    delList.Add(kvp.Key);

                    AssetCache.me.ReleaseByUrl(this, kvp.Key);
                }
            }

            //字典删除方案
            if (delList != null)
            {
                DicUtil.RemoveByKeys<string, List<GameObject>>(m_url2goList, delList);
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽LoadItem∽-★-∽--------∽-★-∽------∽-★-∽--------//

        LoadItem AddLoadItem(string url_, CALLBACK_GO onComplete_, string referId_)
        {
            LoadItem item = m_itemPool.Pop();
            item.Init( url_, onComplete_, referId_);

            m_itemList.Add(item);

            if(referId_ != null)
            {
                Refer.AttachDeactive(referId_, onDeactive); //监听沉默
            }

            return item;
        }

        void onDeactive(object referId_)
        {
            if (m_itemList.Count == 0)
                return;

            string referId = referId_ as string;
            for (int i = m_itemList.Count - 1; i >=0; --i)
            {
                if(m_itemList[i].referId == referId)
                {
                    RemoveLoadItem(m_itemList[i]);
                }
            }
        }

        void RemoveLoadItem(LoadItem item_)
        {
            if(item_.referId != null)
            {
                Refer.DetachDeactive(item_.referId, onDeactive);
            }

            item_.Clear();

            m_itemList.Remove(item_);
            m_itemPool.Push(item_);
        }

        class LoadItem
        {
            public string url;
            public CALLBACK_GO onComplete;
            public string referId;

            public LoadItem() { }
            
            public void Init(string url_, CALLBACK_GO onComplete_, string referId_)
            {
                url = url_;
                onComplete = onComplete_;
                referId = referId_;
            }

            public void Clear()
            {
                url = null;
                onComplete = null;
                referId = null;
            }


        }

    }




}
