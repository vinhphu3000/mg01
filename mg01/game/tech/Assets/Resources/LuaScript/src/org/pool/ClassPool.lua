--类对象池
--@author jr.zeng
--2017年4月14日 下午4:37:57
local modname = "ClassPool"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
ClassPool = class("ClassPool", super)
local ClassPool = ClassPool
local M = ClassPool

--===================module content========================

--@cacheBusy 是否缓存繁忙区
function ClassPool:__ctor(class, cacheBusy)

    if type(class) == "string" then
        self.m_className = class
        self.m_cls = _ENV[class]
    else
        self.m_cls = class
        self.m_className = self.m_cls.cname
    end

	assert(self.m_cls)

    self.m_cacheBusy = cacheBusy

    self.m_idle_arr = new(Array)
    self.m_busy_arr = new(Array)
end

--预分配
function ClassPool:allocChunk(num)

    local obj
    for i=1, num do
        obj = self.m_cls.new()
        self.m_idle_arr[#self.m_idle_arr + 1] = obj
    end
end

function ClassPool:pop()

    local obj = self.m_idle_arr:pop()
    if obj == nil then
        obj = self.m_cls.new()
        obj.__inPool = false
        obj.__fromPool = true   --标记是从池里拿出来的
    else

        obj.__inPool = false
    end
    
    if self.m_cacheBusy then
        --缓存
        self.m_busy_arr[#self.m_busy_arr+1] = obj
    end
    
    return obj
end

--回收对象
function ClassPool:push(obj)

    if obj.cname == nil or obj.cname ~= self.m_className then
        --不是该池的类
        return
    end
    
    if obj.__inPool then
        nslog.warn(modname, "此对象已经在池里", obj.cname)
    else

        if self.m_cacheBusy then
            if not self.m_busy_arr:remove(obj) then
                nslog.warn(modname, "此对象不在繁忙区", obj.cname)
            end
        end

        self.m_idle_arr[#self.m_idle_arr + 1] = obj
        obj.__inPool = true
    end
end

--回收繁忙区
function ClassPool:recycleBuzies()
    
    if not self.m_cacheBusy or
        #self.m_busy_arr == 0 then
        return end

        
    for i, obj in ipairs(self.m_busy_arr) do
        self.m_idle_arr[#self.m_idle_arr + 1] = obj
        obj.__inPool = true
    end
    self.m_busy_arr:clear()
    
    --nslog.debug(modname, self.m_cls.cname, "recycleBuzies", #self.m_idle_arr)
end

function ClassPool:getRemainCount()
    return #self.m_idle_arr
end

--清空空闲区
function ClassPool:clearIdles()

    self.m_idle_arr:clear()
end

--清空全部
function ClassPool:clear()

    self.m_idle_arr:clear()
    self.m_busy_arr:clear()
end


return ClassPool