using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using LitJson;
//using EditorTools.Utilities;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using mg.org;

namespace Edit.PSD4UGUI
{
    public class AtlasGenerator
    {

        //readme内容
        public const string BATCHED_TEMPLATE = "【该文件由PSD4UGUI工具生成】\n{0} 面板资源已经合并至 {1}, 具体请看配置文件BatchSetting.json";

        //psd生成散图时,是根据xml/Shared.xml来判断是否在公共图集内
        public const string SHARED = "Shared";      
        public const string SHARED1 = "Shared1";

        public const int ATLAS_MAX_SIZE = 2048; //最大图集尺寸
        public const int FAVOR_ATLAS_SIZE = 1024;


        static Regex LINK_LANGUAGE_PATTERN = new Regex(@"(?<=\.)\w+(?=#)");
        static string _atlasName;

        //不含重复Texture信息列表，每张图片资源只记录一次，同时公共资源和非当前语言资源也不会记录在内
        static List<LinkTextureData> _uniqueTextureDataList;
        static HashSet<string> _linkSet;
        static Texture2D _atlas;

        //高质量图片
        static bool _isHighQuality = false;

        //记录散图的md5,用来跳过散图处理
        private static Dictionary<string, string> _md5Dict;

        static void Initialize()
        {
            _uniqueTextureDataList = new List<LinkTextureData>();
            _linkSet = new HashSet<string>();
        }

        public static void Generate(string jsonName, InputParam inputParam_)
        {
            Initialize();

            _isHighQuality = inputParam_.isHighQuality;
            
            _atlasName = AtlasBatchSetting.GetBatchedAtlasName(jsonName);   //获取实际的图集名称

            //WriteBatchedReadme(_atlasName, jsonName);   //提醒图集已经移到partxx
            
            List<string> batchedAtlasNameList = AtlasBatchSetting.GetBatchedAtlasNameList(jsonName);    //收集此图集包含的所有json名称
            for (int i = 0; i < batchedAtlasNameList.Count; i++)
            {
                string name = batchedAtlasNameList[i];
                if (batchedAtlasNameList.Count == 1 || i > 0)   //如果多于一个,跳过第一个,因为那个是目标图集,不是界面
                {
                    JsonData jsonData = KAssetManager.GetUIJsonData(name);
                    ProcessJson(jsonData);  //处理所有该图集需要用到的json, 不只是该界面的json
                }
            }

            //处理散图
            ProcessTextureDataList();

            if(_uniqueTextureDataList.Count > 0)
            {
                //需要生成图集
                GenerateAtlas();
            }
            else
            {
                Debug.Log("面板资源都在公共图集中~~~~");
            }
        }

        //保存readme
        static void WriteBatchedReadme(string atlasName, string jsonName)
        {
            string path = KAssetManager.AtlasFolder + "/" + jsonName + "/readme.txt";
            if(atlasName == jsonName)
            {
                AssetDatabase.DeleteAsset(path);
                return;
            }

            AssetDatabase.DeleteAsset(KAssetManager.GetAtlasPath(jsonName));
            AssetDatabase.DeleteAsset(KAssetManager.GetAlphaAtlasPath(jsonName));
            AssetDatabase.DeleteAsset(KAssetManager.GetEtcMaterialPath(jsonName));
            
            //合并到别的图集,删除原来的文件夹
            KAssetManager.DeleteAtlasFolder(jsonName);

            //不导出readme了
            //string content = string.Format(BATCHED_TEMPLATE, jsonName, atlasName);
            //File.WriteAllText(Application.dataPath.Replace("Assets", "") + path, content, System.Text.Encoding.Unicode);
            //AssetDatabase.ImportAsset(path);
        }

        static void ProcessJson(JsonData data)
        {
            string type = (string)data["type"];
            if(type == "Image")
            {
                ProcessImageData(data); //记录所有的图片
            }
            else if( data.Keys.Contains("children") )
            {
                JsonData childrenData = data["children"];
                for(int i = 0; i < childrenData.Count; i++)
                {
                    ProcessJson(childrenData[i]);
                }
            }
        }

