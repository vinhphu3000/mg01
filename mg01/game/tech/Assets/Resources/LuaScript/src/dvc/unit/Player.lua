-- Player
--@author jr.zeng
--@date 2018/7/30  20:51
local modname = "Player"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
Player = class(modname, super, _ENV)
local Player = Player

local CARD_ZONE_TP = CARD_ZONE_TP

--===================module content========================

function Player:__ctor()


	self.m_info = false

	self.uuid = 0
	self.name = false

	--座位序号
	self.index = 0
	--行动序号
	self.act_idx = 0

	self.m_game_zone = new(Array)   --游戏区
	self.m_draw_zone = new(Array)   --抽牌区

	self.m_game_zone_ver = 1  

end


function Player:__setup(playerInfo)

	self.m_info = playerInfo

	self.uuid = playerInfo.uuid
	self.name = playerInfo.name
	self.index = playerInfo.index


end


function Player:__clear()

	self.m_game_zone:clear()
	self.m_draw_zone:clear()

	self.m_info = false
end


function Player:setup_event()

end


function Player:clear_event()

end


function Player:get_label()
	return string.format('%s(%d)', self.name, self.act_idx)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽卡牌相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽--------∽-★-∽抽牌区∽-★-∽------∽-★-∽--------//

function Player:get_draw_zone()
	return self.m_draw_zone
end


--添加到抽牌区
function Player:add_to_draw_zone(cardInfo)

	cardInfo.owner = self
	cardInfo.owner_uuid = self.m_info.uuid
	cardInfo.zone_tp = CARD_ZONE_TP.DRAW

	self.m_draw_zone:push(cardInfo)
	cardInfo.index = #self.m_draw_zone

	nslog.print_t(string.format('%s 抽到了一张 %s \n' .. self:dump_draw_zone(), self:get_label(), cardInfo.name), cardInfo)
	self:notify(DVC_EVENT.PLAYER_DRAW_ZONE_CHANGE, self, cardInfo)
end

--获取抽牌区第一张卡
function Player:get_first_in_draw_zone()
	local card = self.m_draw_zone[1]
	if not card then
		nslog.print_t(string.format('%s 抽牌区没牌了', self:get_label()) )
	end
	return card
end


--打印游戏区
function Player:dump_draw_zone()

	local len = #self.m_draw_zone
	local str = string.format('%s的抽牌区(%d)：', self:get_label(), len)
	local card
	for i=1, len do
		card = self.m_draw_zone[i]
		str = str .. card:get_label() .. ' '
	end
	return str
end

--//-------∽-★-∽--------∽-★-∽游戏区∽-★-∽------∽-★-∽--------//

function Player:get_game_zone()
	return self.m_game_zone
end

--添加到游戏区
function Player:add_to_game_zone(cardInfo, index)

	assert(cardInfo.owner_uuid == self.m_info.uuid)
	assert(cardInfo.zone_tp == CARD_ZONE_TP.DRAW, cardInfo.zone_tp)

	cardInfo.zone_tp = CARD_ZONE_TP.GAME

	local pos_arr = self:get_insert_pos_arr(cardInfo)
	if index then
		if not ArrayUtil.contain(pos_arr, index) then
			--插入位置不对
			assert(false, string.format('卡牌插入位置不对：%s %d', cardInfo:get_label(), index))
		end
	else
		index = math.random(1, #pos_arr) --没传入位置，随机一个
		index = pos_arr[index]
	end

	self.m_draw_zone:remove(cardInfo)
	self:notify(DVC_EVENT.PLAYER_DRAW_ZONE_CHANGE, self, cardInfo)

	self.m_game_zone:insert(index, cardInfo)
	self.m_game_zone_ver = self.m_game_zone_ver + 1
	nslog.print_t(string.format('%s 把 %s 放到位置：%d \n' .. self:dump_game_zone(), self:get_label(), cardInfo.name, index))

	self:notify(DVC_EVENT.PLAYER_GAME_ZONE_CHANGE, self, cardInfo)
end


local function compare_card(new, old)

	local w1 = new.weight
	if w1 <= 0 then
		return -1
	end

	local w2 = old.weight
	if w1 < w2 then
		return true

	elseif w1 == w2 then
		return w1.color <= w2.color
	end

	return false
end


--获取一张新卡在游戏区的插入位置(可能有多个)
function Player:get_insert_pos_arr(cardInfo)

	if cardInfo.insert_ver == self.m_game_zone_ver then
		return cardInfo.insert_pos_arr
	end

	local weight = cardInfo.weight

	local pos_arr = {}
	local tmp_pos_arr   --万能牌的临时位置

	local cur

	if weight >= 0 then
		local b
		local len = #self.m_game_zone
		for i=1, len do
			cur = self.m_game_zone[i]

			if cur.weight >= 0 then
				if weight == cur.weight then
					--同数
					if cardInfo.color <= cur.color then
						--颜色值小，插前面
						if tmp_pos_arr then
							nslog.print_t('有万能卡的临时位置', ArrayUtil.dump(tmp_pos_arr))
							Array.concat(pos_arr, tmp_pos_arr)
							tmp_pos_arr = nil
						end

						pos_arr[#pos_arr+1] = i
					else
						--插后面
						pos_arr[#pos_arr+1] = i+1
					end
					b = true
					break
				elseif weight < cur.weight then
					--数字小，插前面
					if tmp_pos_arr then
						nslog.print_t('有万能卡的临时位置', ArrayUtil.dump(tmp_pos_arr))
						Array.concat(pos_arr, tmp_pos_arr)
						tmp_pos_arr = nil
					end

					pos_arr[#pos_arr+1] = i
					b = true
					break
				else

					tmp_pos_arr = nil   --清除临时位置
				end
			else
				--万能
				tmp_pos_arr = tmp_pos_arr or {}
				tmp_pos_arr[#tmp_pos_arr+1] = i
			end
		end

		if not b then
			--没有找到位置，放最后
			if tmp_pos_arr then
				nslog.print_t('有万能卡的临时位置', ArrayUtil.dump(tmp_pos_arr))
				Array.concat(pos_arr, tmp_pos_arr)
				tmp_pos_arr = nil
			end

			pos_arr[#pos_arr+1] = len+1
		end
	else
		--万能卡？全部位置都可以
		local len = #self.m_game_zone+1
		for i=1,len do
			pos_arr[#pos_arr+1] = i
		end
	end

	cardInfo.insert_ver = self.m_game_zone_ver --记录的版本，版本不改变不用重新计算
	cardInfo.insert_pos_arr = pos_arr

	nslog.print_t('计算插入位置', cardInfo:get_label(), ArrayUtil.dump(pos_arr))

	return pos_arr
end


--打印游戏区
function Player:dump_game_zone()

	local len = #self.m_game_zone
	local str = string.format('%s的游戏区(%d)：', self:get_label(), len)
	local card
	for i=1, len do
		card = self.m_game_zone[i]
		str = str .. card:get_label() .. ' '
	end
	return str
end

--//-------∽-★-∽--------∽-★-∽卡牌操作∽-★-∽------∽-★-∽--------//



return Player