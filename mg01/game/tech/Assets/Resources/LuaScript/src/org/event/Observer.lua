-- Observer
--@author jr.zeng
--@date 2018/7/29  17:51
local modname = "Observer"
--==================global reference=======================

--===================namespace========================
local ns = 'org'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
Observer = class(modname, super, _ENV)
local Observer = Observer

local Refer

--===================module content========================

function Observer:__ctor()

	self.name = false
	self.fun = false
	self.target = false
	self.notifier = false
	self.flag = false   --CBoard用

	self.m_refer = false

end


function Observer:init(name, listener, target, notifier, refer)

	self.name = name
	self.fun = listener
	self.target = target
	self.notifier = notifier

	self:setRefer(refer)
end


function Observer:setRefer(refer)

	if self.m_refer == refer then
		return end

	if not Refer then
		Refer = _ENV.Refer
	end

	if self.m_refer then
		Refer.detachDeactive(self.m_refer, self.onDeactive, self)
	end

	self.m_refer = refer
	if refer then
		--nslog.print_t("监听refer", refer)
		Refer.attachDeactive(refer, self.onDeactive, self)
	end
end

function Observer:onDeactive(referId)
	--nslog.print_t('监听到Deactive', referId)
	self.notifier:detachByObs(self)
end

function Observer:clear()

	self.name = false
	self.fun = false
	self.target = false
	self.notifier = false
	self.flag = false   --CBoard用

	self:setRefer(false)

end


function Observer:dump()
	if self.m_refer then
		return string.format('type: %s, refer: %s, fun: %s', self.name, self.m_refer.referId, tostring(self.fun))
	else
		return string.format('type: %s, fun: %s', self.name, tostring(self.fun))
	end
end



return Observer