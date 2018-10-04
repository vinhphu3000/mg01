--MathUtil
--@author jr.zeng
--2017年4月18日 下午9:35:49
local modname = "MathUtil"
--==================global reference=======================

local math = math

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
MathUtil = {}
local MathUtil = MathUtil

--===================module content========================


--夹值
function MathUtil.clamp(value, min, max)
    
    min = min or 0
    max = max or 1
    if value < min then
        return min, -1
    elseif value < max then
        return value, 0
    end
    return max, 1
end


function MathUtil.clamp_round(value, min, max)
    
    local value, flag = MathUtil.clamp(value, min, max)
    if flag ~= 0 then
        return value
    end
    
    local a = value - min
    local dis = max - min
    local percent = a / dis 
    if percent < 0.5 then
        return min
    end
    return max
end

--按精度处理浮点型 1.1111 -> 1.1
--@significant 精度
function MathUtil.wrap_float(value, significant)
	local sig = significant or 0

	value = value * 10^sig
	value = math.floor(value)
	value = value / 10^sig



	return value
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽随机相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


--获取机会
--value 0~1
function MathUtil.get_chance(chance)

    if chance >= 1 then
        return true
    end
    return math.random() <= chance
end


return MathUtil