        static void ProcessImageData(JsonData data)
        {
            foreach(string key in data.Keys)
            {
                if(data[key].IsObject == false)
                    continue;

                JsonData stateData = data[key];
                if(stateData.Keys.Contains("link") == true)
                {
                    string link = (string)stateData["link"];    //图片名称 "Shard.xxx"

                    if(IsLanguageExclude(link) == true)
                        continue;

                    if(IsShared(link) == true)
                        //在公共图集不打包
                        continue;

                    string name = LinkTextureData.GetTextureName(link);
                    if (_linkSet.Contains(name) == true)    //为何用name不用link来记录？ ->atlas里的名称用的是name,如果用link可能会导致同一张图重复了
                        //每个图片只需要有一个TextureData
                        continue;
                    _linkSet.Add(name);

                    LinkTextureData textureData = new LinkTextureData();
                    textureData.link = link;
                    textureData.name = name;// TextureData.GetTextureName(link);
                    textureData.width = (int)stateData["width"];
                    textureData.height = (int)stateData["height"];

                    //九宫格
                    if (stateData.Keys.Contains("top"))
                        textureData.top = (int)stateData["top"];
                    if(stateData.Keys.Contains("right"))
                        textureData.right = (int)stateData["right"];
                    if(stateData.Keys.Contains("bottom"))
                        textureData.bottom = (int)stateData["bottom"];
                    if(stateData.Keys.Contains("left"))
                        textureData.left = (int)stateData["left"];

                    //填充模式
                    if (stateData.Keys.Contains("param"))
                        textureData.fillParam = (string)stateData["param"];

                    _uniqueTextureDataList.Add(textureData);
                }
            }
        }

        //图片是否在公共图集
        static bool IsShared(string link)
        {
            if(_atlasName == SHARED || _atlasName == SHARED1)
            {
                return false;
            }
            string atlasName = LinkTextureData.GetAtlasName(link);
            return atlasName == SHARED || atlasName == SHARED1;
        }

        //被当前语言排除
        static bool IsLanguageExclude(string link)
        {
            Match m = LINK_LANGUAGE_PATTERN.Match(link);
            if(string.IsNullOrEmpty(m.Value) == true)
            {
                return false;
            }
            if(m.Value == KAssetManager.language)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 处理所有散图
        /// </summary>
        static void ProcessTextureDataList()
        {
            foreach (LinkTextureData textureData in _uniqueTextureDataList)
            {
                string texturePath = KAssetManager.GetTexturePath(textureData.link);    //散图在Assets下的路径(External模式下是用mklink超链过来)
                //string texturePath = KAssetManager.EnsureTextureInAssets(textureData.link);

                TextureImporterUtil.CreateReadableTextureImporter(texturePath);     //处理散图
                AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate); //重新import一次

                Texture2D texture = KAssetManager.GetTextureInAssets(textureData.link); //获取图片资源
                if (textureData.IsScale9Grid == true)
                {
                    //根据九宫格生成图片的最小尺寸
                    texture = Scale9GridTextureProcessor.Process(texture, textureData.top, textureData.right, textureData.bottom, textureData.left);
                }

                Rect rect;
                Vector4 padding;
                texture = TextureAlphaKicker.Kick(texture, out rect, out padding);  //优化掉四周的透明像素，减少图片尺寸
                texture = TextureClamper.Clamp(texture); //四周补2像素
                textureData.texture = texture;
                textureData.spritePadding = padding;
            }
        }

        static void GenerateAtlas()
        {
            _atlas = new Texture2D(ATLAS_MAX_SIZE, ATLAS_MAX_SIZE);
            Rect[] rects = _atlas.PackTextures(GetPackTextures(), 0, ATLAS_MAX_SIZE, false);    //pack!

            string atlasPath = KAssetManager.GetAtlasPath(_atlasName);
            string alphaAtlasPath = KAssetManager.GetAlphaAtlasPath(_atlasName);
            string folderPath = mg.org.FileUtility.GetFolderFromFullPath(atlasPath);
            mg.org.FileUtility.EnsureDirectory(folderPath); //创建文件夹

            _atlas = AtlasOptimizer.Optimize(_atlas, rects, true);  //优化图集
            AtlasWriter.Write(_atlas, atlasPath);   //保存图集
            LogAtlasSize(_atlas, _atlasName);

            // 暂时不采用分开的纹理格式， 删除ALPHA贴图
            AssetDatabase.DeleteAsset(alphaAtlasPath);

            bool isTurecolor = AtlasQualitySetting.Contains(_atlasName);
            //if(isTurecolor)
            //{
            //    AssetDatabase.DeleteAsset(alphaAtlasPath);
            //}
            //else
            //{
            //    //EtcGeneratorWrapper.Execute(atlasPath, _isHighQuality);
            //    ImageChannelSpliterWrapper.Execute(atlasPath, _isHighQuality);
            //    AssetDatabase.ImportAsset(alphaAtlasPath, ImportAssetOptions.ForceUpdate);
            //    TextureImporterUtil.CreateEtcAlphaChannelImporter(alphaAtlasPath);
            //    AssetDatabase.ImportAsset(alphaAtlasPath, ImportAssetOptions.ForceUpdate);
            //}
            
            try
            {
                AssetDatabase.ImportAsset(atlasPath, ImportAssetOptions.ForceUpdate);
            }
            catch (System.Exception e)
            {
                Debug.Log("ImportAssetError:" + atlasPath);
            }

            isTurecolor = false;    //都用压缩格式
            TextureImporterFormat format = isTurecolor ? TextureImporterFormat.ARGB32 : TextureImporterFormat.AutomaticCompressed;
            TextureImporterUtil.CreateMultipleSpriteImporter(atlasPath, rects, GetPackTextureNames(), GetSpriteBorders(), _atlas.width, _atlas.height, format, ATLAS_MAX_SIZE);
            AssetDatabase.ImportAsset(atlasPath, ImportAssetOptions.ForceUpdate);
            //记录所有图片的边距数据
            AtlasSpritePaddingHelper.WriteSpritePaddingRecord(_uniqueTextureDataList, KAssetManager.GetAtlasSpritePaddingRecordPath(_atlasName));
            //暂时不采用分开的纹理格式

            //采用替换内置材质的方式
            //EtcMaterialCreator.CreateWithoutAlpha(EtcMaterialCreator.PSD4UGUI_ONE_TEX_SHADER, atlasPath);   
        }

