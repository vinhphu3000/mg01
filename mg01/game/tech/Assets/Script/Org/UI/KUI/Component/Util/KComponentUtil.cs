/* ==============================================================================
 * KComponentUtil
 * @author jr.zeng
 * 2017/6/16 14:44:48
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KComponentUtil
    {

        //找不到会警告
        public static GameObject NeedChild(GameObject go, string path)
        {
            Transform childTrans = go.transform.Find(path);
            if (childTrans == null)
            {
#if UNITY_EDITOR
                Log.Warn(string.Format("未找到GameObject {0} 上路径为 {1} 的次级GameObject", go.name, path));
#endif
                return null;
            }
            return childTrans.gameObject;
        }


        //找不到会警告
        public static T NeedChildComponent<T>(GameObject go, string path) where T : Component
        {
            GameObject child = NeedChild(go, path);
            if (child == null)
            {
                return null;
            }
            T component = child.GetComponent<T>();
            if (component == null)
            {
#if UNITY_EDITOR
                Log.Warn(string.Format("未找到GameObject {0} 上类型为 {1} 的Component", child.name, typeof(T)));
#endif
            }
            return component;
        }
        


        public static void AttachToParent(GameObject child, GameObject parent)
        {
            child.transform.SetParent(parent.transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
        }
        



    }

}