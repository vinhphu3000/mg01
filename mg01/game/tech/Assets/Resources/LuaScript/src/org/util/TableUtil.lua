--TableUtil
--@author jr.zeng
--2017年4月19日 上午11:45:18
local modname = "TableUtil"
--==================global reference=======================

local pairs = pairs

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
TableUtil = {}
local TableUtil = TableUtil

--===================module content========================

--浅克隆
function TableUtil.clone(a)
    
    local b = {}
    for k,v in pairs(a) do
        b[k] = v
    end
    return b
end

--A复制到B
function TableUtil.copyTo(a, b)
    for k,v in pairs(a) do
        b[k] = v
    end
    return b
end

--从B复制到A
function TableUtil.copyFrom(a, b)

    for k,v in pairs(b) do
        a[k] = v
    end
    return a
end



--获取键数组
--@param 转化为数组的字典
--@return
function TableUtil.get_keys(dic)

	local array = {}
	for k, v in pairs( dic ) do
		array[#array + 1 ] = k
	end
	return array
end


--获取值数组
--@param 转化为数组的字典
--@return
function TableUtil.get_values(dic)

	local array = {}
	for k, v in pairs( dic ) do
		array[#array + 1 ] = v
	end
	return array
end

--合并常量
--@target
--@source
function TableUtil.merge_const(target, source)
	for k,v in pairs(source) do
		target[k] = v
	end
	return target
end

--合并table
function TableUtil.merge_table(source, target)
	for k,v in pairs(source) do
		if target[k] == nil then
			target[k] = v
		end
	end
end

return TableUtil