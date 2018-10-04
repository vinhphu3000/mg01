using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KToggleGroup : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetToggle(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetToggle(a1);
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
	static public int isSelected(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.isSelected(a1);
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
	static public int Select(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.Select(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CancelAll(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.CancelAll(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_allowSwitchOff(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.allowSwitchOff);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_allowSwitchOff(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.allowSwitchOff=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_allowMultiple(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.allowMultiple);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_allowMultiple(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.allowMultiple=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_needReqChange(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.needReqChange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_needReqChange(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.needReqChange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_index(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.index);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_toggleNum(IntPtr l) {
		try {
			mg.org.KUI.KToggleGroup self=(mg.org.KUI.KToggleGroup)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.toggleNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KToggleGroup");
		addMember(l,GetToggle);
		addMember(l,isSelected);
		addMember(l,Select);
		addMember(l,CancelAll);
		addMember(l,"allowSwitchOff",get_allowSwitchOff,set_allowSwitchOff,true);
		addMember(l,"allowMultiple",get_allowMultiple,set_allowMultiple,true);
		addMember(l,"needReqChange",get_needReqChange,set_needReqChange,true);
		addMember(l,"index",get_index,null,true);
		addMember(l,"toggleNum",get_toggleNum,null,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KToggleGroup),typeof(mg.org.KUI.KContainer));
	}
}
