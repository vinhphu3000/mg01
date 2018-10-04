/* ==============================================================================
 * CCResConfig
 * @author jr.zeng
 * 2016/6/15 17:48:17
 * ==============================================================================*/

using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{
    public class ResConfig
    {
        static private Dictionary<string, ResCfgVo> m_id2cfg = new Dictionary<string, ResCfgVo>();

        static ResConfig()
        {

        }
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 添加资源配置列表
        /// </summary>
        /// <param name="arr_"></param>
        static public void AddResCfg(ResCfgVo[] arr_)
        {
            ResCfgVo cfg;
            int len = arr_.Length;
            for (int i = 0; i < len; ++i)
            {
                cfg = arr_[i];
                m_id2cfg[cfg.res_id] = cfg;
            }
        }



        /// <summary>
        /// 获取资源配置
        /// </summary>
        /// <param name="resId_"></param>
        /// <returns></returns>
        static public ResCfgVo GetResCfg(string resId_)
        {
            ResCfgVo vo = null;
            if (m_id2cfg.ContainsKey(resId_))
                vo = m_id2cfg[resId_];
            else
                Log.Assert("找不到资源配置:" + resId_);
            return vo;
        }


        /// <summary>
        /// 获取资源类型
        /// </summary>
        /// <param name="resId_"></param>
        /// <returns></returns>
        static public ResType GetResType(string resId_)
        {
            ResCfgVo vo = GetResCfg(resId_);
            if (vo == null)
                return ResType.INVALID;
            return vo.res_type;
        }

        static public string GetResPath(string resId_)
        {
            ResCfgVo vo = GetResCfg(resId_);
            if (vo == null)
                return "";
            return vo.path;
        }

        /// <summary>
        /// 获取资源位置
        /// </summary>
        /// <param name="resId_"></param>
        /// <returns></returns>
        static public ResLocation GetResLocation(string resId_)
        {
            if (m_id2cfg.ContainsKey(resId_))
                return m_id2cfg[resId_].location;
            return ResLocation.INVALID;
        }

        /// <summary>
        /// 获取资源路径
        /// </summary>
        /// <param name="resId_"></param>
        /// <param name="resName_"></param>
        /// <param name="suffix">额外指定后缀,不使用配置里的</param>
        /// <returns></returns>
        static public string GetResUrl(string resId_, string resName_ = null, string suffix_ = null)
        {

            ResCfgVo cfgVo = GetResCfg(resId_);

            string resName = resName_ ?? "";
            string suffix = suffix_ ?? cfgVo.suffix;

            string path = FileUtility.ModifyFileName(cfgVo.path + resName, suffix);

            //if (cfgVo.location == ResLocation.ASSETS)
            //{
            //    path = FileUtil.AssetsPath(path);
            //}

            return path;
        }
        

    }


    /// <summary>
    /// 资源配置信息
    /// </summary>
    public class ResCfgVo
    {
        //资源id
        public string res_id;
        public ResType res_type;
        //资源路径
        public string path;
        //资源后缀
        public string suffix;
        //资源位置
        public ResLocation location;

        public ResCfgVo(string res_id_, string path_, string suffix_, ResType resType_, ResLocation location_)
        {
            res_id = res_id_;
            location = location_;

            res_type = resType_;
            path = path_;
            suffix = suffix_;
        }
    }


}