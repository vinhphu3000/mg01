/* ==============================================================================
 * 打包日志生成
 * @author jr.zeng
 * 2017/12/2 14:45:30
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit.Bundle
{

    public class BundleLog
    {
        //日志文件目录的路径
        public const string log_folder_path = "./Log_abs";

        const string TAB = "    ";

        public BundleLog()
        {

        }


        public static void MakeLog(List<BundleGenInfo> genInfos_)
        {

            if (!Directory.Exists(log_folder_path))
                Directory.CreateDirectory(log_folder_path);   //创建文件夹

            string filePath = log_folder_path + "/Log_abs " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";

            StringBuilder strBundler = new StringBuilder();

            for (int i = 0; i < genInfos_.Count; ++i)
            {
                BundleGenInfo genInfo = genInfos_[i];

                //包名
                strBundler.AppendLine("bundle_name:" + genInfo.bundleName);

                string tab = TAB;
                //依赖
                strBundler.AppendLine(tab + "bundle_depend:");
                LogAsset(genInfo.dpAssets, strBundler, tab);
                //资源列表
                LogAsset(genInfo.assets, strBundler, "");
            }
            
            File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(strBundler.ToString()));
            Debug.Log("导出打包日志: " + filePath);
        }


        static void LogAsset(List<AssetGroup> assetInfos_, StringBuilder strBundler_, string tab)
        {
            tab = tab + TAB;

            AssetGroup assetInfo;
            for (int j = 0; j < assetInfos_.Count; ++j)
            {
                assetInfo = assetInfos_[j];

                strBundler_.AppendLine(tab + "asset_name:" + assetInfo.name);

                string tab2 = tab + TAB + "├";
                for (int k = 0; k < assetInfo.files.Count; ++k)
                {
                    string file = assetInfo.files[k];
                    strBundler_.AppendLine(tab2 + file);
                }

                List<AssetGroup> dpAssets = assetInfo.dpAssets;

                string tab3 = tab + TAB;
                strBundler_.AppendLine(tab3 + "depends:" + (dpAssets.Count == 0 ? " none" : ""));

                if (dpAssets.Count > 0)
                {
                    string tab4 = tab3 + TAB + "├";
                    for (int k = 0; k < dpAssets.Count; ++k)
                    {
                        strBundler_.AppendLine(tab4 + dpAssets[k].name);    //只打印出包名
                    }
                }

                strBundler_.AppendLine();
                //LogAsset(assetInfo.dpAssets, strBundler_, tab);
            }
        }


    }

}