using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_CCApp : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_keyboard(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCApp.keyboard);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_soundMgr(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCApp.soundMgr);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_soundMgr(IntPtr l) {
		try {
			mg.org.SoundMgr v;
			checkType(l,2,out v);
			mg.org.CCApp.soundMgr=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FrameRate(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCApp.FrameRate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_FrameRate(IntPtr l) {
		try {
			int v;
			checkType(l,2,out v);
			mg.org.CCApp.FrameRate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.CCApp");
		addMember(l,"keyboard",get_keyboard,null,false);
		addMember(l,"soundMgr",get_soundMgr,set_soundMgr,false);
		addMember(l,"FrameRate",get_FrameRate,set_FrameRate,false);
		createTypeMetatable(l,null, typeof(mg.org.CCApp));
	}
}
