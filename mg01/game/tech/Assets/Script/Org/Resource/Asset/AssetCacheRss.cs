/* ==============================================================================
 * AssetCache_Rss
 * @author jr.zeng
 * 2017/5/25 16:31:31
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public class AssetCacheRss : AssetCache
    {

        protected Dictionary<string, ResourceRequest> m_url2req;

        public AssetCacheRss()
        {
            m_url2req = new Dictionary<string, ResourceRequest>();

        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽资源加载∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        /// <returns></returns>
        override public Object LoadSync(string url_, object refer_)
        {
            url_ = url_.ToLower();

            AssetData data = CreateData(url_);
            data.Retain(refer_);
            
            if (data.asset)
            {
                return data.asset;
            }

            Object asset = Resources.Load(data.url);
            if (asset == null)
            {
                Log.Assert("x 同步加载失败: " + data.url, this);
                return null;
            }

            AddAsset(url_, asset);
            Log.Info("<color=yellow>同步加载: " + data.url + "</color>", this);
            return asset;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="refer_"></param>
        /// <param name="url_"></param>
        /// <param name="onComplete_"></param>
        /// <returns></returns>
        override public AssetData LoadAsync(string url_, CALLBACK_1 onComplete_, object refer_)
        {
            url_ = url_.ToLower();

            AssetData data = CreateData(url_);   //先创建data, 用以记录生命周期等
            data.Retain(refer_);
            
            if (data.asset != null)
            {   
                //加载完成
                if (onComplete_ != null)
                    onComplete_(data);
                return data;
            }

            if (m_url2req.ContainsKey(data.url))
            {
                //加载中
                if (onComplete_ != null)
                    Attach(data.url, onComplete_, refer_);

                return data;
            }

            //未启动加载, 至少保持一个引用
            data.Retain(this);
            
            if (onComplete_ != null)
                Attach(data.url, onComplete_, refer_);

            CCApp.StartCoroutine(__LoadAsync(data.url));

            return data;
        }

      
        IEnumerator __LoadAsync(string url_)
        {
            ResourceRequest req;
            if (m_url2req.ContainsKey(url_))
            {
                Log.Assert("[LoadAsync] 错误的启动: " + url_, this);
                req = m_url2req[url_];
            }
            else
            {
                req = Resources.LoadAsync(url_);
            }

            m_url2req[url_] = req;
            Log.Info("☆ load async start: " + url_, this);

            while (!req.isDone)
            {
                yield return 0;
                //yield return new WaitForEndOfFrame();
            }

            if (req.asset)
            {
                OnAssetLoaded(url_, req.asset);
            }
            
            AssetData data = GetData(url_);
            if (data != null)
            {
                if (req.asset)
                {
                   
                    AddAsset(url_, req.asset);

                    Log.Info("★ load async success: " + url_, this);

                    Notify(url_, data);                     //这里实际上是调用完成回调
                    //Notify(RES_EVT.LOAD_COMPLETE, data);    //派发完成事件
                    LuaEvtCenter.AddEvent(RES_EVT.LOAD_COMPLETE, data);
                }
                else
                {
                    Log.Assert("x 异步加载失败: " + url_, this);
                    //Notify(url_, data);                     //加载失败了, 也会调用完成回调
                    // Notify(RES_EVT.LOAD_EXCEPTION, url_);   //派发异常事件
                    LuaEvtCenter.AddEvent(RES_EVT.LOAD_EXCEPTION, url_);
                }

                data.Release(this); //释放之前的引用
            }
            else
            {
                Log.Warn("加载完成但data已被卸载:" + url_, this);
                if (req.asset)
                {
                    UnloadAsset(url_, req.asset);
                }
                //Notify(RES_EVT.LOAD_EXCEPTION, url_);   //派发异常事件
                LuaEvtCenter.AddEvent(RES_EVT.LOAD_EXCEPTION, url_);
            }

            m_url2req.Remove(url_);
            DetachByType(url_); //移除所有完成回调
        }

        public override void StopAsync(CALLBACK_1 onComplete_)
        {
            if (m_url2req.Count == 0)
                return;

            AssetData data;
            foreach (var kvp in m_url2req)
            {
                data = GetData(kvp.Key);
                if (data != null)
                    Detach(data.url, onComplete_);      //无差别全部detach一遍
            }
        }

        //检测加载完成
        override internal bool __CheckLoaded(string url_)
        {
            if (m_url2req.ContainsKey(url_))
            {
                return false;
            }
            else
            {
                if (HasAsset(url_))
                    return true;
            }

            return false;
        }

        //获取加载进度
        internal override float __GetProgress(string url_)
        {
            AssetData data = GetData(url_);
            if (data == null)
                return 0;

            if (m_url2req.ContainsKey(url_))
                return m_url2req[url_].progress;

            if (data.asset != null)
                return 1;
           
            return 0;
        }


    }
    
}
