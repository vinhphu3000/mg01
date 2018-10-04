--ActionInstant
--@author jr.zeng
--2017年7月17日 下午3:54:19
local modname = "ActionInstant"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionBase	--父类
_ENV[modname] = class(modname, super)
local ActionInstant = _ENV[modname]

--===================module content========================

function ActionInstant:__ctor()
    
end

function ActionInstant:init()
    
    self.m_inited = true
end


--override
function ActionInstant:update(dt)

    self:progress(1)
end


--override
function ActionInstant:progress(value)
    
    if value >= 1 then
        self:done()
    end
end

return ActionInstant