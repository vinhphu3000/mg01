/* ==============================================================================
 * Lua_mg_org_KUI_KuiUtil_Manual
 * @author jr.zeng
 * 2018/10/2 16:04:56
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

[OverloadLuaClass(typeof(KuiUtil))]
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KuiUtil_Manual : LuaObject
{


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [UnityEngine.Scripting.Preserve]
    static public int SetPivotSmart_s(IntPtr l)
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


    [UnityEngine.Scripting.Preserve]
    static public void reg(IntPtr l)
    {

        addMember(l, SetPivotSmart_s);
       

    }

}