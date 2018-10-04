--Ref
--@author jr.zeng
--2017年10月11日 上午9:59:26
local modname = "Ref"
--==================global reference=======================

local setmetatable = setmetatable

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
Ref = class(modname, super)
local Ref = Ref

local Refer = Refer
local clear_tbl = clear_tbl

--===================module content========================

function Ref:__ctor()

    self.m_disposed = false

	self.m_refHash = {}
    self.m_refCnt = 0
    self.m_autoPool = false
    
end

function Ref:ref_cnt()
	return self.m_refCnt
end

function Ref:retain(refer_)
    
    if self:isDisposed(true) then
        return false
    end

	if refer_ == false then
		--显式声明不用refer
	else
		local referId = Refer.format(refer_)
		if not self.m_refHash[referId] then
			self.m_refHash[referId] = referId
			Refer.attachDispose(referId, self.onReferDispose, self)
		end
	end


    self.m_refCnt = self.m_refCnt + 1
    --if self.m_refCnt ==  1 then
    --
    --end
    
    return true
end

--监听引用者析构
function Ref:onReferDispose(referId_)
	nslog.print_t('监听到引用者析构: ' .. referId_ )
	self:release(referId_)
end


function Ref:release(refer_)
    
    if self:isDisposed(true) then
        return false
    end

	if refer_ == false then
		--显式声明不用refer
	else
		local referId = Refer.format(refer_)
		if not self.m_refHash[referId] then
			return false    --release失败
		end

		self.m_refHash[referId] = nil
		Refer.detachDispose(referId, self.onReferDispose, self)
	end
    
    if self.m_refCnt > 0 then
        
        self.m_refCnt = self.m_refCnt - 1
        if self.m_refCnt <= 0 then
            self:__onRelease()
        end
    else
        nslog.error("错误的引用计数", self.referId)
        return false
    end
    
    return true
end

function Ref:__onRelease()
    
    self:dispose()
end


--清除所有引用者
function Ref:__clearAllRefer()

	if self.m_refCnt == 0 then
			return end
	self.m_refCnt = 0

	for k, v in pairs( self.m_refHash ) do
		Refer.detachDispose(k, self.onReferDispose, self)
	end

	clear_tbl( self.m_refHash )
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽Dispose模式∽-★-∽--------∽-★-∽------∽-★-∽--------//

local mt_disposed = 
    {
        __index = function(self, k)     --这里self == cls
            assert(false, string.format("obj had been disposed: %s index %s", self.referId, k))
        end,
        __newindex = function(self, k, v)     --这里self == cls
            assert(false, string.format("obj had been disposed: %s newindex %s", self.referId, k))
        end,
    }

--销毁
function Ref:dispose()

    if self.m_disposed then
        return false
    end
    self.m_disposed = true

    nslog.debug("对象销毁", self.referId)

	self:__clearAllRefer()
    self:clearAutoRelease()

	self:__dispose0()
    safe_call(self.__dispose, self)

    Refer.notifyDispose(self)
    
    setmetatable(self, mt_disposed)

	return true
end

function Ref:__dispose0()

end

function Ref:__dispose()

end


function Ref:isDisposed(alarm_)

    if alarm_ and self.m_disposed then
        nslog.error("对象已销毁") 
    end
    return self.m_disposed
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽AutoRelease∽-★-∽--------∽-★-∽------∽-★-∽--------//


function Ref:autoRelease(pool_)
    
    if self:isDisposed(true) then
        return end
    
    if self.m_autoPool then
        nslog.error("已添加到autoRelaease") 
        return end
    self.m_autoPool = pool_ or App.autoRelease
    self.m_autoPool:add(self)
end

function Ref:clearAutoRelease()
    
    if not self.m_autoPool then
        return end
    self.m_autoPool:remove(self)    --析构时才从autorelease移除，用于统计
    self.m_autoPool = false
end

return Ref