--AutoRelease
--@author jr.zeng
--2017年10月11日 上午10:28:12
local modname = "AutoRelease"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local AutoRelease = _ENV[modname]

--===================module content========================

function AutoRelease:__ctor()

    self.m_refDic = {}  --用来保留所有引用, 方便统计
    self.m_autoArr = new(Array) --自动释放列表
        
end

function AutoRelease:add(ref_)
    
    ref_:retain(false)
    self.m_autoArr:add(ref_)
    self.m_refDic[ref_] = ref_
	--nslog.print_t("添加到autoRelaease", ref_.referId)
end

function AutoRelease:remove(ref_)
    
    if not self.m_refDic[ref_] then
        return end
    self.m_refDic[ref_] = nil
    
    if self.m_autoArr:remove(ref_) then
        ref_:release(false)
        nslog.error("怎么可能还在autoArr里?")
    end
    
    nslog.debug("从autoRelease移除")
end

--执行自动释放
function AutoRelease:excute()
    
    if #self.m_autoArr == 0 then
        return end
    
    local ref
    local len = #self.m_autoArr
    for i=len, 1, -1 do
        ref = self.m_autoArr[i]
        self.m_autoArr:remove_at(i)  --先移除再release,不然在remove里又会release一次

	    --if ref:ref_cnt() <= 1 then
		 --   nslog.print_t('被AutoRelease了', ref.referId)
	    --end
	    --nslog.print_t("自动释放", ref.referId)

        ref:release(false)
    end
    self.m_autoArr:clear()
end

function AutoRelease:clear()
    
    self:excute()
    self.m_refDic = {}
end


function AutoRelease:dump()
    --TODO
end

return AutoRelease