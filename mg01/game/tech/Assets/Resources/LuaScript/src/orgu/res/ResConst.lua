--ResConst
--@author jr.zeng
--2017年9月29日 下午7:41:14
local modname = "ResConst"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================





--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//


RES_EVT =
    {
	    --c#
        --加载完成
        LOAD_COMPLETE = "LOAD_COMPLETE",
        --加载异常
        LOAD_EXCEPTION = "LOAD_EXCEPTION",
	    --场景加载完成
	    LOAD_LEVEL_COMPLETE = "LOAD_LEVEL_COMPLETE",
	    LOAD_LEVEL_EXCEPTION = "LOAD_LEVEL_EXCEPTION",

	    --lua
    }