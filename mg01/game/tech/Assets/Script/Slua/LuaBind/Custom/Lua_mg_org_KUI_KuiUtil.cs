using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KuiUtil : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KuiUtil");
		Lua_mg_org_KUI_KuiUtil_Manual.reg(l);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KuiUtil));
	}
}
