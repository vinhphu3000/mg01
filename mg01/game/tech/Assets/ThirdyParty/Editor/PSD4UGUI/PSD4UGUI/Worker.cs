using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

namespace Edit.PSD4UGUI
{

    public class Worker
    {

       
        /// <summary>
        /// 执行生成_文件模式
        /// </summary>
        /// <param name="jsonAssets"></param>
        /// <param name="isGenerateAtlas">是否生成图集</param>
        /// <param name="isHighQuality"></param>
        /// <param name="isBuildAssetbundle"></param>
        public static void ExecuteFileMode(JsonAsset[] jsonAssets, InputParam param_)
        {
            AtlasBatchSetting.Initialize();
            AtlasQualitySetting.Initialize();
            AtlasSpritePaddingHelper.Initialize();
            PrefabGenerator.Initialize();

            for (int i = 0; i < jsonAssets.Length; i++)
            {
                if (jsonAssets[i] != null)
                {
                    ProcessJson(jsonAssets[i], param_);
                }
            }

        }

        static void ProcessJson(JsonAsset json, InputParam param_)
        {

            StopWatch.StartST();

            try
            {
                if (param_.isGenerateAtlas)
                {
                    //需要生成图集
                    if (AtlasBatchSetting.IsAtlasNameRecord(json.name))
                    {
                        //记录已经生成过, 之后不再生成
                    }
                    else
                    {
                        AtlasBatchSetting.RecordAtlasName(json.name);
                        GenerateAtlas(json.name, param_);
                    }
                }


                GeneratePrefab(json, param_);
            }
            catch (Exception e)
            {
                throw e;
            }

            Debug.Log("导出成功： " + json.name + "  耗时:" + (StopWatch.StopST(false)/1000).ToString("f2") + " s");
        }



        /// <summary>
        /// 出错处理
        /// </summary>
        public static void HandleException()
        {
            DeleteTmpAtlasFolders();

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽生成图集∽-★-∽--------∽-★-∽------∽-★-∽--------//

        // 生成图集
        static void GenerateAtlas(string atlasName, InputParam param_)
        {
            //准备临时目录
            PrepareTmpAtlasFolders(); 

            //PrepareAtlasFolders(atlasName);   //这里不预先创建全部文件夹
            AtlasGenerator.Generate(atlasName, param_);

            //删除临时目录
            DeleteTmpAtlasFolders();
        }



        static void PrepareAtlasFolders(string atlasName)
        {

            List<string> names = AtlasBatchSetting.GetBatchedAtlasNameList(atlasName);
            for (int i = 0; i < names.Count; i++)
            {
                KAssetManager.CreateAtlasFolder(names[i]);
            }
        }

        //创建中间目录
        static void PrepareTmpAtlasFolders()
        {
            //DeleteTmpAtlasFolders();

            //mg.org.EditerUtil.EnsureDirectory(KAssetManager.TextureFolderInAsset);    //不再需要建立临时image目录

        }

        // //删除中间目录
        static void DeleteTmpAtlasFolders()
        {
            //if (Directory.Exists(KAssetManager.TextureFolderInAsset))
            //    Directory.Delete(KAssetManager.TextureFolderInAsset, true);   //不再需要建立临时image目录

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽生成预制∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static void GeneratePrefab(JsonAsset json, InputParam param_)
        {
            PrefabGenerator.Generate(json.name, param_);
        }


    }

}