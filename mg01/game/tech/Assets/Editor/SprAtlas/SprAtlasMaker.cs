/* ==============================================================================
 * 图集生成
 * @author jr.zeng
 * 2017/11/1 14:30:02
 * ==============================================================================*/

using System;

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


using Object = UnityEngine.Object;

using mg.org;
using Edit.PSD4UGUI;


namespace Edit
{

    public class SprAtlasMaker
    {


        public const int ATLAS_MAX_SIZE = 2048;     //最大允许图集尺寸
        public const int FAVOR_ATLAS_SIZE = 1024;   //推荐最大尺寸

        //散图目录
        public const string RAW_PATH_IMAGE = "Assets/RawData/Image";
        //图集目录
        public const string RAW_PATH_ATLAS = "Assets/RawData/SpriteAtlas";
        //图集asset目录
        public const string GEN_PATH_ATLAS = "Assets/Resources/SpriteAtlas";

        //历遍文件夹
        static void WalkDir(string dir, System.Action<string> onDir, System.Action<string> onFile)
        {
            var fs = Directory.GetFiles(dir);
            foreach (var f in fs)
            {
                onFile(f);
            }

            var dirs = Directory.GetDirectories(dir);
            foreach (var d in dirs)
            {
                onDir(d);
                WalkDir(d, onDir, onFile);
            }

        }
        

        //asset生成路径
        static string getAssetPath(string filename_)
        {
            return string.Format(GEN_PATH_ATLAS + "/{0}/{1}" + ResSuffix.ASSET, filename_, filename_);
        }

        /// <summary>
        /// 是否在图集文件夹
        /// </summary>
        /// <param name="filePath_"></param>
        /// <returns></returns>
        public static bool IsInAtlasPath(string filePath_)
        {
            return filePath_.IndexOf(RAW_PATH_ATLAS) >= 0;
        }

        public static bool IsInAssetPath(string filePath_)
        {
            return filePath_.IndexOf(GEN_PATH_ATLAS) >= 0;
        }


        [MenuItem("Assets/生成图集/打包选中图集", true)]
        static bool CanPackAtlasByChoose2() { return CanPackAtlasByChoose(); }
        [MenuItem("Assets/生成图集/打包选中图集", false, 1)]
        static void PackAtlasByChoose2() { PackAtlasByChoose(); }
        [MenuItem("Assets/生成图集/生成选中Asset", true)]
        static bool CanGenAssetByChoose2() { return CanGenAssetByChoose(); }
        [MenuItem("Assets/生成图集/生成选中Asset", false, 2)]
        static void GenAssetByChoose2() { GenAssetByChoose(); }

        //[MenuItem("Assets/aaa/aaa")]
        //static void GenAssetBy333() {  }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Asset相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        [MenuItem("工具/生成图集/选中对象生成Asset", true)]
        static bool CanGenAssetByChoose()
        {
            Object obj = Selection.activeObject;
            if (obj == null)
                return false;

            string folderPath = null;

            string path = AssetDatabase.GetAssetPath(obj);
            if (obj is Texture)
            {
                folderPath = mg.org.FileUtility.GetFolderFromFullPath(path);
            }
            else if (Directory.Exists(path))
            {
                folderPath = path;
            }
            else
            {
                return false;
            }

            if (folderPath == RAW_PATH_ATLAS || folderPath.IndexOf(RAW_PATH_ATLAS) < 0)
            {
                //不在目录下
                return false;
            }
            return true;
        }
    


