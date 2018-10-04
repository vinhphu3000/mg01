using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KButtonShrinkable : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ShrinkEnabled(IntPtr l) {
		try {
			mg.org.KUI.KButtonShrinkable self=(mg.org.KUI.KButtonShrinkable)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.ShrinkEnabled(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KButtonShrinkable");
		addMember(l,ShrinkEnabled);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KButtonShrinkable),typeof(mg.org.KUI.KButton));
	}
}
