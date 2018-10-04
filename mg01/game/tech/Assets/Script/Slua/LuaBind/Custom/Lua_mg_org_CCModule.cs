using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_CCModule : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Attach(IntPtr l) {
		try {
			mg.org.CCModule self=(mg.org.CCModule)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			mg.org.CALLBACK_1 a2;
			checkDelegate(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			self.Attach(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Detach(IntPtr l) {
		try {
			mg.org.CCModule self=(mg.org.CCModule)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			mg.org.CALLBACK_1 a2;
			checkDelegate(l,3,out a2);
			self.Detach(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DetachByType(IntPtr l) {
		try {
			mg.org.CCModule self=(mg.org.CCModule)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.DetachByType(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.CCModule");
		addMember(l,Attach);
		addMember(l,Detach);
		addMember(l,DetachByType);
		createTypeMetatable(l,null, typeof(mg.org.CCModule),typeof(mg.org.BaseObject));
	}
}
