--SoundMgr
--@author jr.zeng
--2017年11月14日 下午5:21:18
local modname = "SoundMgr"
--==================global reference=======================

local csSoundMgr = mg.org.CCApp.soundMgr

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = CCModule	--父类
SoundMgr = class(modname, super, _ENV)
local SoundMgr = SoundMgr

--===================module content========================

function SoundMgr:__ctor()
    
    self.m_bgmPath = false
    
end


function SoundMgr:__setup()

    self:ctor()

end

function SoundMgr:__clear()
    
    self:stop_bgm()
    self:stop_all_eff()
    
    self.m_bgmPath = false
end

function SoundMgr:setup_event()

end

function SoundMgr:clear_event()

end


--//-------~★~-------~★~-------~★~BGM~★~-------~★~-------~★~-------//

--播放背景音乐
function SoundMgr:play_bgm(path)
    
    csSoundMgr:PlayBgm(path)
end

function SoundMgr:stop_bgm()

    csSoundMgr:StopBgm()
end



--//-------~★~-------~★~-------~★~音效~★~-------~★~-------~★~-------//

--播放一次性音效
function SoundMgr:play_one_shot(path, volume)
    
    volume = volume or 1
    csSoundMgr:PlayOneShot(path, volume)
end

--播放ui音效
function SoundMgr:play_ui(path, volume)

    volume = volume or 1
    csSoundMgr:PlayUi(path, volume)
end

--停止所有音效
function SoundMgr:stop_all_eff()
    
    csSoundMgr:StopAllEff()
end

--//-------~★~-------~★~-------~★~开关~★~-------~★~-------~★~-------//

function SoundMgr:set_total_on(b)
    csSoundMgr.totalOn = b
end

function SoundMgr:get_total_on()
    return csSoundMgr.totalOn
end


function SoundMgr:set_bgm_on(b)
    csSoundMgr.bgmOn = b
end

function SoundMgr:get_bgm_on()
    return csSoundMgr.bgmOn
end


function SoundMgr:set_eff_on(b)
    csSoundMgr.effOn = b
end

function SoundMgr:get_eff_on()
    return csSoundMgr.effOn
end


--//-------~★~-------~★~-------~★~音量~★~-------~★~-------~★~-------//

--设置总音量
function SoundMgr:set_total_volume(value)
    csSoundMgr.totalVolume = value
end

function SoundMgr:get_total_volume()
    return csSoundMgr.totalVolume
end


--设置bgm音量
--@value 0~1
function SoundMgr:set_bgm_volume(value)
    csSoundMgr.bgmVolume = value
end

function SoundMgr:get_bgm_volume()
    return csSoundMgr.bgmVolume
end

--设置音效音量
function SoundMgr:set_eff_volume(value)
    csSoundMgr.effVolume = value
end

function SoundMgr:get_eff_volume()
    return csSoundMgr.effVolume
end


return SoundMgr