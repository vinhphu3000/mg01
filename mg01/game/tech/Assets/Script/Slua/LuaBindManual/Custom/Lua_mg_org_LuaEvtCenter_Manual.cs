/* ==============================================================================
 * Lua_mg_org_LuaEvtCenter_Manual
 * @author jr.zeng
 * 2018/2/11 15:00:36
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;
using SLua;

using mg.org;


[OverloadLuaClass(typeof(LuaEvtCenter))]
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_LuaEvtCenter_Manual : LuaObject
{

    //快捷写lua数组
    static int __tblIdx = 0;   //写数据到table中的序号
    static int __tblLoc = 0;   //table所在的栈位置
    //写入到lua数组
    static void ArrPush(IntPtr l, object value_)
    {
        pushValue(l, __tblIdx++);   //序号自增
        pushValue(l, value_);
        LuaDLL.lua_rawset(l, __tblLoc - 2); //为指定位置的table的index序号设置value
    }
    //写之前要重置一下
    //@tblLocation_ table这时在栈的位置，-1是栈顶
    static void ArrReset(int tblLocation_=-1)
    {
        __tblIdx = 1;
        __tblLoc = tblLocation_;
    }





    static int evtNum = 0;
    static int goEvtNum = 0;

    /// <summary>
    /// lua获取事件列表
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static public int GetEvents_s(IntPtr l)
    {
        try
        {
            evtNum = 0;
            goEvtNum = 0; //go事件数量

            ArrReset(-1);


            if (LuaEvtCenter.__events.Count > 0)
            {

                List<LuaEvtCenter.LuaEvt> events = LuaEvtCenter.__events;
                LuaEvtCenter.LuaEvt evt;
                int len = events.Count;

                for (int i = 0; i < len; ++i)
                {
                    evt = events[i];
                    ArrPush(l, evt.type);

                    if (evt.args != null)
                    {
                        ArrPush(l, evt.args.Length);    //写入参数长度

                        for (int j = 0; j < evt.args.Length; ++j)
                        {
                            ArrPush(l, evt.args[j]);
                        }
                    }
                    else
                    {
                        ArrPush(l, 0);  //参数长度为0
                    }

                    evtNum++;
                }

                LuaEvtCenter.ClearEvents();
            }


            if (LuaEvtCenter.__goEvents.Count > 0)
            {

                List<LuaEvtCenter.LuaGoEvt> goEvents = LuaEvtCenter.__goEvents;
                LuaEvtCenter.LuaGoEvt evt;
                int len = goEvents.Count;

                for (int i=0; i<len; ++i)
                {
                    evt = goEvents[i];
                    if(evt.go)
                    {
                        //go还活着
                        ArrPush(l, evt.type);
                        ArrPush(l, evt.go);
                        
                        if(evt.args != null)
                        {
                            ArrPush(l, evt.args.Length);    //写入参数长度

                            for(int j=0; j< evt.args.Length;++j)
                            {
                                ArrPush(l, evt.args[j]);
                            }
                        }
                        else
                        {
                            ArrPush(l, 0);  //参数长度为0
                        }

                        goEvtNum++;
                    }
                }

                LuaEvtCenter.ClearGoEvents();
            }
            
            pushValue(l, true);
            pushValue(l, evtNum);       //事件数量
            pushValue(l, goEvtNum);     //事件数量
            return 3;
        }
        catch (Exception e)
        {
            return error(l, e);
        }
    }


    static public void reg(IntPtr l)
    {
        
        addMember(l, GetEvents_s);


    }


}