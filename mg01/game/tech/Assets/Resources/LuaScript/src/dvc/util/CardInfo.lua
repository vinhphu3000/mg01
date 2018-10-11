-- CardInfo
--@author jr.zeng
--@date 2018/7/30  21:07
local modname = "CardInfo"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
CardInfo = class(modname, super, _ENV)
local CardInfo = CardInfo

--===================module content========================

function CardInfo:__ctor()

	self.name = false

	--颜色
	self.color = 0
	self.txt_color = false
	--花色
	self.pattern = false    --'1','2','-', ...
	--排序权重
	self.weight = 0

	--状态
	self.state = 0
	--持有者
	self.owner = false
	self.owner_uuid = 0

	--位于那个卡牌区
	self.zone_tp = 0
	--在卡牌区的序号
	self.index = 0

	--插入位置
	self.insert_ver = -1
	self.insert_pos_arr = false


end

function CardInfo:init(color, card_cfg)

	self.color = color
	self.txt_color = dvc_util.get_card_txt_color(color)

	self.state = CARD_STATE.CLOSE

	self.pattern = card_cfg.pattern
	self.weight = card_cfg.weight

	self.name = dvc_util.get_card_name(color, self.pattern )
end


function CardInfo:get_label()
	return self.name
end


return CardInfo