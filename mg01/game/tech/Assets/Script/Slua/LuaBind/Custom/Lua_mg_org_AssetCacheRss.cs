using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_AssetCacheRss : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.AssetCacheRss");
		createTypeMetatable(l,null, typeof(mg.org.AssetCacheRss),typeof(mg.org.AssetCache));
	}
}
