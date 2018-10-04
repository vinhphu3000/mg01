using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class EtcMaterialCreator
    {
        public const string RAW_RES_PATH = "Assets/RawData";
        
        public const string PSD4UGUI_DEFAULT_SHADER = RAW_RES_PATH + "/Shaders/UI/PSD4UGUI-Default.shader";
        public const string PSD4UGUI_ONE_TEX_SHADER = RAW_RES_PATH + "/Shaders/UI/PSD4UGUI-Default-OneTex.shader";
        public static void Create(string shaderPath, string texturePath, string alphaTexturePath)
        {
            Shader shader = AssetDatabase.LoadAssetAtPath(shaderPath, typeof(Shader)) as Shader;
            Material material = new Material(shader);
            Texture2D texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
            Texture2D alphaTexture = AssetDatabase.LoadAssetAtPath(alphaTexturePath, typeof(Texture2D)) as Texture2D;
            material.SetTexture("_MainTex", texture);
            material.SetTexture("_AlphaTex", alphaTexture);
            string materialPath = texturePath.Replace(".png", "_etc.mat");
            AssetDatabase.CreateAsset(material, materialPath);
            AssetDatabase.SaveAssets();
        }

        public static void CreateWithoutAlpha(string shaderPath, string texturePath)
        {
            Shader shader = AssetDatabase.LoadAssetAtPath(shaderPath, typeof(Shader)) as Shader;
            Material material = new Material(shader);
            Texture2D texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
            material.SetTexture("_MainTex", texture);
            string materialPath = texturePath.Replace(".png", "_etc.mat");
            AssetDatabase.CreateAsset(material, materialPath);
            AssetDatabase.SaveAssets();
        }


    }

}