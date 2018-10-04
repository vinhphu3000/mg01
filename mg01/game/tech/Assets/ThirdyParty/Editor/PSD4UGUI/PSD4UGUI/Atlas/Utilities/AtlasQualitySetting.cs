using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using LitJson;

namespace Edit.PSD4UGUI
{
    public class AtlasQualitySetting
    {

        private static List<string> _highQualityAtlasList;

        public static void Initialize()
        {
            JsonAsset jsonAsset = KAssetManager.GetJson(KAssetManager.AtlasQualitySettingPath);
            if (jsonAsset != null)
            {
                Dictionary<string, List<string>> dict = JsonMapper.ToObject<Dictionary<string, List<string>>>(jsonAsset.text);
                _highQualityAtlasList = dict["highQuality"];
            }
            else
            {
                _highQualityAtlasList = new List<string>();
            }
        }

        public static bool Contains(string atlasName)
        {
            return _highQualityAtlasList.Contains(atlasName);
        }


    }

}