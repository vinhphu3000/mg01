--game_init
--@author jr.zeng
--2017年9月21日 上午10:53:37
local modname = "game_init"
--==================global reference=======================

require("src.env_init")

local safe_call = safe_call
local tostring = tostring
local loadstring = loadstring

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

local game_init = {}
local self = game_init

local funUpdate
local funLateUpdate 
local funNotify
local funNotifyGoEvt
local funCsError

--===================module content========================


function game_init:ctor()

    self.m_entry = require("src.game.main.MainEntry")
    

end

function game_init:setup()
    
    self:ctor()

	funUpdate = self.m_entry.update
	funLateUpdate = self.m_entry.lateUpdate
	funNotify = self.m_entry.onCsNotify
	funCsError = self.m_entry.onCsError

    self.m_entry:setup()
end

function game_init:clear()
    
    self.m_entry:clear()
end

--//-------~★~-------~★~-------~★~luaSvr~★~-------~★~-------~★~-------//


--通知启动
function game_init.luaSvr_launch()
	print('luaSvr_launch')
	safe_call(self.setup, self)
end

function game_init.luaSvr_update(dt)

    safe_call(funUpdate, dt)
end

function game_init.luaSvr_lateUpdate(dt)

	--print('luaSvr_lateUpdate', dt)
    safe_call(funLateUpdate, dt)
end

--通知C#事件
function game_init.luaSvr_notify(evtType, v1, v2, v3)

    safe_call(funNotify, evtType, v1, v2, v3)
end


--通知C#报错
function game_init.luaSvr_notifyCsError(err_str)

    safe_call(funCsError, err_str)
end

--虚拟机关闭
function game_init.luaSvr_dispose()

    self:clear()
end

--控制台命令
function game_init.luaSvr_consoleCmd(input_str)

    --local _ENV = namespace(nil, "org")

    local str

    local script = "local _ENV = namespace(nil, {\"org\"} )\n return " ..  input_str
    local func = loadstring(script)
    if func then
        local b, result = safe_call(func)
        if result ~= nil then
            --有返回值的话,打印到控制台
            str = tostring(result)
        end
    else
        str = "do script failed: "..input_str
    end

    if str then
        if console_print then
            console_print(str)
        end
    end
end

return game_init