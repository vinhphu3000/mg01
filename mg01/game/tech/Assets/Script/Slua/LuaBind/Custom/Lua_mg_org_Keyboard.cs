using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_Keyboard : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			mg.org.Keyboard o;
			o=new mg.org.Keyboard();
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
	static public int Setup(IntPtr l) {
		try {
			mg.org.Keyboard self=(mg.org.Keyboard)checkSelf(l);
			self.Setup();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clear(IntPtr l) {
		try {
			mg.org.Keyboard self=(mg.org.Keyboard)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetKeyPressed(IntPtr l) {
		try {
			mg.org.Keyboard self=(mg.org.Keyboard)checkSelf(l);
			UnityEngine.KeyCode a1;
			checkEnum(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.SetKeyPressed(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsPressed(IntPtr l) {
		try {
			mg.org.Keyboard self=(mg.org.Keyboard)checkSelf(l);
			UnityEngine.KeyCode a1;
			checkEnum(l,2,out a1);
			var ret=self.IsPressed(a1);
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
	static public int HasKeyPressed(IntPtr l) {
		try {
			mg.org.Keyboard self=(mg.org.Keyboard)checkSelf(l);
			var ret=self.HasKeyPressed();
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
	static public int ReleaseAllKeys(IntPtr l) {
		try {
			mg.org.Keyboard self=(mg.org.Keyboard)checkSelf(l);
			self.ReleaseAllKeys();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CheckPressCnd_s(IntPtr l) {
		try {
			var ret=mg.org.Keyboard.CheckPressCnd();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.Keyboard");
		addMember(l,Setup);
		addMember(l,Clear);
		addMember(l,SetKeyPressed);
		addMember(l,IsPressed);
		addMember(l,HasKeyPressed);
		addMember(l,ReleaseAllKeys);
		addMember(l,CheckPressCnd_s);
		createTypeMetatable(l,constructor, typeof(mg.org.Keyboard),typeof(mg.org.Subject));
	}
}
