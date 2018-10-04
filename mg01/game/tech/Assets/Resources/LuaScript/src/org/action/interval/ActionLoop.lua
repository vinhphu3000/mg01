--ActionLoop
--@author jr.zeng
--2017年7月17日 下午2:13:54
local modname = "ActionLoop"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local ActionLoop = _ENV[modname]

--===================module content========================

function ActionLoop:__ctor()
    
    self.m_action = nil
    
    self.m_repeatCnt = 1
    self.m_cnt = 0
    
    self.m_dtAmount = 0 --完成一次占总时间的百分比
    self.m_nextDt = 0
    
end

function ActionLoop:initWith(action, repeat_cnt)

	self.m_action = action

	repeat_cnt = math.max(1, repeat_cnt or 1)
	self.m_repeatCnt = repeat_cnt

	local duration = action:getDuration() * self.m_repeatCnt
	self:initWithDuration(duration)

	self.m_dtAmount = action:getDuration() / duration   --完成一次占总时间的百分比
	self.m_nextDt = self.m_dtAmount
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--override
function ActionLoop:__progress(value)


	if value >= self.m_nextDt then

		while value > self.m_nextDt and self.m_cnt < self.m_repeatCnt do

			self.m_action:progress(1)
			self.m_cnt = self.m_cnt + 1

			self.m_action:reset()
			self.m_action:startWithTarget(self.m_target)

			self.m_nextDt = self.m_dtAmount * (self.m_cnt + 1)
		end

		--fix for issue #1288, incorrect end value of repeat
		if value >= 1 and self.m_cnt < self.m_repeatCnt then
			self.m_cnt = self.m_cnt + 1
		end

		if self.m_cnt == self.m_repeatCnt then

			self.m_action:progress(1)
			self.m_action:reset()
		else

			self.m_action:progress(value - (self.m_nextDt - self.m_dtAmount) )
		end
	else

		self.m_action:progress( (value * self.m_repeatCnt) % 1 )
	end
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--override
function ActionLoop:__start()
    
    super.__start(self)
    
    self.m_action:startWithTarget(self.m_target)
    self.m_nextDt = self.m_dtAmount
end

--override
function ActionLoop:__reset()

    super.__reset(self)

    self.m_action:reset()
    self.m_cnt = 0
end

--override
function ActionLoop:__clear()
    
    super.__clear(self)

    clear_action(self.m_action)
    self.m_action = nil
    
end

return ActionLoop