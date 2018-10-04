using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KListViewScroll : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetScrollable(IntPtr l) {
		try {
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
	static public int ShowLen(IntPtr l) {
		try {
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			mg.org.KUI.KListViewScroll.JumpPosType a3;
			checkEnum(l,4,out a3);
			self.ShowLen(a1,a2,a3);
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
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
	static public int JumpToIndex(IntPtr l) {
		try {
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			mg.org.KUI.KListViewScroll.JumpPosType a2;
			checkEnum(l,3,out a2);
			var ret=self.JumpToIndex(a1,a2);
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
	static public int get_direction(IntPtr l) {
		try {
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
			mg.org.KUI.KListViewScroll self=(mg.org.KUI.KListViewScroll)checkSelf(l);
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
		getTypeTable(l,"mg.org.KUI.KListViewScroll");
		addMember(l,SetScrollable);
		addMember(l,NeedScroll);
		addMember(l,ShowLen);
		addMember(l,JumpToTop);
		addMember(l,JumpToBottom);
		addMember(l,JumpToIndex);
		addMember(l,"direction",get_direction,set_direction,true);
		addMember(l,"contentTrans",get_contentTrans,null,true);
		addMember(l,"maskTrans",get_maskTrans,null,true);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KListViewScroll),typeof(mg.org.KUI.KListView));
	}
}
