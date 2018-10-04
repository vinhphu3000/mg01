using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_LevelMgr_LevelData : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_url(IntPtr l) {
		try {
			mg.org.LevelMgr.LevelData self=(mg.org.LevelMgr.LevelData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.url);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_url(IntPtr l) {
		try {
			mg.org.LevelMgr.LevelData self=(mg.org.LevelMgr.LevelData)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.url=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_progress(IntPtr l) {
		try {
			mg.org.LevelMgr.LevelData self=(mg.org.LevelMgr.LevelData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.progress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isDone(IntPtr l) {
		try {
			mg.org.LevelMgr.LevelData self=(mg.org.LevelMgr.LevelData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isDone);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.LevelMgr.LevelData");
		addMember(l,"url",get_url,set_url,true);
		addMember(l,"progress",get_progress,null,true);
		addMember(l,"isDone",get_isDone,null,true);
		createTypeMetatable(l,null, typeof(mg.org.LevelMgr.LevelData));
	}
}