        [MenuItem("工具/生成图集/选中对象生成Asset", false, 3)]
        static void GenAssetByChoose()
        {
            try
            {
                bool b = false;

                var objs = Selection.objects;
                foreach (var obj in objs)
                {
                    if (obj == null)
                        continue;

                    string path = AssetDatabase.GetAssetPath(obj);

                    var tex = obj as Texture;
                    if (tex != null)
                    {
                        GenAsset(path);
                        b = true;
                    }
                    else if (Directory.Exists(path))
                    {
                        b = GenAssetByDir(path);
                    }

                    if (b)
                    {
                        //Debug.Log("--->> <color=green>生成SpriteAltas完成 !! </color>" + path);
                    }
                    else
                    {
                        Debug.Log("--->> <color=red>没有生成任何SpriteAltas </color>");
                    }

                }

                AssetDatabase.SaveAssets();
                //Debug.Log("--->> <color=green>生成选中对象SpriteAtlasProxy完成 !!</color>");
            }
            catch (System.Exception ex)
            {
                Debug.Log("Gen sprite atlas exception!\t" + ex.Message);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("工具/生成图集/生成全部Asset", false, 4)]
        static void GenAllAsset()
        {
            string rootPath = RAW_PATH_ATLAS;
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            DirectoryInfo[] files = dir.GetDirectories();

            for(int i=0;i<files.Length;++i)
            {
                string path = rootPath + "/" + files[i].Name;
                GenAssetByDir(path);
            }

        }

        static bool GenAssetByDir(string path_)
        {
            bool b = false;
            WalkDir(path_,

               (d) =>
               {
               //var dd = d.Replace("RawData", "Resources");
               //if (!Directory.Exists(dd))
               //{
               //    Directory.CreateDirectory(dd);
               //}
           },

               (f) =>
               {
                   if (f.EndsWith(".png") || f.EndsWith(".tga"))
                   {
                       GenAsset(f);
                       b = true;
                   }
               });

            AssetDatabase.SaveAssets();
            return b;
        }



        public static void GenAsset(string path)
        {

            try
            {
                if (!path.EndsWith(".png") || path.EndsWith(".tag"))
                {
                    Debug.LogError("--->> 目前仅支持.png/.tga");
                    return;
                }

                string fileName = mg.org.FileUtility.GetNameFromFullPath(path, "");
                string genPath = getAssetPath(fileName);

                string dir = mg.org.FileUtility.GetFolderFromFullPath(genPath);
                mg.org.FileUtility.EnsureDirectory(dir);
                
                SprAtlas asset = null;
                if (File.Exists(genPath))
                {
                    File.Delete(genPath);
                }

                asset = ScriptableObject.CreateInstance<SprAtlas>();
                AssetDatabase.CreateAsset(asset, genPath);
                asset.file_name = fileName;

                var all = AssetDatabase.LoadAllAssetsAtPath(path);
                List<Sprite> sprs = new List<Sprite>();
                foreach (var a in all)
                {
                    var t = a as Texture;
                    if (t != null)
                    {
                        asset.texture = t;
                    }
                    var sp = a as Sprite;
                    if (sp)
                    {
                        sprs.Add(sp);
                    }
                }

                asset.sprites = sprs.ToArray();
                EditorUtility.SetDirty(asset);
                //AssetDatabase.SaveAssets();
                Debug.Log("<color=yellow>--->></color> 生成asset完成 !!\n" + genPath);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Gen sprite atlas exception!\t" + ex.Message + "\t" + path);
            }


        }






        //-------∽-★-∽------∽-★-∽--------∽-★-∽Atlas相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


        [MenuItem("工具/生成图集/打包选中图集", true)]
        static bool CanPackAtlasByChoose()
        {
            Object obj = Selection.activeObject;
            if (obj == null)
                return false;

            string folderPath = null;

            string path = AssetDatabase.GetAssetPath(obj);
            if (obj is Texture)
            {
                folderPath = mg.org.FileUtility.GetFolderFromFullPath(path);  //获取所在目录
            }
            else if (Directory.Exists(path))
            {
                folderPath = path;
            }
            else
            {
                return false;
            }


            if (folderPath == RAW_PATH_IMAGE || folderPath.IndexOf(RAW_PATH_IMAGE) < 0)
            {
                //不在目录下
                return false;
            }
            return true;
        }

        

        [MenuItem("工具/生成图集/打包选中图集", false, 1)]
        static void PackAtlasByChoose()
        {
            bool b = false;

            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                if (obj == null)
                    continue;

                string path = AssetDatabase.GetAssetPath(obj);

                var tex = obj as Texture;
                if (tex != null)
                {
                    string folder = mg.org.FileUtility.GetFolderFromFullPath(path);
                    if (PackAtlas(folder))
                        b = true;
                }
                else if (Directory.Exists(path))
                {
                    if (PackAtlasByDir(path))
                        b = true;
                }

                if (b)
                {
                    //Debug.Log("--->> <color=green>生成SpriteAltas完成 !! </color>" + path);
                }
                else
                {
                    Debug.Log("--->> <color=red>没有打包任何图集 </color>");
                }

            }
            
            AssetDatabase.SaveAssets();
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.Log("pack sprite atlas exception!\t" + ex.Message);
            //}

            AssetDatabase.Refresh();

        }

