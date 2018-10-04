using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KComponentEvent_1_UnityEngine_GameObject : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			mg.org.KUI.KComponentEvent<UnityEngine.GameObject> o;
			o=new mg.org.KUI.KComponentEvent<UnityEngine.GameObject>();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddListener(IntPtr l) {
		try {
			mg.org.KUI.KComponentEvent<UnityEngine.GameObject> self=(mg.org.KUI.KComponentEvent<UnityEngine.GameObject>)checkSelf(l);
			UnityEngine.Events.UnityAction<UnityEngine.GameObject> a1;
			checkDelegate(l,2,out a1);
			self.AddListener(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveListener(IntPtr l) {
		try {
			mg.org.KUI.KComponentEvent<UnityEngine.GameObject> self=(mg.org.KUI.KComponentEvent<UnityEngine.GameObject>)checkSelf(l);
			UnityEngine.Events.UnityAction<UnityEngine.GameObject> a1;
			checkDelegate(l,2,out a1);
			self.RemoveListener(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetEvent_s(IntPtr l) {
		try {
			mg.org.KUI.KComponentEvent<UnityEngine.GameObject> a1;
			checkType(l,1,out a1);
			var ret=mg.org.KUI.KComponentEvent<UnityEngine.GameObject>.GetEvent(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int InvokeEvent_s(IntPtr l) {
		try {
			mg.org.KUI.KComponentEvent<UnityEngine.GameObject> a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			mg.org.KUI.KComponentEvent<UnityEngine.GameObject>.InvokeEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		LuaUnityEvent_UnityEngine_GameObject.reg(l);
		getTypeTable(l,"mg.org.KUI.KComponentEvent<<UnityEngine.GameObject, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null>>");
		addMember(l,AddListener);
		addMember(l,RemoveListener);
		addMember(l,GetEvent_s);
		addMember(l,InvokeEvent_s);
		createTypeMetatable(l,constructor, typeof(mg.org.KUI.KComponentEvent<UnityEngine.GameObject>),typeof(LuaUnityEvent_UnityEngine_GameObject));
	}
}
