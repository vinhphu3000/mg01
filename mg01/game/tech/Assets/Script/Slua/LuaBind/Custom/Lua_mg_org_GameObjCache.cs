using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_GameObjCache : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadSync(IntPtr l) {
		try {
			mg.org.GameObjCache self=(mg.org.GameObjCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.LoadSync(a1);
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
	static public int LoadAsync(IntPtr l) {
		try {
			mg.org.GameObjCache self=(mg.org.GameObjCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			mg.org.CALLBACK_GO a2;
			checkDelegate(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			self.LoadAsync(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateGo(IntPtr l) {
		try {
			mg.org.GameObjCache self=(mg.org.GameObjCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			mg.org.CALLBACK_GO a2;
			checkDelegate(l,3,out a2);
			var ret=self.CreateGo(a1,a2);
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
	static public int get_me(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.GameObjCache.me);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.GameObjCache");
		addMember(l,LoadSync);
		addMember(l,LoadAsync);
		addMember(l,CreateGo);
		addMember(l,"me",get_me,null,false);
		createTypeMetatable(l,null, typeof(mg.org.GameObjCache),typeof(mg.org.CCModule));
	}
}
