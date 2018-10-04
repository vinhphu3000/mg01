-- proc_player_place_card
--@author jr.zeng
--@date 2018/8/11  15:48
local modname = "proc_player_place_card"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = proc_base	--父类
proc_player_place_card = class(modname, super, _ENV)
local proc_player_place_card = proc_player_place_card

--===================module content========================

function proc_player_place_card:__ctor()

end



--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_player_place_card:__update(input)

	self.m_state = BEV_STATE.SUCCESS

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_player_place_card:__enter(input)

	--nslog.print_t(string.format('<color=#ffdf58ff>%s 开始放牌</color>', self.m_player:get_label()))
	nslog.print_t(string.format('<color=#ffdf58ff>%s 开始放牌</color>', self.m_agent:get_label()))

	local card = self.m_agent:get_first_in_draw_zone()
	if card then

		--nslog.print_t('放牌', card)

		local pos_arr = self.m_agent:get_insert_pos_arr(card)
		local index = math.random(1, #pos_arr) --没传入位置，随机一个
		index = pos_arr[index]
		self.m_agent:add_to_game_zone(card, index)
	end

end

function proc_player_place_card:__exit(input)



end


return proc_player_place_card