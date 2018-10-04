/* ==============================================================================
 * FileUtil
 * @author jr.zeng
 * 2016/6/15 17:58:58
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;

namespace mg.org
{
    //PS: NGUI和UnityEditor里有FileUtil,因此不能重名
    public class FileUtility
    {

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        static public string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }


        /// <summary>
        /// 格式化路径
        /// </summary>
        /// <param name="path_"></param>
        /// <returns></returns>
        static public string FomatPath(string path_)
        {
            //var newFilePath1 = path_.Replace("\\", "/");
            //var newFilePath2 = newFilePath1.Replace("//", "/").Trim();
            //newFilePath2 = newFilePath2.Replace("///", "/").Trim();
            //newFilePath2 = newFilePath2.Replace("\\\\", "/").Trim();
            return path_.Replace("\\", "/");
        }


        /// <summary>
        /// 根据路径获取文件名
        /// </summary>
        /// <param name="filePath_"></param>
        /// <param name="suffix_">修改后缀</param>
        /// <returns></returns>
        static public string GetNameFromFullPath(string filePath_, string suffix_ = null)
        {
            filePath_ = FomatPath(filePath_);
            string name = "";

            int lastSlashIndex = filePath_.LastIndexOf("/");
            if (suffix_ == null)
            {
                //保留后缀
                name = filePath_.Substring(lastSlashIndex + 1);
            }
            else
            {
                //替换后缀
                int lastDotIndex = filePath_.LastIndexOf(".");
                if(lastDotIndex >= 0)
                {
                    name = filePath_.Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1);
                }
                else
                {
                    name = filePath_.Substring(lastSlashIndex + 1);
                }
                name += suffix_;
            }

            return name;
        }

        /// <summary>
        /// 获取文件所在的文件夹路径
        /// </summary>
        /// <param name="filePath_"></param>
        /// <returns></returns>
        static public string GetFolderFromFullPath(string filePath_)
        {
            filePath_ = FomatPath(filePath_);
            int lastSlashIndex = filePath_.LastIndexOf("/");
            string path = filePath_.Substring(0, lastSlashIndex);
            return path;
        }


        /// <summary>
        /// 格式化文件名
        /// </summary>
        /// <param name="path_"></param>
        /// <param name="suffix_"></param>
        /// <param name="extra_"></param>
        /// <returns></returns>
        static public string ModifyFileName(string path_, string suffix_, string extra_ = null)
        {
            string fileName = FomatPath(path_);
            string fileSuffix = "";

            int index = path_.IndexOf(".");
            if (index >= 0)
            {
                fileName = path_.Substring(0, index);
                fileSuffix = path_.Substring(index);
            }

            if (extra_ != null)
            {
                fileName = fileName + extra_;
            }

            if (suffix_ != null)
            {
                int i = suffix_.IndexOf(".");
                if (i >= 0)
                {
                    fileName = fileName + suffix_;
                }
                else
                {
                    if (!string.IsNullOrEmpty(suffix_))
                    {
                        fileName = fileName + "." + suffix_;
                    }
                }
            }
            else
            {
                fileName = fileName + fileSuffix;
            }

            return fileName;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽文件操作相关∽-★-∽--------∽-★-∽------∽-★-∽--------//
        

        /// <summary>
        /// 确保路径存在
        /// </summary>
        /// <param name="path_"></param>
        public static void EnsureDirectory(string path_)
        {
            path_ = FileUtility.FomatPath(path_);
            if (Directory.Exists(path_))
                return;

            string[] folders = path_.Split('/');
            string path = "";
            for (int i = 0; i < folders.Length; ++i)
            {
                path += (i == 0 ? "" : "/") + folders[i];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path_"></param>
        public static void RemoveDirectory(string path_)
        {
            if (!Directory.Exists(path_))
                return;
            Directory.Delete(path_, true);
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽各种路径∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取可写目录(沙盒目录)
        /// </summary>
        /// <returns></returns>
        public static string WritablePath(string fileName_ = null)
        {
            string path;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

            path = Application.streamingAssetsPath;
#else
            path = Application.persistentDataPath;
#endif
            if (fileName_ != null)
                path = Path.Combine(path, fileName_);

            //return FomatPath(path);
            return path;
        }
        
        /// <summary>
        /// StreamingAssets目录(此目录为只读)
        /// </summary>
        /// <param name="fileName_"></param>
        /// <returns></returns>
        public static string StreamAssetsPath(string fileName_ = null)
        {
            string path;

#if UNITY_ANDROID
            path = Application.dataPath +"!assets";
#else
            path = Application.streamingAssetsPath;
#endif
            if (fileName_ != null)
            {
                path = Path.Combine(path, fileName_);
            }

            //return FomatPath(path);
            return path;
        }


        public static string StreamAssetsPath4WWW(string fileName_ = null)
        {
            string path;

#if UNITY_ANDROID && !UNITY_EDITOR
		    path = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IPHONE && !UNITY_EDITOR
		    path ="file://" +  Application.dataPath + "/Raw";
#else
            path = "file://" + Application.streamingAssetsPath;
#endif
            if (fileName_ != null)
            {
                path = Path.Combine(path, fileName_);
            }

            //return FomatPath(path);
            return path;
        }



    }

}