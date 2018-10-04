--DateUtil
--@author jr.zeng
--2017年7月4日 上午9:45:29
local modname = "DateUtil"
--==================global reference=======================

local tonumber = tonumber
local os = os

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

DateUtil = {} or DateUtil
local DateUtil = DateUtil

--===================module content========================




--//-------~★~-------~★~-------~★~tool~★~-------~★~-------~★~-------//


--转换时间文本 -> 时分秒
--@str "14:59:59" 24小时制
function DateUtil.timeStr2hms(str)
    
    local str_arr = string.split(str, ":")
    return {
        hour = tonumber(str_arr[1] or 0), 
        min = tonumber(str_arr[2] or 0),
        sec = tonumber(str_arr[3] or 0)
    }
end


--转换时间文本 -> 秒
--@str "14:59:59" 24小时制
function DateUtil.timeStr2s(str)
    
    --nslog.debug("timeStr2s", str)
    local str_arr = string.split(str, ":")
    local hour = tonumber(str_arr[1] or 0) 
    local min = tonumber(str_arr[2] or 0)
    local sec = tonumber(str_arr[3] or 0)
    return hour * 3600  + min * 60 + sec
end

--转换hms -> 秒
function DateUtil.hms2sec(hour, min, sec)

    return hour * 3600  + min * 60 + sec
end

return DateUtil