using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KImage : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadFromNet(IntPtr l) {
		try {
			mg.org.KUI.KImage self=(mg.org.KUI.KImage)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.LoadFromNet(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KImage");
		addMember(l,LoadFromNet);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KImage),typeof(UnityEngine.UI.Image));
	}
}
