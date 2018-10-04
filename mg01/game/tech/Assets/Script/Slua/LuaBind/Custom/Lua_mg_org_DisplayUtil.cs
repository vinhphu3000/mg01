using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_DisplayUtil : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddChild_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			var ret=mg.org.DisplayUtil.AddChild(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveFromParent_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			mg.org.DisplayUtil.RemoveFromParent(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsInTrash_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			var ret=mg.org.DisplayUtil.IsInTrash(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.DisplayUtil");
		addMember(l,AddChild_s);
		addMember(l,RemoveFromParent_s);
		addMember(l,IsInTrash_s);
		createTypeMetatable(l,null, typeof(mg.org.DisplayUtil));
	}
}
