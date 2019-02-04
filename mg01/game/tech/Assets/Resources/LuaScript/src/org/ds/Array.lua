--数组
--@author jr.zeng
--2017年4月14日 下午4:47:10
local modname = "Array"
--==================global reference=======================

local table = table
local sort = table.sort

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
Array = class("Array", super)
local Array = Array
local M = Array

--===================module content========================

function Array:__ctor(...)

    local input = {...}
    if #input > 0 then
        for i=1, #input do
            self[#self+1] =  input[i]
        end
    end
end

function Array:indexOf(obj)

    local result = -1

    if #self > 0 then
        for i = 1, #self do
            if self[i] == obj then
                result = i
                break
            end
        end
    end

    return result
end

function Array:contain(obj)
    return  self:indexOf(obj) > 0
end

--将一个或多个元素添加到数组的结尾，并返回该数组的新长度
function Array:push(a1, a2, ...)

    if a2 == nil then
        self[#self+1] = a1
    else
        self[#self+1] = a1
        self[#self+1] = a2
        local add = {...}
	    local len = #add
	    if len > 0 then
		    for i = 1, len do
			    self[#self+1] = add[i]
		    end
	    end
    end

    return #self
end

function Array:add(obj)
    
    --nslog.debug(modname, "add", #self)
    self[#self+1] = obj
end

function Array:insert(i, obj)
    table.insert(self, i, obj)
end

--删除数组中最后一个元素，并返回该元素的值
function Array:pop()
    local cnt = #self
    if cnt > 0 then
        local o = self[#self]
        self[#self] = nil
    end
    return nil
end

--删除数组中第一个元素，并返回该元素
function Array:shift()
    local o = self[1]
    if o ~= nil then
        table.remove(self, 1)
    end
    return o
end

--将一个或多个元素添加到数组的开头，并返回该数组的新长度
function Array:unshift(...)

    local add = {...}
    for i = #add, 1, -1 do
        table.insert(self, 1, add[i])
    end
    return #self
end

--给数组添加元素以及从数组中删除元素。 此方法会修改数组但不制作副本
function Array:splice(start, deleteCount)

    deleteCount = deleteCount or 1

    if start <= 0 or start > #self  then 
        return false
    end

    if deleteCount == 1 then

        table.remove(self, start)
    else

        local _end = start + deleteCount - 1  --获取末尾序号
        _end = math.min(#self, _end)
        for i = start, _end do
            table.remove(self, start)
        end
    end

    return #self
end

function Array:remove(obj)

    local index = Array.indexOf(self, obj)
    if index >= 0 then
	    Array.splice(self, index)
        return true
    end
    return false
end

function Array:removeAt(index)

    if index <= 0 or index > #self  then 
        return end
    table.remove(self, index)
end

function Array:concat(arr2)

    for i = 1, #arr2 do
        self[#self+1] = arr2[i]
    end
end


--翻转数组
function Array:reverse()
    if #self == 1 then
        return
    end

    local a
    local index
    local len = #self
    local l = math.ceil(len / 2)
    for i=1, l do
        index = len - i + 1
        if i ~= index then
            a = self[i]
            self[i] = self[index]
            self[index] = a
        end
    end
end

function Array:empty()
    return #self == 0
end

--数组长度
function Array:size()
    return #self
end

--获取首对象
function Array:head()
    return self[1]
end

--获取末对象
function Array:tail()
    return self[#self]
end


function Array:to_table()
    
    local result = {}
    local len = #self
    for i=1, len do
        result[i] = self[i]
    end
    return result
end

--清空数组
function Array:clear()

    local len = #self
    if len > 0 then
        for i = 1, len do
            self[i] = nil
        end
    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽tool∽-★-∽--------∽-★-∽------∽-★-∽--------//

--打乱数组
function Array:random_shuffle()
    local len = #self
    if len > 1 then
        for i = 1, math.ceil(len / 2) do
            local rdmIndex = math.random(i, len)

            local temp = self[i]
            self[i] = self[rdmIndex]
            self[rdmIndex] = temp
        end
    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽排序∽-★-∽--------∽-★-∽------∽-★-∽--------//

Array.ASCENDING  = 0   --升序
Array.DESCENDING = 1   --降序

local sortMap = {}
sortMap[Array.ASCENDING] = Array.ASCENDING
sortMap[Array.DESCENDING] = Array.DESCENDING

--排序
function Array:sort(option)

    if #self <= 1 then
        return end

    option = sortMap[option] or Array.ASCENDING

    local function back(a, b)
        if option == Array.DESCENDING then
            --降序
            return a > b
        end

        return a < b
    end

    table.sort(self, back)
end

--按属性排序
--例1 arr:sortOn("type")    --默认升序
--例2 arr:sortOn("id", Array.DESCENDING)
--例3 arr:sortOn({"id", "id2", "id3"}, {Array.ASCENDING, Array.DESCENDING, Array.DESCENDING})
function Array:sortOn(names, options)

    if #self <= 1 then
        return end

    local tp = type(names)
    assert(tp == "table" or tp == "string")
    if tp == "string" then
        names = {names}
    end

    options = options or Array.ASCENDING
    local tp = type(options)
    assert(tp == "table" or tp == "number")
    if tp == "number" then
        options = {options}
    end

    local function back(a, b)

        local option
        local name
        local value_a
        local value_b
        for i=1, #names do
            name = names[i]
            option = options[i] and sortMap[options[i]] or ( option or Array.ASCENDING )

            value_a = a[name]
            value_b = b[name]
            if value_a ~= nil and value_a ~= value_b then
                if option == Array.DESCENDING then
                    --降序
                    return value_a > value_b
                else
                    return value_a < value_b
                end
            end
        end

        return false
    end

    sort(self, back)
end

return Array