        //打印图集尺寸
        static void LogAtlasSize(Texture2D atlas, string atlasName)
        {
            if(atlas.width > FAVOR_ATLAS_SIZE || atlas.height > FAVOR_ATLAS_SIZE)
            {
                Debug.Log(string.Format("<color=#ff0000>【警告】图集宽度或高度超过1024像素： {0} </color>", atlasName));
            }
            else
            {
                Debug.Log(string.Format("<color=#00ff00>图集 {0} 尺寸为： {1}x{2}</color>", atlasName, atlas.width, atlas.height));
            }
        }

        //获取散图列表
        static Texture2D[] GetPackTextures()
        {
            Texture2D[] result = new Texture2D[_uniqueTextureDataList.Count];
            for(int i = 0; i < _uniqueTextureDataList.Count; i++)
            {
                result[i] = _uniqueTextureDataList[i].texture;
            }
            return result;
        }

        static string[] GetPackTextureNames()
        {
            string[] result = new string[_uniqueTextureDataList.Count];
            for(int i = 0; i < _uniqueTextureDataList.Count; i++)
            {
                result[i] = _uniqueTextureDataList[i].name;
            }
            return result;
        }

        static Vector4[] GetSpriteBorders()
        {
            Vector4[] result = new Vector4[_uniqueTextureDataList.Count];
            for(int i = 0; i < _uniqueTextureDataList.Count; i++)
            {
                LinkTextureData textureData = _uniqueTextureDataList[i];
                result[i] = new Vector4(textureData.left, textureData.bottom, textureData.right, textureData.top);
            }
            return result;
        }

        static Vector4[] GetSpritePaddings()
        {
            Vector4[] result = new Vector4[_uniqueTextureDataList.Count];
            for(int i = 0; i < _uniqueTextureDataList.Count; i++)
            {
                LinkTextureData textureData = _uniqueTextureDataList[i];
                result[i] = textureData.spritePadding;
            }
            return result;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Md5相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽TP相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
    }



    public class LinkTextureData
    {
        public string link;
        public string name;
        public Texture2D texture;

        public int width;
        public int height;

        //九宫格
        public int top = 0;
        public int right = 0;
        public int bottom = 0;
        public int left = 0;

        //填充模式
        public string fillParam;

        //剔除透明区域后图片相对于原图片边缘的Padding值
        public Vector4 spritePadding = Vector4.zero;

        public bool IsScale9Grid
        {
            get
            {
                return top > 0 && right > 0 && bottom > 0 && left > 0;
            }
        }

        /// <summary>
        /// 有填充模式
        /// </summary>
        public bool IsFillMode
        {
            get
            {
                return fillParam == ImageCreator.PARAM_UV_LEFT2RIGHT
                    || fillParam == ImageCreator.PARAM_UV_RIGHT2LEFT
                    || fillParam == ImageCreator.PARAM_UV_UP2DOWN
                    || fillParam == ImageCreator.PARAM_UV_DOWN2UP
                    || fillParam == ImageCreator.PARAM_UV_RADIAL_360;
            }
        }


        public static string GetAtlasName(string link)
        {
            //{"link":"Announce.BgScrollUp"}
            return link.Split('.')[0];
        }

        public static string GetTextureName(string link)
        {
            return link.Split('.')[1];
        }

    }




}
