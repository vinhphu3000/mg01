-- player_card_item
--@author jr.zeng
--@date 2018/10/11  11:41
local modname = "player_card_item"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = ListViewItem	--父类
player_card_item = class(modname, super, _ENV)
local player_card_item = player_card_item

--===================module content========================

local ui_map =
{
	container_select = "Container_select",
	container_choose = "Container_choose",

	container_info = "Container_Info",

	label_name = "Container_Info/Container_face/Label_Name",
	container_face = {"Container_Info/Container_face",
		icon = 'Image_sharedAnchor',
	},

	container_back = {"Container_Info/Container_back",
		icon = 'Image_sharedAnchor',
	},

}

function player_card_item:__ctor()

	self.m_card = false
	self.m_face = false

	self.m_handler = self:get_handler(nil, ui_map)

end

function player_card_item:__show(card)

	self.m_card = card

	local name = string.format('<color=#%s>%s</color>', self.m_card.txt_color, self.m_card.pattern )
	self.m_handler.label_name:set_text(name)


	self.m_handler.container_face.icon:load_icon_cardFace(self.m_card.color)
	self.m_handler.container_back.icon:load_icon_cardBack(self.m_card.color)

	self:set_face(true, false)
end

function player_card_item:setup_event()


end

function player_card_item:clear_event()


end

function player_card_item:__destroy()


	self.m_card = false
	self.m_face = false

end


--//-------~★~-------~★~-------~★~icon~★~-------~★~-------~★~-------//

--设置面向
function player_card_item:set_face(b, ease)

	if not ease then

		self.m_face = b
		self.m_handler.container_face:set_active(b)
		self.m_handler.container_back:set_active(not b)
	else

		if self.m_face ~= b then
			self.m_face = b


		end
	end
end


return player_card_item