--HelpGo
--@author jr.zeng
--2017年10月19日 下午4:58:51
local modname = "HelpGo"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
HelpGo = class(modname, super)
local HelpGo = HelpGo

local GameObjUtil = GameObjUtil
local ComponentUtil = ComponentUtil


--===================module content========================

function HelpGo.get_gameObject(go_)
    return go_
end



--Active
function HelpGo.set_active(go, b)
    go:SetActive(b and true or false)
end

function HelpGo.getActive(go)
    return go.activeSelf
end

--//-------~★~-------~★~-------~★~Component~★~-------~★~-------~★~-------//

--获取控件
function HelpGo.get_component(go, type_)
    return go:GetComponent(type_)
end

--必须有此控件
--@type_ CTypeName 或者 CType
function HelpGo.need_component(go, type_)
    local c = go:GetComponent(type_)
    if not c then
        nslog.error("Must has component", type_)
    end
    return c
end

--确保有该控件
function HelpGo.ensure_component(go_, type_)
    return ComponentUtil.ensure_component(go_, type_)
end


--//-------~★~-------~★~-------~★~Transform~★~-------~★~-------~★~-------//

--
function HelpGo.get_transform(go_)
    return go_.transform
end

--改变父级
function HelpGo.set_parent(go_, toParent_, worldPosStays_)
	return GameObjUtil.changeParent(go_, toParent_, worldPosStays_)
end

--根据路径获取子对象
--@isRecursive 是否递归查找
function HelpGo.findChlid(go_, path_, isRecursive)
	return GameObjUtil.findChild(go_, path_, isRecursive)
end

--模糊搜索子对象
function HelpGo.fuzzySearchChild(go_, name_)
	return GameObjUtil.FuzzySearchChild(go_, name_)
end


--本地坐标
function HelpGo.set_local_pos(go_, x, y, z)
	go_:SetLocalPos_(x, y, z)
end

--return x,y,z
function HelpGo.get_local_pos(go_)
	return go_:GetLocalPos_()
end


--本地Scale
function HelpGo.set_local_scale(go_, x, y, z)
	go_:SetLocalScale_(x, y, z)
end

--return x,y,z
function HelpGo.get_local_scale_(go_)
	return go_:GetLocalScale_()
end




return HelpGo