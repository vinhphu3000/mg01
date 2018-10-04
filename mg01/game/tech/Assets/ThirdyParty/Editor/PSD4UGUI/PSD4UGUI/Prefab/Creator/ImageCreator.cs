using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using LitJson;
using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ImageCreator : ComponentCreator
    {

        /// <summary>
        /// 镜像图片参数，当参数为right或down时，对图片进行水平或垂直翻转
        /// </summary>
        public const string MIRROR_RIGHT = "right";
        public const string MIRROR_DOWN = "down";
        public const string ROTATION_90 = "rotation90";
        public const string ROTATION_180 = "rotation180";
        public const string ROTATION_270 = "rotation270";

        /// <summary>
        /// uv图片渐变方向参数
        /// </summary>
        public static Regex PATTERN_UV = new Regex(@"(uv)((Left2Right)|(Right2Left)|(Top2Bottom)|(Bottom2Top)|(Radial360))", RegexOptions.IgnoreCase);
        public const string PARAM_UV_LEFT2RIGHT = "uvLeft2Right";
        public const string PARAM_UV_RIGHT2LEFT = "uvRight2Left";
        public const string PARAM_UV_UP2DOWN = "uvTop2Bottom";
        public const string PARAM_UV_DOWN2UP = "uvBottom2Top";
        public const string PARAM_UV_RADIAL_360 = "uvRadial360";

        /// <summary>
        /// icon用
        /// </summary>
        public const string SHARED_ANCHOR = "sharedAnchor";

        /// <summary>
        /// 多边形参数
        /// </summary>
        public static Regex PATTERN_POLYGON = new Regex(@"(poly)\d+", RegexOptions.IgnoreCase);
        public static Regex PATTERN_SCALE_9_GRID = new Regex(@"\d+,\d+,\d+,\d+");

        /// <summary>
        /// 纯色的背景层，不需要纹理
        /// "param":"NoTex"
        /// </summary>
        public static Regex NO_TEX = new Regex(@"noTex", RegexOptions.IgnoreCase);
        public static string PARAM_NO_TEX = "noTex";


        public override string Identifier { get { return ComponentType.Image; }   }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = CreateGameObject(parent, data);

            if (IsRawData(data) == true)
            {
                AddRawImageComponent(go);
            }
            else if (HasOnlyNormalState(data) == true)
            {
                //当图片只有normal态时，直接在go上添加Image组件，不创建名为normal的子go
                AddImageComponent(go, data[STATE_NORMAL]);
            }
            else
            {
                CreateStateList(go, data);
                go.AddComponent<StateImage>();
            }

            ShowDefaultState(go);
            ApplyGameObjectParam(go, data);
            return go;
        }

        private void CreateStateList(GameObject go, JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (data[key].IsObject == true)
                {
                    JsonData stateData = data[key];
                    GameObject stateGo = CreateStateGameObject(go, stateData, key);
                    AddImageComponent(stateGo, stateData);
                    stateGo.SetActive(false);   //先全部隐藏
                }
            }
        }

        protected virtual void AddImageComponent(GameObject go, JsonData stateData)
        {
            KImage image = null;

            if (HasParam(stateData, NO_TEX) == true)     //纯色
            {
                image = go.AddComponent<KImageNoTex>();
                image.type = Image.Type.Simple;
            }
            else if (HasParam(stateData, PATTERN_SCALE_9_GRID) == true) //九宫格
            {
                image = AddScale9GridImage(go);
            }
            else if (HasParam(stateData, PATTERN_UV) == true)    //渐变
            {
                image = AddUvImage(go, stateData);
            }
            else
            {
                image = go.AddComponent<KImage>();
                image.type = Image.Type.Simple;
            }

            string link = (string)stateData["link"];    //资源路径
            if (HasParam(stateData, NO_TEX) == false)
            {
                image.sprite = KAssetManager.GetSprite(link);
                //if (LinkTextureData.GetAtlasName(link) != image.sprite.texture.name)      //引用其他part的图片,暂时不需要
                //{
                //    link = image.sprite.texture.name + "." + LinkTextureData.GetTextureName(link);
                //    link = link.Replace("UI_", "");
                //}

                image.spritePadding = AtlasSpritePaddingHelper.GetAtlasSpritePadding(link);
            }

            if (go.name.Contains(SHARED_ANCHOR) || link.Contains(SHARED_ANCHOR))
            {
                //icon用
                image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                var sc = GetSolidFillColor(stateData);
                var blendMode = GetSolidFillMode(stateData);
                if (blendMode == "normal")
                {
                    image.overlayColor = sc;
                }
                else if (blendMode == "multiply")
                {
                    image.color = sc;
                    image.overlayColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
            }
            else if (HasParam(stateData, NO_TEX))
            {
                //纯色
                image.sprite = null;
                //image.material = KAssetManager.GetNoTexMaterial();
                image.color = GetSolidFillColor(stateData); //配置颜色
                if (GetAlpha(stateData) > 0.03)
                {
                    var comNoTex = (KImageNoTex)image;
                    comNoTex.NeedColor = true;
                }
            }
            else
            {
                //image.material = KAssetManager.GetEtcMaterialByLink(link);  //设置为ui对应的材质球
                var sc = GetSolidFillColor(stateData);
                var blendMode = GetSolidFillMode(stateData);
                if (blendMode == "normal")
                {
                    image.overlayColor = sc;
                }
                else if (blendMode == "multiply")
                {
                    image.color = sc;
                    image.overlayColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                //image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }

            image.alpha = GetAlpha(stateData);  //透明度
            image.rotation = GetRotation(stateData);    //ImageType为Simple时
            image.Initialize();
        }

        //添加图片_九宫格
        private KImage AddScale9GridImage(GameObject go)
        {
            KImage image = go.AddComponent<KImage>();
            image.type = Image.Type.Sliced;
            return image;
        }

        //添加图片_填充模式
        private KImage AddUvImage(GameObject go, JsonData stateData)
        {
            KImage image = go.AddComponent<KImage>();
            image.type = Image.Type.Filled;
            //填充模式
            string param = GetParam(stateData, PATTERN_UV);
            if (param == PARAM_UV_LEFT2RIGHT)
            {
                image.fillMethod = Image.FillMethod.Horizontal;
                image.fillOrigin = (int)Image.OriginHorizontal.Left;
            }
            else if (param == PARAM_UV_RIGHT2LEFT)
            {
                image.fillMethod = Image.FillMethod.Horizontal;
                image.fillOrigin = (int)Image.OriginHorizontal.Right;
            }
            else if (param == PARAM_UV_UP2DOWN)
            {
                image.fillMethod = Image.FillMethod.Vertical;
                image.fillOrigin = (int)Image.OriginVertical.Top;
            }
            else if (param == PARAM_UV_DOWN2UP)
            {
                image.fillMethod = Image.FillMethod.Vertical;
                image.fillOrigin = (int)Image.OriginVertical.Bottom;
            }
            else if (param == PARAM_UV_RADIAL_360)
            {
                image.fillMethod = Image.FillMethod.Radial360;
                image.fillOrigin = (int)Image.Origin360.Top;
                image.fillClockwise = false;
            }
            return image;
        }


        private Image.Rotation GetRotation(JsonData stateData)
        {
            if (stateData.Keys.Contains("rotation") == true)
            {
                string rotation = (string)stateData["rotation"];
                switch (rotation)
                {
                    case ROTATION_90:
                        return Image.Rotation.Rotation90;
                    case ROTATION_180:
                        return Image.Rotation.Rotation180;
                    case ROTATION_270:
                        return Image.Rotation.Rotation270;
                    case MIRROR_RIGHT:
                        return Image.Rotation.FlipHorizontal;
                    case MIRROR_DOWN:
                        return Image.Rotation.FlipVertical;
                }
            }
            return Image.Rotation.None;
        }

        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽SolidFill∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //例子 "solidFill":{"mode":"normal","color":"E8D0AD","alpha":61}

        private Color GetSolidFillColor(JsonData stateData)
        {
            if (stateData.Keys.Contains("solidFill") == true)
            {
                //填充
                JsonData solidFillData = stateData["solidFill"];
                string mode = (string)solidFillData["mode"];
                if (mode == "normal" || mode == "multiply")
                {
                    return GetColor(solidFillData);
                }
            }
            return new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        private String GetSolidFillMode(JsonData stateData)
        {

            if (stateData.Keys.Contains("solidFill") == true)
            {
                JsonData solidFillData = stateData["solidFill"];
                return (string)solidFillData["mode"];
            }
            return "";
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽RawImage∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void AddRawImageComponent(GameObject go)
        {
            var rawimage = go.AddComponent<RawImage>();
            rawimage.material = KAssetManager.GetMaterial("Assets/Resources/Materials/UI_OneChannel.mat");
        }

        bool IsRawData(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (data[key].IsString == true && key == "param")
                {
                    string param = (string)data[key];
                    return param.Contains("rawimage");
                }
            }
            return false;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);

            if (HasParam(data, PATTERN_HIDE) == true)
            {
                go.SetActive(false);
            }

            if (HasParam(data, PATTERN_STATIC) == true)
            {
                AddBuildHelper(go, PARAM_STATIC);
            }

            if (HasParam(data, NO_TEX) == true)
            {
                AddBuildHelper(go, PARAM_NO_TEX);
            }
        }


    }

}