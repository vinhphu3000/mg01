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
	container_deck = "Container_Deck",

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


end

function stage_view:__show(showObj, ...)


	self:show_deck()

end

function stage_view:setup_event()
    
    
end

function stage_view:clear_event()
    
    
end

function stage_view:__destroy()
    
    
end

--//-------~★~-------~★~-------~★~牌库相关~★~-------~★~-------~★~-------//

function stage_view:show_deck()

	for i=1, deck_num do

		local handler = self.m_handler['btn_deck'..i]

		if stage_deck:has_color(i) then
			self:update_deck_item(i)
			handler:set_active(true)
		else
			handler:set_active(false)
		end
	end

end

function stage_view:update_deck_item( color )

	local handler = self.m_handler['btn_deck'..color]

	local num = stage_deck:get_card_num(color)
	handler.label_num:set_text('剩余:' .. num)

	handler.icon_deck:load_sprite('CardIcon', 'CardBack'..color, false)
end

return stage_view