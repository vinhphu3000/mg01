/* ==============================================================================
 * BundleBuilderTest
 * @author jr.zeng
 * 2017/11/27 15:11:34
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
//using Edit.Bundle;

public class BundleBuilderTest
{

    static HashSet<string> _dpHashSet = new HashSet<string>();  //避免依赖重复

    //打包资源的路径
    static string bundleOutPath = Path.Combine(Application.dataPath, "StreamingAssets/bundles");

    public BundleBuilderTest()
    {

    }


    static BuildAssetBundleOptions CurrentBuildAssetOpts
    {
        get
        {
            return BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        }
    }


    [MenuItem("工具/打资源包/测试1", false, 1)]
    //用默认方案打包
    static void BuildAllAssetBundles()
    {
        FileUtility.EnsureDirectory(bundleOutPath);

        //要打包的资源
        string[] resPaths = new string[] {
                "Assets/Resources/GUI/cn/Prefab/Canvas_TestPop4.prefab",
                "Assets/Resources/GUI/cn/Prefab/Canvas_Shared.prefab",

            };

        //收集依赖
        //Dictionary<string, HashSet<string>> path2dpPathHash = new Dictionary<string, HashSet<string>>();  //把依赖按主文件分批收集

        HashSet<string> dpPathHash = new HashSet<string>(); //把全部依赖收集在一起

        string resPath;
        string[] dpPaths;
        for (int i = 0; i < resPaths.Length; ++i)
        {
            resPath = resPaths[i];
            dpPaths = CollectDepends(resPath, dpPathHash);

            Log.Debug(resPath);
            for (int j = 1; j < dpPaths.Length; ++j)
                Log.Debug("---- " + dpPaths[j]);

            //HashSet<string> pathHash;
            //if (!path2dpPathHash.TryGetValue(resPath, out pathHash))
            //{
            //    pathHash = new HashSet<string>();
            //    path2dpPathHash[resPath] = pathHash;
            //}
            //CollectDepends(resPath, pathHash);

        }


        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
        AssetBundleBuild build;
        for (int i = 0; i < resPaths.Length; ++i)
        {
            resPath = resPaths[i];

            //创建主包
            build = new AssetBundleBuild();
            build.assetBundleName = FileUtility.GetNameFromFullPath(resPath, ".j");
            build.assetNames = new string[] { resPath };    //打进这个包的文件列表

            buildList.Add(build);

            //HashSet<string> dpHash = path2dpPathHash[resPath];
            //if (dpHash.Count > 0)
            //{
            //    //创建依赖包
            //    AssetBundleBuild dpBuild = new AssetBundleBuild();
            //    dpBuild.assetBundleName = FileUtility.GetFileName(resPath, ".dp.j");
            //    dpBuild.assetNames = dpHash.ToArray();

            //    buildList.Add(dpBuild);
            //}

        }

        //依赖包
        AssetBundleBuild dpBuild = new AssetBundleBuild();
        dpBuild.assetBundleName = "depend.j";
        dpBuild.assetNames = dpPathHash.ToArray();
        buildList.Add(dpBuild);



        int a = 1;

        //打包资源
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(bundleOutPath, buildList.ToArray(), CurrentBuildAssetOpts, BuildTarget.StandaloneWindows);

        //string[] allAssetPath = manifest.GetAllAssetBundles();
        //foreach (string bundlePath in allAssetPath)
        //{
        //    string[] depends = manifest.GetDirectDependencies(bundlePath);
        //    foreach (string depPundlePath in depends)
        //    {

        //    }
        //}


        //刷新编辑器
        //AssetDatabase.Refresh();

        Log.Debug("打包完成");
    }


  

    static string[] CollectDepends(string path, HashSet<string> pathHash)
    {

        _dpHashSet.Clear();

        path = FileUtility.FomatPath(path);

        // ret.Add(path);

        List<string> dpPaths = new List<string>();

        Object[] objArr = new[] { AssetDatabase.LoadAssetAtPath<Object>(path) };
        Object[] dependsObjArr = EditorUtility.CollectDependencies(objArr);
        foreach (var obj in dependsObjArr)
        {
            string dpPath = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(dpPath))
            {
                dpPath = dpPath.Replace("\\", "/");
                if ( !_dpHashSet.Contains(dpPath) &&
                    (path.ToLower() != dpPath.ToLower()) &&
                    (!dpPath.EndsWith(".cs")) &&
                    (!dpPath.Contains("LightmapSnapshot")) &&
                    (!dpPath.Contains("unity_builtin_extra")) &&
                    (!dpPath.Contains("unity default")) &&
                    (!dpPath.Contains("UnityEngine")) &&
                    (!dpPath.Contains("LightingData.asset")) &&
                    (!dpPath.Contains("RawData/Shaders")) &&
                    (!dpPath.Contains("ShaderForGame")) &&
                    (!(dpPath.EndsWith(".shader") && dpPath.Contains("Resources/Start")))
                    )
                {
                    _dpHashSet.Add(dpPath);
                    pathHash.Add(dpPath);
                    dpPaths.Add(dpPath);
                }
            }
        }

        return dpPaths.ToArray();
    }

}