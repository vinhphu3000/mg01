/* ==============================================================================
 * SprAtlasClipUtility
 * @author jr.zeng
 * 2017/11/7 17:17:24
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections.Generic;
using LitJson;

using Object = UnityEngine.Object;


using mg.org;
using mg.org.KUI;

namespace Edit
{
    public class SprAtlasClipUtility
    {

        //切片类型
        public class ClipType
        {
            static public string SMALL = "small";   //小图标
            static public string MINI = "mini";     //迷你图标
        }

        public static string[] typeList = new string[] {
                SprAtlasClipUtility.ClipType.SMALL,
                SprAtlasClipUtility.ClipType.MINI };


        public static string GetJsonPath(string path)
        {
            string jsonPath = FileUtility.ModifyFileName(path, ".json");
            return jsonPath;
        }

        /// <summary>
        /// 获取json数据
        /// </summary>
        /// <param name="atlasPath"></param>
        /// <param name="clipType"></param>
        /// <returns></returns>
        public static JsonData GetJsonData(string atlasPath, string clipType = null)
        {

            atlasPath = GetJsonPath(atlasPath);    //Icon.png -> Icon.json
            TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath(atlasPath, typeof(TextAsset)) as TextAsset;
            if (jsonAsset == null)
            {
                return null;
            }

            JsonData root = JsonMapper.ToObject(jsonAsset.text);

            return root;

            //string key_name = clipType + "_sprites";    //节点名称
            //if (root.Keys.Contains(key_name))
            //{
            //    return root[key_name];
            //}

            //return null;
        }

        //获取切片名称
        static string GetClipName(string name, string clipType)
        {
            return name + "_" + clipType;
        }

        //获取切片原图片名
        static string GetSourceName(string clipName)
        {

            int idx = clipName.LastIndexOf("_");
            string ext = clipName.Substring(idx);
            string sourceName = clipName.Replace(ext, "");
            return sourceName;
        }

        //是否切片名称
        static bool IsClipName(string name)
        {
            foreach (var kvp in typeList)
            {
                if (name.EndsWith("_" + kvp))
                {
                    return true;
                }
            }
            return false;
        }

        static bool IsClipName(string name, string clipType_)
        {
            if (name.EndsWith("_" + clipType_))
            {
                return true;
            }
            return false;
        }

        //转为字典
        static Dictionary<string, SpriteMetaData> TransNameDic(SpriteMetaData[] metaDatas)
        {
            Dictionary<string, SpriteMetaData> dic = new Dictionary<string, SpriteMetaData>();
            for (int i = 0; i < metaDatas.Length; i++)
            {
                dic[metaDatas[i].name] = metaDatas[i];
            }
            return dic;
        }

        /// <summary>
        /// 添加的切片数据列表
        /// </summary>
        /// <param name="metaDatas"></param>
        /// <param name="data_"></param>
        /// <param name="overwrite"> 是否能覆盖已有的数据 </param>
        static void AddToMetaDatas(List<SpriteMetaData> metaDatas, SpriteMetaData data_, bool overwrite)
        {
            SpriteMetaData data;
            for (int i = 0; i < metaDatas.Count; ++i)
            {
                data = metaDatas[i];
                if (data.name == data_.name)
                {
                    if (overwrite)
                        metaDatas[i] = data_;
                    return;
                }
            }

            metaDatas.Add(data_);
        }


        /// <summary>
        /// 执行切片
        /// </summary>
        /// <param name="atlasPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="clipType"></param>
        public static void ClipAtlas(string atlasPath, float width, float height, string clipType)
        {
            if (!atlasPath.EndsWith(".png") || atlasPath.EndsWith(".tag"))
            {
                Debug.LogError("--->> 目前仅支持.png/.tga");
                return;
            }

            JsonData jsonData = GetJsonData(atlasPath, clipType);

            TextureImporter importer = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
            Dictionary<string, SpriteMetaData> nameDic = TransNameDic(importer.spritesheet);


            List<SpriteMetaData> metaDatasNew = new List<SpriteMetaData>();

            SpriteMetaData[] metaDatas = importer.spritesheet;
            for (int i = 0; i < metaDatas.Length; i++)
            {
                SpriteMetaData source = metaDatas[i];

                string clipName; //切片名称

                if (IsClipName(source.name))
                {
                    //是切片数据,不会为它生成切片
                    AddToMetaDatas(metaDatasNew, source, false);
                    continue;
                }
                else
                {
                    clipName = GetClipName(source.name, clipType); //切片名称

                    metaDatasNew.Add(source);
                }


                SpriteMetaData clipData = new SpriteMetaData();

                Rect source_rect = source.rect;
                clipData.name = clipName;


                if (jsonData != null && jsonData.Keys.Contains(clipName))
                {
                    //json里有数据
                    JsonData data = jsonData[clipName];

                    JsonData rectData = data["rect"];
                    JsonData pivotData = data["pivot"];


                    Rect rectOld = new Rect(source_rect.x + (int)rectData["offetX"], source_rect.y + (int)rectData["offetY"], (int)rectData["width"], (int)rectData["height"]);
                    Vector2 pivotOld = new Vector2((float)(double)pivotData["x"], (float)(double)pivotData["y"]);  //中点取json里面的
                    Vector2 pivotPosOld = KuiUtil.CalcPivotPos(rectOld, pivotOld);

                    Rect rectNew = KuiUtil.CalcRectByPivot(pivotPosOld, pivotOld, new Size(width, height));

                    //偏移坐标取之前的,尺寸取这次的
                    //Rect rect = new Rect(source_rect.x + (int)rectData["offetX"], source_rect.y + (int)rectData["offetY"], width, height);
                    Rect rect = new Rect(rectNew.x, rectNew.y, width, height);
                    clipData.rect = rectNew;
                    clipData.pivot = pivotOld;

                    Debug.Log("切片数据被覆盖\t" + atlasPath + " " + clipType);
                }
                else
                {
                    Rect rect = new Rect(source_rect.x + (source_rect.width - width) / 2, source_rect.y + (source_rect.height - height) / 2, width, height);

                    clipData.rect = rect;
                    clipData.pivot = new Vector2(0.5f, 0.5f);
                }

                AddToMetaDatas(metaDatasNew, clipData, true);
            }

            if (metaDatasNew.Count > 0)
            {

                importer.spritesheet = metaDatasNew.ToArray();
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();

                SaveJsonData(atlasPath, importer.spritesheet);

            }

            AssetDatabase.SaveAssets();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Json相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public static void SaveJsonData(string atlasPath)
        {
            TextureImporter importer = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
            SpriteMetaData[] metaDatas = importer.spritesheet;
            SaveJsonData(atlasPath, metaDatas);
        }


        public static void SaveJsonData(string atlasPath, SpriteMetaData[] metaDatas)
        {
            string jsonPath = GetJsonPath(atlasPath);

            JsonData new_data = GetJsonContent(metaDatas, atlasPath);

            if (File.Exists(jsonPath))
            {
                AssetDatabase.DeleteAsset(jsonPath);//此处删除会引起ReloadSpriteClips读取不到mini
            }


            string content = JsonMapper.ToJson(new_data);
            content = content.Replace("}", "}\n");
            if (string.IsNullOrEmpty(content) == false)
            {
                StreamWriter sw = File.CreateText(jsonPath);
                sw.Write(content);
                sw.Close();

                Debug.Log("切片Json保存成功！\t" + jsonPath);

                //TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset)) as TextAsset;
                //EditorUtility.SetDirty(jsonAsset);
                AssetDatabase.ImportAsset(jsonPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.SaveAssets();
            }
        }

        static JsonData GetJsonContent(SpriteMetaData[] metaDatas, string atlasPath)
        {
            JsonData root = new JsonData();

            Dictionary<string, SpriteMetaData> nameDics = TransNameDic(metaDatas);

            //根据后缀不同，写到不同json组
            for (int i = 0; i < metaDatas.Length; i++)
            {
                SpriteMetaData metaData = metaDatas[i];

                if (!IsClipName(metaData.name))
                    //不是切片数据
                    continue;

                string sourceName = GetSourceName(metaData.name);

                SpriteMetaData sourceMetaData;
                if (!nameDics.TryGetValue(sourceName, out sourceMetaData) )
                {
                    Log.Debug(string.Format("找不到切片对应的原图： {0} {1}", sourceName, atlasPath)  );
                    continue;
                }

                JsonData data = new JsonData();
                //rect
                JsonData rect = new JsonData();
                rect["offetX"] = (int)(metaData.rect.x - sourceMetaData.rect.x);
                rect["offetY"] = (int)(metaData.rect.y - sourceMetaData.rect.y);
                rect["width"] = (int)(metaData.rect.width);
                rect["height"] = (int)(metaData.rect.height);
                data["rect"] = rect;
                //pivot
                JsonData pivot = new JsonData();
                pivot["x"] = (float)metaData.pivot.x;
                pivot["y"] = (float)metaData.pivot.y;
                data["pivot"] = pivot;

                root[metaData.name] = data;

            }

            return root;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽应用数据到图集∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public static void ApplyClipData(string atlasPath)
        {
            JsonData jsonData = GetJsonData(atlasPath);
            if (jsonData == null)
                //没有切片数据
                return;


            TextureImporter importer = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
            Dictionary<string, SpriteMetaData> nameDic = TransNameDic(importer.spritesheet);

            List<SpriteMetaData> metaDatasNew = new List<SpriteMetaData>();

            SpriteMetaData[] metaDatas = importer.spritesheet;
            for (int i = 0; i < metaDatas.Length; i++)
            {
                SpriteMetaData source = metaDatas[i];
                if (!IsClipName(source.name))
                {
                    //去掉旧的切片数据
                    metaDatasNew.Add(source);
                }
            }


            foreach (var name in jsonData.Keys)
            {
                JsonData data = jsonData[name];

                SpriteMetaData source = nameDic[GetSourceName(name)];
                Rect source_rect = source.rect;

                SpriteMetaData clipData = new SpriteMetaData();
                clipData.name = name;

                JsonData rectData = data["rect"];
                JsonData pivotData = data["pivot"];

                Rect rect = new Rect(source_rect.x + (int)rectData["offetX"], source_rect.y + (int)rectData["offetY"], (int)rectData["width"], (int)rectData["height"]);
                Vector2 pivot = new Vector2((float)(double)pivotData["x"], (float)(double)pivotData["y"]);  //中点取json里面的

                clipData.rect = rect;
                clipData.pivot = pivot;

                metaDatasNew.Add(clipData);
            }

            importer.spritesheet = metaDatasNew.ToArray();
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();

            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 获取一个指定切片类型的代表数据
        /// </summary>
        /// <param name="atlasPath"></param>
        /// <param name="clipType_"></param>
        public static JsonData GetClipJsonDataTypical(string atlasPath, string clipType_)
        {
            JsonData jsonData = GetJsonData(atlasPath);
            if (jsonData == null)
                //没有切片数据
                return null;

            JsonData clipJsonData = null;
            foreach (var name in jsonData.Keys)
            {
                if (IsClipName(name, clipType_ ))
                {
                    clipJsonData = jsonData[name];
                    break;
                }
            }
            return clipJsonData;
        }



    }

}