--GameObjUtil
--@author jr.zeng
--2017年10月11日 下午2:47:38
local modname = "GameObjUtil"
--==================global reference=======================

local Object = Unity.Object
local csGameObjUtil = mg.org.GameObjUtil

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
GameObjUtil = {}
local GameObjUtil = GameObjUtil

--===================module content========================


function GameObjUtil.create_gameobject(name)

	return csGameObjUtil.CreateGameobj(name)
end

--转场不销毁
function GameObjUtil.dontDestroyOnLoad(go_)
    
    Object.DontDestroyOnLoad(go_)
end


--根据路径获取子对象
--@isRecursive 是否递归查找
function GameObjUtil.findChild(go_, path_, isRecursive)

    return csGameObjUtil.FindChild(go_, path_, isRecursive)
end

--模糊搜索子对象
function GameObjUtil.fuzzySearchChild(go_, name_)

    return csGameObjUtil.FuzzySearchChild(go_, name_)
end

--改变父级(用addChild)
function GameObjUtil.change_parent(child_, toParent_, worldPosStays_)

	csGameObjUtil.ChangeParent(child_, toParent_, worldPosStays_ or false)
end

function GameObjUtil.remove_from_parent(child_)
	local trans = child_.transform
	if trans ~= App.trashTrans then
		csGameObjUtil.ChangeParent(child_, App.trash, false)
	end
end

return GameObjUtil