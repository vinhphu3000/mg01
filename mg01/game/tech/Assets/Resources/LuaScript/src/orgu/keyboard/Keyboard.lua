--Keyboard
--@author jr.zeng
--2017年10月12日 下午2:19:43
local modname = "Keyboard"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = Subject	--父类
_ENV[modname] = class(modname, super)
local Keyboard = _ENV[modname]

local KEY_EVENT = KEY_EVENT

--===================module content========================


function Keyboard:__ctor()

    self.m_code2down = {}
    self.m_enabled = true
end


function Keyboard:setup()

    self:set_enabled(self.m_enabled, true)

end

function Keyboard:clear()

    self:clear_event()

    self.m_code2down = {}
end


function Keyboard:setup_event()
    
    local App = App
    App:attach(KEY_EVENT.PRESS, self.on_key_press, self)
    App:attach(KEY_EVENT.RELEASE, self.on_key_release, self)
    
end

function Keyboard:clear_event()

    local App = App
    App:detach(KEY_EVENT.PRESS, self.on_key_press, self)
    App:detach(KEY_EVENT.RELEASE, self.on_key_release, self)

end


--按键是否按下
function Keyboard:is_pressed(code)
    return self.m_code2down[code] == true
end

function Keyboard:set_enabled(b, force)

    if self.m_enabled == b and not force then
        return end
    self.m_enabled = b
    if b then
        self:setup_event()
    else
        self:clear_event()
    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function Keyboard:on_key_press(keycode)
    
    self:setKeyPressed(keycode, true)
    --nslog.debug("on_key_press", keycode)
end


function Keyboard:on_key_release(keycode)

    self:setKeyPressed(keycode, false)
    --nslog.debug("on_key_release", keycode)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//



--设置键盘按下
function Keyboard:setKeyPressed(code, pressed)

    if pressed == true then
        --按下
        if not self.m_code2down[code] then
            self.m_code2down[code] = true
            self:notifyWithEvent(KEY_EVENT.PRESS, code)
            --nslog.debug(modname, "press", KeyCode2Name[code])
        end    
    else
        --弹起
        if self.m_code2down[code] then
            self.m_code2down[code] = false
            self:notifyWithEvent(KEY_EVENT.RELEASE, code)
            --nslog.debug(modname, "release", KeyCode2Name[code])
        end    
    end
end


--弹起所有按键
function Keyboard:releaseAllKeys()

    for k,v in pairs(self.m_code2down) do
        if v then   --按下了
            self.m_code2down[k] = false
            self:notifyWithEvent(KEY_EVENT.RELEASE, k)
        end
    end
end

return Keyboard