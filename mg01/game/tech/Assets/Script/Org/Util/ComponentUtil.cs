/* ==============================================================================
 * CmptUtil
 * @author jr.zeng
 * 2017/1/2 21:51:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class ComponentUtil
    {
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="go_"></param>
        /// <returns></returns>
        static public Component Delete(Component go_, bool immediate_=false)
        {
            if (go_)
            {
                if (immediate_)
                    UnityEngine.Object.DestroyImmediate(go_);
                else
                    UnityEngine.Object.Destroy(go_);
            }
                
            return null;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="createIfNull">如没有则新建此组件</param>
        /// <returns></returns>
        //static public T GetComponent<T>(GameObject obj, bool createIfNull = false) where T : Component
        static public T GetComponent<T>(GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                //if (createIfNull)
                //component = obj.AddComponent<T>();
                return component;
            }
            return component;
        }


        /// <summary>
        /// 确保组件
        /// (已经存在则不创建)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public T EnsureComponent<T>(GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                component = obj.AddComponent<T>();
                return component;
            }
            return component;
        }

        static public Component EnsureComponent(GameObject obj, Type type_)
        {
            Component component = obj.GetComponent(type_);
            if (component == null)
            {
                component = obj.AddComponent(type_);
                return component;
            }
            return component;
        }

        static public Component EnsureComponent(GameObject obj, string type_)
        {
            Component component = obj.GetComponent(type_);
            if (component == null)
            {
                Type type = Type.GetType(type_, true);
                component = obj.AddComponent(type);
                return component;
            }
            return component;
        }



        static public T NeedComponent_<T>(GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.Assert(component, string.Format("Must has component: {0}, {1}", typeof(T), obj.name ) );
                return component;
            }
            return component;
        }


       


        /// <summary>
        /// 移除组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        static public T RemoveComponent<T>(GameObject obj, bool removeAll = false) where T : Component
        {
            if (removeAll)
            {
                Component[] coms = obj.GetComponents<T>();
                for (int i = 0; i < coms.Length; i++)
                {
                    Delete(coms[i]);
                }
            }
            else
            {
                T component = obj.GetComponent<T>();
                if (component != null)
                {
                    Delete(component);
                }
            }

            return null;
        }

        /// <summary>
        /// 递归删除所有组件
        /// </summary>
        /// <param name="root"></param>
        /// <param name="com"></param>
        /// <param name="immediate"></param>
        public static void RemoveComponentRecursively(GameObject root, Type com, bool immediate = false)
        {
            Component[] coms = root.GetComponentsInChildren(com, true);
            for (int i = 0; i < coms.Length; i++)
            {
                Delete(coms[i], immediate);
            }
        }
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽ChildComponent∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取子对象的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go_"></param>
        /// <param name="path_"></param>
        /// <returns></returns>
        public static T GetChildComponent<T>(GameObject go_, string path_) where T : Component
        {
            GameObject child = GameObjUtil.FindChild(go_, path_);
            if (child == null)
            {
                Debug.LogWarning(string.Format("miss child: {0}, {1}", go_, path_));
                return null;
            }

            T component = child.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// 为子对象添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T AddChildComponent<T>(GameObject go, string path) where T : Component
        {
            GameObject child = GameObjUtil.FindChild(go, path);
            if (child == null)
            {
                Debug.LogWarning(string.Format("miss child: {0}, {1}", go, path));
                return null;
            }
            return child.AddComponent<T>();
        }

        /// <summary>
        /// 确保子对象有组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T EnsureChildComponent<T>(GameObject go, string path) where T : Component
        {
            GameObject child = GameObjUtil.FindChild(go, path);
            if (child == null)
            {
                Debug.LogWarning(string.Format("miss child: {0}, {1}", go, path));
                return null;
            }
            T component = child.GetComponent<T>();
            if (component == null)
                component = child.AddComponent<T>();
            return component;
        }

        /// <summary>
        /// 查找孩子的组件列表
        /// (不会查找孙辈或以下, 需要的请使用GameObject.GetComponentsInChildren)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj_"></param>
        /// <param name="result_"></param>
        /// <param name="includeInactive_">包括不active的</param>
        static public List<T> FindComponentsInChildren<T>(GameObject obj_, ref List<T> result_, bool includeInactive_ = false) where T : Component
        {
            if (result_ == null)
            {
                result_ = new List<T>();
            }

            Transform trans = obj_.transform;
            Transform child;

            for (int i = 0, len = trans.childCount; i < len; ++i)
            {
                child = trans.GetChild(i);
                if (!includeInactive_)
                {
                    if (!child.gameObject.activeSelf)
                    {
                        continue;
                    }
                }

                T comp = child.GetComponent<T>();
                if (comp)
                {
                    result_.Add(comp);
                }
            }

            return result_;
        }

        /// <summary>
        /// 查找孩子的组件数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj_"></param>
        /// <param name="includeInactive_"></param>
        /// <returns></returns>
        static public T[] FindComponentsInChildren<T>(GameObject obj_, bool includeInactive_ = false) where T : Component
        {
            List<T> result = new List<T>();
            FindComponentsInChildren(obj_, ref result, includeInactive_);
            return result.ToArray();
        }

        


    }


}