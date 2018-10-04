--SubjectEvent
--@author jr.zeng
--2017年4月14日 下午4:26:53
local modname = "SubjectEvent"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
SubjectEvent = class("SubjectEvent", super)
local SubjectEvent = SubjectEvent
local M = SubjectEvent

--===================module content========================

function SubjectEvent:__ctor(type, data)

    self.type = type or "unknow"
    self.data = data
    self.target = nil

    self.stop = false   --用于判断是否需要继续传递
end

function SubjectEvent:stopPropagation()

    self.stop = true
end

--清空数据
function SubjectEvent:clear()

    self.data = nil
    self.target = nil

    self.stop = false
end

return SubjectEvent