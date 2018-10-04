using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_AssetCache : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadSync(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			var ret=self.LoadSync(a1,a2);
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
	static public int LoadAsync(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			mg.org.CALLBACK_1 a2;
			checkDelegate(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			var ret=self.LoadAsync(a1,a2,a3);
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
	static public int LoadSync_Level(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			var ret=self.LoadSync_Level(a1,a2);
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
	static public int LoadAsync_Level(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			mg.org.CALLBACK_1 a2;
			checkDelegate(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			var ret=self.LoadAsync_Level(a1,a2,a3);
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
	static public int UnloadAsset(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.UnloadAsset(a1);
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
	static public int UnloadAssetsUnused(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			self.UnloadAssetsUnused();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearAllAssets(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			self.ClearAllAssets();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetAsset(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetAsset(a1);
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
	static public int HasAsset(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.HasAsset(a1);
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
	static public int RetainByUrl(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RetainByUrl(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReleaseByUrl(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.ReleaseByUrl(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReleaseByRefer(IntPtr l) {
		try {
			mg.org.AssetCache self=(mg.org.AssetCache)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.ReleaseByRefer(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_me(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.AssetCache.me);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.AssetCache");
		addMember(l,LoadSync);
		addMember(l,LoadAsync);
		addMember(l,LoadSync_Level);
		addMember(l,LoadAsync_Level);
		addMember(l,UnloadAsset);
		addMember(l,UnloadAssetsUnused);
		addMember(l,ClearAllAssets);
		addMember(l,GetAsset);
		addMember(l,HasAsset);
		addMember(l,RetainByUrl);
		addMember(l,ReleaseByUrl);
		addMember(l,ReleaseByRefer);
		addMember(l,"me",get_me,null,false);
		createTypeMetatable(l,null, typeof(mg.org.AssetCache),typeof(mg.org.CCModule));
	}
}
