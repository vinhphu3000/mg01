/* ==============================================================================
 * 打包配置读入
 * @author jr.zeng
 * 2017/11/27 16:41:00
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Edit.Bundle
{
    public class AbsConfigRead
    {
        //配置文件路径
        static string CFG_FILE_NAME = "abs_config.xls";
        static string CFG_FILE_SHEET = "ABsInfo";

        /// <summary>
        /// excel里的列id
        /// </summary>
        public enum ExcelRID
        {
            ID = 0,
            Path,       //打包路径
            Suffixs,    //需要打包的文件后缀
            BundleName, //包名
            PackType,   //打包方式
            Exclude,    //排除项
            CombineNum, //分包数

            Depends,    //预留
            AsScene,    //预留
        }




        public AbsConfigRead()
        {

        }

        public static List<BundleGenInfo> ReadConfig(List<BundleGenInfo> genInfos)
        {
            string excelPath = Path.Combine(BundleBuilder.PATH_GEN_CFG, CFG_FILE_NAME);
            return ReadConfig(excelPath, genInfos);
        }

        public static List<BundleGenInfo> ReadConfig(string excelPath_, List<BundleGenInfo> genInfos)
        {
            FileStream stream = File.Open(excelPath_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            ReadExcelConfig(stream, CFG_FILE_SHEET, genInfos);
            return genInfos;
        }


        static void ReadExcelConfig(FileStream stream, string sheetName, List<BundleGenInfo> genInfos)
        {

            Dictionary<string, BundleGenInfo> name2genInfo = new Dictionary<string, BundleGenInfo>();

            /// 读取excel配置
            IWorkbook workbook = new HSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet(sheetName);

            int rowCount = sheet.LastRowNum;
            for (int i = 1; i <= rowCount; ++i)     //数据从第二行开始
            {
                IRow row = sheet.GetRow(i); //获取行数据
                if (row == null)
                    break;

                ICell pathCell = row.GetCell((int)ExcelRID.Path);
                if (pathCell == null || string.IsNullOrEmpty(pathCell.StringCellValue) )
                    break;

                //打包路径
                string path = pathCell.StringCellValue;
                
                ICell bundleNameCell = row.GetCell((int)ExcelRID.BundleName);
                if (bundleNameCell == null || string.IsNullOrEmpty(bundleNameCell.StringCellValue))
                    break;

                //包名
                string bundleName = bundleNameCell.StringCellValue;

                BundleGenInfo genInfo;
                if (name2genInfo.ContainsKey(bundleName))
                {
                    genInfo = name2genInfo[bundleName];     //同一包名的项收集在一个genInfo里
                }
                else
                {
                    genInfo = new BundleGenInfo();
                    //包名
                    genInfo.bundleName = bundleName;
                    //打包方式
                    genInfo.packType = row.GetCell((int)ExcelRID.PackType).StringCellValue; //因此同一个包名的打包方式应该一致
                    //分包数
                    genInfo.combineNum = 1;
                    
                    //作为场景打包
                    //var isSceneCell = row.GetCell((int)ExcelRID.AsScene);
                    //if (isSceneCell != null && !string.IsNullOrEmpty(isSceneCell.StringCellValue))
                    //{
                    //    genInfo.isScene = true;
                    //}

                    name2genInfo[bundleName] = genInfo;
                }

                BundleGenInfo.PathInfo pathInfo = new BundleGenInfo.PathInfo();
                genInfo.pathInfos.Add(pathInfo);

                pathInfo.path = path.Trim();
                
                //ID(并没有作用)
                string idStr = row.GetCell((int)ExcelRID.ID).StringCellValue;
                if (!string.IsNullOrEmpty(idStr))
                {
                    pathInfo.id = idStr.Trim();   //去掉边缘空格
                }

                var combineNumCell = row.GetCell((int)ExcelRID.CombineNum);
                if (combineNumCell != null && !string.IsNullOrEmpty(combineNumCell.StringCellValue))
                {
                    pathInfo.combineNum = int.Parse(combineNumCell.StringCellValue);
                    pathInfo.combineNum = Math.Max(pathInfo.combineNum, 1);

                    genInfo.combineNum = pathInfo.combineNum;
                }


                //打包的后缀名
                string suffixStr = row.GetCell((int)ExcelRID.Suffixs).StringCellValue;
                if (!string.IsNullOrEmpty(suffixStr))
                {
                    pathInfo.suffixs = suffixStr.Split(',');
                    for (int j = 0; j < pathInfo.suffixs.Length; ++j)
                        pathInfo.suffixs[j] = pathInfo.suffixs[j].Trim();   //去掉边缘空格
                }

                //排除的文件/目录名称
                string exclueStr = row.GetCell((int)ExcelRID.Exclude).StringCellValue;
                if (!string.IsNullOrEmpty(exclueStr))
                {
                    pathInfo.exclude = exclueStr.Split(',');
                    for (int j = 0; j < pathInfo.exclude.Length; ++j)
                        pathInfo.exclude[j] = pathInfo.exclude[j].Trim();   //去掉边缘空格
                }

                if (!genInfos.Contains(genInfo))    //保证只有一个
                    genInfos.Add(genInfo);

            }


            workbook.Close();
        }





    }

}