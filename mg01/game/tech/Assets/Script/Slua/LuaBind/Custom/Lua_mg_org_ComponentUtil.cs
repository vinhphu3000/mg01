using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_ComponentUtil : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int EnsureComponent_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				var ret=mg.org.ComponentUtil.EnsureComponent<UnityEngine.Component>(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GameObject),typeof(string))){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=mg.org.ComponentUtil.EnsureComponent(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GameObject),typeof(System.Type))){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.Type a2;
				checkType(l,2,out a2);
				var ret=mg.org.ComponentUtil.EnsureComponent(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function EnsureComponent to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.ComponentUtil");
		addMember(l,EnsureComponent_s);
		createTypeMetatable(l,null, typeof(mg.org.ComponentUtil));
	}
}
