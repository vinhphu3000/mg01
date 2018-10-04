using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_Custom.reg,
				Lua_Custom_IFoo.reg,
				Lua_Deleg.reg,
				Lua_foostruct.reg,
				Lua_FloatEvent.reg,
				Lua_ListViewEvent.reg,
				Lua_SLuaTest.reg,
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_XXList.reg,
				Lua_AbsClass.reg,
				Lua_HelloWorld.reg,
				Lua_NewCoroutine.reg,
				Lua_System_Collections_Generic_Dictionary_2_int_string.reg,
				Lua_System_String.reg,
				Lua_mg_org_CCDefine.reg,
				Lua_mg_org_CCApp.reg,
				Lua_mg_org_Subject.reg,
				Lua_mg_org_CCModule.reg,
				Lua_mg_org_AssetData.reg,
				Lua_mg_org_AssetCache.reg,
				Lua_mg_org_AssetCacheRss.reg,
				Lua_mg_org_bundle_AssetCacheBdl.reg,
				Lua_mg_org_GameObjCache.reg,
				Lua_mg_org_SprAtlasCache.reg,
				Lua_mg_org_LevelMgr.reg,
				Lua_mg_org_LevelMgr_LevelData.reg,
				Lua_mg_org_SoundMgr.reg,
				Lua_mg_org_Keyboard.reg,
				Lua_mg_org_LuaEvtCenter.reg,
				Lua_mg_org_GameObjUtil.reg,
				Lua_mg_org_ComponentUtil.reg,
				Lua_mg_org_DisplayUtil.reg,
				Lua_mg_org_DateUtil.reg,
				Lua_mg_org_FileUtility.reg,
				Lua_mg_org_KUI_KContainer.reg,
				Lua_mg_org_KUI_KText.reg,
				Lua_mg_org_KUI_KButton.reg,
				Lua_mg_org_KUI_KButtonShrinkable.reg,
				Lua_mg_org_KUI_KInputField.reg,
				Lua_mg_org_KUI_KScrollView.reg,
				Lua_mg_org_KUI_KListView.reg,
				Lua_mg_org_KUI_KListViewScroll.reg,
				Lua_mg_org_KUI_KToggle.reg,
				Lua_mg_org_KUI_KToggleGroup.reg,
				Lua_mg_org_KUI_KSlider.reg,
				Lua_mg_org_KUI_KProgressBar.reg,
				Lua_mg_org_KUI_KImage.reg,
				Lua_mg_org_KUI_KComponentEvent_1_UnityEngine_GameObject.reg,
				Lua_mg_org_KUI_KuiUtil.reg,
			};
			return list;
		}
	}
}
