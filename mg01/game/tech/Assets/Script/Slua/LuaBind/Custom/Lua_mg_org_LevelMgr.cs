using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_LevelMgr : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadSync(IntPtr l) {
		try {
			mg.org.LevelMgr self=(mg.org.LevelMgr)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.LoadSync(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadAsync(IntPtr l) {
		try {
			mg.org.LevelMgr self=(mg.org.LevelMgr)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			mg.org.CALLBACK_1 a3;
			checkDelegate(l,4,out a3);
			System.Object a4;
			checkType(l,5,out a4);
			var ret=self.LoadAsync(a1,a2,a3,a4);
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
	static public int IsLevelLoaded(IntPtr l) {
		try {
			mg.org.LevelMgr self=(mg.org.LevelMgr)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.IsLevelLoaded(a1);
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
			pushValue(l,mg.org.LevelMgr.me);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.LevelMgr");
		addMember(l,LoadSync);
		addMember(l,LoadAsync);
		addMember(l,IsLevelLoaded);
		addMember(l,"me",get_me,null,false);
		createTypeMetatable(l,null, typeof(mg.org.LevelMgr),typeof(mg.org.CCModule));
	}
}
