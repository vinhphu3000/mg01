--DeviceUtil
--@author jr.zeng
--2017年10月12日 下午3:01:08
local modname = "DeviceUtil"
--==================global reference=======================

local SystemInfo = Unity.SystemInfo
local CCDefine = mg.org.CCDefine

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
_ENV[modname] = {}
local DeviceUtil = _ENV[modname]

--===================module content========================

OS_Name = 
    {
        NONE    = "none",
        UNKNOWN = "unknown",
        WINDOWS = "windows",
        ANDROID = "android",
        IOS     = "ios",
        MAC     = "mac",
        LINUX   = "linux",
    }

Platform_Name = 
{
        EDITOR = "editor",
        PC = "pc",
        IOS     = "ios",
        ANDROID = "android",
}

local function get_os()

    local name 
    local os = SystemInfo.operatingSystem  --操作系统
    local s, e = string.find(os, 'iOS')
    if s and e then
        name = OS_Name.IOS
    end
    local s, e = string.find(os, 'iPhone OS')
    if s and e then
        name = OS_Name.IOS
    end
    s, e = string.find(os, 'Android OS')
    if s and e then
        name = OS_Name.ANDROID
    end
    s, e = string.find(os, 'Windows')
    if s and e then
        name = OS_Name.WINDOWS
    end
    
    name = name or OS_Name.IOS
    return name
end

OS_NAME = get_os()
PLATFORM_NAME = CCDefine.Platform
--nslog.debug("PLATFORM_NAME", PLATFORM_NAME)

IS_EDITOR = PLATFORM_NAME == Platform_Name.EDITOR
IS_DEBUG = CCDefine.DEBUG


return DeviceUtil