--PopConst
--@author jr.zeng
--2017年10月13日 下午5:58:07
local modname = "pop_const"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================

local POP_ID =
    {
	    --TEST_1 = {script_url = "脚本路径", prefab_url = "Prefab路径", res_urls = {预加载资源路径} },
        TEST_1 = {script_url = "src.game.test.pop.TestPop1", prefab_url = "GUI/cn/Prefab/Canvas_Test2", res_urls = {} },
        TEST_3 = {script_url = "src.game.test.pop.TestPop3", prefab_url = "GUI/cn/Prefab/Canvas_Test3", res_urls = {} },
	    TEST_4 = {script_url = "src.game.test.pop.TestPop4", prefab_url = "GUI/cn/Prefab/Canvas_TestPop4", res_urls = {} },

	    MSG_TIPS = {script_url = "src.game.ui.tips.msg_tips", prefab_url = "GUI/cn/Prefab/Canvas_NotifyTips", res_urls = {} },

    }

PopMgr.addPopConfig(POP_ID)