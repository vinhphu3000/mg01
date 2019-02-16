--functions
--@author jr.zeng
--2017年4月14日 下午12:17:41
local modname = "functions"
--==================global reference=======================

local type = type
local pairs = pairs
local ipairs = ipairs
local unpack = table.unpack
local setmetatable = setmetatable
local getmetatable = getmetatable
local rawget = rawget
local rawset = rawset
local next = next

--t2str
local string = string
local srep = string.rep
local tostring = tostring
local tconcat = table.concat

tinsert = table.insert
local tinsert = tinsert

--upvalue
local debug = debug
local getlocal = debug.getlocal

--===================namespace========================
local ns = nil
local using = {}
local _ENV = namespace(ns, using)
--===================module property========================


function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local newObject = {}
        lookup_table[object] = newObject
        for key, value in pairs(object) do
            newObject[_copy(key)] = _copy(value)
        end
        return setmetatable(newObject, getmetatable(object))
    end
    return _copy(object)
end

function clear_tbl(tbl)
    for k,v in pairs(tbl) do
        tbl[k] = nil
    end
end

--分配唯一id
local _obj_id = 1
--local _obj_id = 1000000
function alloc_obj_id()
    _obj_id = _obj_id + 1
    return _obj_id
end



--//-------~★~-------~★~-------~★~单例模式~★~-------~★~-------~★~-------//

local cls2inst = {}
inst = function(class)

    if class == nil then
        assert(class, "[Instance] miss class")
    end

    local obj = cls2inst[class]
    if obj then
        return obj
    end

    obj = class.new()
    cls2inst[class] = obj
    return obj
end
Inst = inst

--移除单例
delInst = function(class)

    if class == nil then
        assert(class, "[Instance] miss class")
    end

    local inst = cls2inst[class]
    if not inst then
        return end

    cls2inst[class] = nil
end

--//-------~★~-------~★~-------~★~call~★~-------~★~-------~★~-------//

--空表
empty_tbl = setmetatable({}, {
	__newindex = function(self, k, v)
		assert(false, "don't write to empty_tbl")
	end,
})

--空函数
function empty_fun() 

end

function call_fun(fun, sender, ...)
    if sender then
        fun(sender, ...)
    else
        fun(...)
    end
end

--回调函数
function callback(fun, sender)

	if not fun then
		return nil
	end

    local excute = function(...)
        if sender == nil then
            return fun(...)
        else
            return fun(sender, ...)
        end
    end

    return excute
end


--回调函数_带不定参
function callback_n(fun, sender, ...)

    if not fun then
        return nil
    end

    local p = {...}
    local excute = function(...)

        local p2 = {...}
        if #p2 > 0 then
            for i,v in ipairs(p2) do
                p[#p+1] = v
            end
        end
        if sender == nil then
            return fun(unpack(p))
        else
            return fun(sender, unpack(p))
        end
    end

    return excute
end





--upvalue转为table
--@level 栈级别
function upv2t(level)

    level = level or 2

    local result = {}
    local i = 1
    while true do
        local n, v = debug.getlocal(level, i)
        if not n then 
            break end

        if n ~= "self" then --忽略self
            result[n] = v
        end

        i = i + 1
    end

    --log.print_t(result, "upv2t")

    return result
end
