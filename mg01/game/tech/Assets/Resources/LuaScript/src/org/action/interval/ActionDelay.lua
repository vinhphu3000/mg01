--ActionDelay
--@author jr.zeng
--2017年7月17日 上午11:22:13
local modname = "ActionDelay"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local ActionDelay = _ENV[modname]

--===================module content========================

function ActionDelay:__ctor()

end



return ActionDelay