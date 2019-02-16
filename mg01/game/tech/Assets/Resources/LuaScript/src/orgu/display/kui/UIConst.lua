--UIConst
--@author jr.zeng
--2017年10月12日 下午4:41:03
local modname = "UIConst"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================


--镜头层
CAMERA_LAYER = 
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,

    }
    

--滚动方向
ScrollDir =
    {
        Horizontal = 0,
        Vertical = 1,
    }

--跳转位置类型
JumpPosType =
    {
        TOP = 1,
        CENTER = 2,
        BOTTOM = 3,
    }


--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//


--ui事件
UI_EVT =
{
	--c#
	POINTER_DOWN = "POINTER_DOWN",
	POINTER_UP = "POINTER_UP",
	POINTER_ENTER = "POINTER_ENTER",
	POINTER_EXIT = "POINTER_EXIT",
	POINTER_MOVE = "POINTER_MOVE",
	POINTER_CLICK = "POINTER_CLICK",
	--数值改变
	VALUE_CHANGE = "VALUE_CHANGE",
	--列表项数据改变
	LIST_ITEM_DATA = "LIST_ITEM_DATA",
	--编辑结束
	EDIT_END = "EDIT_END",
	--请求改变
	REQ_VALUE_CHANGE = "REQ_VALUE_CHANGE",

	--lua

}
