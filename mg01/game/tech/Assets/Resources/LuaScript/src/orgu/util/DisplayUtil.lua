--DisplayUtil
--@author jr.zeng
--2017年10月13日 下午4:28:18
local modname = "DisplayUtil"
--==================global reference=======================

local csDisplayUtil = mg.org.DisplayUtil

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================

DisplayUtil = {}
local DisplayUtil = DisplayUtil

--===================module content========================

--添加到父节点
function DisplayUtil.addChild(parent_, child_)
    
    return csDisplayUtil.AddChild(parent_, child_)
end

--从父节点移除
function DisplayUtil.removeFromParent(child_)

    csDisplayUtil.RemoveFromParent( child_)
end

--是否在垃圾桶
function DisplayUtil.isInTrash(child_)
    
    return csDisplayUtil.IsInTrash( child_)
end

return DisplayUtil