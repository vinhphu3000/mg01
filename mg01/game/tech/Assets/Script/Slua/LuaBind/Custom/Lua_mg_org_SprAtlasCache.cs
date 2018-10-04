using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_SprAtlasCache : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReleaseSprite(IntPtr l) {
		try {
			mg.org.SprAtlasCache self=(mg.org.SprAtlasCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.ReleaseSprite(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int UnloadSprite(IntPtr l) {
		try {
			mg.org.SprAtlasCache self=(mg.org.SprAtlasCache)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.UnloadSprite(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadSprite(IntPtr l) {
		try {
			mg.org.SprAtlasCache self=(mg.org.SprAtlasCache)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			UnityEngine.UI.Image a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.Boolean a5;
			checkType(l,6,out a5);
			self.LoadSprite(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetSprite(IntPtr l) {
		try {
			mg.org.SprAtlasCache self=(mg.org.SprAtlasCache)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			UnityEngine.UI.Image a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.Boolean a5;
			checkType(l,6,out a5);
			self.SetSprite(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FormatPath_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=mg.org.SprAtlasCache.FormatPath(a1);
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
	static public int get_me(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,mg.org.SprAtlasCache.me);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.SprAtlasCache");
		addMember(l,ReleaseSprite);
		addMember(l,UnloadSprite);
		addMember(l,LoadSprite);
		addMember(l,SetSprite);
		addMember(l,FormatPath_s);
		addMember(l,"me",get_me,null,false);
		createTypeMetatable(l,null, typeof(mg.org.SprAtlasCache),typeof(mg.org.CCModule));
	}
}
