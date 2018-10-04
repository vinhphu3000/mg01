-- DateUtil
--@author jr.zeng
--@date 2017/11/17  16:35
local modname = "DateUtil"
--==================global reference=======================

local csDateUtil = mg.org.DateUtil

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

DateUtil = {} or DateUtil
local DateUtil = DateUtil

--===================module content========================



--//-------~★~-------~★~-------~★~C#接口~★~-------~★~-------~★~-------//


--从启动开始的时长(秒)
function DateUtil.time_from_start()
	return csDateUtil.TimeFromStart

end

--当前时间戳(毫秒)
function DateUtil.time_st_ms()
	return csDateUtil.TimeST_ms
end





return DateUtil