--class
--@author jr.zeng
--2017年5月22日 上午11:31:02
local modname = "class"
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

--===================namespace========================
local ns = nil
local using = {}
local _ENV = namespace(ns, using)


local alloc_obj_id = alloc_obj_id

--===================module property========================


--源码修改 jr.zeng_20150825
--Create an class.
--function class(classname, super)
--
--    local superType = type(super)
--    local cls
--
--    --if superType ~= "function" and superType ~= "table" then
--    if superType ~= "table" then
--        superType = nil
--        super = nil
--    end
--
--    -- inherited from Lua Object
--    if super then
--        cls = clone(super)
--        cls.super = super
--    else
--        cls = {ctor = function() end}
--    end
--
--    cls.cname = classname
--    --cls.__CTypeName = 2 -- lua
--    cls.__index = cls
--
--    
--    function cls.new(...)
--        local instance = setmetatable({}, cls)
--        instance.class = cls
--        instance:ctor(...)
--        return instance
--    end
--
--    return cls
--end

--克隆父类
local function clone_super(super)

    local new_super = {}
    new_super.cname = super.cname --类名
    new_super.__supers = super.__supers
    new_super.__native = super      --记录源头
    
    local tp
    for key, value in pairs(super) do
        
        tp = type(value)
        --if type == "function" then   --只拷贝方法
        if tp ~= "table" then   --不拷贝table
            new_super[key] = value
        end
    end

    setmetatable(new_super, getmetatable(super))    --为何?
    return new_super
end



--源码修改 jr.zeng_20170418
--Create an class.
--@super 类 或 类数组
--@params 一些参数
function inherit(cls, super)

    cls.cname = cls.cname or "unknown"

    local supers
    if type(super) == "table" then
        if #super > 0 then
            --多super
            supers = super
        else
            supers = {super}
        end
    end

    if supers then

        for _, super in ipairs(supers) do

            local superType = type(super)
            assert(superType == "nil" or superType == "table",
                string.format("class() - create class \"%s\" with invalid super class type \"%s\"",
                    cls.cname, superType))

            if superType == "table" then

                --local c = clone(super)
                local c = clone_super(super)
                -- super is pure lua class
                cls.__supers = cls.__supers or {}
                cls.__supers[#cls.__supers + 1] = c
                if not cls.super then
                    -- set first super pure lua class as class.super
                    cls.super = c
                end

            else
                error(string.format("class() - create class \"%s\" with invalid super type",
                    cls.cname), 0)
            end
        end
    end

    local cmeta = {
        __call = function (t, ...)
            return cls.new(...)
        end,
    --        __tostring = function(self)       --用了就没办法打出table(0xxxxx)
    --            return cls.cname .. "(class instance)"
    --        end
    }


    if not cls.__supers or #cls.__supers == 1 then

        if cls.super then
            cmeta.__index = function(self, key)     --这里self == cls
                
                --log.print_r("self", key, self)
                local value = cls.super[key]
                if value then
                    rawset(self, key, value) --缓存起来, 不用每次都__index
                end
                return value
            end
        end
    else
        cmeta.__index = function(self, key)
            local value
            local supers = cls.__supers
            for i = 1, #supers do
                local super = supers[i]
                if super[key] then 
                    value = super[key]
                    break 
                end
            end
            if value then
                rawset(self, key, value) --缓存起来, 不用每次都__index
            end
            return value
        end
    end

    cls.__index = cls   --这个要放setmetatable后面, 不然会被调用__index
    setmetatable(cls, cmeta)


    if not rawget(cls, "ctor") then
        -- add default constructor
        cls.ctor = function(self, ...) 

            self.__had_ctor = self.__had_ctor or {}

            if cls.__supers then
                --调用基类构造函数
                for i = 1, #cls.__supers do
                    local super = cls.__supers[i]
	                if not self.__had_ctor[super.__native.obj_id] then
		                --log.debug(modname, "ctor super: ", super.cname)
		                super.ctor(self, ...)
		                self.__had_ctor[super.__native.obj_id] = true --补充标记, 要放到后面
	                --else
		             --   log.print_t('已经构造过了, super:', super.__native.cname)
	                end
                end
            end

            if not self.__had_ctor[cls.obj_id] then
                self.__had_ctor[cls.obj_id] = true --标记已经构造过
                --log.debug(modname, "ctor: ", cls.cname)
                cls.__ctor(self, ...)
	        --else
	        --    log.print_t('已经构造过了, cls:', cls.cname)
            end
        end

        cls.__ctor = function() 
        --实际的构造函数
        end 
    end

    cls.new = function(...)

        local meta = cls

        --        meta = {
        --            __index = function(self, k)
        --                return cls[k]                        
        --            end,
        --            __newindex = function(self, k, v)
        --                rawset(self, k, v); 
        --            end,
        --        }

        local instance = { obj_id = alloc_obj_id() }
        instance.class = cls
        instance.cname = cls.cname
	    instance.isClass = false
	    instance.isInstance = true --标记为实例
        instance.referId = cls.cname .. "*#LUA#" .. instance.obj_id      --引用者id
        setmetatable(instance, meta)
        instance:ctor(...)
        return instance
    end

    return cls
end

local inherit = inherit
--@env_ 作用域
function class(classname, super, env_)
    local cls = {  obj_id = alloc_obj_id() }
    cls.cname = classname
	cls.isClass = true
    cls.referId = cls.cname .. "#LUA_CLS#" .. cls.obj_id      --引用者id
    inherit(cls, super)
    
    --if env_ then
    --    env_[classname] = cls
    --end
    return cls
end

--创建类实例
function new(cls, ...)
    if cls.new then
        return cls.new(...)
    else
        assert(false, "miss new")
    end
    return nil
end

