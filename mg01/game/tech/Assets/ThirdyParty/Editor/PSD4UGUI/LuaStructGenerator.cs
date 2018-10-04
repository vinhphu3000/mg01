/* ==============================================================================
 * LuaStructGenerator
 * @author jr.zeng
 * 2018/9/5 18:28:23
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{
    public class LuaStructGenerator
    {
        public LuaStructGenerator()
        {

        }


        [MenuItem("GameObject/标记Lua结构  %1", false, 10)]
        public static void MarkNodeBinderModel2()
        {
            GameObject[] goArr = Selection.gameObjects;

            bool all_on = true;
            foreach (GameObject go in goArr)
            {
                string name = go.name;
                int mark_index = name.IndexOf("@");
                if (mark_index < 0)
                {
                    all_on = false;
                    break;
                }
            }

            List<Object> undoList = new List<Object>();
            Dictionary<GameObject, string> go2newName = new Dictionary<GameObject, string>();

            foreach (GameObject go in goArr)
            {
                string name = go.name;
                if (all_on)
                {
                    //全部有标记,全部去掉
                    int mark_index = name.IndexOf("@");
                    name = name.Remove(mark_index);
                    go2newName[go] = name;
                    undoList.Add(go);
                }
                else
                {
                    //其中有没标记过
                    int mark_index = name.IndexOf("@");
                    if (mark_index < 0)
                    {
                        name = name + "@";
                        go2newName[go] = name;
                        undoList.Add(go);
                    }
                }

            }

            if (undoList.Count > 0)
            {
                Undo.RecordObjects(undoList.ToArray(), "rename");

                foreach (var kvp in go2newName)
                {
                    kvp.Key.name = kvp.Value;
                }
            }
        }


        [MenuItem("GameObject/生成lua结构", false, 9)]
        public static void GetNodeBinderModel()
        {
            Transform selectedTrans = Selection.activeGameObject.transform;
            string str = CopyPanelDesc(selectedTrans);

            EditorGUIUtility.systemCopyBuffer = str;
            Debug.Log("复制成功，ctrl+v粘贴使用 ");
        }


         static string CopyPanelDesc(Transform root)
        {
            List<PanelNode> nodes = new List<PanelNode>();
            dfs_traverse(root, "", nodes);
            List<string> strs = new List<string>();
            foreach (var node in nodes)
            {
                strs.Add(node.GetNodeDesc());
            }
            string desc = "{\n\t" + string.Join("\n\t", strs.ToArray()) + "\n}";
            return desc;
           
        }

         static void dfs_traverse(Transform trans, string path, List<PanelNode> nodes)
        {
            //transName@varName, 例如Container_hairstyle@style_tab1
            string[] names = trans.name.Split('@');
            string transName = names[0];

            //只导出带@的节点
            if (names.Length > 1)
            {
                string userName = names[1];
                var node = new PanelNode()
                {
                    varName = userName.Length > 0 ? userName : transName.ToLower(),
                    path = path,
                };
                nodes.Add(node);
            }

            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                dfs_traverse(child, string.Format("{0}{1}", (string.IsNullOrEmpty(path) ? "" : path + "/"), GetTransName(child)), nodes);
            }
        }

         static string GetTransName(Transform trans)
        {
            return trans.name.Split('@')[0];
        }

         class PanelNode
        {
            public string varName;
            public string path;
            public string GetNodeDesc()
            {
                return string.Format("{0} = \"{1}\",", varName, path);
            }
        }


    }

}
  