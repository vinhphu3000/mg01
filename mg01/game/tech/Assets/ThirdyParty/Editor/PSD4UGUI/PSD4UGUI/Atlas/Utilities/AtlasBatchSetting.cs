using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LitJson;
using UnityEngine;
using mg.org;

using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    // 合并图集配置
    public class AtlasBatchSetting
    {

        private static List<List<string>> _batchSetting;
        private static HashSet<string> _atlasNameSet; //在某次运行中已经生成的Atlas集合，避免重复生成Atlas

        public static void Initialize()
        {
            _batchSetting = new List<List<string>>();
            _atlasNameSet = new HashSet<string>();

            JsonAsset jsonAsset  = KAssetManager.GetJson(KAssetManager.AtlasBatchSettingPath);
            if (jsonAsset == null)
            {
                Debug.LogWarning("未找到图集合并设置 " + KAssetManager.AtlasBatchSettingPath);
            }
            else
            {
                Dictionary<string, List<List<string>>> dict = JsonMapper.ToObject<Dictionary<string, List<List<string>>>>(jsonAsset.text);
                _batchSetting = dict["setting"];
                for(int i=0;i<_batchSetting.Count;++i)
                    _batchSetting[i] = ListUtil.RemoveRepeat(_batchSetting[i]); //剔除重复项
            }
        }

        /// <summary>
        /// 根据图集名称查询其所在的合并后的图集名称，
        /// 若当前图集不存在设置项，则返回当前图集名称
        /// 当存在合并配置项时使配置项的第一个图集名称为合并图集名称
        /// </summary>
        /// <param name="atlasName"></param>
        /// <returns></returns>
        public static string GetBatchedAtlasName(string atlasName)
        {
            for (int i = 0; i < _batchSetting.Count; i++)
            {
                List<string> jsonList = _batchSetting[i];
                for (int j = 0; j < jsonList.Count; j++)
                {
                    if (jsonList[j] == atlasName)
                    {
                        return jsonList[0];
                    }
                }
            }
            return atlasName;
        }

        /// <summary>
        /// 根据图集名称查询其所在的配置项
        /// </summary>
        /// <param name="atlasName"></param>
        /// <returns></returns>
        public static List<string> GetBatchedAtlasNameList(string atlasName)
        {
            for (int i = 0; i < _batchSetting.Count; i++)
            {
                List<string> jsonList = _batchSetting[i];
                for (int j = 0; j < jsonList.Count; j++)
                {
                    if (jsonList[j] == atlasName)
                    {
                        return jsonList;
                    }
                }
            }
            return new List<string> { atlasName };
        }

        /// <summary>
        /// 图集是否已生成
        /// </summary>
        /// <param name="atlasName"></param>
        /// <returns></returns>
        public static bool IsAtlasNameRecord(string atlasName)
        {
            string batchedAtlasName = GetBatchedAtlasName(atlasName);   //实际的图集名称
            return _atlasNameSet.Contains(batchedAtlasName);
        }

        /// <summary>
        /// 记录图集已生成
        /// </summary>
        /// <param name="atlasName"></param>
        public static void RecordAtlasName(string atlasName)
        {
            string batchedAtlasName = GetBatchedAtlasName(atlasName);
            _atlasNameSet.Add(batchedAtlasName);
        }
        

    }

}