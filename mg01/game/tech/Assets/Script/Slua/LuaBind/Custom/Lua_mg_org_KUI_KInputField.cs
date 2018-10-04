using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KInputField : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AppendString(IntPtr l) {
		try {
			mg.org.KUI.KInputField self=(mg.org.KUI.KInputField)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.AppendString(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GainFocus(IntPtr l) {
		try {
			mg.org.KUI.KInputField self=(mg.org.KUI.KInputField)checkSelf(l);
			self.GainFocus();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MoveIndexEnd(IntPtr l) {
		try {
			mg.org.KUI.KInputField self=(mg.org.KUI.KInputField)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.MoveIndexEnd(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KInputField");
		addMember(l,AppendString);
		addMember(l,GainFocus);
		addMember(l,MoveIndexEnd);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KInputField),typeof(UnityEngine.UI.InputField));
	}
}
