/* ==============================================================================
 * GameobjUtil
 * @author jr.zeng
 * 2016/9/20 10:57:22
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{

    public class GameObjUtil
    {
        //gameobject的默认名称
        static public string GAME_OBJ_NAME_DEFAULT = "GameObject";

        //-------∽-★-∽------∽-★-∽∽-★-∽销毁相关∽-★-∽∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="go_"></param>
        /// <returns></returns>
        static public GameObject Instantiate(GameObject go_)
        {
            return GameObject.Instantiate(go_);
        }
       
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="go_"></param>
        /// <returns></returns>
        static public GameObject Delete(GameObject go_, bool now_=false)
        {
            if (go_)
            {
                if (now_)
                    GameObject.DestroyImmediate(go_);
                else
                    Object.Destroy(go_);
            }
                
            return null;
        }
        
        /// <summary>
        /// 转场不销毁
        /// </summary>
        /// <param name="go_"></param>
        static public void DontDestroyOnLoad(GameObject go_)
        {
            Object.DontDestroyOnLoad(go_);
        }
        



        //-------∽-★-∽------∽-★-∽∽-★-∽视图相关∽-★-∽∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取父级
        /// </summary>
        /// <param name="child_"></param>
        /// <returns></returns>
        static public GameObject GetParent(GameObject child_)
        {
            GameObject parent = null;
            Transform tran = child_.transform.parent;
            if (tran != null)
                parent = tran.gameObject;
            return parent;
        }


        /// <summary>
        /// 根据路径获取子对象
        /// </summary>
        /// <param name="parent_"></param>
        /// <param name="path_"></param>
        /// <param name="isRecursive">是否递归查找,此时path_应为名称而不是路径</param>
        /// <returns></returns>
        static public GameObject FindChild(GameObject parent_, string path_, bool isRecursive = false)
        {
            GameObject child = null;

            Transform tran = parent_.transform;
            Transform childTran = tran.FindChild(path_);
            if (childTran == null)
            {
                if (isRecursive)
                    childTran = FindDescendentTransform(tran, path_);
            }

            if(childTran != null)
            {
                child = childTran.gameObject;
            }
            
            return child;
        }

        static public T FindChlid<T>(GameObject parent_, string path_, bool isRecursive = false)
        {
            T cmpt = default(T);
            GameObject child = FindChild(parent_, path_, isRecursive);
            if (child != null)
            {
                cmpt = child.GetComponent<T>();
            }
            
            return cmpt;
        }

        /// <summary>
        /// 递归查找子对象
        /// </summary>
        /// <param name="searchTransform"></param>
        /// <param name="descendantName"></param>
        /// <returns></returns>
        public static Transform FindDescendentTransform(Transform searchTransform, string descendantName)
        {
            Transform result = null;

            int childCount = searchTransform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = searchTransform.GetChild(i);

                // Not it, but has children? Search the children.
                if (childTransform.name != descendantName && childTransform.childCount > 0)
                {
                    Transform grandchildTransform = FindDescendentTransform(childTransform, descendantName);
                    if (grandchildTransform == null)
                        continue;

                    result = grandchildTransform;
                    break;
                }
                // Not it, but has no children?  Go on to the next sibling.
                else if (childTransform.name != descendantName && childTransform.childCount == 0)
                {
                    continue;
                }

                // Found it.
                result = childTransform;
                break;
            }

            return result;
        }

        /// <summary>
        /// 获取子对象数组
        /// </summary>
        /// <param name="parent_"></param>
        /// <returns></returns>
        static public GameObject[] GetChildren(GameObject parent_)
        {
            Transform parent = parent_.transform;
            var len = parent.childCount;

            GameObject[] arr = new GameObject[len];

            for (int i = 0; i < len; i++)
            {
                var childObj = parent.GetChild(i).gameObject;
                arr[i] = childObj;
            }
            return arr;
        }

        /// <summary>
        /// 获取子对象列表
        /// </summary>
        /// <param name="parent_"></param>
        /// <param name="result_"></param>
        /// <returns></returns>
        static public List<GameObject> GetChildren(GameObject parent_, List<GameObject> result_)
        {
            Transform parent = parent_.transform;
            var len = parent.childCount;

            if (result_ == null)
                result_ = new List<GameObject>();

            for (int i = 0; i < len; i++)
            {
                var childObj = parent.GetChild(i).gameObject;
                result_.Add(childObj);
            }
            return result_;
        }

        /// <summary>
        /// 获取一个Transfrom下所有active=true的child
        /// </summary>
        /// <param name="parent_"></param>
        /// <returns></returns>
        static public List<GameObject> GetActiveChildren(GameObject parent_, List<GameObject> result_)
        {
            Transform parent = parent_.transform;

            if (result_ == null)
                result_ = new List<GameObject>();

            var max = parent.childCount;
            for (int idx = 0; idx < max; idx++)
            {
                var childObj = parent.GetChild(idx).gameObject;
                if (childObj.activeInHierarchy)
                {
                    result_.Add(childObj);
                } 
            }
            return result_;
        }

        /// <summary>
        /// 模糊搜索子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GameObject FuzzySearchChild(GameObject go, string childName)
        {
            string name;
            childName = childName.ToLower();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                name = child.name.ToLower();
                if (name.Contains(childName))
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// 模糊搜索所有子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="childName"></param>
        /// <param name="ret"></param>
        public static void FuzzySearchChildren(GameObject go, string childName, ref List<GameObject> ret)
        {
            string name;
            childName = childName.ToLower();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                name = go.transform.GetChild(i).name.ToLower();
                if (name.Contains(childName) == true)
                {
                    ret.Add(go.transform.GetChild(i).gameObject);
                }
                FuzzySearchChildren(go.transform.GetChild(i).gameObject, childName, ref ret);
            }
        }

        //-------∽-★-∽------∽-★-∽∽-★-∽Visible相关∽-★-∽∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 设置可见
        /// </summary>
        /// <param name="input_"></param>
        /// <param name="b_"></param>
        static public void SetVisible(GameObject go_, bool b_)
        {
            go_.SetActive(b_);
        }

        /// <summary>
        /// 获取可见
        /// </summary>
        /// <param name="input_"></param>
        /// <returns></returns>
        static public bool GetVisible(GameObject go_)
        {
            //物体本身的active状态，对应于其在inspector中的checkbox是否被勾选
            return go_.activeSelf; 
            //物体在层次中是否是active的。也就是说要使这个值为true，这个物体及其所有父物体(及祖先物体)的activeself状态都为true。
            //return go_.activeInHierarchy; 
        }

        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 创建go
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        static public GameObject CreateGameobj(string name_ = "GameObject")
        {
            GameObject obj = new GameObject(name_);
            return obj;
        }

        /// <summary>
        /// 创建go并添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name_"></param>
        /// <returns></returns>
        static public T CreateGameObj<T>(string name_ = "GameObject") where T : Component
        {
            GameObject obj = new GameObject(name_);
            T cmpt = obj.AddComponent<T>();
            return cmpt;
        }

        //static public GameObject CreateGameObj(string name_ = "GameObj", params Type[] components_)
        //{
        //    GameObject obj = new GameObject(name_, components_);
        //    return obj;
        //}

        /// <summary>
        /// 根据预制创建go
        /// </summary>
        /// <param name="prefab_"></param>
        /// <returns></returns>
        static public GameObject CreateGameObj(GameObject prefab_)
        {
            GameObject go = GameObject.Instantiate(prefab_) as GameObject;
            if (go != null)
            {
                //Transform t = go.transform;
                //Transform tPF = prefab_.transform;
                //EquateLocalMatrix(tPF, t);
            }
            else
            {
                //Log.Error("此预制不是GameObject: " + prefab.name);
                Log.Assert(false, "此预制不是GameObject: " + prefab_.name);
            }
            return go;
        }
        

        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽CreateChild∽-★-∽--------∽-★-∽------∽-★-∽--------//

        

        static public GameObject CreateChildByName(GameObject parent_, string name_)
        {
            GameObject go = GameObjUtil.FindChild(parent_, name_);
            if (go == null)
            {
                //还没创建
                go = GameObjUtil.CreateGameobj(name_);
                DisplayUtil.AddChild(parent_, go);
            }

            return go;
        }

        static public T CreateChildByName<T>(GameObject parent_, string name_) where T : Component
        {
            GameObject go = GameObjUtil.FindChild(parent_, name_);
            if (go == null)
            {
                //还没创建
                go = GameObjUtil.CreateGameobj(name_);
                DisplayUtil.AddChild(parent_, go);
            }

            T cmpt = ComponentUtil.EnsureComponent<T>(go);
            return cmpt;
        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽本地矩阵记录∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static private Vector3 __rcPos;
        static private Quaternion __rcRt;
        static private Vector3 __rcScale;

        //记录本地矩阵
        static public void RecordLocalMatrix(Transform trans_)
        {
            if (trans_ is RectTransform)
            {
                RectTransform rect = trans_ as RectTransform;
                __rcPos = rect.anchoredPosition3D; //ui的的话,记录锚点位置
            }
            else
            {
                __rcPos = trans_.localPosition;
            }

            __rcRt = trans_.localRotation;
            __rcScale = trans_.localScale;
        }
        

        //应用本地矩阵
        static public void ApplyLocalMatrix(Transform trans_)
        {
            if (trans_ is RectTransform)
            {
                RectTransform rect = trans_ as RectTransform;
                rect.anchoredPosition3D = __rcPos;
            }
            else
            {
                trans_.localPosition = __rcPos;
            }

            trans_.localRotation = __rcRt;
            trans_.localScale = __rcScale;
        }
        
        /// <summary>
        /// 改变父级
        /// </summary>
        /// <param name="child_"></param>
        /// <param name="parent_"></param>
        static public void ChangeParent(GameObject child_,  GameObject parent_)
        {
            if (child_.transform.parent == (parent_ ? parent_.transform : null))
                return;

            RecordLocalMatrix(child_.transform);
            child_.transform.SetParent( parent_ ? parent_.transform : null);
            ApplyLocalMatrix(child_.transform);
        }

        //使本地矩阵相等
        static public void EquateLocalMatrix(Transform from_, Transform to_)
        {
            from_.localPosition = to_.localPosition;
            from_.localRotation = to_.localRotation;
            from_.localScale = to_.localScale;
        }
    }

}