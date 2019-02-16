--Const
--@author jr.zeng
--2017年9月25日 下午3:13:37
local modname = "const"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

--平台类型
G_OS_NAME = 
    {
        NONE    = "none",
        UNKNOWN = "unknown",
        WINDOWS = "windows",
        ANDROID = "android",
        IOS     = "ios",
        MAC     = "mac",
        LINUX   = "linux",
    }


COMPONENT_TYPE = 
{
    

}


--按键事件
KEY_EVENT =
{
	--按下
	PRESS   = "KEY_EVENT_PRESS",
	--弹起
	RELEASE = "KEY_EVENT_RELEASE",
}


local EVENT_TYPE =
    {
        --进帧
        UPDATE = get_evt_type("UPDATE"),

    }
