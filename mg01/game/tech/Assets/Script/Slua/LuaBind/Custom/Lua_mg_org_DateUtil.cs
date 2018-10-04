using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_DateUtil : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TimeFromStart(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.DateUtil.TimeFromStart);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TimeST_ms(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.DateUtil.TimeST_ms);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.DateUtil");
		addMember(l,"TimeFromStart",get_TimeFromStart,null,false);
		addMember(l,"TimeST_ms",get_TimeST_ms,null,false);
		createTypeMetatable(l,null, typeof(mg.org.DateUtil));
	}
}
