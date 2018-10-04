-- dvc_util
--@author jr.zeng
--@date 2018/8/10  21:08
local modname = "dvc_util"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================

dvc_util = {}
local dvc_util = dvc_util

local CARD_COLOR_NAME = CARD_COLOR_NAME

--===================module content========================


function dvc_util.get_card_name(color, pattern)

	local color_name = CARD_COLOR_NAME[color]
	return string.format('%s[%s]', pattern, color_name)
end


return dvc_util