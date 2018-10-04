using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using LitJson;

namespace Edit.PSD4UGUI
{
    /// <summary>
    /// 指定语言专用
    /// </summary>
    public class LanguageDividerCreator : ContainerCreator
    {
        public override string Identifier { get { return ComponentType.Language; } }


        public override GameObject Create(JsonData data, GameObject parent)
        {
            if ((string)data["name"] != KAssetManager.language)
            {
                return null;
            }

            CreateChildren(parent, data);
            return parent;
        }

    }

}