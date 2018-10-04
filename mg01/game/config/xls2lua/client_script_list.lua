
--@includes     需要引用的表
--@export_name  输出的表名
--@need_script  是否有导表脚本
local function item(includes, export_name, need_script)
	includes = type(includes) == 'string' and {includes} or includes
	assert(includes and #includes>0 and export_name )
	return {includes=includes, export_name=export_name, need_script=need_script or false}
end

return {

	item('魂卡秘境', 'card_war_config', true),
	item('黑暗深渊常量', 'guild_abyss_const', true),



}