--TimerMgr
--@author jr.zeng
--2017年6月30日 上午11:33:31
local modname = "TimerMgr"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
TimerMgr = class(modname, super)
local TimerMgr = TimerMgr

local next = next
local pairs = pairs
local empty_tbl = {}

--注册到主空间
--@param {repeat_cnt}
function setTimeOut(delay, callback, target, param)
    return TimerMgr:setTimeOut(delay, callback, target, param)
end

function clearTimeOut(index)
    return TimerMgr:clearTimeOut(index)
end

function callDelay(delay, callback, target, param)
    TimerMgr:callDelay(delay, callback, target, param)
end

function removeDelay(fun, target)
    TimerMgr:removeDelay(fun, target)
end

g_ns.setTimeOut = setTimeOut
g_ns.clearTimeOut = clearTimeOut
g_ns.callDelay = callDelay
g_ns.removeDelay = removeDelay

--===================module content========================

function TimerMgr:__ctor()

    self.m_delay_dic = {}
	self.m_delay_num = 0

    self.m_index2obj = {}
	self.m_index2objAdd = {}
	self.m_indexNum = 0
	self.m_timeOutId = 1
	self.m_timeOuting = false
	self.m_timeOutAdded = false

    self.m_cur_frame = 1   --当前帧数
end


function TimerMgr:setup()

    self:ctor()

    self:setup_event()
end

function TimerMgr:clear()

    self:clear_event()

	self.m_delay_dic = {}
	self.m_delay_num = 0

	self.m_index2obj = {}
	self.m_index2objAdd = {}
	self.m_indexNum = 0
end

function TimerMgr:setup_event()

end

function TimerMgr:clear_event()

end

function TimerMgr:getCurFrame()
    return self.m_cur_frame
end

function TimerMgr:update(dt)

    --self.m_cur_frame = self.m_cur_frame + 1
    self:updateTimeOut(dt)
    self:updateCallDelay(dt)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽setTimeOut∽-★-∽--------∽-★-∽------∽-★-∽--------//

--@param {repeat_cnt}
function TimerMgr:setTimeOut(delay, callback, target, param)

    --assert(type(callback) == "function")

    delay = delay or 0.001
    param = param or empty_tbl
    
    local index = self.m_timeOutId
    self.m_timeOutId = self.m_timeOutId + 1

    local obj = {index = index,
        delay = delay,
        time = delay,
        _repeat = param.repeat_cnt or 1,
        fun = callback,
        target = target,
    }

	if self.m_timeOuting then
		self.m_index2objAdd[index] = obj
		self.m_timeOutAdded = true
	else
		self.m_index2obj[index] = obj
	end

    self.m_indexNum = self.m_indexNum + 1

    return index
end

function TimerMgr:clearTimeOut(index)

	local obj = self.m_index2obj[index]
	if obj then
		self.m_index2obj[index] = nil
		self.m_indexNum = self.m_indexNum - 1
	elseif self.m_timeOutAdded then

		obj = self.m_index2objAdd[index]
		if obj then
			self.m_index2objAdd[index] = nil
			self.m_indexNum = self.m_indexNum - 1
		end
	end

	return 0
end

function TimerMgr:updateTimeOut(t)

    if self.m_indexNum <= 0 then
        return end

	self.m_timeOuting = true

    for index, obj in pairs(self.m_index2obj) do

        obj.time = obj.time - t
        if obj.time <= 0 then

            obj.time = obj.delay
            if obj._repeat > 0 then
                obj._repeat = obj._repeat - 1
            end

            if obj._repeat == 0 then
                self:clearTimeOut(index)
            end

            if obj.target then
                obj.fun(obj.target, obj)
            else
                obj.fun(obj)
            end
        end
    end

	self.m_timeOuting = false

	if self.m_timeOutAdded then
		self.m_timeOutAdded = false

		for index, obj in pairs(self.m_index2objAdd) do
			self.m_index2obj[index] = obj
		end
		clear_tbl(self.m_index2objAdd)
	end
end

--获取当前的TimeOutId
function TimerMgr:getTimeOutId()
    return self.m_timeOutId
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽callDelay∽-★-∽--------∽-★-∽------∽-★-∽--------//

--@delay 毫秒
--@param + repeat_cnt 重复次数, -1时无限循环
--       + sole_cb 当前已经有注册回调时,不再注册
function TimerMgr:callDelay(delay, callback, target, param)

    target = target or 0
    param = param or empty_tbl
    
    if self.m_delay_dic[target] == nil then
        self.m_delay_dic[target] = {}
    end

    if self.m_delay_dic[target][callback] == nil then
        self.m_delay_num = self.m_delay_num + 1
    else
        if param.sole_cb then
            return end
    end

    local obj =  {
        fun = callback,
        target = target,
        delay = delay or 0.001,
        time = delay or 0.001,
        _repeat = param.repeat_cnt or 1,
    }

    self.m_delay_dic[target][callback] = obj
end

function TimerMgr:removeDelay(fun, target)

    target = target or 0

    if self.m_delay_dic[target] ~= nil then

        if self.m_delay_dic[target][fun] ~= nil then
            self.m_delay_dic[target][fun] = nil

            if next(self.m_delay_dic[target]) == nil then
                self.m_delay_dic[target] = nil
            end

            self.m_delay_num = self.m_delay_num - 1
        end
    end
end

function TimerMgr:updateCallDelay(t)

    if self.m_delay_num == 0 then
        return end

    for target, dic in pairs(self.m_delay_dic) do
        for fun, obj in pairs(dic) do

            obj.time = obj.time - t
            if obj.time <= 0 then

                obj.time = obj.delay
                if obj._repeat > 0 then
                    obj._repeat = obj._repeat - 1
                end

                if obj._repeat == 0 then
                    self:removeDelay(obj.fun, obj.target)
                end

                if obj.target == 0 then
                    obj.fun()
                else
                    obj.fun(target)
                end
            end
        end
    end 

end

return TimerMgr