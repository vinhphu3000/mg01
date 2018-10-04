--ComponentUtil
--@author jr.zeng
--2017年10月19日 下午5:21:58
local modname = "ComponentUtil"
--==================global reference=======================

local csComponentUtil = mg.org.ComponentUtil

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
ComponentUtil = {}
local ComponentUtil = ComponentUtil

--===================module content========================

--确保有该控件
function ComponentUtil.ensure_component(go_, type_)
    return csComponentUtil.EnsureComponent(go_, type_)
end



return ComponentUtil