-- stage_deck
--@author jr.zeng
--@date 2018/7/31  11:10
local modname = "stage_deck"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
stage_deck = class(modname, super, _ENV)
local stage_deck = stage_deck

local CARD_COLOR = CARD_COLOR

--===================module content========================

function stage_deck:__ctor()

	self.m_deckInfo = false

	self.m_deckType = 0

	self.m_card_list = false
	self.m_color2cards = false
end

function stage_deck:__setup(deckInfo)

	self:ctor()

	self.m_deckInfo = deckInfo
	self.m_deckType = deckInfo.deckType

	self:setup_deck()
end

function stage_deck:__clear()


	self:clear_deck()

	self.m_deckInfo = false
end

function stage_deck:setup_event()


end

function stage_deck:clear_event()


end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--初始化牌库
function stage_deck:setup_deck()

	self.m_card_list = {}
	self.m_color2cards = {}

	local deckCfg = self.m_deckInfo.deckCfg

	local cardInfo
	for color, card_cfgs in pairs(deckCfg) do

		for i, cfg in ipairs(card_cfgs) do
			cardInfo = new(CardInfo)
			cardInfo:init(color, cfg)
			self.m_card_list[#self.m_card_list+1] = cardInfo

			local cards = self.m_color2cards[color]
			if not cards then
				cards = {}
				self.m_color2cards[color] = cards

			end
			cards[#cards+1] = cardInfo
		end
	end
end


function stage_deck:clear_deck()

	self.m_card_list = false
	self.m_color2cards = false
end

--获取指定颜色的剩余牌数
function stage_deck:get_card_num(color)
	local cards = self.m_color2cards[color]
	if cards then
		return #cards
	end
	return nil
end



function stage_deck:has_color(color)
	return self.m_color2cards[color] ~= nil
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

--获取可用牌库
function stage_deck:get_enable_color()

	local cards
	for k,color in pairs(CARD_COLOR) do
		cards = self.m_color2cards[color]
		if #cards > 0 then
			return color
		end
	end
	return false
end


--抽牌
function stage_deck:draw_card(player, color)

	if not color then
		--随机一个颜色
		color = math.random(1, CARD_COLOR_NUM)
	end

	local cards = self.m_color2cards[color]
	if #cards == 0 then
		--没有牌了
		color = self:get_enable_color()
		cards = self.m_color2cards[color]
	end

	self:dump_deck()

	if color then
		local index = math.random(1, #cards)    --随机一张
		local card = cards[index]   --从牌库移除
		table.remove(cards, index)
		player:add_to_draw_zone(card)
		return card
	end

	nslog.print_t(string.format('%s 想抽牌， 但已经没牌了', player:get_label() ))
	return false
end


--打印牌库
function stage_deck:dump_deck()

	local str = '当前牌库：\n'
	local str2 = ''
	local cards
	for k,color in pairs(CARD_COLOR) do
		cards = self.m_color2cards[color]
		local name = CARD_COLOR_NAME[color]
		str = str .. name .. '(' .. #cards ..'张) '

		str2 = str2 .. '\n' .. name .. '：'
		if #cards > 0 then
			for i, card in ipairs(cards) do
				str2 = str2 .. card:get_label() .. ' '
			end
		end
	end

	nslog.print_t(str .. str2)
end


return stage_deck