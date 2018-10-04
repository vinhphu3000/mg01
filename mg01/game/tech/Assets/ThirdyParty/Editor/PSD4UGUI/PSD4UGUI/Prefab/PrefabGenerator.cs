using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEditor.Animations;

using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class PrefabGenerator
    {
        //canvas的默认sortingOrder
        public const int DEFAULT_SORTING_ORDER = 100;

        public static void Initialize()
        {

        }


        public static string Generate(string jsonName, InputParam param_)
        {
            JsonData data = KAssetManager.GetUIJsonData(jsonName);

            //1.创建canvas
            GameObject root = new GameObject("Canvas_" + jsonName);
            root.layer = LayerMask.NameToLayer("UI");
            AddCanvas(root, jsonName);  //canvas组件

            //2.构建go
            ComponentCreator creator = ComponentCreatorFactory.GetTypeCreator("Container");
            GameObject panel = creator.Create(data, root);  //创建对象
            AddPanelToRoot(panel, root);

            //3.执行builder
            //AddPanelBuilder(root);  
            //RemoveAllBuildHelper(panel);

            //4.创建预制
            GenPrefabs(root, param_.isBuildAssetbundle);
            
            return string.Empty;
        }


        //添加canvas
        static void AddCanvas(GameObject go, string name)
        {
            Canvas canvas = go.AddComponent<Canvas>();
            canvas.sortingOrder = DEFAULT_SORTING_ORDER;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            canvas.planeDistance = 100;
            canvas.pixelPerfect = false;
            CanvasScaler canvasScaler = go.AddComponent<CanvasScaler>();
            //canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(KUIConst.DESIGN_WIN_SIZE.x, KUIConst.DESIGN_WIN_SIZE.y);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand; //CanvasScaler.ScreenMatchMode.MatchWidthOrHeight; 
            canvasScaler.matchWidthOrHeight = 1.0f;

            //if (!NonInteractCanvas.Contains(name.ToLower()))
            //    go.AddComponent<GraphicRaycaster>();
            go.AddComponent<GraphicRaycaster>();    //射线接收，用于点击事件
        }

        //添加到根容器
        static void AddPanelToRoot(GameObject panel, GameObject root)
        {
            panel.transform.SetParent(root.transform);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Builder∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static void AddPanelBuilder(GameObject panel)
        {
            GameObject go = panel.transform.GetChild(0).gameObject; //从子对象开始build
            ComponentBuilder builder = ComponentBuilderFactory.GetBuilder(go.name);
            builder.Build(go);
        }

        //删除所有BuildHelper
        static void RemoveAllBuildHelper(GameObject panel)
        {
            ComponentUtil.RemoveComponentRecursively(panel, typeof(BuildHelper), true);
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Prefab∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //生成预制
        static void GenPrefabs(GameObject root, bool isBuildAssetBundle)
        {

            Dictionary<GameObject, GameObject> go2parent = null;    //记录父节点，方便还原

            //List<GameObject> toGenerate = GetPrefabList(root, jsonName);
            List<GameObject> toGenerate = GetPrefabList(root, root.name);

            for (int i = toGenerate.Count-1;i>=0;--i)
            {
                GameObject go = toGenerate[i];
                if (go.transform.parent)
                {
                    //需要export
                    GameObject parent = go.transform.parent.gameObject;

                    if (go2parent == null)
                        go2parent = new Dictionary<GameObject, GameObject>();
                    go2parent[go] = parent;

                    var pos = go.GetComponent<RectTransform>().anchoredPosition3D;
                    go.transform.parent = null; //放到根目录
                    go.GetComponent<RectTransform>().anchoredPosition3D = pos;

                }

                //保存预制文件
                CreatePrefabFile(go, isBuildAssetBundle);
            }

            if (go2parent != null)
            {
                List<KeyValuePair<GameObject, GameObject>> list = new List<KeyValuePair<GameObject, GameObject>>(go2parent);
                list.Reverse();
                foreach (var kvp in list)
                {
                    //移回原来的地方,这样比较方便写界面逻辑
                    GameObjUtil.ChangeParent(kvp.Key, kvp.Value); 
                }
            }
        }


        //获取ui实际输出的预制列表(有export标记的独立输出)
        static List<GameObject> GetPrefabList(GameObject root, string parentPanelName)
        {
            string export_flag = "export";

            List<GameObject> ret = new List<GameObject>();
            ret.Add(root);  //默认添加根对象

            List<GameObject> toExport = new List<GameObject>();
            GameObjUtil.FuzzySearchChildren(root, export_flag, ref toExport);  //名称有export就是额外导出
            //toExport.Clear();
            foreach (GameObject go in toExport)
            {
                int start = go.name.IndexOf(export_flag);  //Container_exportPart2

                //string name = go.name.Substring(0, start) + go.name.Substring(start + 6) + "__" + parentPanelName;
                string name = parentPanelName + "__" + go.name.Substring(0, start) + go.name.Substring(start + export_flag.Length);  //Canvas_Bag__Container_Part2
                go.name = name;
                ret.Add(go);
            }
            return ret;
        }



        //保存预制文件
        static void CreatePrefabFile(GameObject panel, bool isBuildAssetBundle)
        {
            //读取记录
            PerfabRecorder.ReadRecord(panel);
            
            panel.SetActive(false); //为啥一定要隐藏


            string path = string.Format("{0}/{1}.prefab", KAssetManager.PrefabFolder, panel.name);
            GameObject go = PrefabUtility.CreatePrefab(path, panel, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            AssetDatabase.SaveAssets();

            if (isBuildAssetBundle == true)
            {
                //call build asset bundle API here
                //创建Bundle
            }

            panel.SetActive(true);


            Selection.activeGameObject = go;    //选中预制
            //lss.editor.GenerateUIWrapper.DoGenGUIWrapper(true);   //不需要搞这种了
        }


      

    }

}