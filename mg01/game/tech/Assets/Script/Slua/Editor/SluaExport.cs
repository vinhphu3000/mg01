/* ==============================================================================
 * LuaCodeExport
 * @author jr.zeng
 * 2018/5/2 19:31:44
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEditor;

using UnityEngine;
using Object = UnityEngine.Object;

using SLua;

using mg.org;
using mg.org.bundle;
using mg.org.KUI;

public class SluaExport: ICustomExportPost
{
    public SluaExport()
    {

    }


    /// <summary>
    /// 会被LuaCodeGen的Custom接口调用
    /// </summary>
    /// <param name="add"></param>
    static public void OnAddCustomClass(LuaCodeGen.ExportGenericDelegate add)
    {
        foreach(var kvp in Type2MemberExport)
        {
            add(kvp.Key, null);
        }
    }

    

    /// <summary>
    /// 检测指定接口是否不导出
    /// </summary>
    /// <param name="m"></param>
    static public bool OnCheckDontExport(MemberInfo m)
    {
        Type tp = m.DeclaringType;

        if (Type2MemberExport.ContainsKey(tp) )
        {
            List<string> mNames = Type2MemberExport[tp];
            if (mNames == null)
                return false;   //没有成员列表，全部导出

            if (mNames.Count == 0)
                return true;    //成员列表长度为0，全部不导出

            if (!mNames.Contains(m.Name))
                return true;    //不在成员列表里，不导出
        }
        
        return false;
    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽Export∽-★-∽--------∽-★-∽------∽-★-∽--------//

    /// <summary>
    /// 需要导出的Type及成员列表
    /// </summary>
    static Dictionary<Type, List<string>> Type2MemberExport = new Dictionary<Type, List<string>>
    {
        //{ typeof(mg.org.SluaTest), null },

        //mg.org
        { typeof(CCDefine), null },
        { typeof(CCApp), new List<string> {
            "keyboard",
            "soundMgr",
            "FrameRate",
        } },
        { typeof(Subject), new List<string> {
                "Attach",
                "Detach",
                "DetachByType",
            } },
        { typeof(CCModule), new List<string> {
                "Attach",
                "Detach",
                "DetachByType",
            } },
        { typeof(AssetData), new List<string> {
                "url",
                "progress",
                "isDone",
                "asset",
            } },
        { typeof(AssetCache), new List<string> {
                "me",
                "GetAsset",
                "HasAsset",
                "ClearAllAssets",
                "UnloadAsset",
                "UnloadAssetsUnused",
                "LoadSync",
                "LoadAsync",
                "LoadSync_Level",
                "LoadAsync_Level",
                "RetainByUrl",
                "ReleaseByUrl",
                "ReleaseByRefer",
            } },
        { typeof(AssetCacheRss),  new List<string> { } },
        { typeof(AssetCacheBdl), new List<string> { } },
        { typeof(GameObjCache), new List<string> {
                "me",
                "CreateGo",
                "LoadSync",
                "LoadAsync",
            } },
        { typeof(SprAtlasCache),  new List<string> {
                "me",
                "FormatPath",
                "LoadSprite",
                "SetSprite",
                "ReleaseSprite",
                "UnloadSprite",
            } },
        { typeof(LevelMgr),  new List<string> {
                "me",
                "LoadSync",
                "LoadAsync",
                "IsLevelLoaded",
            } },
        { typeof(LevelMgr.LevelData), new List<string> {
                "url",
                "progress",
                "isDone",
            } },
        { typeof(SoundMgr), new List<string> {
                "totalOn",      "bgmOn",    "effOn",
                "totalVolume",  "bgmVolume","effVolume",
                "PlayBgm",      "StopBgm",  "isBgmPlaying",
                "PlayOneShot",
                "PlayUi",
                "StopAllEff",
            } },
        { typeof(Keyboard), null },
        { typeof(LuaEvtCenter), new List<string> { }  },
        //--util
        { typeof(GameObjUtil), new List<string> {
                "FindChild",
                "FuzzySearchChild",
                "ChangeParent",
                "DontDestroyOnLoad",
            } },
        { typeof(ComponentUtil), new List<string> {
                "EnsureComponent",
            } },
        { typeof(DisplayUtil),new List<string> {
                "AddChild",
                "RemoveFromParent",
                "IsInTrash",
            } },
        { typeof(DateUtil),  new List<string> {
                "TimeFromStart",
                "TimeST_ms",
            } },
        { typeof(FileUtility),  new List<string> {
            } },

        //mg.org.KUI
        { typeof(KContainer), null },
        { typeof(KText), null },
        { typeof(KButton),new List<string> {
                "isPassPoint",
                "pressure",
                "maximumPossiblePressure",
            } },
        { typeof(KButtonShrinkable),new List<string> {
                "ShrinkEnabled",
            } },
        { typeof(KInputField), new List<string> {
                "AppendString",
                "GainFocus",
                "MoveIndexEnd",
            } },
        { typeof(KScrollView), new List<string> {
                "direction",
                "contentTrans",
                "maskTrans",
                "ResetContentPosition",
                "SetScrollable",
                "NeedScroll",
                "StopMovement",
                "JumpToTop",
                "JumpToBottom",
            } },
        { typeof(KListView), new List<string> {
                "InitLayoutParam",
                "GetItemByIndex",
                "ShowLen",
                "ClearList",
            } },
        { typeof(KListViewScroll), new List<string> {
                "direction",
                "contentTrans",
                "maskTrans",
                "SetScrollable",
                "NeedScroll",
                "JumpToTop",
                "JumpToBottom",
                "JumpToIndex",
                "ShowLen",
            } },
        { typeof(KToggle), new List<string> {
                "needReqChange",
                "Select",
            } },
        { typeof(KToggleGroup), new List<string> {
                "allowSwitchOff",
                "allowMultiple",
                "needReqChange",
                "index",
                "toggleNum",
                "GetToggle",
                "isSelected",
                "Select",
                "CancelAll",
            } },
        { typeof(KSlider), null },
        { typeof(KProgressBar), new List<string> {
                "percent",
                "value",
            } },
        { typeof(KImage), new List<string> {
                "LoadFromNet",
            } },

        //event
        { typeof(KComponentEvent<GameObject>), null },
        //util
        { typeof(KuiUtil), new List<string> { } },

    };

}