using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using LitJson;

using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{


    public class ScrollPageCreator : ScrollViewCreator
    {
        
        public override string Identifier  {  get  { return ComponentType.ScrollPage;  }  }

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {

            go.AddComponent<KScrollPage>();
        }

     }


}