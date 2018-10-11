-- player_zone_item
--@author jr.zeng
--@date 2018/10/11  11:39
local modname = "player_zone_item"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = ListViewItem	--父类
player_zone_item = class(modname, super, _ENV)
local player_zone_item = player_zone_item

local ui_map =
{
	label_name = "Label_Name",

	container_draw = {"Container_draw",
		container_content = "Container_content",
	},

	container_game = {"Container_game",
		container_content = "Container_content",
	},



}
--===================module content========================

function player_zone_item:__ctor()

	self.m_player = false


	self.m_handler = self:get_handler(nil, ui_map)

	self.m_listViewDraw = self:create(ListView, {
		container=self.m_handler.container_draw.container_content,
		dir = LayoutDir.LeftToRight,
		item_cls = player_card_item,
	})

	self.m_listViewGame = self:create(ListView, {
		container=self.m_handler.container_game.container_content,
		dir = LayoutDir.LeftToRight,
		item_cls = player_card_item,
	})
end

function player_zone_item:__show(player)

	self.m_player = player
	--nslog.print_t(player)

	local name = self.index .. '. ' .. self.m_player.name
	self.m_handler.label_name:set_text(name)

	self:show_draw_zone()
	self:show_game_zone()
end

function player_zone_item:setup_event()

	self:add_listener(DVC_EVENT.PLAYER_DRAW_ZONE_CHANGE, self.on_draw_zone, self)
	self:add_listener(DVC_EVENT.PLAYER_GAME_ZONE_CHANGE, self.on_game_zone, self)
end

function player_zone_item:clear_event()


end

function player_zone_item:__destroy()

	self:clear_draw_zone()

	self.m_player = false

end


function player_zone_item:on_draw_zone(player, card)

	if self.m_player ~= player then
		return end
	self:show_draw_zone()
end

function player_zone_item:on_game_zone(player, card)

	if self.m_player ~= player then
		return end
	self:show_game_zone()
end

--//-------~★~-------~★~-------~★~抽牌区~★~-------~★~-------~★~-------//


function player_zone_item:show_draw_zone()

	local card_list = self.m_player:get_draw_zone()
	self.m_listViewDraw:show(card_list)

end



function player_zone_item:clear_draw_zone()

	self.m_listViewDraw:destroy()
end

--//-------~★~-------~★~-------~★~游戏区~★~-------~★~-------~★~-------//


function player_zone_item:show_game_zone()

	local card_list = self.m_player:get_game_zone()
	self.m_listViewGame:show(card_list)

end


function player_zone_item:clear_game_zone()

	self.m_listViewGame:destroy()

end


return player_zone_item