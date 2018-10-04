using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KText : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetPreferredHeight(IntPtr l) {
		try {
			mg.org.KUI.KText self=(mg.org.KUI.KText)checkSelf(l);
			var ret=self.GetPreferredHeight();
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
	static public int GetPreferredWidth(IntPtr l) {
		try {
			mg.org.KUI.KText self=(mg.org.KUI.KText)checkSelf(l);
			var ret=self.GetPreferredWidth();
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
	static public int get_langId(IntPtr l) {
		try {
			mg.org.KUI.KText self=(mg.org.KUI.KText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.langId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_langId(IntPtr l) {
		try {
			mg.org.KUI.KText self=(mg.org.KUI.KText)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.langId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KText");
		addMember(l,GetPreferredHeight);
		addMember(l,GetPreferredWidth);
		addMember(l,"langId",get_langId,set_langId,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KText),typeof(UnityEngine.UI.Text));
	}
}
