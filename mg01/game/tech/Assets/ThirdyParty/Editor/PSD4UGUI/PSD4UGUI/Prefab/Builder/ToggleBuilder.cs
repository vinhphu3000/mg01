using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ToggleBuilder : ContainerBuilder
    {

        public override string Identifier { get { return ComponentType.Toggle; } }

        protected override void AddComponent(GameObject go)
        {
            go.AddComponent<KToggle>();
        }
    }

}