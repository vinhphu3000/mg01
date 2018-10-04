--动作_基类
--@author jr.zeng
--2017年7月14日 上午11:04:53
local modname = "ActionBase"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local ActionBase = _ENV[modname]

--===================module content========================

function ActionBase:__ctor()
    
    self.m_target = false --缓动目标
    
    self.m_inited = false
    self.m_isDone = false
    
end

function ActionBase:isDone()
	return self.m_isDone
end

function ActionBase:running()
	return self.m_target ~= false
end

function ActionBase:getTarget()
	return self.m_target
end

--virtual
function ActionBase:getDuration()
	return 0
end



function ActionBase:ensureInited()

	if not self.m_inited then
		nslog.error("缓动还没初始化")
		return false
	end

	return true
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

function ActionBase:update(dt)

end


--* Called once per frame. time a value between 0 and 1.
--* For example:
--* - 0 Means that the action just started.
--* - 0.5 Means that the action is in the middle.
--* - 1 Means that the action is over.
--*
--* @param A value between 0 and 1.
function ActionBase:progress(value)

end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


--开始
function ActionBase:startWithTarget(target)

	self:ensureInited()

	self:reset()

	self.m_target = target

	self:__start()
end

function ActionBase:__start()

end


--完成
function ActionBase:done()
    
    if self.m_isDone then
        return end
    self.m_isDone = true
    
    self:__done()
end

--virtual
function ActionBase:__done()

end


--重置
function ActionBase:reset()

    if not self.m_target then
        return end

    self:__reset()

    self.m_target = false
    self.m_isDone = false
end

--virtual
function ActionBase:__reset()

end

--清除
function ActionBase:clear()
    
    if not self.m_inited then
        return end
    self.m_inited = false
    
    self:__clear()
    
    self.m_target = false
    --self.m_isDone = false --清除时保留原状态
end

function ActionBase:__clear()

end


return ActionBase