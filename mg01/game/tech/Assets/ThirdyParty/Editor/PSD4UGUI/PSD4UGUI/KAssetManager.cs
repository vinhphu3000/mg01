using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using LitJson;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using mg.org;

namespace Edit.PSD4UGUI
{
    public class KAssetManager
    {

        //-------∽-★-∽------∽-★-∽--------∽-★-∽路径相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        public const string FOLDER_ROOT = "Assets";   //psd源目录在Assets
        //public const string FOLDER_ROOT = "External";   //psd源目录在外部

        //放json的目录
        public const string FOLDER_JSON = FOLDER_ROOT + "/PSD/Style/Json";  
        //放散图的目录
        public const string FOLDER_IMAGE = FOLDER_ROOT + "/PSD/Style/Image";
        //放xml的目录
        public const string FOLDER_XML = FOLDER_ROOT + "/PSD/Style/Xml";
        //setting目录
        public const string FOLDER_SETTING = FOLDER_ROOT + "/PSD/Style/Setting";


        //ui特效路径
        public const string FOLDER_PARTICLE = "Assets/RawData/Effects_LP/UI_Effects";
        //默认字体
        public const string DEFAULT_FONT_NAME = "DroidSansFallback";



        /// <summary>
        /// 图集合并配置文件的路径
        /// </summary>
        public static string AtlasBatchSettingPath
        {
            get { return string.Format(FOLDER_SETTING + "/{0}/BatchSetting.json", language); }
        }

        /// <summary>
        /// 图集保存的路径
        /// </summary>
        public static string AtlasFolder
        {
            get { return string.Format("Assets/Resources/GUI/{0}/Atlas", language); }
        }


        /// <summary>
        /// 字体资源路径
        /// </summary>
        public static string FontFolder
        {
            get { return string.Format("Assets/Resources/GUI/{0}/Font", language); }
        }
        
        /// <summary>
        /// 散图在assets下的路径
        /// </summary>
        public static string TextureFolderInAsset
        {
            get { return string.Format("Assets/Resources/GUI/{0}/Image", language); }
        }

        /// <summary>
        /// ui预制的路径
        /// </summary>
        public static string PrefabFolder
        {
            get { return string.Format("Assets/Resources/GUI/{0}/Prefab", language); }
        }

        /// <summary>
        /// 图集质量设置
        /// </summary>
        public static string AtlasQualitySettingPath
        {
            get { return string.Format(FOLDER_SETTING + "/{0}/QualitySetting.json", language); }
        }

        //图集路径
        public static string GetAtlasPath(string atlasName)
        {
            //UI_Part2.png
            return string.Format("{0}/{1}/UI_{2}.png", AtlasFolder, atlasName, atlasName);
        }

        public static string GetAlphaAtlasPath(string atlasName)
        {
            //UI_Part2.png
            return string.Format("{0}/{1}/UI_{2}_alpha.png", AtlasFolder, atlasName, atlasName);
        }

        public static string GetEtcMaterialPath(string atlasName)
        {
            //UI_Part2_etc.mat
            return string.Format("{0}/{1}/{2}_etc.mat", AtlasFolder, atlasName, atlasName);
        }


