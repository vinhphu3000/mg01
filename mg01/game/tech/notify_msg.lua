local global = require "global"
local const = require "const"
local rich_text = require "ui.tool.rich_text"
local rich_text_helper = require "ui.tool.rich_text_helper"
local SpriteAtlas = lss.ui.SpriteAtlas
local sprite_atlas = "notify_tips"
local desc_path = "GUIDesc/Msgbox/NotifyMsg"
local TOTAL_CNT = 5
local ANIM_TOTAL_LENGTH = 1.8
local POP_DELTA = 0.1

local mt = {
	last_anchor_index = 0,
	msg_queue = {},
	obj_pool = {},
	anchor_list = {},
	living_obj = {},
	pop_interval = 0,
	is_pause = false,
}

local INITED = false
function mt:init()
	if not INITED then
		INITED = true
		self.is_loading = true
		global.res_mgr:load_ui_async(desc_path, function(go)
			self.go = go
			lss.res.ResMgr.AttachUICam(go)
			self.canvas = go:GetComponent("Canvas")
	        self.canvas.sortingOrder = 99
	        self.canvas.sortingLayerName = const.UI_LAYER_LOCK
	        self.canvas.planeDistance = const.UI_PLANE_TIPS

			self.is_loading = false
			self.is_init = true
			local layout_node = go:FindChild("layoutNode")
			local _, layout_node_y = layout_node:GetAnchoredPos2D()
			for i = 1, TOTAL_CNT do
				local obj = go:FindChild("Item"..i)
				local anchor = go:FindChild("layoutNode/Anchor"..i)

				self:_release_render_obj(self:_create_render_obj(obj))
				local anchor_x, anchor_y = anchor:GetAnchoredPos2D()
				self.anchor_list[i] = {go = anchor, x = 0, y = layout_node_y+anchor_y}
			end
		end)
	end
end

function mt:set_layer(layer)
	self.go:SetLayerRecursive(layer)
end

function mt:set_canvas(layer)
    self.canvas.sortingLayerName = layer
end

function mt:revert_canvas()
    self.canvas.sortingLayerName = const.UI_LAYER_LOCK
end


function mt:show_msg(str)
	self:_push_to_queue(str)
	self:start_update()
end

function mt:start_update()
	if not self.update_key then
		self.pop_interval = 0
		self.update_key = global.updater:register(self.update, self)
	end
end

function mt:stop_update()
	if self.update_key then
		global.updater:unregister(self.update_key)
		self.update_key = nil
		global.UnloadCategory(sprite_atlas)
	end
end

function mt:pause(is_pause)
	self.is_pause = is_pause
end

function mt:update(dt)
	if self.is_loading then
		return
	end
	self:update_obj(dt)
	self.pop_interval = self.pop_interval - dt
	if (not self.is_pause) and #self.msg_queue > 0 and self.pop_interval <= 0 then
		--check if there is empty slot
		local idx = self:find_next_anchor_idx()
		if idx then
			local obj = self:_retain_render_obj()
			if obj then
				local msg = self:_pop_from_queue()
				self:_show_msg(obj, idx, msg)
				self.pop_interval = POP_DELTA
			end
		end
	else
		if #self.living_obj == 0 and #self.msg_queue == 0 then
			self:stop_update()
		end
	end
end

function mt:_show_msg(obj, anchor_idx, msg)
	self.last_anchor_index = anchor_idx
	local anchor = self.anchor_list[anchor_idx]
	obj:set_position(anchor.x, anchor.y)
	obj:awake()
	obj:set_content(msg)
	table.insert(self.living_obj, obj)
end

function mt:update_obj(dt)
	local to_del
	for i, v in ipairs(self.living_obj) do
		v:update(dt)
		if not v:is_alive() then
			to_del = to_del or {}
			table.insert(to_del, i)
		end
	end
	if to_del then
		for i = #to_del, 1, -1 do
			local obj = table.remove(self.living_obj, to_del[i])
			self:_release_render_obj(obj)
		end

		if #self.living_obj < 1 then
			self.last_anchor_index = 0
		end
	end
end

function mt:find_next_anchor_idx()
	if self.last_anchor_index < 5 then
		return self.last_anchor_index + 1
	else
		return nil
	end
end

function mt:_create_render_obj(obj)
	local ret = {}
	ret.go = obj
	ret.rt = obj
	ret.animator = obj:FindChild("ItemObj"):GetComponent("Animator")
	ret.canvasGroup = obj:GetComponent("CanvasGroup")
	ret.life_time = 0
	ret.rt_obj = rich_text.new()
	ret.rt_obj:init({
        panel = obj:FindChild("ItemObj"),
        text = obj:FindChild("ItemObj/Text"),
        image = obj:FindChild("ItemObj/Image"),
    }, {margin = {left = 70, top = 15, bottom = 20, right = 70}}, rich_text.ONE_LINE)
	function ret:set_active(bool)
		-- self.go:SetActive(bool)
		if bool then
			self.animator:Play("ResourceGetAnimGroup", -1, 0)
			self.canvasGroup.alpha = 1
		else
			self.canvasGroup.alpha = 0
		end
	end
	function ret:set_content(str)
		local block = rich_text_helper.convert_string_to_block(str, sprite_atlas)
		self.rt_obj:set_content(block)
	end
	function ret:update(dt)
		self.life_time = self.life_time - dt
	end
	function ret:is_alive()
		return self.life_time > 0
	end
	function ret:awake()
		self.life_time = ANIM_TOTAL_LENGTH
		self:set_active(true)
	end
	function ret:set_position(x, y)
		self.rt:SetAnchoredPos2D(x, y)
	end
	function ret:release_sprite()
		SpriteAtlas.ClearSprite(obj)
	end

	return ret
end

function mt:_retain_render_obj()
	if #self.obj_pool > 0 then
		return table.remove(self.obj_pool)
	else
		return nil
	end
end

function mt:_release_render_obj(obj)
	obj:set_active(false)
	obj:release_sprite()
	table.insert(self.obj_pool, obj)
end

function mt:_push_to_queue(str)
	table.insert(self.msg_queue, str)
end

function mt:_pop_from_queue()
	return table.remove(self.msg_queue, 1)
end

global.notify_msg = mt
return mt