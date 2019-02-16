-- SurvivorInfo
--@author jr.zeng
--@date 2019/2/11  17:03
local modname = "SurvivorInfo"
--==================global reference=======================

--===================namespace========================
local ns = 'md'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = RoleInfo	--父类
SurvivorInfo = class(modname, super, _ENV)
local SurvivorInfo = SurvivorInfo

--===================module content========================

function SurvivorInfo:__ctor()

	--性别
	self.sex = 1

	--生存点
	self.survival = 0
	--移动力
	self.movement = 5
	--力量
	self.strength = 0
	--回避
	self.evasion = 0
	--幸运
	self.luck = 0

end




return SurvivorInfo