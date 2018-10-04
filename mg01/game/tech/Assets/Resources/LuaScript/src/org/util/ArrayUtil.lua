--ArrayUtil
--@author jr.zeng
--2017年4月25日 上午10:33:19
local modname = "ArrayUtil"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

ArrayUtil = {}
local ArrayUtil = ArrayUtil

--数组拷贝
function ArrayUtil.clone(arr)

    local result = {};
    for i = 1, #arr do
        result[#result+1] = arr[i];
    end
    return result;
end

--数组连接
function ArrayUtil.concat(arr1, arr2)

    local result = {}

    if #arr1 > 0 then
        for i = 1, #arr1 do
            result[#result+1] = arr1[i];
        end
    end

    if #arr2 > 0 then
        for i = 1, #arr2 do
            result[#result+1] = arr2[i];
        end
    end

    return result
end

--连接并剔除重复的项duplicate
function ArrayUtil.concat_rip_dup(arr1, arr2)
    
    local result = {}
    local check = {}
    local a
    
    if arr1 then
        for i = 1, #arr1 do
            a =  arr1[i]
            if not check[a] then
                check[a] = true
                result[#result+1] = a
            end
        end
    end

    if arr2 then
        for i = 1, #arr2 do
            a =  arr2[i]
            if not check[a] then
                check[a] = true
                result[#result+1] = a
            end
        end
    end
    
    return result
end


function ArrayUtil.indexOf(arr, obj)

	local result = -1

	if #arr > 0 then
		for i = 1, #arr do
			if arr[i] == obj then
				result = i
				break
			end
		end
	end

	return result
end

function ArrayUtil.contain(arr, obj)
	return  ArrayUtil.indexOf(arr, obj) > 0
end

function ArrayUtil.dump(arr)

	local str = ''
	for i,v in ipairs(arr) do

		if i > 1 then
			str = str .. ', '
		end


		if type(v) == "table" then
			local s = t2str(v, 4, true)   --默认扫描3层
			str = str .. "\n" .. s .. "\n"
		else
			str = str .. tostring(v)
		end
	end
	return str
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽工具方法∽-★-∽--------∽-★-∽------∽-★-∽--------//


--打乱数组
--返回打乱后的数组
function ArrayUtil.random_shuffle(arr)

    if arr == nil then
        return 
    end

    local len = #arr
    if len == 0 then
        return arr
    end

    local ret = {}
    TableUtil.copyTo(arr, ret)

	local times = math.ceil(len / 2)
    for i = 1, times do
        local rdmIndex = math.random(i, len)
        local temp = ret[i]
        ret[i] = ret[rdmIndex]
        ret[rdmIndex] = temp
    end
    return ret
end

--產生一個序列数组
--@param len       数组长度
--@param base      基数
--@param increase  步长
function ArrayUtil.genSequence(len, base, increase)

    base = base or 1
    increase = increase or 1

    local arr = {}
    for i=1, len do
        arr[#arr+1] = base
        base = base + increase
    end
    return arr
end


return ArrayUtil