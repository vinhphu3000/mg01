--HelpImage
--@author jr.zeng
--2017年11月2日 下午4:15:33
local modname = "HelpImage"
--==================global reference=======================

local sprite_msg = require "data.sprite_msg"

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpImage = class(modname, super, _ENV)
local HelpImage = HelpImage

local CType = CType
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local SprAtlasCache = SprAtlasCache



local function getKImage(go)
    return HelpImage.need_component(go, CType.KImage)
end

local function getImage(go)
    return HelpImage.need_component(go, CType.Image)
end

--===================module content========================

--形参以(go, refer)开头的函数
HelpImage.fnSet_gr =
{
	load_sprite =1,
	set_sprite =1,
}


--加载图片_异步
--@nativeSize 是否重置size
function HelpImage.load_sprite(go_, refer_, spriteName_,  nativeSize)

    local a = getImage(go_)
    if not a then
        return end
	local atlasName = sprite_msg[spriteName_]   --获取图片所在的图集
	if not atlasName then
		nslog.error(false, 'miss sprite atlas ' .. spriteName_)
	end
    SprAtlasCache:loadSprite(refer_, a, atlasName, spriteName_,  nativeSize)
end

--加载图片_同步
--@nativeSize 是否重置size
function HelpImage.set_sprite(go_, refer_, spriteName_,  nativeSize)

    local a = getImage(go_)
    if not a then
        return end
	local atlasName = sprite_msg[spriteName_]  --获取图片所在的图集
	if not atlasName then
		nslog.error(false, 'miss sprite atlas ' .. spriteName_)
	end
    SprAtlasCache:setSprite(refer_, a, atlasName, spriteName_,  nativeSize)
end


return HelpImage