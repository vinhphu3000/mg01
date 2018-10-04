using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{
    public class TextureImporterUtil
    {
        public static void CreateReadableTextureImporter(string path)
        {
            //这里是修改图片的Import Settings, 所以只能从图片只能放在Assets下

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter; 
            if (importer == null)
                Debug.Log("找不到图片:" + path);

            //importer.textureType = TextureImporterType.Default;
            importer.textureType = 0;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.isReadable = true;
            importer.mipmapEnabled = false;
            importer.textureFormat = TextureImporterFormat.ARGB32;
        }

        public static void CreateEtcColorChannelImporter(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.textureFormat = GetTextureFormat();
        }

        public static void CreateEtcAlphaChannelImporter(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            //importer.textureType = TextureImporterType.Default;
            importer.textureType = 0;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.textureFormat = GetTextureFormat();
        }

        /// <summary>
        /// 根据目标平台选择不同压缩格式
        /// </summary>
        /// <returns></returns>
        public static TextureImporterFormat GetTextureFormat()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.WebPlayer:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return TextureImporterFormat.DXT1;
                case BuildTarget.Android:
                    return TextureImporterFormat.ETC_RGB4;
                case BuildTarget.iOS:
                    return TextureImporterFormat.PVRTC_RGB4;
            }
            return TextureImporterFormat.DXT1;
        }

        /// <summary>
        /// 设置图集的import settings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rects"></param>
        /// <param name="spriteNames"></param>
        /// <param name="borders"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        /// <param name="maxSize"></param>
        public static void CreateMultipleSpriteImporter(string path, Rect[] rects, string[] spriteNames, Vector4[] borders, int width, int height, TextureImporterFormat format, int maxSize)
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
                        rect.height * height - TextureClamper.BORDER * 2 );

                if (borders != null)
                {
                    metaData.border = borders[i];
                }

                metaData.pivot = new Vector2(0.5f, 0.5f);
                metaDatas[i] = metaData;
            }
            importer.spritesheet = metaDatas;
            importer.maxTextureSize = maxSize;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.textureFormat = format;
        }

        // 挪几个像素 mixi在ChangeTextureSize里用
        public static void ChangeSpriteImporter(string path, int offfsetX, int offfsetY)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            SpriteMetaData[] metaDatas = importer.spritesheet;
            if (metaDatas.Length > 0)
            {
                for (int i = 0; i < metaDatas.Length; i++)
                {
                    SpriteMetaData metaData = metaDatas[i];
                    Rect rect = metaData.rect;
                    metaDatas[i].rect = new Rect(rect.x + offfsetX, rect.y + offfsetY, rect.width, rect.height);
                }
            }
            else
            {
                importer.spriteImportMode = SpriteImportMode.Multiple;
                Texture2D tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                SpriteMetaData metaData = new SpriteMetaData();
                metaData.name = tex.name;
                metaData.rect = new Rect(offfsetX, offfsetY, tex.width, tex.height);
                metaData.pivot = new Vector2(0.5f, 0.5f);

                metaData.border = importer.spriteBorder;

                metaDatas = new SpriteMetaData[] { metaData };
            }

            importer.spritesheet = metaDatas;
        }

    }

}