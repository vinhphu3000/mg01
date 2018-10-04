-- DeckInfo
--@author jr.zeng
--@date 2018/7/31  16:57
local modname = "DeckInfo"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
DeckInfo = class(modname, super, _ENV)
local DeckInfo = DeckInfo

--===================module content========================

function DeckInfo:__ctor()

	self.deckType = 0
	self.deckCfg = false
end


function DeckInfo:init(data)

	self.deckType = data.deckType
	self.deckCfg = DECK_CONFIG[ data.deckType]

end


return DeckInfo