        [MenuItem("工具/生成图集/打包全部图集", false, 2)]
        static void PackAllAtlas()
        {
            string rootPath = RAW_PATH_IMAGE;
            PackAtlasByDir(rootPath);

            //DirectoryInfo dir = new DirectoryInfo(rootPath);
            //DirectoryInfo[] files = dir.GetDirectories();

            //for (int i = 0; i < files.Length; ++i)
            //{
            //    string path = rootPath + "/" + files[i].Name;
            //    PackAtlas(path);
            //}

        }
       

        static bool PackAtlasByDir(string folderPath_)
        {

            bool b = false;
            if (Directory.Exists(folderPath_))
            {
                WalkDir(folderPath_, (d) => {
                    //文件夹
                    string f = d;
                    if (PackAtlas(d))
                        b = true;

                }, (f) =>
                {

                });

                if (PackAtlas(folderPath_))
                    b = true;
            }

            return b;
        }

        static bool PackAtlas(string folderPath_)
        {
            List<Texture2D> textures = new List<Texture2D>();
            Texture2D tex;

            string suffix = ".png";

            string[] fs = Directory.GetFiles(folderPath_);
            foreach (string f in fs)
            {
                if (f.EndsWith(".png"))
                {
                    //suffix = ".png";
                }
                else if (f.EndsWith(".tga"))
                {
                    suffix = ".tga";
                }
                else continue;

                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(f);
                if (tex)
                {

                    textures.Add(tex);
                }
            }

            if (textures.Count <= 0)
            {
                return false;
            }


            string atlasPath = PackTextures(textures, folderPath_, suffix);   //打包图集
            AssetDatabase.Refresh();
            
            SprAtlasClipUtility.ApplyClipData(atlasPath);   //还原切片数据到图集中

            GenAsset(atlasPath);

            return true;
        }

        static string PackTextures(List<Texture2D> texturesList_, string folderPath_, string suffix_=".png")
        {

            string atlasName_ = mg.org.FileUtility.GetNameFromFullPath(folderPath_, suffix_);  //Image/icon/icon1001 -> icon1001.png

            string atlasPath = folderPath_.Replace("Image/", "SpriteAtlas/") + "/" + atlasName_;  //Image/icon/icon1001 -> SpriteAtlas/icon/icon1001/icon1001.png

            string folderPath = mg.org.FileUtility.GetFolderFromFullPath(atlasPath);
            mg.org.FileUtility.EnsureDirectory(folderPath); //创建文件夹

            if (File.Exists(atlasPath))
            {
                File.Delete(atlasPath);
            }

            Texture2D[] textures = new Texture2D[texturesList_.Count];
            string[] textureNames = new string[texturesList_.Count];
            Texture2D tex;
            for (int i = 0; i < texturesList_.Count; ++i)
            {
                tex = texturesList_[i];
                textureNames[i] = tex.name;
                string texturePath = AssetDatabase.GetAssetPath(tex);

                CreateReadableTextureImporter(texturePath);     //处理import参数
                AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate); //重新import一次

                //Rect rect;
                //Vector4 padding;    //获取实际有像素的外框(暂时不处理)
                //tex = TextureAlphaKicker.Kick(tex, out rect, out padding);
                tex = TextureClamper.Clamp(tex); //四周补2像素

                textures[i] = tex;
                //textureData.spritePadding = padding;

                //测试单张图片的质量
                //string exportPath = texturePath.Replace("Image", "SpriteAtlas");
                //AtlasWriter.Write(tex, exportPath);   //保存图集
            }

            Texture2D _atlas = new Texture2D(ATLAS_MAX_SIZE, ATLAS_MAX_SIZE);

