/* ==============================================================================
 * Lua_Unity_Transform
 * @author jr.zeng
 * 2017/9/22 16:33:35
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;
using SLua;

[OverloadLuaClass(typeof(Transform))]
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Transform_Manual : LuaObject
{
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int GetLocalPos_(IntPtr l)
    {
        try
        {
            UnityEngine.Transform self = (UnityEngine.Transform)checkSelf(l);
            var p = self.localPosition;
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
    static public int SetLocalPos_(IntPtr l)
    {
        try
        {
            UnityEngine.Transform self = (UnityEngine.Transform)checkSelf(l);
            float x, y, z;
            checkType(l, 2, out x);
            checkType(l, 3, out y);
            checkType(l, 4, out z);
            self.localPosition = new Vector3(x, y, z);
            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    [UnityEngine.Scripting.Preserve]
    static public void reg(IntPtr l)
    {
        //new
        addMember(l, GetLocalPos_);
        addMember(l, SetLocalPos_);

    }


}
