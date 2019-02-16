-- RoleInfo
--@author jr.zeng
--@date 2019/2/11  17:02
local modname = "RoleInfo"
--==================global reference=======================

--===================namespace========================
local ns = 'md'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
RoleInfo = class(modname, super, _ENV)
local RoleInfo = RoleInfo

--===================module content========================

function RoleInfo:__ctor()

	self.uuid = alloc_obj_id()

	self.name = "unkown"

end


return RoleInfo