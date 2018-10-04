using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class AtlasSpritePaddingHelper
    {
        private static Dictionary<string, Dictionary<string, int[]>> _atlasSpritePaddingRecordDict;

        public static void Initialize()
        {
            _atlasSpritePaddingRecordDict = new Dictionary<string, Dictionary<string, int[]>>();
        }

        public static void WriteSpritePaddingRecord(List<LinkTextureData> textureDataList, string path)
        {
            Dictionary<string, int[]> record = new Dictionary<string, int[]>();
            foreach (LinkTextureData t in textureDataList)
            {
                if (t.spritePadding != Vector4.zero)
                {
                    int[] v = new int[] { (int)t.spritePadding.x, (int)t.spritePadding.y, (int)t.spritePadding.z, (int)t.spritePadding.w };
                    record.Add(t.name, v);
                }
            }
            if (record.Count > 0)
            {
                string content = JsonMapper.ToJson(record);
                JsonFileWriter.Write(content, path);
            }
        }

        public static Vector4 GetAtlasSpritePadding(string link)
        {
            string atlasName = LinkTextureData.GetAtlasName(link);
            string spriteName = LinkTextureData.GetTextureName(link);
            return GetAtlasSpritePadding(atlasName, spriteName);
        }

        public static Vector4 GetAtlasSpritePadding(string atlasName, string spriteName)
        {
            if (_atlasSpritePaddingRecordDict.ContainsKey(atlasName) == false)
            {
                LoadAtlasSpritePaddingRecord(atlasName);
            }
            Dictionary<string, int[]> record = _atlasSpritePaddingRecordDict[atlasName];
            if (record.ContainsKey(spriteName) == true)
            {
                int[] v = record[spriteName];
                return new Vector4(v[0], v[1], v[2], v[3]);
            }
            return Vector4.zero;
        }

        private static void LoadAtlasSpritePaddingRecord(string atlasName)
        {
            string path = KAssetManager.GetAtlasSpritePaddingRecordPath(atlasName);
            TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (jsonAsset != null)
            {
                Dictionary<string, int[]> record = JsonMapper.ToObject<Dictionary<string, int[]>>(jsonAsset.text);
                _atlasSpritePaddingRecordDict.Add(atlasName, record);
            }
            else
            {
                _atlasSpritePaddingRecordDict.Add(atlasName, new Dictionary<string, int[]>());
            }
        }



    }

}