            Rect[] rects = _atlas.PackTextures(textures, 0, ATLAS_MAX_SIZE, false);    //pack!
            _atlas = AtlasOptimizer.Optimize(_atlas, rects, true);  //优化图集
            AtlasWriter.Write(_atlas, atlasPath);   //保存图集


            AssetDatabase.Refresh();

            //AssetDatabase.ImportAsset(atlasPath, ImportAssetOptions.ForceUpdate);

            bool isTurecolor = false;   //一般不使用高质量
            TextureImporterFormat format = isTurecolor ? TextureImporterFormat.ARGB32 : TextureImporterFormat.AutomaticCompressed;
            CreateMultipleSpriteImporter(atlasPath, rects,
                    textureNames,
                    _atlas.width,
                    _atlas.height,
                    format,
                    ATLAS_MAX_SIZE);

            AssetDatabase.ImportAsset(atlasPath, ImportAssetOptions.ForceUpdate);

            ////AtlasSpritePaddingHelper.WriteSpritePaddingRecord(textures_, KAssetManager.GetAtlasSpritePaddingRecordPath(atlasName_));
            //// 暂时不采用分开的纹理格式
            //EtcMaterialCreator.CreateWithoutAlpha(EtcMaterialCreator.PSD4UGUI_ONE_TEX_SHADER, atlasPath);

            string str = "<color=yellow>--->></color> 生成图集完成 !!\n" + atlasPath;
            str += string.Format("<color=#00ff00> 尺寸： {0}x{1}</color>", _atlas.width, _atlas.height);
            if (_atlas.width > FAVOR_ATLAS_SIZE || _atlas.height > FAVOR_ATLAS_SIZE)
            {
                str += string.Format("<color=#ff0000> 【警告】尺寸超{0}像素</color>", FAVOR_ATLAS_SIZE);
            }
            Debug.Log(str);

            return atlasPath;
        }


        //修正单张图片的配置
        static void CreateReadableTextureImporter(string path)
        {
            //这里是修改图片的Import Settings, 所以只能从图片只能放在Assets下

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
                Debug.Log("找不到图片:" + path);

            //importer.textureType = TextureImporterType.Default;
            importer.textureType = 0;
            importer.npotScale = TextureImporterNPOTScale.None;     //2次幂缩放
            importer.isReadable = true;
            importer.mipmapEnabled = false;
            importer.textureFormat = TextureImporterFormat.ARGB32;

            //以下为增加 jr.zeng
            importer.maxTextureSize = 8192;
            importer.textureCompression = TextureImporterCompression.Uncompressed;  //原图不压缩
            importer.filterMode = FilterMode.Point; //点对点
            //importer.alphaIsTransparency = true;    //原理是在压缩之前对贴图进行颜色放大处理来搞定边缘锯齿问题

        }



        //修正单张图集的配置
        public static void CreateMultipleSpriteImporter(string path, Rect[] rects, string[] spriteNames, int width, int height, TextureImporterFormat format, int maxSize)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            SpriteMetaData[] metaDatas = new SpriteMetaData[spriteNames.Length];
            for (int i = 0; i < metaDatas.Length; i++)
            {
                SpriteMetaData metaData = new SpriteMetaData();
                metaData.name = spriteNames[i];
                Rect rect = rects[i];
                metaData.rect = new Rect(
                        rect.xMin * width + TextureClamper.BORDER,
                        rect.yMin * height + TextureClamper.BORDER,
                        rect.width * width - TextureClamper.BORDER * 2,
                        rect.height * height - TextureClamper.BORDER * 2);

                //if (borders != null)
                //{
                //    metaData.border = borders[i];
                //}

                metaData.pivot = new Vector2(0.5f, 0.5f);
                metaDatas[i] = metaData;
            }

            importer.spritesheet = metaDatas;
            importer.maxTextureSize = maxSize;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.textureFormat = format;

            //以下为增加 jr.zeng
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.alphaIsTransparency = true;    //原理是在压缩之前对贴图进行颜色放大处理来搞定边缘锯齿问题

            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);

            settings.spriteMeshType = rects.Length > 1 ? SpriteMeshType.FullRect : SpriteMeshType.Tight;

            importer.SetTextureSettings(settings);

        }

    }

}