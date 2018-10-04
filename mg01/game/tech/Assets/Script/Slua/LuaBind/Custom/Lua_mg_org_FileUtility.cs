using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_FileUtility : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.FileUtility");
		Lua_mg_org_FileUtility_Manual.reg(l);
		createTypeMetatable(l,null, typeof(mg.org.FileUtility));
	}
}
