/* ==============================================================================
 * BundleUtil
 * @author jr.zeng
 * 2017/11/27 16:36:26
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using System.Runtime.InteropServices;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;


using mg.org;

namespace Edit.Bundle
{


    public class BundleUtility
    {


        public static string Utf8toAscii(string text)
        {
            return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(text)).Replace("?", "a");
        }


        /// <summary>
        /// 获取路径与文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static void AnalyzeBundlePath(string filePath, out string path, out string fileName)
        {
            var suffix = "Assets/".Length;
            var ext = Path.GetExtension(filePath);
            var rp = filePath.Substring(suffix, filePath.Length - suffix - ext.Length);
            rp = rp.Trim();
            var lastIdx = rp.LastIndexOf('/');
            fileName = rp.Substring(lastIdx + 1);
            path = rp.Substring(0, rp.LastIndexOf('/'));
        }

        /// <summary>
        /// 获取路径获取bundleName
        /// </summary>
        /// <param name="bundlePath"></param>
        /// <returns></returns>
        static string GetBdlNameByExportPath(string bundlePath)
        {
            bundlePath = bundlePath.Replace("\\", "/");
            var id = "bundles/";

            if (bundlePath.Contains(id))
                return bundlePath.Substring(bundlePath.IndexOf(id) + id.Length);        //bundles\resources\guiprefab_4.j -> resources\guiprefab_4.j
            id = "patches/";

            if (bundlePath.Contains(id))
                return bundlePath.Substring(bundlePath.IndexOf(id) + id.Length);
            return bundlePath;
        }

        /// <summary>
        /// 获取bundle实际输出的名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isPatch"></param>
        /// <returns></returns>
        public static string GetBundleOutputName(string path, bool isPatch = false)
        {
            path = path.ToLower();
            path = path.Replace("\\", "/");
            path = path.Replace(" ", "_");
            path = path.Replace("(", "_");
            path = path.Replace(")", "_");
            if (path.StartsWith("assets/"))
                path = path.Substring(7);
            if (path.StartsWith("resources/"))
                path = path.Substring(10);
            if (isPatch)
                return path.Replace("/", "_") + ".patch.j";
            return path.Replace("/", "_") + ".j";
        }
        

        /// <summary>
        /// 根据后缀收集文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suffix"></param>
        /// <param name="filelist"></param>
        /// <param name="dirExcluded"></param>
        public static void CollectPathWithSuffix(string path, string[] suffix, List<string> filelist, string[] dirExcluded)
        {
            path = FileUtility.FomatPath(path);

            if (Directory.Exists(path))     
            {
                //是文件夹
                var file = Directory.GetFiles(path);
                foreach (var f in file)
                {
                    CollectPathWithSuffix(f, suffix, filelist, dirExcluded);
                }
                var dirs = Directory.GetDirectories(path);  //查找子目录
                foreach (var dir in dirs)
                {
                    if (dirExcluded != null && dirExcluded.Length > 0)
                    {
                        var dirName = Path.GetFileNameWithoutExtension(dir);
                        if (Array.IndexOf(dirExcluded, dirName) >= 0)
                        {
                            continue;
                        }
                    }
                    CollectPathWithSuffix(dir, suffix, filelist, dirExcluded);
                }
            }
            else if (File.Exists(path))     //是文件
            {
                //path = path.ToLower();
                foreach (var s in suffix)
                {
                    if (path.ToLower().EndsWith(s)) //以后缀结尾
                    {
                        filelist.Add(path);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 收集依赖
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ret"></param>
        public static void CollectDepends(string path, HashSet<string> ret)
        {
            CollectDepends(new[] { path }, ret);
        }

        /// <summary>
        /// 收集依赖
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathHash"></param>
        public static void CollectDepends(string[] paths, HashSet<string> ret)
        {
            var op = new HashSet<string>();
            for (var i = 0; i < paths.Length; ++i)
            {
                op.Add(paths[i].Replace("\\", "/"));
            }
            var objs = new List<UnityEngine.Object>();
            foreach (var p in paths)
            {
                objs.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p));
            }
            var dd = EditorUtility.CollectDependencies(objs.ToArray());
            foreach (var d in dd)
            {
                var dp = AssetDatabase.GetAssetPath(d);
                if (!string.IsNullOrEmpty(dp))
                {
                    dp = dp.Replace("\\", "/");
                    if ((!ret.Contains(dp)) &&
                        (!op.Contains(dp)) &&
                        (!dp.EndsWith(".cs")) &&
                        (!dp.EndsWith(".ttf")) &&
                        (!dp.EndsWith(".dll")) &&
                        (!dp.Contains("LightmapSnapshot")) &&
                        (!dp.Contains("unity_builtin_extra")) &&
                        (!dp.Contains("unity default")) &&
                        (!dp.Contains("UnityEngine")) &&
                        (!dp.Contains("LightingData.asset")) &&
                        (!dp.Contains("RawData/Shaders")) &&
                        (!dp.Contains("ShaderForGame")) &&
                        (!(dp.EndsWith(".shader") && dp.Contains("Resources/Start"))))
                    {
                        ret.Add(dp);
                    }
                }
            }
        }


       


        /// <summary>
        /// 根据manifest重置单个GenInfo的依赖关系
        /// (主要是补全顶级文件依赖顶级文件这种情况)
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="genInfo_"></param>
        public static void ResetDependsByManifest(AssetBundleManifest manifest, BundleGenInfo genInfo_)
        {

            string[] bundlePaths = manifest.GetAllAssetBundles();

            foreach (string bundlePath in bundlePaths)
            {
                string bundleName = GetBdlNameByExportPath(bundlePath);

                foreach (AssetGroup assetInfo in genInfo_.assets)
                {
                    if (assetInfo.name.ToLower() != bundleName)
                        continue;
                    
                    List<AssetGroup> newDpAssets = new List<AssetGroup>();

                    string[] dpPaths = manifest.GetDirectDependencies(bundlePath);  //获取实际用到的依赖路径
                    foreach (var dpPath in dpPaths)
                    {
                        var dpBundleName = GetBdlNameByExportPath(dpPath);
                        bool areadyIn = false;
                        foreach (AssetGroup a in newDpAssets)
                        {
                            if (a.name.ToLower() == dpBundleName)
                            {
                                areadyIn = true;
                                break;
                            }
                        }
                        if (!areadyIn)
                        {
                            bool found = false;
                            foreach (AssetGroup assetInfo2 in genInfo_.assets)
                            {
                                if (assetInfo2.name.ToLower() == dpBundleName)
                                {
                                    found = true;
                                    newDpAssets.Add(assetInfo2);
                                }
                                foreach (AssetGroup dpAsset in assetInfo2.dpAssets)
                                {
                                    if (dpAsset.name.ToLower() == dpBundleName)
                                    {
                                        newDpAssets.Add(dpAsset);
                                        found = true;
                                    }
                                }
                                if (found)
                                    break;
                            }
                        }
                    }

                    List<AssetGroup> oldDpAssets = assetInfo.dpAssets;
                    assetInfo.dpAssets = newDpAssets;

                    //打印修正信息
                    //增加
                    foreach (AssetGroup a1 in newDpAssets)
                    {
                        if (!oldDpAssets.Contains(a1))
                            Log.Debug(string.Format("依赖修正: {0} {1} 增加: {2}", genInfo_.bundleName, assetInfo.name,  a1.name));
                    }
                    //删除
                    foreach (AssetGroup a1 in oldDpAssets)
                    {
                        if (!newDpAssets.Contains(a1))
                            Log.Debug(string.Format("依赖修正: {0} {1} 删除: {2}", genInfo_.bundleName, assetInfo.name, a1.name));
                    }

                }
            }
        }

    }



    /// <summary>
    /// 打包信息
    /// </summary>
    public partial class BundleGenInfo
    {
        //打包类型
        public string packType;
        // 多个相同 bundle_name会打成一个bundle
        public string bundleName;

        public int combineNum = 1;

        public List<PathInfo> pathInfos = new List<PathInfo>();
        
        /// <summary>
        /// 配置信息,对应xls里一行
        /// </summary>
        public class PathInfo
        {
            //ID
            public string id = "default";       //打包过程暂时没有用到
            //
            public string path;
            //分包数
            public int combineNum = 1;

            //需要打包的文件后缀
            public string[] suffixs = new string[0];
            //排除的文件
            public string[] exclude = new string[0];
            //(预留)
            public string[] dependsStr = new string[0];
        }


        //以上是config读入
        //资源组
        public List<AssetGroup> assets = new List<AssetGroup>();
        //依赖组
        public List<AssetGroup> dpAssets = new List<AssetGroup>();
        //
        public Dictionary<string, int> curCombineIdx = new Dictionary<string, int>();
        

    }

    
    public class AssetGroup
    {

        public string name;
        //是否顶级文件
        public bool isTopFile = false;
        //是否补丁
        public bool isPatch = false;
        //生成配置时用
        public string bundleId = null; 
        //包含的文件
        public List<string> files = new List<string>();
        //依赖组
        public List<AssetGroup> dpAssets = new List<AssetGroup>();
    }




}