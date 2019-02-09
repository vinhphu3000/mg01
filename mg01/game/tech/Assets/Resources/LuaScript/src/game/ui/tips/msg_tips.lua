-- msg_tips
--@author jr.zeng
--@date 2019/2/4  13:45
local modname = "msg_tips"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {'org.kui'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = UIPop	--父类
msg_tips = class(modname, super, _ENV)
local msg_tips = msg_tips


local ui_root = 'Container_NotifyTips/Container_Panel'

local ui_map = {

	--btn_close = 'Button_close',
	container_tips1 = 'Container_tips1',

}


local ui_map_item =
{
	label_tips = "Container_item/Label_Tips",
	image_bg = "Container_item/Image_zhezhao",
}

local tips_num_max = 8

--窗口id
msg_tips.pop_id = POP_ID.MSG_TIPS
--层级
msg_tips.layerId = POP_LAYER_ID.LAYER_TIPS
--
msg_tips.lifeType = POP_LIFE.FOREVER

--===================module content========================

local anim_time = 1.8
local pop_internal = 0.1

local function gen_item(handler)
	return {
		handler = handler,
		delay = 0,
	}
end

function msg_tips:__ctor()

	self.m_handler = self:get_handler(ui_root, ui_map)     --代理

	local tips =  self.m_handler.container_tips1
	tips:set_active(false)

	self.m_idx2item = {
		gen_item( self:get_handler(tips, ui_map_item) )
	}

	local parent = tips:get_parent()
	local ox,oy = self.m_handler.container_tips1.get_ach_pos()
	local ow,oh = self.m_handler.container_tips1.get_size_delta()
	for i=2, tips_num_max do

		local y = oy - ( (i-1) * oh + 1*i )
		local new_go = instantiate(tips.gameObject)
		new_go:SetActive(false)
		GameObjUtil.change_parent(new_go, parent)
		new_go:SetAchPos_(ox, y)
		self.m_idx2item[i] = gen_item( self:get_handler(new_go, ui_map_item) )
	end

	self.m_itemArr = new(Array)

	----

	self.m_msgQueue = new(Array)
	self.m_msgNum = 0

	self.m_pause = false
	self.m_running = false
	self.m_curIdx = 0
	self.m_pop_internal = 0
end

function msg_tips:__show(showObj, ...)
    
    
end

function msg_tips:setup_event()
    
    
end

function msg_tips:clear_event()
    
    
end

function msg_tips:__destroy()
    
    self:clear_all_msg()

end


--//-------~★~-------~★~-------~★~msg~★~-------~★~-------~★~-------//

--清除所有提示
function msg_tips:clear_all_msg()

	--nslog.print_t('clear_all_msg')

	self.m_msgQueue:clear()
	self.m_msgNum = 0

	self:__stop()

	local num = #self.m_itemArr
	if num > 0 then
		local item
		for i=1, num do
			item = self.m_itemArr[i]
			item.handler:set_active(false)
		end
		self.m_itemArr:clear()
	end
end


--清除剩余提示
function msg_tips:clear_rest_msg()

	self.m_msgQueue:clear()
	self.m_msgNum = 0
end

function msg_tips:show_msg(str)
	
	self.m_msgQueue:add(str)
	self.m_msgNum = self.m_msgNum + 1

	if not self.m_pause and not self.m_running  then
		self:__start()
	end
end

function msg_tips:__start()
	if self.m_running then
		return end
	self.m_running = true
	self:attach_update(self.update, self)
end

function msg_tips:__stop()
	if not self.m_running then
		return end
	self.m_running = false
	self:detach_update(self.update, self)
	--nslog.print_t('__stop')
end

function msg_tips:update(dt)

	local len = #self.m_itemArr
	if len > 0 then
		local item
		for i=len, 1, -1 do
			item = self.m_itemArr[i]
			item.delay = item.delay - dt
			if item.delay <= 0 then
				item.handler:set_active(false)
				self.m_itemArr:remove_at(i)
				len = len -1
			end
		end
	end

	if self.m_pop_internal > 0 then
		self.m_pop_internal = self.m_pop_internal - dt
	end

	if self.m_msgNum > 0 then

		if not self.m_pause then

			if len <= 0 then
				--当前没有正在播放的msg, 从第一个开始播
				self.m_curIdx = 0
			end

			if self.m_curIdx < tips_num_max then

				if self.m_pop_internal <= 0 then

					self.m_curIdx = self.m_curIdx + 1
					local item = self.m_idx2item[self.m_curIdx]
					self.m_itemArr:add(item)

					local str = self.m_msgQueue:shift()
					self.m_msgNum = self.m_msgNum - 1

					item.handler.label_tips:set_text(str)
					item.handler:set_active(true)
					item.delay = anim_time

					self.m_pop_internal = pop_internal
				end
			end
		end
	else

		if len <= 0 then
			self:clear_all_msg()
		end
	end


end


--//-------~★~-------~★~-------~★~暂停~★~-------~★~-------~★~-------//


function msg_tips:pause()

	if self.m_pause then
		return end
	self.m_pause = true

	--self:detach_update(self.update, self)
end


function msg_tips:resume()

	if not self.m_pause then
		return end
	self.m_pause = false

	if self.m_msgNum > 0 then
		self:__start()
	end

end


return msg_tips