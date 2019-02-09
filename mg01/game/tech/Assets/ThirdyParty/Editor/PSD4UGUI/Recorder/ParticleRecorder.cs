/* ==============================================================================
 * ParticleRecorder
 * @author jr.zeng
 * 2017/8/2 16:23:25
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using LitJson;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class ParticleRecorder : BaseRecorder
    {

        //特效的前缀
        public const string FX_TAG = "UI_FX";
        public const string FX_PREFIX = "fx_";
        public const string POS_PREFIX = "pos_";

        static ParticleRecorder _window;

        [MenuItem("PSD4UGUI/分类信息记录/特效信息记录")]
        public static void Main()
        {
            _window = EditorWindow.GetWindow<ParticleRecorder>("特效信息生成器");
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
            List<GameObject> effectList = FindAttachedEffectList(_panelPrefabObj);
            _parent2targets = GetAttachSpotEffectDict(effectList);

        }
        

        //搜索特效列表
        static List<GameObject> FindAttachedEffectList(GameObject go)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                if (child.name.StartsWith(FX_PREFIX) || child.name.StartsWith(POS_PREFIX) || child.tag == FX_TAG)
                {
                    //按名称判断是否特效
                    result.Add(child);
                }
                else
                {
                    result.AddRange(FindAttachedEffectList(child));
                }
            }
            return result;
        }

        //获取特效字典_按父节点区分
        static Dictionary<GameObject, List<GameObject>> GetAttachSpotEffectDict(List<GameObject> effectList)
        {
            //Key为特效父节点，Value为特效列表
            Dictionary<GameObject, List<GameObject>> result = new Dictionary<GameObject, List<GameObject>>();
            for (int i = 0; i < effectList.Count; i++)
            {
                GameObject effect = effectList[i];
                GameObject parent = effect.transform.parent.gameObject;
                if (result.ContainsKey(parent) == false)
                {
                    result.Add(parent, new List<GameObject>());
                }
                List<GameObject> spotEffectList = result[parent];
                spotEffectList.Add(effect);
            }
            return result;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽生成记录∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //获取ui特效存放的路径
        static string GetParticlePath(GameObject go)
        {
            return string.Format("{0}/{1}.prefab", KAssetManager.FOLDER_PARTICLE, go.name);
        }


        public static string GetRecordPath(GameObject panel)
        {
            string path = RecordFolder + "/" + panel.name + "_effect.json";
            return path;
        }



        /// <summary>
        /// 生成配置
        /// </summary>
        /// <param name="panel"></param>
        public static void GenerateRecord(GameObject panel)
        {
            Dictionary<string, List<List<string>>> path2configs = null;

            List<GameObject> effectList = FindAttachedEffectList(panel);
            if (effectList.Count > 0)
            {
                path2configs = new Dictionary<string, List<List<string>>>();
                Dictionary<GameObject, List<GameObject>> spotEffectDict = GetAttachSpotEffectDict(effectList);
                foreach (GameObject spot in spotEffectDict.Keys)
                {
                    string spotPath = GetSpotPath(spot);  //挂点路径
                    if (path2configs.ContainsKey(spotPath) == false)
                    {
                        path2configs.Add(spotPath, new List<List<string>>());
                    }

                    List<List<string>> spotEffectConfigList = path2configs[spotPath];
                    List<GameObject> spotEffectList = spotEffectDict[spot];
                    foreach (GameObject effect in spotEffectList)
                    {
                        spotEffectConfigList.Add(GetParticleConfig(effect));    //特效配置内容List<string>
                    }
                }
            }

            WriteParticleRecord(panel, path2configs);
        }

       

        //获取配置文本列表
        static List<string> GetParticleConfig(GameObject go)
        {
            string path = GetParticlePath(go);
            int index = go.transform.GetSiblingIndex();
            float x = go.transform.localPosition.x;
            float y = go.transform.localPosition.y;
            float z = go.transform.localPosition.z;

            var rect = go.GetComponent<RectTransform>();   
            if (rect != null)
            {
                //如果是ui特效，取锚定坐标
                x = rect.anchoredPosition3D.x;
                y = rect.anchoredPosition3D.y;
                z = rect.anchoredPosition3D.z;
            }

            float scaleX = go.transform.localScale.x;
            float scaleY = go.transform.localScale.y;
            float scaleZ = go.transform.localScale.z;

            float rotationX = go.transform.localRotation.x;
            float rotationY = go.transform.localRotation.y;
            float rotationZ = go.transform.localRotation.z;
            float rotationW = go.transform.localRotation.w;

            return new List<string>{path,
                                    index.ToString(),
                                    x.ToString(), y.ToString(), z.ToString(),
                                    scaleX.ToString(), scaleY.ToString(), scaleZ.ToString(),
                                    rotationX.ToString(), rotationY.ToString(), rotationZ.ToString(), rotationW.ToString()};
        }



        private static void WriteParticleRecord(GameObject root, Dictionary<string, List<List<string>>> path2configs)
        {
            string path = GetRecordPath(root);
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
            }

            if (path2configs != null)
            {
                // 有特效
                string content = GetJsonContent(path2configs);
                if (string.IsNullOrEmpty(content) == false)
                {
                    StreamWriter sw = File.CreateText(path);
                    sw.Write(content);
                    sw.Close();
                }
                AssetDatabase.ImportAsset(path);

                Debug.Log("生成记录成功！" + path);
            }
            else
            {
                Debug.Log("没发现特效， 不进行记录：" + path);
            }
        }

     
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽读取记录∽-★-∽--------∽-★-∽------∽-★-∽--------//

        

        /// <summary>
        /// 读取特效记录
        /// </summary>
        /// <param name="panel"></param>
        public static void ReadRecord(GameObject panel)
        {
            string path = GetRecordPath(panel);
            if (!File.Exists(path))
            {
                return;
            }


            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            Dictionary<string, List<List<string>>> particleAttachSpotDict = JsonMapper.ToObject<Dictionary<string, List<List<string>>>>(asset.text);
            foreach (string key in particleAttachSpotDict.Keys)
            {
                List<List<string>> configList = particleAttachSpotDict[key];
                foreach (List<string> config in configList)
                {
                    AttachParticle(panel, key, config);
                }
            }
        }

        private static void AttachParticle(GameObject panel, string attachSpotPath, List<string> particleConfig)
        {

            string path = particleConfig[0];
            int index = int.Parse(particleConfig[1]);
            float x = float.Parse(particleConfig[2]);
            float y = float.Parse(particleConfig[3]);
            float z = float.Parse(particleConfig[4]);
            float scaleX = float.Parse(particleConfig[5]);
            float scaleY = float.Parse(particleConfig[6]);
            float scaleZ = float.Parse(particleConfig[7]);
            float rotationX = float.Parse(particleConfig[8]);
            float rotationY = float.Parse(particleConfig[9]);
            float rotationZ = float.Parse(particleConfig[10]);
            float rotationW = float.Parse(particleConfig[11]);

            Transform spotTrans = panel.transform.Find(attachSpotPath);
            Debug.Assert(spotTrans, "Anchor GameObject Not Found:" + attachSpotPath);
            GameObject spot = spotTrans.gameObject;  //挂点

            //GameObject particle = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject))) as GameObject;
            //particle.transform.SetParent(spot.transform);
            //particle.transform.SetSiblingIndex(index);
            //particle.transform.localPosition = new Vector3(x, y, z);
            //var rect = particle.GetComponent<RectTransform>();
            //if (rect != null)
            //{
            //    //如果是2d特效，设置锚定坐标
            //    rect.anchoredPosition3D = new Vector3(x, y, z);
            //}
            //particle.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            //particle.transform.localRotation = new Quaternion(rotationX, rotationY, rotationZ, rotationW);
            //if (particle.name.StartsWith(ParticleRecorder.FX_PREFIX))
            //{
            //    ParticleWrapper wrapper = particle.AddComponent<ParticleWrapper>();
            //    wrapper.lifetime = GetParticleLifetime(particle);
            //}
        }

        private static float GetParticleLifetime(GameObject particle)
        {
            float result = 0;
            ParticleSystem[] particles = particle.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in particles)
            {
                result = Mathf.Max(result, p.startDelay + p.startLifetime);
            }
            return result;
        }



    }

}