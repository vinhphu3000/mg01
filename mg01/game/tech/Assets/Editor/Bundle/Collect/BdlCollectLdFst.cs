/* ==============================================================================
 * 资源包收集方案_加载效率高
 * @author jr.zeng
 * 2017/11/27 19:52:03
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;

namespace Edit.Bundle
{

    public class BdlCollectLdFst : BdlCollect
    {


        public BdlCollectLdFst()
        {

        }

        
        public override void Collect(BundleGenInfo genInfo_)
        {

            string bundleName = genInfo_.bundleName;

            //顶级文件列表
            List<string> topFiles = new List<string>();
            foreach(var pathInfo in genInfo_.pathInfos)
            {
                BundleUtility.CollectPathWithSuffix(pathInfo.path, pathInfo.suffixs, topFiles, pathInfo.exclude);    //递归收集所有文件
            }
            //按名称排序
            topFiles.Sort();


            foreach ( string path in topFiles )
            {
                AssetGroup assetInfo = BundleBuilder.CreateAsset(path);
                assetInfo.isTopFile = true;   //是顶级文件
                genInfo_.assets.Add(assetInfo);
            }

            //收集依赖
            var asset2dpPathSet = new Dictionary<AssetGroup, HashSet<string>>();
            foreach(AssetGroup asset in genInfo_.assets)
            {
                HashSet<string> dpPathSet = new HashSet<string>();
                asset2dpPathSet[asset] = dpPathSet;

                foreach (string filePath in asset.files)     //到这里的files应该只有一个文件，之后会添加
                {
                    BundleUtility.CollectDepends(filePath, dpPathSet);
                }

                //排除顶级依赖
                List<string> excludes = new List<string>();
                foreach (string path in dpPathSet)
                {
                    if (topFiles.Contains(path))    //这个依赖是顶级文件
                    {
                        excludes.Add(path);
                        //如果有依赖项是顶级文件，便直接添加到asset的depends中， 这些文件不视为外部依赖
                        asset.dpAssets.Add(BundleBuilder.CreateAsset(path));  
                    }
                }

                foreach (string path in excludes)   //从pathHash移除掉
                {
                    dpPathSet.Remove(path);
                }
            }

            // 统计一个dependPath被包含到多少个Asset里
            var dpPath2assetSet = new Dictionary<string, HashSet<AssetGroup> >();
            foreach (var kvp in asset2dpPathSet)
            {
                AssetGroup asset = kvp.Key;
                HashSet<string> dpPathSet = kvp.Value;
                foreach (string dpPath in dpPathSet)
                {
                    HashSet<AssetGroup> assetHash;
                    if (!dpPath2assetSet.TryGetValue(dpPath, out assetHash))
                    {
                        assetHash = new HashSet<AssetGroup>();
                        dpPath2assetSet[dpPath] = assetHash;
                    }
                    assetHash.Add(asset);         //dpPath -> {assetInfo, ...}
                }
            }
            
            List<string> toDelPath = new List<string>();

            // 只被一个文件依赖的 就和这个文件打包在一起，不再区分
            foreach (var kvp in dpPath2assetSet)      
            {
                string dpPath = kvp.Key;
                HashSet<AssetGroup> assetHash = kvp.Value;
                if (assetHash.Count <= 1) //排查出只有一个引用的依赖
                {
                    AssetGroup parentAsset = assetHash.First();
                    HashSet<string> dpPathSet = asset2dpPathSet[parentAsset];
                    dpPathSet.Remove(dpPath);      //从引用者的dpPathHash里面移除, 从而不属于公共依赖

                    toDelPath.Add(dpPath);
                }
            }

            //dpPath2assetSet里只保留引用数>1的路径
            foreach (string path in toDelPath)
            {
                dpPath2assetSet.Remove(path);
            }



            //建立无向图(所有依赖的连通图) { {A(a,b,c), B(a,d,e) } -> { a(b,c,d,e), b(c,a), c(a,b), d(a,e), e(a,d) }
            var path2node = new Dictionary<string, GraphNode>();
            foreach(var kvp in asset2dpPathSet)
            {
                AssetGroup asset = kvp.Key;
                HashSet<string> dpPathSet = kvp.Value;   //被同一个资源引用的依赖列表
                foreach ( string dpPath in dpPathSet) 
                {
                    GraphNode dpNode;
                    if (!path2node.TryGetValue(dpPath,out dpNode))  //为所有依赖创建节点
                    {
                        dpNode = new GraphNode();
                        dpNode.path = dpPath;
                        path2node[dpPath] = dpNode;
                    }
                    
                    foreach(string dpPath2 in dpPathSet )
                    {
                        if (dpPath2 == dpPath)
                            continue;
                        
                        GraphNode dpNode2;
                        if (path2node.TryGetValue(dpPath2, out dpNode2)) 
                        {
                            //互相把自己添加到对方的edges
                            dpNode2.edges.Add(dpNode);  
                            dpNode.edges.Add(dpNode2);
                        }
                    }
                }
            }

            //创建节点块(把所有连通的依赖独立出来一块) { (a,b,c,d,e), (...) }
            var nodeSets = new List< HashSet<GraphNode> >();
            foreach(var kvp in path2node)
            {
                GraphNode node = kvp.Value;
                if(!node.visited)
                {
                    HashSet<GraphNode> nodeSet = new HashSet<GraphNode>();
                    VisitNode(node, nodeSet);
                    nodeSets.Add(nodeSet);
                }
            }


            int idx = 0;
            foreach( var nodeSet in nodeSets)
            {
                AssetGroup dpAsset = new AssetGroup();
                string dpBdlName = Path.Combine("depends", string.Format("{0}_{1}", bundleName, idx));  //depends/gui_0
                dpAsset.name = FileUtility.FomatPath(dpBdlName);
                dpAsset.isTopFile = false;

                foreach (GraphNode node in nodeSet)
                {
                    dpAsset.files.Add(node.path);
                }


                //如果顶级文件的依赖项里有依赖这个块的其中一个文件，便把这个块的包作为顶级文件的依赖项
                foreach (var kvp in asset2dpPathSet)   
                {
                    AssetGroup asset = kvp.Key;
                    HashSet<string> dpPathSet = kvp.Value;

                    foreach (string dpPath in dpAsset.files)
                    {
                        if (dpPathSet.Contains(dpPath))  
                        {
                            asset.dpAssets.Add(dpAsset);
                            break;  
                        }
                    }
                }

                genInfo_.dpAssets.Add(dpAsset);      //添加到总依赖列表
                ++idx;
            }

        }

        //递归访问节点
        void VisitNode(GraphNode node, HashSet<GraphNode> nodeSet_)
        {
            nodeSet_.Add(node);
            node.visited = true;

            foreach (var e in node.edges)
            {
                if (!e.visited)
                {
                    VisitNode(e, nodeSet_);
                }
            }
        }

        class GraphNode
        {
            public string path;
            public bool visited = false;

            public HashSet<GraphNode> edges = new HashSet<GraphNode>();
        }

    }

}