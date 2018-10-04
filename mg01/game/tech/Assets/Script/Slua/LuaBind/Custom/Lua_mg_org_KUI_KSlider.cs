using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KSlider : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnPointerDown(IntPtr l) {
		try {
			mg.org.KUI.KSlider self=(mg.org.KUI.KSlider)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerDown(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnPointerUp(IntPtr l) {
		try {
			mg.org.KUI.KSlider self=(mg.org.KUI.KSlider)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerUp(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CHILD_NAME_HANDLE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.KUI.KSlider.CHILD_NAME_HANDLE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CHILD_NAME_FILL(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.KUI.KSlider.CHILD_NAME_FILL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_onValueChanged(IntPtr l) {
		try {
			mg.org.KUI.KSlider self=(mg.org.KUI.KSlider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onValueChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SlicedFilled(IntPtr l) {
		try {
			mg.org.KUI.KSlider self=(mg.org.KUI.KSlider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SlicedFilled);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SlicedFilled(IntPtr l) {
		try {
			mg.org.KUI.KSlider self=(mg.org.KUI.KSlider)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.SlicedFilled=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KSlider");
		addMember(l,OnPointerDown);
		addMember(l,OnPointerUp);
		addMember(l,"CHILD_NAME_HANDLE",get_CHILD_NAME_HANDLE,null,false);
		addMember(l,"CHILD_NAME_FILL",get_CHILD_NAME_FILL,null,false);
		addMember(l,"onValueChanged",get_onValueChanged,null,true);
		addMember(l,"SlicedFilled",get_SlicedFilled,set_SlicedFilled,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KSlider),typeof(UnityEngine.UI.Slider));
	}
}
