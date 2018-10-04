using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KProgressBar : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_percent(IntPtr l) {
		try {
			mg.org.KUI.KProgressBar self=(mg.org.KUI.KProgressBar)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.percent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_percent(IntPtr l) {
		try {
			mg.org.KUI.KProgressBar self=(mg.org.KUI.KProgressBar)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.percent=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_value(IntPtr l) {
		try {
			mg.org.KUI.KProgressBar self=(mg.org.KUI.KProgressBar)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_value(IntPtr l) {
		try {
			mg.org.KUI.KProgressBar self=(mg.org.KUI.KProgressBar)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KProgressBar");
		addMember(l,"percent",get_percent,set_percent,true);
		addMember(l,"value",get_value,set_value,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KProgressBar),typeof(mg.org.KUI.KContainer));
	}
}
