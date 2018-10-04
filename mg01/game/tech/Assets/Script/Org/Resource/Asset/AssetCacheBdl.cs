/* ==============================================================================
 * AssetCache_Bdl
 * @author jr.zeng
 * 2017/5/25 16:41:30
 * ==============================================================================*/

#define UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.bundle
{  
    
    
    //特定的id
    public class BUNDLE_ID
    {
        public static readonly string FONT = "011";
        public static readonly string SHADER = "012";
    }
    

    public class AssetCacheBdl : AssetCache
    {
        

        //bundle路径
        public static readonly string BUNDLE_PATH = FileUtility.StreamAssetsPath("bundles");
        //补丁路径
        public static readonly string PATCH_PATH = FileUtility.WritablePath("patches");
        //资源配置路径
        public static readonly string RES_CFG_PATH = FileUtility.StreamAssetsPath("abs_res.json");
        
        

        public AssetCacheBdl()
        {

        }


        override protected void __Setup(params object[] params_)
        {
            base.__Setup();

            //Application.backgroundLoadingPriority
        }

        override protected void __Clear()
        {
            base.__Clear();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        protected override AssetData CreateData(string url_)
        {
            AssetData data;
            if (m_url2data.TryGetValue(url_, out data))
                return data as BundleData;
            
            AssetDataII data2 = new AssetDataII();
            data2.Init(url_);

            ResCfgInfo resCfg = AbsResConfig.GetResCfg(url_);
            BundleData dpData = CreateBdlData(resCfg.bundle.id);
            data2.SetDepend(dpData);

            m_url2data[url_] = data2;
            return data2;
        }

        //创建bundle数据
        BundleData CreateBdlData(string bundle_id)
        {
            BdlCfgInfo bdlCfg = AbsResConfig.GetBdlCfg(bundle_id);
            string url = bdlCfg.path;    //修正路径(已经是小写)
            //url = url.ToLower();

            AssetData data;
            if (m_url2data.TryGetValue(url, out data))
                return data as BundleData;

            BundleData bdlData = new BundleData();
            bdlData.Init(url);

            if (bdlCfg.depends != null)
            {
                //有依赖项
                BundleData dpData;
                for (int i = 0; i < bdlCfg.depends.Length; ++i)
                {
                    dpData = CreateBdlData(bdlCfg.depends[i].id);
                    bdlData.AddDepend(dpData);
                }
            }

            bdlData.active_time = DateUtil.TimeFromStart;

            m_url2data[url] = bdlData;

            return bdlData;
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

            Object asset = null;
            if (asset == null)
            {
                Log.Assert("x 同步加载失败: " + url_, this);
                return null;
            }

            AddAsset(url_, asset);
            Log.Info("<color=yellow>同步加载: " + url_ + "</color>", this);
            return asset;
        }



        override public AssetData LoadAsync(string url_, CALLBACK_1 onComplete_, object refer_)
        {
            url_ = url_.ToLower();

            AssetData data = CreateData(url_);   //先创建data, 用以记录生命周期等
            data.Retain(refer_);

            //if (m_url2req.ContainsKey(url_))
            //{
            //    //加载中
            //    if (onComplete_ != null)
            //        Attach(url_, onComplete_, refer_);

            //    return data;
            //}

            if (data.asset != null)
            { 
                //加载完成
                if (onComplete_ != null)
                    onComplete_(data);
                return data;
            }


            //未启动加载, 至少保持一个引用
            data.Retain(this);

            if (onComplete_ != null)
                Attach(url_, onComplete_, refer_);

            //CCApp.StartCoroutine(__LoadAsync(url_));

            return data;
        }

        public override void StopAsync(CALLBACK_1 onComplete_)
        {


        }



        //检测加载完成
        override internal bool __CheckLoaded(string url_)
        {
            //if (m_url2req.ContainsKey(url_))
            //{
            //    return false;
            //}
            //else
            //{
            //    if (HasAsset(url_))
            //        return true;
            //}

            return false;
        }


        //获取加载进度
        internal override float __GetProgress(string url_)
        {
            AssetData data = GetData(url_);
            if (data == null)
                return 0;

            //if (m_url2req.ContainsKey(url_))
            //    return m_url2req[url_].progress;

            if (data.asset != null)
                return 1;

            return 0;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Bundle加载相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        void LoadBundleAsync()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽AssetData∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public class BundleData : AssetData
        {
            
            public AssetBundle bundle;
            //依赖包列表
            List<BundleData> m_depends = new List<BundleData>();

            public BundleData()
            {

            }

            public override void Clear()
            {
                ClearDepends();
                base.Clear();
            }


            //-------∽-★-∽------∽depend相关∽------∽-★-∽--------//

            /// <summary>
            /// 添加依赖包
            /// </summary>
            /// <param name="data_"></param>
            public void AddDepend(BundleData data_)
            {
                if (m_depends.Contains(data_))
                    return;
                m_depends.Add(data_);
                data_.Retain(this);
            }

            //清空所有依赖包
            void ClearDepends()
            {
                if (m_depends.Count == 0)
                    return;

                foreach (BundleData data in m_depends)
                {
                    data.Release(this); //释放依赖包的引用
                }
                m_depends.Clear();
            }


        }


        public class AssetDataII : AssetData
        {

            BundleData m_depend;  //依赖包

            public AssetDataII()
            {

            }


            public override void Clear()
            {
                ClearDepend();
                base.Clear();
            }


            //-------∽-★-∽------∽depend相关∽------∽-★-∽--------//


            public void SetDepend(BundleData data_)
            {
                if (m_depend == data_)
                    return;
                ClearDepend();

                m_depend = data_;
                m_depend.Retain(this);
            }

            public void ClearDepend()
            {
                if (m_depend == null)
                    return;
                m_depend.Release(this);
            }


        }


    }
}
