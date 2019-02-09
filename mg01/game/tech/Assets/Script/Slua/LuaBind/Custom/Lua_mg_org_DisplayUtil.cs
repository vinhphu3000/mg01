using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_DisplayUtil : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.DisplayUtil");
		createTypeMetatable(l,null, typeof(mg.org.DisplayUtil));
	}
}
