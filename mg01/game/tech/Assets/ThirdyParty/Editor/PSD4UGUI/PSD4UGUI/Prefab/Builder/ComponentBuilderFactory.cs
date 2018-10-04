using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{

    /// <summary>
    /// 组件类型
    /// </summary>
    public class ComponentType
    {
        public static string None = "None";
        public static string Container = "Container";
        public static string Button = "Button";
        public static string Image = "Image";
        public static string Input = "Input";
        public static string Label = "Label";
        public static string Language = "Language";
        public static string List = "List";
        public static string ProgressBar = "ProgressBar";
        public static string ScrollView = "ScrollView";
        public static string ScrollPage = "ScrollPage";
        public static string MaskContainer = "MaskContainer";
        public static string Slider = "Slider";
        public static string Toggle = "Toggle";
        public static string ToggleGroup = "ToggleGroup";

        public static string Image_mask = "Image_mask";
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽ComponentBuilders∽-★-∽--------∽-★-∽------∽-★-∽--------//
    
    public class ComponentBuilderFactory
    {

        static Dictionary<string, ComponentBuilder> _typeDict = new Dictionary<string, ComponentBuilder>();
        static Dictionary<string, ComponentBuilder> _customDict = new Dictionary<string, ComponentBuilder>();


        static ComponentBuilderFactory()
        {
            AddTypeBuilder(new ContainerBuilder());
            //AddTypeBuilder(new ButtonBuilder());
            //AddTypeBuilder(new ImageBuilder());
            //AddTypeBuilder(new InputBuilder());
            //AddTypeBuilder(new LabelBuilder());
            AddTypeBuilder(new ListBuilder());
            //AddTypeBuilder(new ProgressBarBuilder());
            //AddTypeBuilder(new ScrollViewBuilder());
            //AddTypeBuilder(new ScrollPageBuilder());
            //AddTypeBuilder(new SliderBuilder());
            //AddTypeBuilder(new ToggleBuilder());
            //AddTypeBuilder(new ToggleGroupBuilder());
            //AddTypeBuilder(new MaskContainerBuilder());
            //AddTypeBuilder(new ParticleBuilder());    //稍后处理

            //Custom
            //AddCustomBuilder(new MaskBuilder());

        }



        public static void AddTypeBuilder(ComponentBuilder builder)
        {
            _typeDict.Add(builder.Identifier, builder);
        }



        public static ComponentBuilder GetBuilder(string name)
        {
            ComponentBuilder result;
            _customDict.TryGetValue(name, out result);
            if (result != null)
            {
                return result;
            }
            
            string type = GetTypeByName(name);
            if (type == null)
            {
                return null;
            }

            _typeDict.TryGetValue(type, out result);
            if (result != null)
            {
                return result;
            }

            //不符合命名规范的节点直接略过，不处理
            return _typeDict["Container"];
        }

        //根据名称获取组件类型
        private static string GetTypeByName(string name)
        {
            int index = name.IndexOf("_");
            if (index < 0)
                return null;

            string type = name.Substring(0, index);
            return type;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽自定义Builder∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public static void AddCustomBuilder(ComponentBuilder builder)
        {
            _customDict.Add(builder.Identifier, builder);
        }


    }

}