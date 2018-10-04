using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_CCDefine : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			mg.org.CCDefine o;
			o=new mg.org.CCDefine();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DEBUG(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCDefine.DEBUG);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FPS_DEFAULT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCDefine.FPS_DEFAULT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_USE_LUA(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCDefine.USE_LUA);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Platform(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCDefine.Platform);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_USE_KEYBOARD(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.CCDefine.USE_KEYBOARD);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.CCDefine");
		addMember(l,"DEBUG",get_DEBUG,null,false);
		addMember(l,"FPS_DEFAULT",get_FPS_DEFAULT,null,false);
		addMember(l,"USE_LUA",get_USE_LUA,null,false);
		addMember(l,"Platform",get_Platform,null,false);
		addMember(l,"USE_KEYBOARD",get_USE_KEYBOARD,null,false);
		createTypeMetatable(l,constructor, typeof(mg.org.CCDefine));
	}
}
