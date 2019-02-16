/* ==============================================================================
 * Lua_UnityEngine_GameObject_Manual
 * @author jr.zeng
 * 2018/5/5 21:09:45
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using SLua;
using mg.org;
using mg.org.KUI;

[OverloadLuaClass(typeof(GameObject))]
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GameObject_Manual : LuaObject
{

    [SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetComponent(IntPtr l)
    {
        try
        {
            int argc = LuaDLL.lua_gettop(l);
            if (matchType(l, argc, 2, typeof(string)))
            {
                UnityEngine.GameObject self = (UnityEngine.GameObject)checkSelf(l);
                System.String a1;
                checkType(l, 2, out a1);
                var ret = self.GetComponent(a1);
                pushValue(l, true);
                pushValue(l, ret);
                return 2;
            }
            else if (matchType(l, argc, 2, typeof(System.Type)))
            {
                UnityEngine.GameObject self = (UnityEngine.GameObject)checkSelf(l);
                System.Type a1;
                checkType(l, 2, out a1);
                var ret = self.GetComponent(a1);
                pushValue(l, true);
                pushValue(l, ret);
                return 2;
            }
            pushValue(l, false);
            LuaDLL.lua_pushstring(l, "No matched override function GetComponent to call");
            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽new∽-★-∽--------∽-★-∽------∽-★-∽--------//


    /// <summary>
    /// 必须要有指定控件
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int NeedComponent_(IntPtr l)
    {
        try
        {
            int argc = LuaDLL.lua_gettop(l);
            if (matchType(l, argc, 2, typeof(string)))
            {
                UnityEngine.GameObject self = (UnityEngine.GameObject)checkSelf(l);
                System.String a1;
                checkType(l, 2, out a1);
                var ret = self.GetComponent(a1);
                pushValue(l, true);
                pushValue(l, ret);

#if !RELEASE
                Debug.Assert(ret, string.Format("Must has component: {0}, {1}", a1, self.name));
#endif

                return 2;
            }
            else if (matchType(l, argc, 2, typeof(System.Type)))
            {
                UnityEngine.GameObject self = (UnityEngine.GameObject)checkSelf(l);
                System.Type a1;
                checkType(l, 2, out a1);
                var ret = self.GetComponent(a1);
                pushValue(l, true);
                pushValue(l, ret);

#if !RELEASE
                Debug.Assert(ret, "Must has Component: " + a1.Name);
#endif
                return 2;
            }
            pushValue(l, false);
            LuaDLL.lua_pushstring(l, "No matched override function to call");
            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    /// <summary>
    /// 获取层级路径
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetHierarchy_(IntPtr l)
    {
        try
        {
            UnityEngine.GameObject self = (UnityEngine.GameObject)checkSelf(l);
            System.String a1;
            a1 = self.name;
            var t = self.transform;
            while (t.parent != null)
            {
                a1 = t.parent.name + "/" + a1;
                t = t.parent;
            }
            pushValue(l, true);
            pushValue(l, a1);
            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int FindChild_(IntPtr l)
    {
        try
        {
            UnityEngine.GameObject self = (UnityEngine.GameObject)checkSelf(l);
            System.String a1;
            checkType(l, 2, out a1);

            GameObject go = null;
            Transform trans = self.transform.FindChild(a1);
            if (trans != null)
            {
                go = trans.gameObject;
            }

            pushValue(l, true);
            pushValue(l, go);
            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    /// <summary>
    /// 获取本地坐标
    /// return x,y,z
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetLocalPos_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            var p = self.transform.localPosition;
            pushValue(l, true);
            pushValue(l, p.x);
            pushValue(l, p.y);
            pushValue(l, p.z);
            return 4;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    /// <summary>
    /// 设置全部坐标
    /// SetLocalPos_(x, y, z)
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetPos_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            float x, y, z;
            checkType(l, 2, out x);
            checkType(l, 3, out y);
            checkType(l, 4, out z);
            self.transform.position = new Vector3(x, y, z);
            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetPos_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            var p = self.transform.position;
            pushValue(l, true);
            pushValue(l, p.x);
            pushValue(l, p.y);
            pushValue(l, p.z);
            return 4;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    /// <summary>
    /// 设置本地坐标
    /// SetLocalPos_(x, y, z)
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetLocalPos_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            float x, y, z;
            checkType(l, 2, out x);
            checkType(l, 3, out y);
            checkType(l, 4, out z);
            self.transform.localPosition = new Vector3(x, y, z);
            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetLocalScale_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            var p = self.transform.localScale;
            pushValue(l, true);
            pushValue(l, p.x);
            pushValue(l, p.y);
            pushValue(l, p.z);
            return 4;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }



    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetLocalScale_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            float x, y, z;
            checkType(l, 2, out x);
            checkType(l, 3, out y);
            checkType(l, 4, out z);
            self.transform.localScale = new Vector3(x, y, z);
            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }



    //-------∽-★-∽RectTransform∽-★-∽--------//

    /// <summary>
    /// 获取锚定坐标
    /// return x,y
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetAchPos_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            RectTransform rectTrans = self.GetComponent<RectTransform>();
            if (rectTrans)
            {
                var p = rectTrans.anchoredPosition;
                pushValue(l, true);
                pushValue(l, p.x);
                pushValue(l, p.y);
            }
            else
            {
                Transform trans = self.transform;
                var p = trans.localPosition;
                pushValue(l, true);
                pushValue(l, p.x);
                pushValue(l, p.y);
            }
            return 3;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    /// <summary>
    /// 设置锚定坐标
    /// SetAnchoredPos_(x, y)
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetAchPos_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);

            float x, y;
            checkType(l, 2, out x);
            checkType(l, 3, out y);

            RectTransform rectTrans = self.GetComponent<RectTransform>();
            if (rectTrans)
            {
                rectTrans.anchoredPosition = new Vector2(x, y);
            }
            else
            {
                Transform trans = self.transform;
                trans.localPosition = new Vector3(x, y, trans.localPosition.z);
            }

            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetSizeDelta_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            var p = rectTrans.sizeDelta;
            pushValue(l, true);
            pushValue(l, p.x);
            pushValue(l, p.y);
            return 3;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetSizeDelta_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);

            float x, y;
            checkType(l, 2, out x);
            checkType(l, 3, out y);

            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            rectTrans.sizeDelta = new Vector2(x, y);

            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetPivot_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            var p = rectTrans.pivot;
            pushValue(l, true);
            pushValue(l, p.x);
            pushValue(l, p.y);
            return 3;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetPivot_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);

            float x, y;
            checkType(l, 2, out x);
            checkType(l, 3, out y);

            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            rectTrans.pivot = new Vector2(x, y);

            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetPivotSmart_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);

            float x, y;
            checkType(l, 2, out x);
            checkType(l, 3, out y);

            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            KuiUtil.SetPivotSmart(rectTrans, new Vector2(x, y), true);

            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetRect_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            var p = rectTrans.rect;
            pushValue(l, true);
            pushValue(l, p.x);
            pushValue(l, p.y);
            pushValue(l, p.width);
            pushValue(l, p.height);
            return 5;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetRectSize_(IntPtr l)
    {
        try
        {
            GameObject self = (GameObject)checkSelf(l);
            RectTransform rectTrans = ComponentUtil.NeedComponent_<RectTransform>(self);
            var p = rectTrans.rect;
            pushValue(l, true);
            pushValue(l, p.width);
            pushValue(l, p.height);
            return 3;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    [UnityEngine.Scripting.Preserve]
    static public void reg(IntPtr l)
    {

        addMember(l, FindChild_);
        addMember(l, GetHierarchy_);    // 获取层级路径
        addMember(l, NeedComponent_);
        addMember(l, SetPos_);
        addMember(l, GetPos_);
        addMember(l, SetLocalPos_);
        addMember(l, GetLocalPos_);
        addMember(l, SetLocalScale_);
        addMember(l, GetLocalScale_);

        //rect
        addMember(l, GetAchPos_);
        addMember(l, SetAchPos_);
        addMember(l, GetPivot_);
        addMember(l, SetPivot_);
        addMember(l, SetPivotSmart_);
        addMember(l, GetSizeDelta_);
        addMember(l, SetSizeDelta_);
        addMember(l, GetRect_);
        addMember(l, GetRectSize_);

    }


}