--UIAbs
--@author jr.zeng
--2017年10月12日 下午4:48:01
local modname = "UIAbs"
--==================global reference=======================

local type = type

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = ImageAbs	--父类
UIAbs = class(modname, super)
local UIAbs = UIAbs

--===================module content========================

function UIAbs:__ctor()

    --nslog.debug("__ctor step1")
end

--获取ui控制器
function UIAbs:get_handler(GoOrPath_, uiMap_)
    local go
    if type(GoOrPath_) == "string" then
        go = self.m_gameObject:FindChild_(GoOrPath_)
    else
        go = GoOrPath_ or self.m_gameObject
    end

	if not go then
		nslog.error("找不到ui的gameobject", GoOrPath_)
	end

    return UIHandler(go, uiMap_, self)
end

--//-------~★~-------~★~-------~★~gameobject~★~-------~★~-------~★~-------//

function UIAbs:__onShowGameObject()
    

    
end


return UIAbs