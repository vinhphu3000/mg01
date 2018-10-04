--观察者模式
--@author jr.zeng
--2017年4月14日 下午4:13:56
local modname = "Subject"
--==================global reference=======================

local table = table
local type = type
local pairs = pairs
local ipairs = ipairs
local assert = assert

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
Subject = class("Subject", super)
local Subject = Subject

local TablePool = TablePool
local clear_tbl = clear_tbl

--Subject.e_pool = Instance(ClassPools):createPool(SubjectEvent)   --事件对象池, 不能挂在这里，会被复制

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

function Subject:__ctor()

    self.m_obsDic = {}
    self.m_obsNum = 0        --监听者的数量

end

function Subject:hasType(type)
    return self.m_obsDic[type] ~= nil
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--优化点：
--1. obsArr使用对象池
--2. obs使用对象池
--3. 支持单一插槽
--4. 支持refer

--注意
--1. notify中detachByType

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

--添加监听
--@refer 为false时，显式指定不用refer
--@priority TODO
function Subject:attach(name, listener, target, refer)

    if "function" ~= type(listener) then
        assert(false)
    end

	if refer ~= false then
		refer = refer or target
	end

	local obs

    local isExist = false --查找已有
    local obsArr = self.m_obsDic[name]
    if obsArr then
    
        for k,v in pairs(obsArr) do
            if v.fun == listener and target == v.target then
                isExist = true
                break
            end
        end

        if not isExist then 

	        obs = createObs(name, listener, target, self, refer)
            
--            local l = obsArr[#obsArr]
--            if l then
--                if obs.priority >= l.priority then
--                    obsArr[#obsArr+1] = obs
--                else
--                    table.insert(obsArr, 1, obs)
--                end
--            else
                obsArr[#obsArr+1] = obs
--            end
            
            self.m_obsNum = self.m_obsNum + 1
        end
    else

	    obs = createObs(name, listener, target, self, refer)
	    local obsArr = TablePool.pop()
	    obsArr[#obsArr+1] = obs
        self.m_obsDic[name] = obsArr
        self.m_obsNum = self.m_obsNum + 1
    end

	--return obs
end

--移除监听
function Subject:detach(name, listener, target)

    if "function" ~= type(listener) then
        assert(false)
    end

    local obsArr = self.m_obsDic[name]
    if obsArr == nil  then 
        return end

    for k,obs in ipairs(obsArr) do
        if obs.fun == listener and obs.target == target then
            obsArr[k] = nil
	        clearObs(obs)
            self.m_obsNum = self.m_obsNum - 1
            break
        end
    end    
end


--根据类型移除监听
function Subject:detachByType(type)

    local obsArr = self.m_obsDic[type]
    if obsArr == nil then 
        return end
        
    for k,obs in ipairs(obsArr) do
	    clearObs(obs)
        self.m_obsNum = self.m_obsNum - 1
    end
    self.m_obsDic[type] = nil
    TablePool.push(obsArr)
end


--根据目标移除监听
function Subject:detachByType2(type, target)

    local obsArr = self.m_obsDic[type]
    if obsArr == nil then 
        return end
        
    for k, obs in ipairs(obsArr) do
        if obs.target == target then
            obsArr[k] = nil
	        clearObs(obs)
            self.m_obsNum = self.m_obsNum - 1
        end
    end
end

--根据目标移除监听
function Subject:detachByTarget(target)

    if target == nil then
        return end

    for t, obsArr in pairs(self.m_obsDic) do
        for k, obs in ipairs(obsArr) do
            if obs.target == target then
                obsArr[k] = nil
                clearObs(obs)
                self.m_obsNum = self.m_obsNum - 1
            end
        end
    end
end


function Subject:detachByObs(obs_)

	local name = obs_.name
	local obsArr = self.m_obsDic[obs_.name]
	if not obsArr then
		clearObs(obs_)
		assert(false, '错误的观察者：' .. name)
		return
	end

	--nslog.print_t('detachByObs', obsArr, obs_)

	for k, obs in ipairs(obsArr) do
		if obs == obs_ then

			nslog.print_t(string.format('移除观察者 %s, 事件类型：%s, 剩余数量：%d',
				Refer.format(obs.m_refer),
				name,
				self.m_obsNum-1 ))

			obsArr[k] = nil
			clearObs(obs)
			self.m_obsNum = self.m_obsNum - 1
		end
	end

	--nslog.print(self:dump())
end


--移除所有监听
--@clear 清空观察者
function Subject:detachAll(clear)

    if self.m_obsNum ~= 0 then
        self.m_obsNum = 0
        
        for k,obsArr in pairs(self.m_obsDic) do
            for i, obs in ipairs(obsArr) do
	            clearObs(obs)
            end
            TablePool.push(obsArr)
        end
        clear_tbl(self.m_obsDic)
    end

    --    if clear ~= false then
    --        
    --        self:clearNfyTimeOut() --清空延时派发 
    --    end
end


function Subject:dump()

	local str = string.format('%s, obsNum:%d\n', self.referId, self.m_obsNum )
	for k,obsArr in pairs(self.m_obsDic) do
		str = str ..'type: '.. k .. '\n'
		for i, obs in ipairs(obsArr) do
			str = str ..'\t' .. obs:dump() .. '\n'
		end
	end
	return str
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽派发相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

local tmp_k = nil

--派发
function Subject:notify(name, ...)

    local obsArr = self.m_obsDic[name]
    if obsArr == nil then 
        return end

    tmp_k = nil
    for k, obs in ipairs(obsArr) do
        if obs.target then
	        obs.fun(obs.target, ...)
        else
	        obs.fun(...)
        end
        tmp_k = k
    end

    if tmp_k == nil then
        --已经没有监听
        self.m_obsDic[name] = nil
        TablePool.push(obsArr)
    end
    tmp_k = nil
end


--派发事件
function Subject:notifyEvent(e)

    assert(e and e.type)

    local obsArr = self.m_obsDic[e.type]
    if obsArr == nil then 
        return end

    e.target = e.target or self

    tmp_k = nil
    for k, obs in ipairs(obsArr) do
        if obs.target then
            obs.fun(obs.target, e)
        else
            obs.fun(e)
        end
        tmp_k = k

        if e.stop then
            --不再往下传递
            break
        end
    end

    if tmp_k == nil then
        --已经没有监听
        self.m_obsDic[e.type] = nil
	    TablePool.push(obsArr)
    end
    tmp_k = nil
end

--用事件派发
function Subject:notifyWithEvent(type, data)

    if self.m_obsDic[type] == nil then 
        return end

    local e = evtPool:pop() --事件可回收
    e.type = type
    e.data = data
    
    self:notifyEvent(e)

    e:clear()
    evtPool:push(e)
end

function Subject:notifyEach(type, fun)

    local obsArr = self.m_obsDic[type]
    if obsArr == nil then
        return end

    tmp_k = nil
    for k, obs in ipairs(obsArr) do
        if obs.target  then
            fun(obs.target, obs.fun)
        else
            fun(nil, obs.fun )
        end
        tmp_k = k
    end

    if tmp_k == nil then
        --已经没有监听
        self.m_obsDic[type] = nil
	    TablePool.push(obsArr)
    end
    tmp_k = nil
end


return Subject