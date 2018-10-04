--unity新增的Action
--@author jr.zeng
--@date 2018/1/2  22:52
local modname = "Action"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================

local Action = Action

--===================module content========================

local pool = Inst(ClassPools)

local __new = new
local function new(cls)
	return pool:pop(cls)
end


--移动(localPosition)
function mov_to2(duration, toPos)


end

--移动(anchoredPosition)
function mov_to3(duration, toPos)

	local action = new(MovTo3)
	action:initWith(duration, toPos)
	return action
end




return Action