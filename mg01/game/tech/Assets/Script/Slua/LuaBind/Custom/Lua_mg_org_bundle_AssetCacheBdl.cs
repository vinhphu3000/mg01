using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_bundle_AssetCacheBdl : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.bundle.AssetCacheBdl");
		createTypeMetatable(l,null, typeof(mg.org.bundle.AssetCacheBdl),typeof(mg.org.AssetCache));
	}
}
