/* ==============================================================================
 * FileUtil_Ed
 * @author jr.zeng
 * 2017/7/27 16:14:09
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;


namespace Edit
{
    public class EditorUtil
    {
        

        /// <summary>
        /// 项目路径
        /// </summary>
        /// <param name="fileName_"></param>
        /// <returns></returns>
        public static string ProjectPath(string fileName_ = null)
        {
            string path = Application.dataPath.Replace("Assets", "");
            if (string.IsNullOrEmpty(fileName_))
                return path;
            return Path.Combine(path, fileName_);
        }


        /// <summary>
        /// 确保路径存在
        /// </summary>
        /// <param name="path_"></param>
        public static void EnsureDirectory(string path_)
        {
            path_ = mg.org.FileUtility.FomatPath(path_);
            if (Directory.Exists(path_))
                return;

            string[] folders = path_.Split('/');
            string path = "";
            for (int i=0;i<folders.Length;++i)
            {
                path += (i==0 ? "" : "/") + folders[i];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }


    }

}