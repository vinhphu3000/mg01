using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class JsonFileWriter
    {
        public static void Write(string content, string path)
        {
            string jsonPath = EditorUtil.ProjectPath(path);
            if (File.Exists(jsonPath) == true)
            {
                File.Delete(jsonPath);
            }
            
            if (string.IsNullOrEmpty(content) == false)
            {
                StreamWriter sw = File.CreateText(jsonPath);
                sw.Write(content);
                sw.Close();
                AssetDatabase.ImportAsset(path);
            }
        }
    }

}