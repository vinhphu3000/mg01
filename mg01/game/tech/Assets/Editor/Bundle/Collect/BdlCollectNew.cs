/* ==============================================================================
 * BdlCollectNew
 * @author jr.zeng
 * 2018/1/23 14:23:22
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


using mg.org;

namespace Edit.Bundle
{

    using PathInfo = BundleGenInfo.PathInfo;

    public class BdlCollectNew : BdlCollect
    {
        public BdlCollectNew()
        {

        }

        public override void Collect(BundleGenInfo genInfo_)
        {
            
            string bundleName = genInfo_.bundleName;

            HashSet<string> topFileHash = new HashSet<string>();

            foreach (PathInfo pathInfo in genInfo_.pathInfos)
            {
                //顶级文件列表
                List<string> topFiles = new List<string>(); 
                //递归收集所有文件
                BundleUtility.CollectPathWithSuffix(pathInfo.path, pathInfo.suffixs, topFiles, pathInfo.exclude);
                //按名称排序
                topFiles.Sort();

                int bundleId = 0;
                int curIdx = 0;
                while(curIdx < topFiles.Count)
                {
                    string name = bundleName + "_" + bundleId;  //gui_0
                    AssetGroup asset = new AssetGroup();
                    asset.name = name;
                    asset.isPatch = false;
                    asset.isTopFile = true;

                    //合并
                    for (int i = 0; i < pathInfo.combineNum && curIdx < topFiles.Count; ++i, ++curIdx)
                    {
                        asset.files.Add(topFiles[curIdx]);
                        topFileHash.Add(topFiles[curIdx]);

                        BundleBuilder.AddAsset(topFiles[curIdx], asset);    //关联文件路径与BundleAsset
                    }

                    bundleId++;
                    genInfo_.assets.Add(asset);
                }
            }


            //收集依赖
            var asset2dpPathSet = new Dictionary<AssetGroup, HashSet<string>>();    //A的dpPathSet与B的此时可能会有重复依赖项
            foreach (AssetGroup asset in genInfo_.assets)
            {
                var __dpPathSet = new HashSet<string>();
                asset2dpPathSet[asset] = __dpPathSet;

                foreach (string path in asset.files)
                {
                    BundleUtility.CollectDepends(path, __dpPathSet);
                }

                //排除顶级依赖
                List<string> excludes = new List<string>();
                foreach (string path in __dpPathSet)
                {
                    if (topFileHash.Contains(path))    //这个依赖是顶级文件
                    {
                        excludes.Add(path);
                        //如果有依赖项是顶级文件，便直接添加到asset的depends中， 这些文件不视为外部依赖
                        AssetGroup topAsset = BundleBuilder.CreateAsset(path);
                        if (!asset.dpAssets.Contains(topAsset))
                        {
                            asset.dpAssets.Add(topAsset);
                        }
                    }
                }

                foreach (string path in excludes)   //从pathHash移除掉
                {
                    __dpPathSet.Remove(path);
                }
            }
            
            // 统计一个dependPath被包含到多少个Asset里
            var dpPath2assetSet = new Dictionary<string, HashSet<AssetGroup>>();
            foreach (var kvp in asset2dpPathSet)
            {
                AssetGroup asset = kvp.Key;
                HashSet<string> __dpPathSet = kvp.Value;
                foreach (string dpPath in __dpPathSet)
                {
                    HashSet<AssetGroup> assetHash;
                    if (!dpPath2assetSet.TryGetValue(dpPath, out assetHash))
                    {
                        assetHash = new HashSet<AssetGroup>();
                        dpPath2assetSet[dpPath] = assetHash;
                    }
                    assetHash.Add(asset);         //dpPath -> {AssetGroup, ...}
                }
            }
            
            List<string> toDelPaths = new List<string>();

            //只被一个文件依赖, 和此文件一起打包
            foreach (var kvp in dpPath2assetSet)
            {
                string dpPath = kvp.Key;
                HashSet<AssetGroup> assetHash = kvp.Value;
                if (assetHash.Count <= 1) //排查出只有一个引用的依赖
                {
                    AssetGroup parentAsset = assetHash.First();
                    HashSet<string> __dpPathSet = asset2dpPathSet[parentAsset];
                    __dpPathSet.Remove(dpPath);      //从引用者的dpPathHash里面移除, 从而不属于公共依赖

                    toDelPaths.Add(dpPath);
                }
            }

            //dpPath2assetSet里只保留引用数>1的路径
            foreach (string path in toDelPaths)
            {
                dpPath2assetSet.Remove(path);
            }

            //对所有依赖进行排序
            List<string> mainDpPaths = new List<string>();
            mainDpPaths.AddRange(dpPath2assetSet.Keys);
            mainDpPaths.Sort( (r, l) =>
            {
                if(dpPath2assetSet[r].Count != dpPath2assetSet[l].Count )
                {
                    //优先按依赖数量排序
                    return dpPath2assetSet[l].Count - dpPath2assetSet[r].Count;
                }
                return l.CompareTo(r);
            });

            //依赖总表
            HashSet<string> mainDpPathSet = new HashSet<string>();
            foreach (string path in mainDpPaths)
            {
                mainDpPathSet.Add(path);
            }


            HashSet<string> tmpDpPathSet = new HashSet<string>();
            List<string> tmpDpPaths = new List<string>();

            int index = 0;
            while (index < mainDpPaths.Count)
            {
                if(!mainDpPathSet.Contains( mainDpPaths[index] ) )
                {
                    //已经处理过
                    index++;
                    continue;
                }

                //50个依赖文件->dpPaths
                int combineCnt = 50;    //一个包的依赖文件个数
                List<string> dpFiles = new List<string>();  
                while(dpFiles.Count < combineCnt && index < mainDpPaths.Count)
                {
                    string path = mainDpPaths[index];
                    if (mainDpPathSet.Contains(path))
                    {
                        //还没处理过
                        mainDpPathSet.Remove(path);
                        dpFiles.Add(path);
                    }
                    index++;
                }

                //此时dpFiles有50个依赖文件

                bool hasMore = true;
                while (hasMore)
                {
                    hasMore = false;

                    tmpDpPaths.Clear();
                    tmpDpPaths.AddRange(dpFiles);

                    tmpDpPathSet.Clear();
                    BundleUtility.CollectDepends(tmpDpPaths.ToArray(), tmpDpPathSet); //收集这个依赖列表的所有依赖项

                    foreach (string path in tmpDpPathSet)
                    {
                        if (mainDpPathSet.Contains(path))
                        {
                            //次依赖在总表里，收集到本次列表里
                            mainDpPathSet.Remove(path);
                            dpFiles.Add(path);

                            hasMore = true;
                        }
                    }


                    //查找剩下全部依赖
                    tmpDpPaths.Clear();
                    tmpDpPaths.AddRange(mainDpPathSet);
                    foreach (string path in tmpDpPaths)  
                    {
                        if (mainDpPathSet.Contains(path))
                        {
                            tmpDpPathSet.Clear();
                            BundleUtility.CollectDepends(tmpDpPaths.ToArray(), tmpDpPathSet); //收集剩下的全部依赖的所有依赖项

                            bool packed = false;
                            foreach (string dpPath in tmpDpPathSet)
                            {
                                if (dpFiles.Contains(dpPath) )
                                {
                                    //如果这些依赖在这次的收集列表里, 也收集进来
                                    dpFiles.Add(path);
                                    mainDpPathSet.Remove(path);
                                    
                                    hasMore = true;
                                    packed = true;
                                    break;
                                }
                            }

                            if (packed)
                            {
                                foreach (var dpPath in tmpDpPathSet)
                                {
                                    if (mainDpPathSet.Contains(dpPath))
                                    {
                                        mainDpPathSet.Remove(dpPath);
                                        //所有依赖里面在总表的都添加到这次的收集
                                        dpFiles.Add(dpPath);
                                    }
                                }
                            }
                        }
                    }
                }

                if (dpFiles.Count > 0)
                {


                }




            }


        }


    }

}