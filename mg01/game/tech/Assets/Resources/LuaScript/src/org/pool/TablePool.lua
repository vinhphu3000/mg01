--TablePool
--@author jr.zeng
--2017年10月21日 上午11:49:00
local modname = "TablePool"
--==================global reference=======================

local assert = assert
local setmetatable = setmetatable


--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
TablePool = {}
local TablePool = TablePool

local clear_tbl = clear_tbl

--===================module content========================

local cnt_max = 1000

local _pool = {}
local _check = {}


function TablePool.pop()
    
    local cnt = #_pool
    if cnt > 0 then
        local tbl = _pool[cnt]
        _check[tbl] = nil
        _pool[cnt] = nil
        return tbl
    end
    return {}
end


function TablePool.push(tbl)
    
    assert(not _check[tbl])

    clear_tbl(tbl)
    
    local cnt = #_pool
    if cnt < cnt_max then
        --不超过容量
        setmetatable(tbl, nil)
        
        _check[tbl] = 1
        _pool[cnt+1] = tbl
        
        nslog.debug("cnt" , cnt+1)
    end
end

return TablePool