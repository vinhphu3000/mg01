--对象池
--@author jr.zeng
--2017年4月14日 下午4:41:49
local modname = "ClassPools"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
ClassPools = class("ClassPools", super)
local ClassPools = ClassPools
local M = ClassPools

--===================module content========================

function ClassPools:__ctor()

    self.m_cls2pool = {}

end



function ClassPools:createPool(class)
    if self.m_cls2pool[class] == nil then
        self.m_cls2pool[class] = new(ClassPool, class) end
    return self.m_cls2pool[class]
end


function ClassPools:pop(class)
    local pool = self:createPool(class)
    return pool:pop()
end

function ClassPools:push(obj)

    if obj.class == nil then
        assert(obj.class, "[ClassPools] miss class")    --不是类
    end

    local pool =  self:createPool(obj.class)
    pool:push(obj)
end

--清空对象池
function ClassPools:clear()

    for k, v in pairs(self.m_cls2pool) do
        v:clear()
    end
end


return ClassPools