using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_mg_org_SoundMgr : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PlayBgm(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.PlayBgm(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopBgm(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			self.StopBgm();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PlayOneShot(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.PlayOneShot(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PlayUi(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.PlayUi(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopAllEff(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			self.StopAllEff();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_totalOn(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.totalOn);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_totalOn(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.totalOn=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_bgmOn(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bgmOn);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_bgmOn(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.bgmOn=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_effOn(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.effOn);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_effOn(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.effOn=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_totalVolume(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.totalVolume);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_totalVolume(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.totalVolume=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_bgmVolume(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bgmVolume);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_bgmVolume(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.bgmVolume=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_effVolume(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.effVolume);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_effVolume(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.effVolume=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isBgmPlaying(IntPtr l) {
		try {
			mg.org.SoundMgr self=(mg.org.SoundMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isBgmPlaying);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"mg.org.SoundMgr");
		addMember(l,PlayBgm);
		addMember(l,StopBgm);
		addMember(l,PlayOneShot);
		addMember(l,PlayUi);
		addMember(l,StopAllEff);
		addMember(l,"totalOn",get_totalOn,set_totalOn,true);
		addMember(l,"bgmOn",get_bgmOn,set_bgmOn,true);
		addMember(l,"effOn",get_effOn,set_effOn,true);
		addMember(l,"totalVolume",get_totalVolume,set_totalVolume,true);
		addMember(l,"bgmVolume",get_bgmVolume,set_bgmVolume,true);
		addMember(l,"effVolume",get_effVolume,set_effVolume,true);
		addMember(l,"isBgmPlaying",get_isBgmPlaying,null,true);
		createTypeMetatable(l,null, typeof(mg.org.SoundMgr),typeof(mg.org.CCModule));
	}
}
