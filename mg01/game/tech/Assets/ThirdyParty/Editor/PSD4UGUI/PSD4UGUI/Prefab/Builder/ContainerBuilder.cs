/* ==============================================================================
 * ContainerBuilder
 * @author jr.zeng
 * 2017/8/1 16:43:11
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class ContainerBuilder : ComponentBuilder
    {

        public const string PARAM_DEFERRED = "deferred";


        public override string Identifier { get { return ComponentType.Container; } }


        public override void Build(GameObject go, bool applyDeferred = true)
        {
            RefreshParamList(go);

            BuildChildren(go);
            AddComponent(go);   //为传入对象添加组件 
        }

        protected virtual void BuildChildren(GameObject go)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                ComponentBuilder builder = ComponentBuilderFactory.GetBuilder(child.name);
                if (builder != null)
                {
                    builder.Build(child);
                }
            }
        }

        protected virtual void AddComponent(GameObject go)
        {
            //go.AddComponent<KContainer>();   //不再默认添加KContainer
        }


    }

}