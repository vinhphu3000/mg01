--bev_tree
--@author jr.zeng
--2017年4月17日 下午2:23:38
local modname = "bev_tree"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = bev_parent	--父类
bev_tree = class(modname, super)
local bev_tree = bev_tree

--===================module content========================

function bev_tree:ctor(setting)

	--完成后重新开始
	self.restart_when_done = false

	super.ctor(self, setting)
end

--@setting{
--  bev_board
--  restart_when_done   完成后重新开始
--}
function bev_tree:setting(setting)

	super.setting(self, setting)

	local bev_board = setting.bev_board or bev_board
	if bev_board.isInstance then
		self.m_board = setting.bev_board
	else
		self.m_board = new(bev_board, setting)
	end

	self.m_input = self.m_board

	self.restart_when_done = setting.restart_when_done or false
end


function bev_tree:__setup(...)

	self.m_board:setup(...)
	self:set_board(self.m_board)
end


function bev_tree:__clear()

	self.m_board:clear()

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function bev_tree:__update(input)
    
    if self.m_cur_bev then
    
        self.m_cur_bev:update(input)

        if self.m_cur_bev:done() then
            --整棵树完成
	        local state = self.m_cur_bev:getState()
	        self:set_cur_bev(nil, input)
	        --self:print_t("行为树返回了", state, self.restart_when_done)

	        if self.restart_when_done then
		        --完成后重新开始
		        local bev = self:get_fore_bev()
		        if bev:can_enter(input) then
			        self:set_cur_bev(bev, input)
		        else
			        self.m_state = BEV_STATE.FAIL
		        end
	        else
		        self.m_state = state
	        end
        end
    else
        self.m_state = BEV_STATE.FAIL
    end
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

--整个树完成后, 外部可根据需要重新进入 或 不再进入
function bev_tree:__enter(input)

	local bev = self:get_fore_bev()
	if bev:can_enter(input) then
		self:set_cur_bev(bev, input)
	else
		self.m_state = BEV_STATE.FAIL
	end
end

return bev_tree