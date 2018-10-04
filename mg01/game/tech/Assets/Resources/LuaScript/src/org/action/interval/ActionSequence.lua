--ActionSequence
--@author jr.zeng
--2017年7月14日 下午8:10:03
local modname = "ActionSequence"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local ActionSequence = _ENV[modname]

--===================module content========================

function ActionSequence:__ctor()
    
    self.m_action1 = nil
    self.m_action2 = nil
    
    self.m_curAction = nil
    
    self.m_split = 0
end


function ActionSequence:initWithActions(...)

	local actions = {...}

	local prev = actions[1]
	local len = #actions - 1
	if len > 0 then

		for i = 2, len do
			prev = seq2(prev, actions[i])
		end

		self:initWithTwoActions(prev, actions[#actions])

	elseif len == 0 then

		self:initWithTwoActions(prev, nil)
	else
		nslog.error(modname, "至少传入一个action")
	end

end


function ActionSequence:initWithTwoActions(a1, a2)

	self.m_action1 = a1

	if a2 then
		self.m_action2 = a2
	else
		self.m_action2 = delay(0)
	end

	local duration = self.m_action1:getDuration() + self.m_action2:getDuration()
	self:initWithDuration(duration)

	if duration > 0 then
		self.m_split = self.m_action1:getDuration() / duration
	else
		self.m_split = 0
	end
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--override
function ActionSequence:__progress(value)

	local next
	local new_dt

	if value < self.m_split then

		next = self.m_action1
		new_dt = value / self.m_split

	else

		next = self.m_action2

		if self.m_split >= 1 then
			new_dt = 1
		else
			new_dt = (value - self.m_split) / (1 - self.m_split)
		end
	end

	if next == self.m_action2 then

		if not self.m_curAction then
			--还没开始,就跳2了,直接完成1
			if not self.m_action1:running() then
				self.m_action1:startWithTarget(self.m_target)
			end
			self.m_action1:progress(1)

		elseif self.m_curAction == self.m_action1 then

			self.m_action1:progress(1)
		end
	else
		--next == action1
		if self.m_curAction == self.m_action2 then
			self.m_action2:progress(0)
		end
	end

	if self.m_curAction == next then

		if self.m_curAction:isDone() then
			return end
	else

		self.m_curAction = next
		if not self.m_curAction:running() then
			self.m_curAction:startWithTarget(self.m_target)
		end
	end

	self.m_curAction:progress(new_dt)
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--override
function ActionSequence:__reset()
    
    super.__reset(self)
    
    self.m_action1:reset()
    self.m_action2:reset()
    
    self.m_curAction = nil
    
end


--override
function ActionSequence:__clear()

    super.__clear(self)

    self.m_curAction = nil
    
    clear_action(self.m_action1)
    self.m_action1 = nil
    
    clear_action(self.m_action2)
    self.m_action2 = nil
end


return ActionSequence