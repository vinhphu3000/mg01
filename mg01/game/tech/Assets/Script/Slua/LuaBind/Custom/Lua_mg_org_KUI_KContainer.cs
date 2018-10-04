using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KContainer : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Initialize(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			self.Initialize();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetChild(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetChild(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetChildComponent(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetChildComponent<UnityEngine.Component>(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddChildComponent(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.AddChildComponent<UnityEngine.Component>(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RecalculateSize(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			self.RecalculateSize();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int NotifyDeactive(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			self.NotifyDeactive();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int NotifyDispose(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			self.NotifyDispose();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Visible(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Visible);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Visible(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.Visible=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ReferId(IntPtr l) {
		try {
			mg.org.KUI.KContainer self=(mg.org.KUI.KContainer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ReferId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KContainer");
		addMember(l,Initialize);
		addMember(l,GetChild);
		addMember(l,GetChildComponent);
		addMember(l,AddChildComponent);
		addMember(l,RecalculateSize);
		addMember(l,NotifyDeactive);
		addMember(l,NotifyDispose);
		addMember(l,"Visible",get_Visible,set_Visible,true);
		addMember(l,"ReferId",get_ReferId,null,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KContainer),typeof(UnityEngine.EventSystems.UIBehaviour));
	}
}
