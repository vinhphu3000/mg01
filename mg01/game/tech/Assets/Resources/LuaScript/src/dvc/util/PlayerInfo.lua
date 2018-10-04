-- 玩家信息
--@author jr.zeng
--@date 2018/7/30  20:16
local modname = "PlayerInfo"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
PlayerInfo = class(modname, super, _ENV)
local PlayerInfo = PlayerInfo

--===================module content========================

function PlayerInfo:__ctor()

	self.uuid = alloc_obj_id()
	self.name = '路人甲'
	--性别
	self.sex = 0

	--玩家序号
	self.index = 0

	--是否自己
	self.is_me = false
	--是否cpu
	self.is_cpu = false

end



return PlayerInfo