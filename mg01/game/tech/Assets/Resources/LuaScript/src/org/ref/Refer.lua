--Refer
--@author jr.zeng
--2017年10月11日 下午12:00:47
local modname = "Refer"
--==================global reference=======================

local type = type
local string = string

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil    --父类
Refer = {}
local Refer = Refer

--===================module content========================

--@must_ 必须传refer
function Refer.format(refer_, must_)

	if refer_ == nil then
		if must_ ~= false then
			Refer.assert(refer_)
		end
		return nil
	end

	local str
	if type(refer_) == "string" then
		str = refer_
	elseif refer_.referId then
		str = refer_.referId
	else
		log.error("不支持的Refer", refer_)
	end

	if string.isNullOrEmpty(str) then
		return nil
	end

	return str
end

function Refer.assert(refer_, error)

	if not refer_ or not refer_.referId then
		log.error(error or "此对象不是Refer ", refer_)
		return false
	end
	return true
end

function Refer.dump()

end

-------∽-★-∽------∽-★-∽Notify∽-★-∽------∽-★-∽--------//

local __subDeactive
local __subDispose

function Refer.a()  --TODO
	__subDeactive = new(Subject)
	__subDispose =  new(Subject)
end

function Refer.clearNotify()
	__subDeactive:detachAll()
	__subDispose:detachAll()
end

-- 通知沉默
function Refer.notifyDeactive(refer_)
	local referId = Refer.format(refer_)
	--nslog.print_t('notifyDeactive', referId)
	__subDeactive:notify(referId, referId)
end

function Refer.attachDeactive(refer_, fun, target)
	local referId = Refer.format(refer_)
	--nslog.print_t("attachDeactive", referId)
	__subDeactive:attach(referId, fun, target, false)    --这里不能传入refer，要主动detachDeactive
end

function Refer.detachDeactive(refer_, fun, target)
	local referId = Refer.format(refer_)
	__subDeactive:detach(referId, fun, target)
end

-- 通知析构
function Refer.notifyDispose(refer_)
	local referId = Refer.format(refer_)
	__subDispose:notify(referId, referId)
end

function Refer.attachDispose(refer_, fun, target)
	local referId = Refer.format(refer_)
	__subDispose:attach(referId, fun, target, false)    --这里不能传入refer，要主动detachDispose
end

function Refer.detachDispose(refer_, fun, target)
	local referId = Refer.format(refer_)
	__subDispose:detach(referId, fun, target)
end


return Refer