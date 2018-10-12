/* ==============================================================================
 * Lua_UnityEngine_UI_Graphic_Manual
 * @author jr.zeng
 * 2018/10/12 20:42:34
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using SLua;
using mg.org;

[OverloadLuaClass(typeof(Graphic))]
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_UI_Graphic_Manual : LuaObject
{
    [SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetColor32_(IntPtr l)
    {
        try
        {
            UnityEngine.UI.Graphic self = (UnityEngine.UI.Graphic)checkSelf(l);
            int i;
            checkType(l, 2, out i);
            self.color = ColorUtil.ColorToC4B(i);
            pushValue(l, true);
            return 1;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }

    [SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetColor24_(IntPtr l)
    {
        try
        {
            UnityEngine.UI.Graphic self = (UnityEngine.UI.Graphic)checkSelf(l);
            int i;
            checkType(l, 2, out i);
            self.color = ColorUtil.ColorToC3B(i);
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

        addMember(l, SetColor24_);
        addMember(l, SetColor32_);
       

    }


}