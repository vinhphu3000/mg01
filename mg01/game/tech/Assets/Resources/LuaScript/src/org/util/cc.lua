--cc
--@author jr.zeng
--2017年9月25日 下午5:53:54
local modname = "cc"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
cc = {}
local cc = cc

--===================module content========================

function cc.p(x, y)
    return {x=x or 0, y=y or 0}
end

function cc.p3(x, y, z)
    return {x=x or 0, y=y or 0, z=z or 0}
end

--尺寸
function cc.size(w, h)
    return {w=w or 0, h=h or 0}
end

--边距
function cc.padding(left, top, right, bottom)
    return {left=left or 0, top=top or 0, right = right or 0, bottom = bottom or 0}
end


function cc.rect(x, y, w, h)
	return {x = x or 0, y = y or 0, w = w or 0, h = h or 0}
end


return cc