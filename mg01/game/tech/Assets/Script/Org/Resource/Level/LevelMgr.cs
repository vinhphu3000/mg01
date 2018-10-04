/* ==============================================================================
 * Level场景管理
 * @author jr.zeng
 * 2017/6/7 17:37:54
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class LevelMgr : CCModule
    {
        static LevelMgr __me;


        static string LEVEL_NAME_EMPTY = "SceneEmpty";

        public static LevelMgr me
        {
            get
            {
                if (__me != null)
                    return __me;
                __me = new LevelMgr();
                return __me;
            }
        }

        Dictionary<string, LevelData> m_url2data = new Dictionary<string, LevelData>();
        

        public LevelMgr()
        {
            m_notifier = new Subject();  //独立观察者
        }

        override protected void __Setup(params object[] params_)
        {
            base.__Setup();

        }

        override protected void __Clear()
        {
            base.__Clear();

            RemoveAllDatas();


        }

        //转场开始
        void TransSceneBegin()
        {
            RemoveAllDatas();   //转场,清除当前场景数据
            
        }

        //转场完成
        void TransSceneFinsh()
        {
            //释放资源

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void LoadSync(string url_, bool isAdditive)
        {
            url_ = url_.ToLower();
            LevelData data = GetData(url_);
            if (data != null)
            {
                if (data.isComplete)
                    return;
            }
            
            if (!isAdditive)
                TransSceneBegin();


            AssetCache.me.LoadSync_Level(url_, this);   //加载资源

            LoadSceneMode mode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            SceneManager.LoadScene(url_, mode);

            data = CreateData(url_, isAdditive);
            if(!data.isLoading)
            {
                data.isComplete = true;
            }

            if (!isAdditive)
                TransSceneFinsh();
        }



        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="url_"></param>
        /// <param name="isAdditive"></param>
        /// <param name="onComplete_"></param>
        /// <param name="refer_">只用于onComplete_</param>
        /// <returns></returns>
        public LevelData LoadAsync(string url_, bool isAdditive, CALLBACK_1 onComplete_ , object refer_)
        {
            url_ = url_.ToLower();
            LevelData data = GetData(url_);
            if(data != null)
            {
                if (data.isLoading)
                {
                    if (onComplete_ != null)
                        Attach(url_, onComplete_, refer_);
                    return data;
                }

                if(data.isComplete)
                {
                    if (onComplete_ != null)
                        onComplete_(data);
                    return data;
                }
            }


            if (!isAdditive)
                TransSceneBegin();   //转场

            if (onComplete_ != null)
                Attach(url_, onComplete_, refer_);
            
            data = CreateData(url_, isAdditive);
            data.isLoading = true;
            data.isComplete = false;
            data.enumerator = __LoadAsync(data);

            CCApp.StartCoroutine(data.enumerator);

            return m_url2data[url_];
        }

        IEnumerator __LoadAsync(LevelData data)
        {
            string url_ = data.url;
            bool isAdditive = data.isAdditive;
            LoadSceneMode mode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;

            AsyncOperation op;

            float progress = 0f;
            float weight = 0.1f;

            if (!isAdditive)
            {
                op = SceneManager.LoadSceneAsync(LEVEL_NAME_EMPTY, mode);

                while (!op.isDone)
                {
                    data._progress = progress + weight * op.progress;
                    yield return null;
                }
                //Log.Debug("空场景加载完成：" + LEVEL_NAME_EMPTY, this);
                yield return new WaitForSeconds(0.1f);  //等一会儿
            }

            progress += weight;
            weight = 0.2f;

            AssetData assetData = AssetCache.me.LoadAsync_Level(url_, null, this);      //先加载此场景所需的资源
            if (assetData != null)
            {
                while (!assetData.isDone)
                {
                    data._progress = progress + weight * assetData.progress;
                    yield return null;
                }
            }

            progress += weight;
            weight = 0.7f;

            op = SceneManager.LoadSceneAsync(data.url, mode);
            if (op == null)
            {
                //Log.Assert("x level load fail: " + url_, this);

                CCApp.StopCoroutine(data.enumerator);
                data.enumerator = null;

                LuaEvtCenter.AddEvent(RES_EVT.LOAD_LEVEL_EXCEPTION, data.url);
                yield return 0;
            }


            Log.Info("☆ load level async start: " + url_, this);
            
            while (!op.isDone)
            {
                data._progress = progress + weight * op.progress;
                yield return null;
            }

            Log.Info("★ load level async success: " + url_, this);

            LevelData data2 = GetData(url_);
            if(data2 != null)
            {
                if(data == data2)
                {
                    data.__OnComplete();

                    if (!isAdditive)
                        TransSceneFinsh();

                    Notify(url_, data);
                    LuaEvtCenter.AddEvent(RES_EVT.LOAD_LEVEL_COMPLETE, data);

                }
                else
                {
                    //启动了异步, 同时又启动同步
                    data.__OnComplete();

                    Notify(url_, data);
                    LuaEvtCenter.AddEvent(RES_EVT.LOAD_LEVEL_COMPLETE, data);
                }
            }
            else
            {
                //已销毁
            }

            data.enumerator = null;

            DetachByType(url_);

        }

        public bool IsLevelLoaded(string url_)
        {
            url_ = url_.ToLower();
            if (!m_url2data.ContainsKey(url_))
                return false;
            return m_url2data[url_].isDone;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽data管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        LevelData CreateData(string url_, bool isAdditive_)
        {
            LevelData data;
            if (m_url2data.TryGetValue(url_, out data))
                return data;

            data = new LevelData();
            data.Init(url_, isAdditive_);

            m_url2data[url_] = data;
            return data;
        }
        

        LevelData GetData(string url_)
        {
            if (!m_url2data.ContainsKey(url_))
                return null;
            return m_url2data[url_];
        }


        void RemoveAllDatas()
        {
            if (m_url2data.Count == 0)
                return;

            foreach (var v in m_url2data)
            {
                v.Value.Clear();
            }

            m_url2data.Clear();
            AssetCache.me.ReleaseByRefer(this);
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽LevelData∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        public class LevelData : IProgress
        {

            public string url;
            public bool isAdditive;


            public bool isLoading = false;
            public bool isComplete = false;

            internal float _progress = 0;

            public IEnumerator enumerator;


            public LevelData()
            {

            }

            public void Init(string url_, bool isAdditive_)
            {
                url = url_;
                isAdditive = isAdditive_;
            }

            public void Clear()
            {

            }

            public float progress
            {
                get
                {
                    if (isComplete)
                        return 1;

                    if (isLoading)
                    {
                        return _progress;
                    }

                    return 0;
                }
            }

            public bool isDone
            {
                get { return isComplete; }
            }

            //-------∽-★-∽------∽-★-∽--------∽-★-∽加载回调∽-★-∽--------∽-★-∽------∽-★-∽--------//

            public void __OnComplete()
            {
               

                isComplete = true;
                isLoading = false;
            }


        }

    }

   
}
