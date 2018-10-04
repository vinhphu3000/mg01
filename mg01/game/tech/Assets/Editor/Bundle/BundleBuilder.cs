/* ==============================================================================
 * BundleBuilder
 * @author jr.zeng
 * 2017/11/22 16:39:48
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using UnityEditor;


using Object = UnityEngine.Object;

using mg.org;
using mg.org.bundle;

namespace Edit.Bundle
{

    public class BundleBuilder
    {


        //支持的打包平台
        static HashSet<BuildTarget> BUILD_TARGETS = new HashSet<BuildTarget> {
            //window
            BuildTarget.StandaloneWindows64,
            //安卓
            BuildTarget.Android,
            //ios
            BuildTarget.iOS,
            //pc
            BuildTarget.StandaloneOSXUniversal,
        };

        const string PATH_ROOT = "./";
        //const string PATH_ROOT = "./../../lss_ab/tech";

        //打包bundle根目录
        public static readonly string PATH_BUNDLE_ROOT = Path.Combine(PATH_ROOT, "Assets/StreamingAssets/bundles");
        //打包patch根目录
        public static readonly string PATH_BUNDLE_PATCH = Path.Combine(PATH_ROOT, "patches");
        //打包配置excel所在的目录
        public static readonly string PATH_GEN_CFG = ".";
        //bundle配置生成目录
        public static readonly string PATH_RES_CFG = Path.Combine( PATH_ROOT, "Assets/StreamingAssets" );

        //打包目标
        static BuildTarget m_buildTarget;
        //
        static Dictionary<string, AssetGroup> m_path2asset;

        public BundleBuilder()
        {

        }


        [MenuItem("工具/打资源包/打包PC平台", false, 51)]
        //用默认方案打包
        static void BuildPC()
        {
            //BuildBundles(BuildTarget.StandaloneWindows64, true);
            Excute(BuildTarget.StandaloneWindows64, false);
        }

      

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 目前只用于顶级文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static AssetGroup CreateAsset(string filePath)
        {
            if (m_path2asset.ContainsKey(filePath))
                return m_path2asset[filePath];
            
            string name;
            string path;
            BundleUtility.AnalyzeBundlePath(filePath, out path, out name);  //path->Resources/GUI/cn/Prefab, name->Canvas_Account
            
            AssetGroup assetInfo = new AssetGroup();
            // Assets/Resources/GUI/cn/Prefab/Canvas_Account.prefab -> Resources/GUI/cn/Prefab/Canvas_Account
            assetInfo.name = BundleUtility.Utf8toAscii(String.Format("{0}/{1}", path, name));   //为什么要转为asc码？ 
            assetInfo.isPatch = false;
            assetInfo.files.Add(filePath);

            m_path2asset[filePath] = assetInfo;

            return assetInfo;
        }


        public static void AddAsset(string filePath, AssetGroup asset_)
        {
            //if (m_path2asset.ContainsKey(filePath))
            //{
            //    Debug.Assert(asset_ != m_path2asset[filePath]);
            //    return;
            //}
            m_path2asset[filePath] = asset_;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽公共打包项∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static AssetGroup GetShareAsset_Font()
        {
            AssetGroup assetInfo = new AssetGroup();
            assetInfo.name = "font"; //"font.j";
            assetInfo.bundleId = BUNDLE_ID.FONT;
            assetInfo.files = new List<string> {
                "Assets/Resources/GUI/cn/font/DroidSansFallback.ttf",
                "Assets/Resources/GUI/cn/font/FontMaterial.mat"
            };

            return assetInfo;
        }


        static AssetGroup GetShareAsset_Shader()
        {
            var allShaderFiles = new List<string>();
            string[] suffixs = new[] { ".shader" };
            BundleUtility.CollectPathWithSuffix("Assets/RawData/Shaders/", suffixs, allShaderFiles, null);
            BundleUtility.CollectPathWithSuffix("Assets/Resources/Start/", suffixs, allShaderFiles, null);
            BundleUtility.CollectPathWithSuffix("Assets/ShaderForGame/", suffixs, allShaderFiles, null);

            AssetGroup assetInfo = new AssetGroup();
            assetInfo.name = "shader";//"shader.j";
            assetInfo.bundleId = BUNDLE_ID.SHADER;
            assetInfo.files = allShaderFiles;
            
            return assetInfo;
        }


        static BundleGenInfo GetShareBundle()
        {
            AssetGroup[] assetInfos = new AssetGroup[]
            {
                GetShareAsset_Font(),
                GetShareAsset_Shader(),
            };

            BundleGenInfo genInfo = new BundleGenInfo();
            genInfo.bundleName = "share";
            genInfo.combineNum = 1;
            genInfo.assets.AddRange(assetInfos);

            return genInfo;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Tools∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static BuildAssetBundleOptions CurrentBuildAssetOpts
        {
            get { return BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle; }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽打包操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public static void Excute(BuildTarget target_, bool needBuild_)
        {

            StopWatch.StartST();

            InitBundleEnv(target_);

            m_path2asset = new Dictionary<string, AssetGroup>();

            List<BundleGenInfo> genInfos = new List<BundleGenInfo>();

            //解析打包配置
            AbsConfigRead.ReadConfig(genInfos);
            //收集打包文件
            BdlCollect.Collect_s(genInfos);

            BuildBundles(genInfos, needBuild_);

            Debug.Log( String.Format("打包完成: {0}, 耗时: {1} s", target_, (StopWatch.StopST(false) / 1000).ToString("f2")) );
        }



        //初始化打包环境
        static void InitBundleEnv(BuildTarget target_)
        {
            if (!CheckPlatformEnable(target_))
                return;

            m_buildTarget = target_;
            
            //重置bundle输出路径
            FileUtility.RemoveDirectory(PATH_BUNDLE_ROOT);
            FileUtility.EnsureDirectory(PATH_BUNDLE_ROOT);

            //abs_res.json
            FileUtility.EnsureDirectory(PATH_RES_CFG);
        }


        //检查打包平台可用
        static bool CheckPlatformEnable(BuildTarget target_)
        {
            if (!BUILD_TARGETS.Contains(target_))
            {
                Debug.Assert(false, "不支持的打包平台: " + target_);
                return false;
            }
            
            return true;
        }
        

        //
        static void BuildBundles(List<BundleGenInfo> genInfos_, bool needBuild)
        {

            AssetBundleBuild[] shareBuilds = null;
            
            BundleGenInfo shareGenInfo = GetShareBundle();
            if (shareGenInfo != null)
            { 
                //公共打包项, 这里仅为拿到AssetBundleBuild列表
                shareBuilds = BuildBundle(shareGenInfo, null, false).ToArray(); 
            }

            foreach ( BundleGenInfo genInfo in genInfos_ )
            {
                BuildBundle(genInfo, shareBuilds, needBuild);
            }

            if(shareGenInfo != null)
            {
                //把公共项添加到总表
                genInfos_.Add(shareGenInfo);   
            }


            BundleLog.MakeLog(genInfos_);   //生成日志
            AbsResExport.Export(genInfos_); //生辰资源配置
        }

        /// <summary>
        /// 根据单个生成信息打包
        /// </summary>
        /// <param name="genInfo"></param>
        /// <param name="shareBuilds_">公共打包项</param>
        /// <param name="needBuild">是否需要build</param>
        /// <returns></returns>
        static List<AssetBundleBuild> BuildBundle(BundleGenInfo genInfo, AssetBundleBuild[] shareBuilds_, bool needBuild)
        {
            //先排个序
            genInfo.assets.Sort((l, r) =>
            {
                return l.name.CompareTo(r.name);
            });

            List<AssetBundleBuild> bundleBuilds = new List<AssetBundleBuild>();

            if (shareBuilds_ != null) //公共打包项
                bundleBuilds.AddRange(shareBuilds_);

            //建立依赖的BundleBuild
            foreach (var dpAsset in genInfo.dpAssets)
            {
                dpAsset.name = BundleUtility.GetBundleOutputName(dpAsset.name);     //重新定义包名

                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = dpAsset.name;
                build.assetNames = dpAsset.files.ToArray();      //每个依赖列表打成一个包
                bundleBuilds.Add(build);
            }

            int bdlIdx = 0;
            int combineNum = genInfo.combineNum;

            List<AssetGroup> newAssets = new List<AssetGroup>();
            List<AssetGroup> assetInfos = genInfo.assets;
            while (bdlIdx < assetInfos.Count)
            {
                AssetGroup assetInfo = assetInfos[bdlIdx];

                AssetBundleBuild build = new AssetBundleBuild();
                var dpAssetSet = new HashSet<AssetGroup>();

                //按分包数收集包含的所有files
                List<string> files = new List<string>();
                for (int i = 0; i < combineNum && bdlIdx < assetInfos.Count; ++i)
                {
                    files.AddRange(assetInfos[bdlIdx].files);    //添加文件路径

                    List<AssetGroup> dpAssets = assetInfos[bdlIdx].dpAssets;
                    foreach (AssetGroup dpAsset in dpAssets)
                    {
                        dpAssetSet.Add(dpAsset);
                    }
                    ++bdlIdx;
                }


                //重定义名称
                if (combineNum > 1 && !string.IsNullOrEmpty(genInfo.bundleName))   //需要分包
                {
                    var name = assetInfo.name.Replace("\\", "/");   //只取第一个asset的话,感觉会有问题, 如果里面掺杂了其他remainName,会单独打出一个包了
                    var lastIdx = name.LastIndexOf("/");

                    var remainPath = name.Substring(0, lastIdx);        //Resources\GUI\cn\Prefab
                    var remainName = Path.GetFileName(remainPath);      //Prefab  其实就是文件夹名称
                    remainPath = remainPath.Substring(0, remainPath.Length - remainName.Length - 1);    //Resources\GUI\cn
                    int idxSuffix = 0;
                    if (!genInfo.curCombineIdx.ContainsKey(remainName))     //这里有点繁琐,可以优化一下
                    {
                        genInfo.curCombineIdx[remainName] = 0;
                    }
                    idxSuffix = genInfo.curCombineIdx[remainName];
                    genInfo.curCombineIdx[remainName] = idxSuffix + 1;

                    assetInfo.name = BundleUtility.GetBundleOutputName(remainPath + "/" + remainName + "_" + idxSuffix.ToString());
                    build.assetBundleName = assetInfo.name;

                }
                else
                {
                    assetInfo.name = BundleUtility.GetBundleOutputName(assetInfo.name);   //格式化成输出用的名称
                    build.assetBundleName = assetInfo.name;
                }

                assetInfo.files = files;        //覆盖为新的文件列表
                newAssets.Add(assetInfo);       //添加到新的asset列表

                //覆盖为新的依赖列表
                List<AssetGroup> newDpAssets = new List<AssetGroup>();
                foreach (AssetGroup dpAsset in dpAssetSet)
                {
                    newDpAssets.Add(dpAsset);
                }
                assetInfo.dpAssets = newDpAssets;

                build.assetNames = files.ToArray();
                bundleBuilds.Add(build);
            }

            //覆盖为新的Asset列表
            genInfo.assets = newAssets;

            if (needBuild)
            {
                //执行打包
                AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(PATH_BUNDLE_ROOT, bundleBuilds.ToArray(), CurrentBuildAssetOpts, m_buildTarget);
                //根据manifest矫正一下依赖情况
                BundleUtility.ResetDependsByManifest(manifest, genInfo);
            }

            return bundleBuilds;
        }




    }

}