        public static string GetAtlasSpritePaddingRecordPath(string atlasName)
        {
            return string.Format("{0}/{1}/{2}_padding.json", AtlasFolder, atlasName, atlasName);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽KAssetManager∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //当前语言
        public static string language = LanguageSetting.CN;

        static Dictionary<string, JsonAsset> _path2json = new Dictionary<string, JsonAsset>();

        private static Dictionary<string, Font> _cachedFontDict = new Dictionary<string, Font>();
        //private static Dictionary<string, Material> _cachedFontMaterialDict = new Dictionary<string, Material>();
        private static Dictionary<string, Object[]> _cachedSpritesDict = new Dictionary<string, Object[]>();
        private static Dictionary<string, Material> _chachedMaterialDict = new Dictionary<string, Material>();

        static KAssetManager()
        {

        }

        public static void Initialize()
        {
            _path2json.Clear();
            
            _cachedFontDict.Clear();
            _cachedSpritesDict.Clear();
            _chachedMaterialDict.Clear();
        }

        /// <summary>
        /// 获取json文件
        /// </summary>
        /// <param name="path_"></param>
        /// <returns></returns>
        public static JsonAsset GetJson(string path_)
        {
            JsonAsset json;
            if (_path2json.ContainsKey(path_))
            {
                json = _path2json[path_];
            }
            else
            {
                json = new JsonAsset(path_);
                _path2json[path_] = json;
            }
            
            if (json.text == null)
            {
                //没有这文件
                return null;
            }

            return json;
        }
        
        public static JsonAsset GetUIJson(string name)
        {
            string path = string.Concat(FOLDER_JSON, "/", name, ".json");
            JsonAsset jsonAsset = GetJson(path);
            if (jsonAsset == null)
            {
                throw new Exception("未找到Json： " + path);
            }
            return jsonAsset;
        }


        public static JsonData GetUIJsonData(string name)
        {
            JsonAsset jsonAsset = GetUIJson(name);
            if (jsonAsset != null)
            {
                return jsonAsset.GetJsonData();
            }
            return null;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="folderName"></param>
        public static void CreateFolder(string parentFolder, string folderName)
        {
            mg.org.FileUtility.EnsureDirectory(parentFolder + "/" + folderName);
            
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽图集相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        /// <summary>
        /// 创建图集文件夹
        /// </summary>
        /// <param name="name"></param>
        public static void CreateAtlasFolder(string name)
        {
            CreateFolder(AtlasFolder, name);
        }

        /// <summary>
        /// 删除图集文件夹
        /// </summary>
        /// <param name="name"></param>
        public static void DeleteAtlasFolder(string name)
        {
            string path = AtlasFolder + "/" + name;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 通过link获取散图在Assets下的路径
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string GetTexturePath(string link)
        {
            //Announce.BtnCommon1
            link = link.Replace(".", "/");

            if (FOLDER_ROOT == "Assets")
            {
                return string.Concat(FOLDER_IMAGE, "/", link, ".png");
            }
            else
            {
                return string.Concat(TextureFolderInAsset, "/", link, ".png");
            }
        }
        

        /// <summary>
        /// 获取散图资源
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Texture2D GetTextureInAssets(string link)
        {
            string path = GetTexturePath(link);
            Texture2D texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;    //只能从Assets里取？
            if (texture == null)
            {
                throw new Exception("未找到图片： " + path);
            }
            return texture;
        }

        /// <summary>
        /// 确保assets里有这张散图 (这方案不需要了)
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string EnsureTextureInAssets(string link)
        {
            if (FOLDER_ROOT == "Assets")
                return GetTexturePath(link);
            
            //psd源目录在外部,需要拷贝图片到assets里做处理

            string pathInAssets = GetTexturePath(link);

            if (File.Exists(pathInAssets))
                //图片已存在
                return pathInAssets;

            //确保文件夹存在
            mg.org.FileUtility.EnsureDirectory(mg.org.FileUtility.GetFolderFromFullPath(pathInAssets));

            string texturePath = GetTexturePath(link);
            File.Copy(texturePath, pathInAssets);
            AssetDatabase.ImportAsset(pathInAssets, ImportAssetOptions.ForceUpdate); //重新import
            return pathInAssets;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽资源相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取assets下的图片sprite
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Sprite GetSprite(string link)
        {
            string atlasName = LinkTextureData.GetAtlasName(link);
            atlasName = AtlasBatchSetting.GetBatchedAtlasName(atlasName);   //获取打包的图集
            string spriteName = LinkTextureData.GetTextureName(link);
            string path = GetAtlasPath(atlasName);

            if (_cachedSpritesDict.ContainsKey(path) == false)
            {
                _cachedSpritesDict.Add(path, AssetDatabase.LoadAllAssetsAtPath(path));
            }

            Object[] sprites = _cachedSpritesDict[path];
            if (sprites.Length == 0)
            {
                throw new Exception("未找到图集： " + path);
            }

            foreach (Object obj in sprites)
            {
                if (obj.name == spriteName)
                {
                    return obj as Sprite;
                }
            }
            throw new Exception("未找到Sprite：　" + path + " " + spriteName);
        }

        /// <summary>
        /// 获取纯色材质球
        /// </summary>
        /// <returns></returns>
        public static Material GetNoTexMaterial()
        {
            string path = "Assets/Resources/Materials/ImgNoTex.mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                throw new Exception("未找到无纹理材质" + path);
            }
            return mat;
        }


        //public static Material GetEtcMaterialByLink(string link)
        //{
        //    string atlasName = LinkTextureData.GetAtlasName(link);
        //    return GetEtcMaterialByAtlasName(atlasName);
        //}

        /// <summary>
        /// 获取ui图集对应的材质球
        /// </summary>
        /// <param name="atlasName"></param>
        /// <returns></returns>
        //public static Material GetEtcMaterialByAtlasName(string atlasName)
        //{
        //    atlasName = AtlasBatchSetting.GetBatchedAtlasName(atlasName);
        //    if (_chachedMaterialDict.ContainsKey(atlasName) == false)
        //    {
        //        string path = string.Format("{0}/{1}/UI_{2}_etc.mat", AtlasFolder, atlasName, atlasName);
        //        Material material = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
        //        if (material == null)
        //        {
        //            throw new Exception("未找到Material： " + path);
        //        }
        //        _chachedMaterialDict.Add(atlasName, material);
        //    }
        //    return _chachedMaterialDict[atlasName];
        //}
        
        public static Material GetMaterial(string path)
        {
            if (_chachedMaterialDict.ContainsKey(path) == false)
            {
                Material material = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
                if (material == null)
                {
                    throw new Exception("未找到Material： " + path);
                }
                _chachedMaterialDict.Add(path, material);
            }
            return _chachedMaterialDict[path];
        }
        
        public static Material GetFontMaterial()
        {
            //return GetMaterial("Assets/Resources/GUIDesc/font/FontMaterial.mat");
            return GetMaterial("Assets/Resources/GUI/cn/font/FontMaterial.mat");
        }

        /// <summary>
        /// 获取字体资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Font GetFont(string name)
        {
            if (_cachedFontDict.ContainsKey(name) == false)
            {
                string path = FontFolder + "/" + name + ".ttf";
                Font font = AssetDatabase.LoadAssetAtPath(path, typeof(Font)) as Font;
                if (font == null)
                {
                    if (name == DEFAULT_FONT_NAME)
                    {
                        throw new Exception("没找到默认字体: " + path);
                    }
                    else
                    {
                        Debug.LogWarning("未找到字体: " + path + " 已经替换成默认字体");
                        return GetFont(DEFAULT_FONT_NAME);
                    }
                }
                _cachedFontDict.Add(name, font);
            }
            return _cachedFontDict[name];
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽LanguageSetting∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public class LanguageSetting
    {
        public const string CN = "cn";
        public const string TW = "tw";
        public const string KR = "kr";

        public static string[] languages = new string[] { CN, TW, KR };

        public static string GetLanguage(int index)
        {
            return languages[index];
        }

        public static int GetLanguageIndex(string language)
        {
            return Array.IndexOf<string>(languages, language);
        }
    }


}