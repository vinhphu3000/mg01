--PopConst
--@author jr.zeng
--2017年10月14日 下午4:50:36
local modname = "PopConst"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================


--窗口层级id
POP_LAYER_ID =
    {
        LAYER_NONE = 0,
        --场景
        LAYER_SCENE = 1,
        --主UI
        LAYER_MAIN_UI = 2,
        --
        LAYER_POP_1 = 3,
        --
        LAYER_POP_2 = 4,
        --
        LAYER_POP_3 = 5,
        --加载条
        LAYER_LOADING = 40,
        --tips
        LAYER_TIPS = 50,
        --顶层
        LAYER_TOP = 99,

    }

--窗口生命周期
POP_LIFE =
    {
        --存栈,适当时会释放
        STACK       = 0,
        --永远存在
        FOREVER     = 1,
        --用完立刻释放
        WEAK        = 2,
    }
    


--窗口事件
POP_EVT = 
    {
        --窗口打开
        POP_OPEN = "POP_OPEN",
        --窗口关闭
        POP_CLOSE = "POP_CLOSE",
        --所有面板窗口
        POP_CLOSE_ALL = "POP_CLOSE_ALL",
        --窗口淡入开始
        POP_IN_BEGIN = "POP_IN_BEGIN",
        --窗口淡入完成
        POP_IN_FINISH = "POP_IN_FINISH",
        --窗口淡出开始
        POP_OUT_BEGIN = "POP_OUT_BEGIN",
        --窗口淡出完成
        POP_OUT_FINISH = "POP_OUT_FINISH",
    }


--窗口信息注册例子   
--local POP_ID = 
--{
--    TEST_1 = {file = "TestPop1", prefab_url = "GUI/cn/Prefab/Canvas_Test2", res_urls = {} }
--}


