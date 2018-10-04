using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    

    public class ComponentCreatorFactory
    {
      

        static Dictionary<String, ComponentCreator> _typeCreatorDict = new Dictionary<string, ComponentCreator>();
        static Dictionary<string, ComponentCreator> _customCreatorDict = new Dictionary<string, ComponentCreator>();

        static ComponentCreatorFactory()
        {
            AddTypeCreator(new ContainerCreator());
            AddTypeCreator(new ButtonCreator());
            AddTypeCreator(new ImageCreator());
            AddTypeCreator(new InputCreator());
            AddTypeCreator(new LabelCreator());
            AddTypeCreator(new LanguageDividerCreator());
            AddTypeCreator(new ListCreator());
            AddTypeCreator(new ProgressBarCreator());
            AddTypeCreator(new ScrollViewCreator());
            AddTypeCreator(new ScrollPageCreator());
            AddTypeCreator(new MaskContainerCreator());
            AddTypeCreator(new SliderCreator());
            AddTypeCreator(new ToggleCreator());
            AddTypeCreator(new ToggleGroupCreator());
            //Custom
            AddCustomCreator(new MaskCreator());
        }

        static void AddTypeCreator(ComponentCreator creator)
        {
            _typeCreatorDict.Add(creator.Identifier, creator);
        }


        public static ComponentCreator GetTypeCreator(string type)
        {
            if (_typeCreatorDict.ContainsKey(type) == true)
            {
                return _typeCreatorDict[type];
            }
            return _typeCreatorDict[ComponentType.Container];
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽自定义Creator∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static void AddCustomCreator(ComponentCreator creator)
        {
            _customCreatorDict.Add(creator.Identifier, creator);
        }

        public static ComponentCreator GetCustomCreator(string type, string name)
        {
            string identifier = string.Concat(type, "_", name);
            if (_customCreatorDict.ContainsKey(identifier) == true)
            {
                return _customCreatorDict[identifier];
            }
            return null;
        }


    }
}