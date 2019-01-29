--MainEntry
--@author jr.zeng
--2017年9月20日 上午10:49:16
local modname = "MainEntry"
--==================global reference=======================

require "src.game.main.lib"

local CCApp = mg.org.CCApp
local CCDefine = mg.org.CCDefine

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local MainEntry = _ENV[modname]
local self = MainEntry

local App = App
local KeyCode = KeyCode
local EvtCenter = EvtCenter

--===================module content========================

function MainEntry:__ctor()
    
    self.m_gameEntered = false  --是否已进入游戏
    
end

function MainEntry:setup()
    
    self:ctor()
    
    App:setup()
    
    self:setup_event()
    
    self:enter_game()
end

function MainEntry:clear()
    
    self:clear_event()
    
    self:quit_game()
    
    App:clear()
end

function MainEntry:setup_event()
    
    if CCDefine.USE_KEYBOARD then
        local csKeyboard = CCApp.keyboard
        csKeyboard:Attach(KEY_EVENT.PRESS, self.on_key_press)     --TODO: 这个也要改在GetEvents
        csKeyboard:Attach(KEY_EVENT.RELEASE, self.on_key_release)
    end
end

function MainEntry:clear_event()
    
    if CCDefine.USE_KEYBOARD then
        local Keyboard = CCApp.keyboard
        Keyboard:Detach(KEY_EVENT.PRESS, self.on_key_press)
        Keyboard:Detach(KEY_EVENT.RELEASE, self.on_key_release)
    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--c#报错处理
function MainEntry.onCsError(err_str)


end

--c#通知处理
function MainEntry.onCsNotify(evtType, v1, v2, v3)
    
    print("luaSvr_notify", evtType, v1, v2)

    --safe_call(mt.test_error)

    if evtType == 2 then

        --local result = t2str(_G, 3, false)   --默认扫描3层
        --log.print(result)
        --local result = t2str(table, 3, true)   --默认扫描3层
        --log.print(result)

        nslog.print_r(UnityEngine)

    elseif evtType == 3 then

        nslog.warn(modname, "法规的规范梵蒂冈")
    end
end


function MainEntry.update(delta)
    
    --nslog.debug(delta)
    App:update(delta)
end

function MainEntry.lateUpdate(delta)
    
    App:lateUpdate(delta)
end


function MainEntry.on_key_press(keycode)
    
    --nslog.debug("on_key_press", keycode)
    safe_call(App.notify, App, KEY_EVENT.PRESS, keycode)
end


function MainEntry.on_key_release(keycode)

    nslog.debug("on_key_release", keycode)
    safe_call(App.notify, App, KEY_EVENT.RELEASE, keycode)
    
    if keycode == KeyCode.KeypadMinus then
        
    elseif keycode == KeyCode.F10 then

        nslog.debug("mg", t2str(UnityEngine, 99))
        --nslog.debug("mg", t2str(mg, 99))
        
    elseif keycode == KeyCode.F11 then
        
        kui.UIHandler.dump()
        
    elseif keycode == KeyCode.F12 then

	    --dump_namespace()
        reload_lua()

    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽资源加载∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽登陆流程∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽进入流程∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽模块初始化∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽进入游戏∽-★-∽--------∽-★-∽------∽-★-∽--------//


--退出游戏
function MainEntry:quit_game()

    if not self.m_gameEntered then 
        return end
    self.m_gameEntered = false

    nslog.debug("quit_game")

end

--进入游戏
function MainEntry:enter_game() 

    if self.m_gameEntered then 
        return end
    self.m_gameEntered = true

	nslog.print_t("enter_game")

    require("src.game.lib")

	LevelMgr:load_async('SceneStart', false, self.on_level_loaded, self)

	local name = '红叶秘境'
	local aa = string.sub(name, 2)
	nslog.print_r(aa)
end

function MainEntry:on_level_loaded(url_)

	--nslog.print_t("on_level_loaded", url_)

	--test.UnityTest:setup()
	--test.UnityTestPop:setup()

	require "src.dvc.lib"
	dvc.dvc_mgr:setup()
end


return MainEntry