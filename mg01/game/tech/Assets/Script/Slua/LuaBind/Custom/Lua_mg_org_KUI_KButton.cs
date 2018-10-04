using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KButton : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isPassPoint(IntPtr l) {
		try {
			mg.org.KUI.KButton self=(mg.org.KUI.KButton)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isPassPoint);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_isPassPoint(IntPtr l) {
		try {
			mg.org.KUI.KButton self=(mg.org.KUI.KButton)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.isPassPoint=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_pressure(IntPtr l) {
		try {
			mg.org.KUI.KButton self=(mg.org.KUI.KButton)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pressure);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_maximumPossiblePressure(IntPtr l) {
		try {
			mg.org.KUI.KButton self=(mg.org.KUI.KButton)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.maximumPossiblePressure);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KButton");
		addMember(l,"isPassPoint",get_isPassPoint,set_isPassPoint,true);
		addMember(l,"pressure",get_pressure,null,true);
		addMember(l,"maximumPossiblePressure",get_maximumPossiblePressure,null,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KButton),typeof(UnityEngine.UI.Button));
	}
}
