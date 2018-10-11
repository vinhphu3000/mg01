-- HelpUi
--@author jr.zeng
--@date 2018/10/11  21:34
local modname = "HelpUi_dvc"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module content========================
local super = nil	--父类
HelpUi_dvc = class(modname, super, _ENV)
local HelpUi_dvc = HelpUi_dvc

local HelpImage = HelpImage

--===================module content========================

--形参以(go, refer)开头的函数
HelpUi_dvc.fnSet_gr =
{
	load_icon_cardFace =1,
	load_icon_cardBack =1,
}



function HelpUi_dvc.load_icon_cardFace(go_, refer_, card_color,  nativeSize)

	HelpImage.load_sprite(go_, refer_, 'CardIcon', 'CardFace'..card_color,  nativeSize)
end


function HelpUi_dvc.load_icon_cardBack(go_, refer_, card_color,  nativeSize)

	HelpImage.load_sprite(go_, refer_, 'CardIcon', 'CardBack'..card_color,  nativeSize)
end


UIHandler.addHelp(HelpUi_dvc)


return HelpUi_dvc