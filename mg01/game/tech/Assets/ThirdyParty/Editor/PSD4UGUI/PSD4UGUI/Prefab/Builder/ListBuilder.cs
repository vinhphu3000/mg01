/* ==============================================================================
 * ListBuilder
 * @author jr.zeng
 * 2017/8/1 17:54:42
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
    public class ListBuilder : ContainerBuilder
    {
        public const string ITEM = "item";
        public const string TEMPLATE = "template";
        public const string PARAM_PAGEABLE = "pageable";
        public const string PARAM_SHRINK = "shrink";

        private ComponentBuilder _itemBuilder;


        public override string Identifier { get { return ComponentType.List; } }

        //重写了
        protected override void BuildChildren(GameObject go)
        {
            bool has_shrink = HasParam(PARAM_SHRINK);

            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                if (child.name.Contains(ITEM) == true)
                {
                    //是列表项
                    _itemBuilder = ComponentBuilderFactory.GetBuilder(child.name);
                    _itemBuilder.Build(child);
                    if (has_shrink)
                    {
                        child.AddComponent<KButtonShrinkable>();
                        child.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                    }
                    continue;
                }

                if (child.name.Contains(TEMPLATE) == true)
                {
                    //模板不build
                    continue;
                }

                //其他情况,尝试build
                ComponentBuilder builder = ComponentBuilderFactory.GetBuilder(child.name);
                if (builder != null)
                {
                    builder.Build(child);
                }
            }
        }

        protected override void AddComponent(GameObject go)
        {
            KList list = null;
            if (HasParam(PARAM_PAGEABLE) == true)
            {
                list = go.AddComponent<KListPageable>();
            }
            else
            {
                list = go.AddComponent<KList>();
            }

            //list.ItemBuilder = _itemBuilder;  //不采用这方案
        }
    }

}