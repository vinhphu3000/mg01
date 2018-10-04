--ActionSpawn
--@author jr.zeng
--2017年7月17日 下午4:11:41
local modname = "ActionSpawn"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local ActionSpawn = _ENV[modname]

--===================module content========================

function ActionSpawn:__ctor()

    self.m_action1 = nil
    self.m_action2 = nil
    
    
end

function ActionSpawn:initWithActions(...)

	local actions = {...}
	local size = #actions
	if size == 0 then
		self:initWithDuration(0)
		return
	end

	if size == 1 then
		self:initWithTwoActions(actions[1], delay(0))
		return
	end

	local prev = actions[1]

	for i=2, size-1 do
		prev = spawn2(prev, actions[i])
	end

	self:initWithTwoActions(prev, actions[#actions])
end

function ActionSpawn:initWithTwoActions(a1, a2)

	a2 = a2 or delay(0)
	local d1 = a1:getDuration()
	local d2 = a2:getDuration()
	local max = math.max(d1, d2)

	self:initWithDuration(max)

	if d1 > d2 then

		self.m_action1 = a1
		self.m_action2 = seq2(a2, delay(d1-d2))

	elseif d1 < d2 then

		self.m_action1 = seq2(a1, delay(d2-d1))
		self.m_action2 = a2
	else

		self.m_action1 = a1
		self.m_action2 = a2
	end
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--override
function ActionSpawn:__progress(value)

	self.m_action1:progress(value)
	self.m_action2:progress(value)

end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--override
function ActionSpawn:__start()

    super.__start(self)

    self.m_action1:startWithTarget(self.m_target)
    self.m_action2:startWithTarget(self.m_target)
end


--override
function ActionSpawn:__reset()
    
    super.__reset(self)
    
    self.m_action1:reset()
    self.m_action2:reset()
end

--override
function ActionSpawn:__clear()

    super.__clear(self)

    clear_action(self.m_action1)
    self.m_action1 = nil

    clear_action(self.m_action2)
    self.m_action2 = nil

end

return ActionSpawn