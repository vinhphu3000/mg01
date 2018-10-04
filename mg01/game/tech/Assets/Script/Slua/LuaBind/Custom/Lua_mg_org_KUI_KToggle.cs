using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KToggle : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Select(IntPtr l) {
		try {
			mg.org.KUI.KToggle self=(mg.org.KUI.KToggle)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.Select(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_needReqChange(IntPtr l) {
		try {
			mg.org.KUI.KToggle self=(mg.org.KUI.KToggle)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.needReqChange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_needReqChange(IntPtr l) {
		try {
			mg.org.KUI.KToggle self=(mg.org.KUI.KToggle)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.needReqChange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KToggle");
		addMember(l,Select);
		addMember(l,"needReqChange",get_needReqChange,set_needReqChange,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KToggle),typeof(UnityEngine.UI.Toggle));
	}
}
