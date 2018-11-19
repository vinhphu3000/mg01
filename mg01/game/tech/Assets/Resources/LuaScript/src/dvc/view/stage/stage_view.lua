-- stage_view
--@author jr.zeng
--@date 2018/10/8  17:37
local modname = "stage_view"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {'org.kui'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = UIPop	--父类
stage_view = class(modname, super, _ENV)
local stage_view = stage_view


local ui_root = 'Container_Stage/Container_Panel'

local ui_map =
{
	container_players = "Container_Players",

	container_deck = {"Container_Deck",
		container_content = 'Container_content'
	},

	container_players = {"Container_Players",
		container_content = 'Container_content'
	},

	label_time = "Container_Top/Label_TIme",
	btn_quit = "Button_quit",

}

local ui_map_deck =
{
	icon_deck = "Image_sharedAnchor",
	label_num = "Label_Text",
}

local deck_num = 2
for i=1, deck_num do
	ui_map['btn_deck'..i] = {"Container_Deck/Button_Deck"..i, ui_map=ui_map_deck}
end

--窗口id
stage_view.pop_id = POP_ID.STAGE
--层级
stage_view.layerId = POP_LAYER_ID.LAYER_MAIN_UI

--===================module content========================

function stage_view:__ctor()

	self.m_handler = self:get_handler(ui_root, ui_map)

	self.m_listViewDeck = self:create(ListView, {
		container=self.m_handler.container_deck.container_content,
		on_item_data = callback(self.update_deck_item, self),
	})

	self.m_listViewPlayer = self:create(ListView, {
		container=self.m_handler.container_players.container_content,
		item_cls = player_zone_item,
	})

end

function stage_view:__show(showObj, ...)

	self.m_handler.label_time:set_color24(0xffff00)

	self:show_deck()
	self:show_player_list()

	stage_mgr:set_phase(STAGE_PHASE.DRAW)
end

function stage_view:on_click_quit()

end

function stage_view:setup_event()
    
    self.m_handler.btn_quit:attach_click(self.on_click_quit, self)

	self:add_listener(DVC_EVENT.DECK_CARD_CHANGE, self.on_deck_change, self)
end

function stage_view:clear_event()

	--self.m_handler.btn_quit:detach_click(self.on_click_quit, self)
end

function stage_view:__destroy()

    self:clear_deck()
	self:clear_player_list()
end

function stage_view:on_deck_change(color)

	self:refrsh_deck_data(color)
end

--//-------~★~-------~★~-------~★~牌库相关~★~-------~★~-------~★~-------//

function stage_view:show_deck()

	self.m_color2deckItem = {}

	local deckItem = stage_deck:get_deck_items()
	self.m_listViewDeck:show(deckItem)
end

function stage_view:clear_deck()

	self.m_listViewDeck:destroy()

	self.m_color2deckItem = false
end


function stage_view:update_deck_item( item_go, data, index )

	local handler = self:get_handler(item_go, ui_map_deck)

	handler.icon_deck:load_sprite('CardBack'..data.color, false)

	self.m_color2deckItem[data.color] = handler

	self:refrsh_deck_data(data.color)

	handler:attach_click(callback_n(self.on_click_deck, self, data.color))
end

function stage_view:refrsh_deck_data(color)

	local handler = self.m_color2deckItem[color]

	local num = stage_deck:get_card_num(color)
	handler.label_num:set_text('剩余:' .. num)
end


function stage_view:on_click_deck( color )

	nslog.print_t('on_click_deck', color)
end


--//-------~★~-------~★~-------~★~玩家列表相关~★~-------~★~-------~★~-------//


function stage_view:show_player_list()

	local playerList = player_cache:get_act_players()

	self.m_listViewPlayer:show(playerList)
end

function stage_view:clear_player_list()

	self.m_listViewPlayer:destroy()

end




return stage_view