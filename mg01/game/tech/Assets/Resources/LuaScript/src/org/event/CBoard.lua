--CBoard
--@author jr.zeng
--2017年10月22日 上午10:56:38
local modname = "CBoard"
--==================global reference=======================

local table = table
local type = type
local pairs = pairs
local assert = assert

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local CBoard = _ENV[modname]

local TablePool = TablePool
local clear_tbl = clear_tbl


local evtPool = Inst(ClassPools):createPool(SubjectEvent)
local obsPool = Inst(ClassPools):createPool(Observer)


--创建observer
local function createObs(name, listener, target, notifier, refer)
	local obs = obsPool:pop()
	obs:init(name, listener, target, notifier, refer)
	return obs
end

local function clearObs(obs)
	obs:clear()
	obsPool:push(obs)
end

--===================module content========================

function CBoard:__ctor()

    self.m_obsDic = {}
    self.m_obsNum = 0        --监听者的数量

end

function CBoard:hasType(type)
    return self.m_obsDic[type] ~= nil
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

--@priority TODO
function CBoard:attach(name, listener, target, refer)

    if type(listener) ~= "function" then
        assert(false)
    end

	refer = refer or target

    local obs = self.m_obsDic[name]
    if obs then
	    obs:init(name, listener, target, self, refer)
    else
        obs = createObs(name, listener, target, self, refer)
        self.m_obsDic[name] = obs
        self.m_obsNum = self.m_obsNum + 1
    end

	return obs
end

--移除监听
function CBoard:detach(name, listener, target)

	if type(listener) ~= "function" then
		assert(false)
	end

	local obs = self.m_obsDic[name]
	if not obs then
		return false
	end

	if obs.fun ~= listener or obs.target ~= target then
		return false
	end

	self.m_obsDic[name] = nil
	clearObs(obs)
	self.m_obsNum = self.m_obsNum - 1
	return true
end


--根据类型移除监听
function CBoard:detachByType(type)

    local obs = self.m_obsDic[type]
    if not obs then
        return false 
    end

    self.m_obsDic[type] = nil
	clearObs(obs)
    self.m_obsNum = self.m_obsNum - 1
    return true
end


--根据目标移除监听
function CBoard:detachByTarget(target)

    if target == nil then
        return end

    for t, obs in pairs(self.m_obsDic) do
        if obs.target == target then
            self.m_obsDic[t] = nil
	        clearObs(obs)
            self.m_obsNum = self.m_obsNum - 1
        end
    end
end

function CBoard:detachByObs(obs_)

	local name = obs_.name
	local obs = self.m_obsDic[name]
	if obs ~= obs_ then
		clearObs(obs_)
		assert(false, '错误的观察者：' .. type)
		return
	end

	nslog.print_t(string.format('移除观察者 %s, 事件类型：%s, 剩余数量：%d',
		Refer.format(obs.m_refer),
		name,
		self.m_obsNum-1 ))

	self.m_obsDic[type] = nil
	clearObs(obs)
	self.m_obsNum = self.m_obsNum - 1
end


--根据标志移除监听
function CBoard:detachByFlag(flag)

    if flag == nil then
        return false end

    local b = false
    
    for t, obs in pairs(self.m_obsDic) do
        if obs.flag == flag then
            self.m_obsDic[t] = nil
	        clearObs(obs)
            self.m_obsNum = self.m_obsNum - 1
            b = true
        end
    end
    
    return b
end

--移除所有监听
--@clear 清空观察者
function CBoard:detachAll(clear)

    if self.m_obsNum ~= 0 then
        self.m_obsNum = 0

        for k,obs in pairs(self.m_obsDic) do
	        clearObs(obs)
        end
        clear_tbl(self.m_obsDic)
    end

end


function CBoard:dump()

	local str = string.format('%s, obsNum:%d\n', self.referId, self.m_obsNum )
	for k, obs in pairs(self.m_obsDic) do
		str = str ..'type: '.. k .. '\n'
		str = str ..'\t' .. obs:dump() .. '\n'
	end
	return str
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽派发相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

local tmp_k = nil

--派发
function CBoard:notify(name, ...)

    local obs = self.m_obsDic[name]
    if obs == nil then 
        return end
        
    if obs.target then
        obs.fun(obs.target, ...)
    else
        obs.fun(...)
    end
end


--派发事件
function CBoard:notifyEvent(e)

    assert(e and e.type)

    local obs = self.m_obsDic[e.type]
    if obs == nil then 
        return end

    e.target = e.target or self
    if obs.target  then
        obs.fun(obs.target, e)
    else
        obs.fun(e)
    end
end

--用事件派发
function CBoard:notifyWithEvent(type, data)

    if self.m_obsDic[type] == nil then 
        return end

    local e = evtPool:pop() --事件可回收
    e.type = type
    e.data = data

    self:notifyEvent(e)

    e:clear()
    evtPool:push(e)
end

return CBoard