/* ==============================================================================
 * BundleCollect_Default
 * @author jr.zeng
 * 2017/11/27 19:42:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;

namespace Edit.Bundle
{
    
    /// <summary>
    /// 打包方式
    /// </summary>
    public class PACK_TYPE
    {
        //打成单包
        public const string SINGLE = "single";
        //按文件夹打包
        public const string FOLDER = "folder";
        //加载效率高
        public const string LD_FST = "loadfast";
        //细包
        public const string TINY = "tiny";
    }

    public class BdlCollect
    {
        
        public BdlCollect()
        {

        }


      
        public virtual void Collect(BundleGenInfo info_)
        {


        }




        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public static void Collect_s(List<BundleGenInfo> infos_)
        {
            foreach (var info in infos_)
            {
                Collect_s(info);
            }
        }
        

        public static void Collect_s(BundleGenInfo info_)
        {
            BdlCollect collect = null;
            switch (info_.packType)
            {
                case PACK_TYPE.LD_FST:
                    collect = new BdlCollectLdFst();
                    break;
            }
            
            if(collect != null)
                collect.Collect(info_);
        }


    }

}