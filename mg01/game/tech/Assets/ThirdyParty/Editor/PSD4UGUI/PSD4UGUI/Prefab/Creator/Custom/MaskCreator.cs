/* ==============================================================================
 * MaskCreator
 * @author jr.zeng
 * 2017/8/1 15:17:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using LitJson;

using UnityEngine;
using UnityEngine.UI;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class MaskCreator : ComponentCreator
    {

        public static Regex PARAM_CLIP_EFFECT = new Regex(@"effectClip", RegexOptions.IgnoreCase);
        public static Regex PARAM_MASK_STENCIL = new Regex(@"stencilMask", RegexOptions.IgnoreCase);

        public override string Identifier { get { return ComponentType.Image_mask; } }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = CreateGameObject(parent, data);
            AddMask(go, data);
            ApplyGameObjectParam(go, data);
            return go;
        }


        private void AddMask(GameObject go, JsonData data)
        {
            if (HasParam(data, PARAM_CLIP_EFFECT))
            {
                go.AddComponent<ClipMask>();
            }
            if (HasParam(data, PARAM_MASK_STENCIL))
            {
                var mask = go.AddComponent<Mask>();
                KImage image = go.AddComponent<KImage>();
                string link = (string)(data["normal"]["link"]);
                image.sprite = KAssetManager.GetSprite(link);
                //image.material = KAssetManager.GetEtcMaterialByLink(link);
                //image.color = new Color(0, 0, 0, 0);
                Vector4 border = image.sprite.border;
                if (border.x > 0 || border.y > 0 || border.z > 0 || border.w > 0)
                {
                    image.type = Image.Type.Sliced;
                }
                mask.showMaskGraphic = false;
            }
            else
            {
                go.AddComponent<KImageNoTex>();  //RectMask2D还需要一张白图接受点击事件，否则scrollview不法拖动
                go.AddComponent<RectMask2D>();
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {
            base.ApplyGameObjectParam(go, data);
        }

    }

}