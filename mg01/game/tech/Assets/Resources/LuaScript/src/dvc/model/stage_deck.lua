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
	self.m_deckItems = false    --牌库配置列表

	self.m_card_list = false
	self.m_color2cards = false
end

function stage_deck:__setup(deckInfo)

	self:ctor()

	self.m_deckInfo = deckInfo
	self.m_deckType = deckInfo.deckType

	self:setup_deck()
	self:shuffle_deck()
end

function stage_deck:__clear()


	self:clear_deck()

	self.m_deckInfo = false
	self.m_deckItems = false
end

function stage_deck:setup_event()


end

function stage_deck:clear_event()


end

function stage_deck:get_deck_items()
	return self.m_deckItems
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--初始化牌库
function stage_deck:setup_deck()

	self.m_deckItems = self.m_deckInfo.deckCfg

	self.m_card_list = {}
	self.m_color2cards = {}

	local cardInfo
	for i, deck_item in ipairs(self.m_deckItems) do
		for i, cfg in ipairs(deck_item.cards) do
			cardInfo = new(CardInfo)
			cardInfo:init(deck_item.color, cfg)
			self.m_card_list[#self.m_card_list+1] = cardInfo

			local cards = self.m_color2cards[deck_item.color]
			if not cards then
				cards = {}
				self.m_color2cards[deck_item.color] = cards

			end
			cards[#cards+1] = cardInfo
		end
	end
end


function stage_deck:clear_deck()

	self.m_card_list = false
	self.m_color2cards = false
end

--洗牌
function stage_deck:shuffle_deck(color)

	local cards

	if color then
		--指定颜色
		cards = self.m_color2cards[color]
		Array.random_shuffle(cards)
	else
		for color, cards in pairs(self.m_color2cards) do
			Array.random_shuffle(cards)
		end
	end
end


--获取指定颜色的剩余牌数
function stage_deck:get_card_num(color)
	local cards = self.m_color2cards[color]
	if cards then
		return #cards
	end
	return nil
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


function stage_deck:__draw_one_card(player, color)

	local cards = self.m_color2cards[color]
	local card = cards[#cards]   --取末尾那张
	cards[#cards] = nil --从牌库移除
	self:notify(DVC_EVENT.DECK_CARD_CHANGE, color)

	player:add_to_draw_zone(card)
	return card
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


--//-------∽-★-∽抽牌相关∽-★-∽--------//


function stage_deck:can_draw_one_card(player, color, alarm)

	if #self.m_color2cards[color] == 0 then
		if alarm then
			nslog.print_t('没有牌了')
		end
		return false
	end


	return true
end

--抽牌
function stage_deck:draw_one_card(player, color)

	if not self:can_draw_one_card(player, color, true) then
		return end

	self:__draw_one_card(player, color)
end

--随机抽牌
function stage_deck:draw_rand_card(player, color)

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


	if color then
		local card = self:__draw_one_card(player, color)
		self:dump_deck()
		return card
	end

	nslog.print_t(string.format('%s 想抽牌， 但已经没牌了', player:get_label() ))
	return false
end

return stage_deck