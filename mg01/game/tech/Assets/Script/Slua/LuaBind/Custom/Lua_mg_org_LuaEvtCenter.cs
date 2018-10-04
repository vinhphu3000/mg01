using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_LuaEvtCenter : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.LuaEvtCenter");
		Lua_mg_org_LuaEvtCenter_Manual.reg(l);
		createTypeMetatable(l,null, typeof(mg.org.LuaEvtCenter));
	}
}
