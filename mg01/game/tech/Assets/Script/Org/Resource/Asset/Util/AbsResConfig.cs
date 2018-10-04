/* ==============================================================================
 * AbsResConfig
 * @author jr.zeng
 * 2017/12/5 11:27:10
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.bundle
{

    public class AbsResConfig
    {


        static Dictionary<string, BdlCfgInfo> m_id2bdl = new Dictionary<string, BdlCfgInfo>();
        static Dictionary<string, ResCfgInfo> m_path2res = new Dictionary<string, ResCfgInfo>();
        

        public AbsResConfig()
        {

        }

        /// <summary>
        /// 读入配置文件
        /// </summary>
        /// <param name="www_"></param>
        public static void ReadFromFile(WWW www_)
        {

            BdlNodeMap nodeMap = JsonUtility.FromJson<BdlNodeMap>( Encoding.UTF8.GetString(www_.bytes) );

            Dictionary<string, BdlNode> id2bdlNode = new Dictionary<string, BdlNode>();

            BdlCfgInfo bdlCfg;
            foreach(BdlNode bdlNode in nodeMap.bdlNodes)
            {
                bdlCfg = new BdlCfgInfo();
                bdlCfg.id = bdlNode.bundleId;
                
                m_id2bdl[bdlCfg.id] = bdlCfg;
                id2bdlNode[bdlNode.bundleId] = bdlNode;
            }
            

            foreach (var kvp in m_id2bdl)
            {
                bdlCfg = kvp.Value;
                BdlNode bdlNode = id2bdlNode[bdlCfg.id];

                //确定加载路径
                bdlCfg.path = bdlCfg.isPatch ? 
                    Path.Combine(AssetCacheBdl.PATCH_PATH, bdlNode.bundleName) :
                    Path.Combine(AssetCacheBdl.BUNDLE_PATH, bdlNode.bundleName);


                //关联依赖
                string[] dependIds = bdlNode.depends;
                if (dependIds.Length > 0)
                {
                    bdlCfg.depends = new BdlCfgInfo[dependIds.Length];
                    for (int i=0; i< dependIds.Length;++i)
                    {
                        bdlCfg.depends[i] = m_id2bdl[dependIds[i]];
                    }
                }
            }

            ResCfgInfo resCfg;
            foreach(ResNode resNode in nodeMap.resNodes)
            {
                resCfg = new ResCfgInfo();
                resCfg.path = resNode.path;
                resCfg.suffix = resNode.suffix;
                resCfg.bundle = m_id2bdl[resNode.bundleId];
            }



            
        }


        //public static void AddBdlCfg(BdlCfgInfo cfg_)
        //{
        //    m_id2bdl[cfg_.bundleId] = cfg_;
        //}

        /// <summary>
        /// 获取bundle配置
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public static BdlCfgInfo GetBdlCfg(string id_)
        {
            BdlCfgInfo bdlCfg;
            if (m_id2bdl.TryGetValue(id_, out bdlCfg) )
                return bdlCfg;
            return null;
        }


        public static bool HasBdlCfg(string id_)
        {
            return m_id2bdl.ContainsKey(id_);
        }



        /// <summary>
        /// 获取资源配置
        /// </summary>
        /// <param name="path_"></param>
        /// <returns></returns>
        public static ResCfgInfo GetResCfg(string path_)
        {
            ResCfgInfo resCfg;
            if (m_path2res.TryGetValue(path_, out resCfg))
                return resCfg;
            Log.Assert("找不到资源: " + path_, typeof(AbsResConfig));
            return null;
        }
        


    }

    /// <summary>
    /// bundle配置信息
    /// </summary>
    public class BdlCfgInfo
    {
        public string id;
        public string path;     //资源路径

        public bool isPatch = false;    //是否补丁

        public BdlCfgInfo[] depends;

    }

    /// <summary>
    /// 资源配置信息
    /// </summary>
    public class ResCfgInfo
    {
        public string path;     //资源路径
        public string suffix;   //后缀

        public BdlCfgInfo bundle;  //所在bundle
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽json节点∽-★-∽--------∽-★-∽------∽-★-∽--------//
    

    [Serializable]
    public class BdlNodeBase
    {
        public string bundleName;     //bundle名称
    }

    /// <summary>
    /// 单个bundle节点
    /// </summary>
    [Serializable]
    public class BdlNode : BdlNodeBase
    {
        public string bundleId;
        public string[] depends;    //依赖的id列表
    }

    /// <summary>
    /// 单个资源节点
    /// </summary>
    [Serializable]
    public class ResNode
    {
        public string path;     //资源路径
        public string suffix;   //后缀
        public string bundleId;
    }

    /// <summary>
    /// 节点总表
    /// </summary>
    [Serializable]
    public class BdlNodeMap
    {
        public BdlNode[] bdlNodes;
        public ResNode[] resNodes;
    }

   

}