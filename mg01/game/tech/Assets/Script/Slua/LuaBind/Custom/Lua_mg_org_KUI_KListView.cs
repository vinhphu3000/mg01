using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_KUI_KListView : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetItemByIndex(IntPtr l) {
		try {
			mg.org.KUI.KListView self=(mg.org.KUI.KListView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetItemByIndex(a1);
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
	static public int InitLayoutParam(IntPtr l) {
		try {
			mg.org.KUI.KListView self=(mg.org.KUI.KListView)checkSelf(l);
			mg.org.KUI.LayoutDirection a1;
			checkEnum(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Single a7;
			checkType(l,8,out a7);
			System.Single a8;
			checkType(l,9,out a8);
			System.Single a9;
			checkType(l,10,out a9);
			System.Single a10;
			checkType(l,11,out a10);
			System.Single a11;
			checkType(l,12,out a11);
			System.Single a12;
			checkType(l,13,out a12);
			System.Single a13;
			checkType(l,14,out a13);
			System.Single a14;
			checkType(l,15,out a14);
			self.InitLayoutParam(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ShowLen(IntPtr l) {
		try {
			mg.org.KUI.KListView self=(mg.org.KUI.KListView)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.ShowLen(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearList(IntPtr l) {
		try {
			mg.org.KUI.KListView self=(mg.org.KUI.KListView)checkSelf(l);
			self.ClearList();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.KUI.KListView");
		addMember(l,GetItemByIndex);
		addMember(l,InitLayoutParam);
		addMember(l,ShowLen);
		addMember(l,ClearList);
		createTypeMetatable(l,null, typeof(mg.org.KUI.KListView),typeof(mg.org.KUI.KContainer));
	}
}
