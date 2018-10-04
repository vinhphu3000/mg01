/* ==============================================================================
 * Lua_mg_org_FileUtility
 * @author jr.zeng
 * 2018/7/16 15:22:28
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;

using SLua;

using mg.org;

[OverloadLuaClass(typeof(FileUtility))]
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_FileUtility_Manual : LuaObject
{


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static public int CombinePath_s(IntPtr l)
    {
        try
        {
            System.String path1;
            checkType(l, 1, out path1);
            System.String path2;
            checkType(l, 2, out path2);
            var ret = Path.Combine(path1, path2);
            pushValue(l, true);
            pushValue(l, ret);
            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    /// <summary>
    /// Directory.GetFiles
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    public static int GetFiles_s(IntPtr l)
    {
        try
        {
            System.String path;
            checkType(l, 1, out path);

            pushValue(l, true);

            var fl = Directory.GetFiles(path);
            LuaDLL.lua_newtable(l);
            for (int i = 0; i < fl.Length; ++i)
            {
                pushValue(l, fl[i]);
                LuaDLL.lua_rawseti(l, -2, i + 1);   //t[i] = v 
            }
            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    /// <summary>
    /// Directory.GetDirectories
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    public static int GetDirectories_s(IntPtr l)
    {
        try
        {
            System.String path;
            checkType(l, 1, out path);

            pushValue(l, true);

            var fl = Directory.GetDirectories(path);
            LuaDLL.lua_newtable(l);
            for (int i = 0; i < fl.Length; ++i)
            {
                pushValue(l, fl[i]);
                LuaDLL.lua_rawseti(l, -2, i + 1);   //t[i] = v 
            }

            return 2;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    /// <summary>
    /// 获取文件的最新修改时间(毫秒)
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    public static int GetLastWriteTime_s(IntPtr l)
    {
        try
        {
            System.String path;
            checkType(l, 1, out path);
            var fi = new FileInfo(path);

            pushValue(l, true);
            pushValue(l, fi.LastWriteTime.Ticks);
            return 2;
        }
        catch (Exception e)
        {
            error(l, e);
            return 0;
        }
    }

    

    static public void reg(IntPtr l)
    {

        addMember(l, CombinePath_s);
        addMember(l, GetFiles_s);
        addMember(l, GetDirectories_s);
        addMember(l, GetLastWriteTime_s);

    }


}