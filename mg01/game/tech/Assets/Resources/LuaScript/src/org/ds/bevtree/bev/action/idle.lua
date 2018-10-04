-- idle
--@author jr.zeng
--@date 2018/4/22  10:50
local modname = "idle"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_base	--父类
idle = class(modname, super, _ENV)
local idle = idle

--===================module content========================

function idle:ctor(setting)

	super.ctor(self, setting)
end


function idle:__update(input)

	--一直running
end


return idle