/* ==============================================================================
 * AnimatorRecorder
 * @author jr.zeng
 * 2019/2/5 23:38:24
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using LitJson;

using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{

    public class AnimatorRecorder : BaseRecorder
    {

        static AnimatorRecorder _window;

        [MenuItem("PSD4UGUI/分类信息记录/动画信息记录")]
        public static void Main()
        {
            _window = EditorWindow.GetWindow<AnimatorRecorder>("动画信息生成器");
            _window.Show();

        }


        //生成记录
        protected override void OnClickGenerate()
        {
            GenerateRecord(_panelPrefabObj);

        }




        //-------∽-★-∽------∽-★-∽--------∽-★-∽目标位置∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected override void OnClickTargets()
        {
            _parent2targets = GenTargetDic(_panelPrefabObj, new Dictionary<GameObject, List<GameObject>>() );

        }

        static List<Animator> FindAnimatorList(GameObject go)
        {
            List<Animator> result = new List<Animator>();
            Animator[] anims = go.GetComponentsInChildren<Animator>(true);
            if (anims.Length > 0)
            {
                foreach (var animObj in anims)
                {
                    result.Add(animObj);
                }
            }
            return result;
        }

        static List<Animation> FindAnimationList(GameObject go)
        {
            List<Animation> result = new List<Animation>();
            Animation[] anims = go.GetComponentsInChildren<Animation>(true);
            if (anims.Length > 0)
            {
                foreach (var animObj in anims)
                {
                    result.Add(animObj);
                }
            }
            return result;
        }

        public static Dictionary<GameObject, List<GameObject>> GenTargetDic(GameObject panel, Dictionary<GameObject, List<GameObject>> output )
        {
            List<Animator> animators = FindAnimatorList(panel);
            List<Animation> animations = FindAnimationList(panel);
            
            for (int i = 0; i < animators.Count; i++)
            {
                Animator obj = animators[i];
                GameObject parent = obj.transform.parent.gameObject;
                if (!output.ContainsKey(parent))
                    output.Add(parent, new List<GameObject>());
                List<GameObject> targetList = output[parent];
                if (!targetList.Contains(obj.gameObject))
                    targetList.Add(obj.gameObject);
            }

            for (int i = 0; i < animations.Count; i++)
            {
                Animation obj = animations[i];
                GameObject parent = obj.transform.parent.gameObject;
                if (!output.ContainsKey(parent))
                    output.Add(parent, new List<GameObject>());
                List<GameObject> targetList = output[parent];
                if (!targetList.Contains(obj.gameObject))
                    targetList.Add(obj.gameObject);
            }

            return output;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽生成记录∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public static string GetRecordPath(GameObject panel)
        {
            string path = RecordFolder + "/" + panel.name + "_anim.json";
            return path;
        }


        public static void GenerateRecord(GameObject panel)
        {
          
            Dictionary<string, List<List<string>>> path2list = new Dictionary<string, List<List<string>>>();

            List<Animator> animators = FindAnimatorList(panel);
            for (int i = 0; i < animators.Count; i++)
            {
                Animator animObj = animators[i];

                string spotPath = GetSpotPath(animObj.gameObject);
                if(!path2list.ContainsKey(spotPath))
                    path2list.Add(spotPath, new List<List<string>>() );
                var list = path2list[spotPath];

                var cfgList = GenConfigList(animObj);
                if(cfgList != null)
                    list.Add(cfgList);
            }

            List<Animation> animations = FindAnimationList(panel);
            for (int i = 0; i < animations.Count; i++)
            {
                Animation animObj = animations[i];

                string spotPath = GetSpotPath(animObj.gameObject);
                if (!path2list.ContainsKey(spotPath))
                    path2list.Add(spotPath, new List<List<string>>() );
                var list = path2list[spotPath];

                var cfgList = GenConfigList(animObj);
                if (cfgList != null)
                    list.Add(cfgList);
            }

            string path = GetRecordPath(panel);
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
            }

            string content = GetJsonContent(path2list);
            if (string.IsNullOrEmpty(content) == false)
            {
                StreamWriter sw = File.CreateText(path);
                sw.Write(content);
                sw.Close();
                AssetDatabase.ImportAsset(path);
                Debug.Log("生成动画记录成功！" + path);
            }

        }

        static List<string> GenConfigList(Animator anim)
        {
            List<string> ret = null;

            var ctrl = anim.runtimeAnimatorController;
            if (ctrl)
            {
                ret = new List<string> {
                            "Animator",
                            AssetDatabase.GetAssetPath(ctrl)
                };
            }

            return ret;
        }

        static List<string> GenConfigList(Animation anim)
        {
            List<string> ret = null;

            return ret;
        }

        public static void ReadRecord(GameObject panel)
        {
            string path = GetRecordPath(panel);
            if (!File.Exists(path))
            {
                return;
            }

            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            var animDict = JsonMapper.ToObject< Dictionary<string, List<List<string>> > >(asset.text);

            foreach (string goPath in animDict.Keys)
            {
                List<List<string>> animList = animDict[goPath];

                var c = panel.transform.FindChild(goPath);
                Debug.Assert(c, "Miss Gameobject: " + goPath);

                foreach(var kvp in animList)
                {
                    string tp = kvp[0];
                    if (tp == "Animator")
                    {
                        string ctrl_path = kvp[1];

                        try
                        {
                            var anim = c.GetComponent<Animator>();
                            if (anim == null)
                            {
                                anim = c.gameObject.AddComponent<Animator>();
                                //c.gameObject.AddComponent<AnimEventHandler>();
                            }
                            var ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(ctrl_path);
                            anim.runtimeAnimatorController = ctrl;
                            Debug.Log("Animator: " + ctrl_path + " -> " + goPath);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                            Debug.LogError("Panel Attach Animator Error :" + ctrl_path);
                        }

                    }
                    else if(tp == "Animation")
                    {

                    }

                }
                    
            }

         }


    }
}