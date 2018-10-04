--ActionInterval(抽象)
--@author jr.zeng
--2017年7月14日 下午2:28:14
local modname = "ActionInterval"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionBase	--父类
_ENV[modname] = class(modname, super)
local ActionInterval = _ENV[modname]

local clamp = MathUtil.clamp

--===================module content========================

function ActionInterval:__ctor()
    
    self.m_duration = 1 --周期
    self.m_perDelta = 1 --周期系数
    
    self.m_elapsed = 0  --经过时间
    self.m_progress = 0 --当前进度
    
end


function ActionInterval:initWithDuration(duration)
    
    self:setDuration(duration)
    self.m_inited = true
end


--设置动画时间
function ActionInterval:setDuration(duration)

	self.m_duration = duration
	if duration > 0 then
		self.m_perDelta = 1 / self.m_duration
	else
		self.m_duration = 0
		self.m_perDelta = 0
	end
end

--override
function ActionInterval:getDuration()
	return self.m_duration
end


--获取经过时间
function ActionInterval:getElapsed()
	return self.m_elapsed
end



--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


function ActionInterval:update(dt)
    
    self.m_elapsed = self.m_elapsed + dt
    
    local progress = 1
    if self.m_duration > 0 then
        progress = self.m_elapsed * self.m_perDelta
    end
    
    self:progress(progress)
end

--override
function ActionInterval:progress(value)

    --更新进度
    value = clamp(value)
    self.m_progress = value
    
    self:__progress(value)
    
    if value >= 1 then
        self:done()
    end
end

--virtual
function ActionInterval:__progress(value)

end



--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


--override
function ActionInterval:__reset()

    self.m_elapsed = 0
    self.m_progress = 0
end

--override
function ActionInterval:__clear()

    self.m_elapsed = 0
    self.m_progress = 0
end

return ActionInterval