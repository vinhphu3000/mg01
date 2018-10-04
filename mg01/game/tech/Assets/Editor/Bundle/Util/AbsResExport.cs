/* ==============================================================================
 * 资源信息输出
 * @author jr.zeng
 * 2017/12/2 17:03:53
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;
using mg.org;
using mg.org.bundle;


namespace Edit.Bundle
{

    public class AbsResExport
    {

        static string CFG_FILE_NAME = "abs_res.json";

        static int BUNDLE_IDX_BEGIN = 100;  //分配的起始id

        public AbsResExport()
        {

        }
        

        /// <summary>
        /// 还原资源路径
        /// </summary>
        /// <param name="path_"></param>
        /// <returns></returns>
        public static string RestoreResPath(string path_)
        {
            string resPath = path_.Replace("\\", "/");
            if (resPath.EndsWith(".unity")) //场景
            {
                return Path.GetFileNameWithoutExtension(resPath);
            }

            var resIdx = resPath.IndexOf("Resources");
            if (resIdx >= 0)
            {
                resPath = resPath.Substring(resIdx + 10);
                var ext = Path.GetExtension(resPath);
                return resPath.Substring(0, resPath.Length - ext.Length);       //去掉后缀
            }

            return resPath;
        }


        static public void Export( List<BundleGenInfo> genInfos_ )
        {

            //收集bundle节点
            var bdlNodes = new List<BdlNode>();

            
            HashSet<string> assetNameSet = new HashSet<string>();
            List<AssetGroup> assetInfos = new List<AssetGroup>();
            

            //收集assetInfo
            foreach (BundleGenInfo genInfo in genInfos_)
            {
                //收集顶级文件
                foreach (AssetGroup assetInfo in genInfo.assets)
                {
                    if (!assetNameSet.Contains(assetInfo.name))
                    {
                        assetNameSet.Add(assetInfo.name);
                        assetInfos.Add(assetInfo);
                    }
                }

                //收集依赖
                foreach (AssetGroup assetInfo in genInfo.assets)
                {
                    foreach (var dpAsset in assetInfo.dpAssets)
                    {
                        if (!assetNameSet.Contains(dpAsset.name))
                        {
                            assetNameSet.Add(dpAsset.name);
                            assetInfos.Add(dpAsset);
                        }
                    }
                }
            }

            //排序
            assetInfos.Sort((a, c) =>
            {
                if (a.isPatch && (!c.isPatch))
                    return 1;
                if ((!a.isPatch) && (c.isPatch))
                    return -1;

                return a.name.CompareTo(c.name);
            });

            //分配id
            int id = 0;
            for (int i = 0; i < assetInfos.Count; ++i)
            {
                if (assetInfos[i].bundleId == null)
                {
                    assetInfos[i].bundleId = (id + BUNDLE_IDX_BEGIN).ToString();
                    ++id;
                }
            }

            //再按id排一次序
            assetInfos.Sort((a, c) =>
            {
                return a.bundleId.CompareTo(c.bundleId);
            });

            BdlNode bdlNode;

            for (var i = 0; i < assetInfos.Count; ++i)
            {
                AssetGroup assetInfo = assetInfos[i];
                bdlNode = new BdlNode();
                bdlNode.bundleId = assetInfo.bundleId;
                bdlNode.bundleName = assetInfo.name.ToLower();

                List<string> dependIds = new List<string>();
                foreach (AssetGroup dpAsset in assetInfo.dpAssets)   //收集依赖包的id
                {
                    dependIds.Add(dpAsset.bundleId);
                }

                bdlNode.depends = dependIds.ToArray();

                bdlNodes.Add(bdlNode);
            }

            //收集资源节点
            var resNodes = new List<ResNode>();
            ResNode resNode;

            foreach (var assetInfo in assetInfos)
            {
                if (assetInfo.isTopFile)
                {
                    //是顶级文件
                    foreach (string path in assetInfo.files)
                    {
                        string resPath = RestoreResPath(path);  //还原为游戏里用到的路径(asset下)

                        if (!string.IsNullOrEmpty(resPath))
                        {
                            resNode = new ResNode();
                            resNode.path = resPath;
                            resNode.bundleId = assetInfo.bundleId;
                            resNode.suffix = Path.GetExtension(path);
                            resNodes.Add(resNode);

                            if (resNode.suffix.ToLower() != ".unity") // 除场景其他小写
                            {
                                resNode.path = resNode.path.ToLower();
                            }
                        }
                    }

                }
                   
            }


            var nodeMap = new BdlNodeMap();
            nodeMap.bdlNodes = bdlNodes.ToArray();
            nodeMap.resNodes = resNodes.ToArray();

            string exportPath = Path.Combine(BundleBuilder.PATH_RES_CFG, CFG_FILE_NAME);
            JsonUtil.WriteToJson(nodeMap, exportPath, true);

            Debug.Log("导出资源配置: " + exportPath);
        }



    }


}