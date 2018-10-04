using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KScrollView : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ResetContentPosition(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			self.ResetContentPosition();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetScrollable(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.SetScrollable(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int NeedScroll(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			var ret=self.NeedScroll();
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
	static public int StopMovement(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			self.StopMovement();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int JumpToTop(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			self.JumpToTop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int JumpToBottom(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			self.JumpToBottom();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_direction(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.direction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_direction(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			mg.org.KUI.KScrollView.ScrollDir v;
			checkEnum(l,2,out v);
			self.direction=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_contentTrans(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.contentTrans);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_maskTrans(IntPtr l) {
		try {
			mg.org.KUI.KScrollView self=(mg.org.KUI.KScrollView)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.maskTrans);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KScrollView");
		addMember(l,ResetContentPosition);
		addMember(l,SetScrollable);
		addMember(l,NeedScroll);
		addMember(l,StopMovement);
		addMember(l,JumpToTop);
		addMember(l,JumpToBottom);
		addMember(l,"direction",get_direction,set_direction,true);
		addMember(l,"contentTrans",get_contentTrans,null,true);
		addMember(l,"maskTrans",get_maskTrans,null,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KScrollView),typeof(mg.org.KUI.KContainer));
	}
}
