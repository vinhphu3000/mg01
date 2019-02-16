-- const
--@author jr.zeng
--@date 2019/2/11  15:22
local modname = "const"
--==================global reference=======================

--===================namespace========================
local ns = 'org'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module content========================

local evt_id = 10000

EVT_TYPE = {}
local EVT_TYPE = EVT_TYPE

--所有事件都注册到这里
function get_evt_type(flag)
	evt_id = evt_id + 1
	EVT_TYPE[flag] = evt_id
	return evt_id
end