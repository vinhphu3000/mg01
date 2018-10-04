-- reload
--@author jr.zeng
--@date 2018/7/16  17:05
local modname = "reload_lua"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

local DateUtil = DateUtil
local FileUtil = FileUtil

local hard_reload = require 'src.orgu.util.hard_reload'
function hard_reload.error_log(...)
	nslog.print_t("ERROR:", ...)
end

--===================module property========================

--===================module content========================

local script_path = 'Assets/Resources/LuaScript'
local __last_time = DateUtil.time_st_ms()



local function collect_scripts(path, path2name)

	local files = FileUtil.get_files(path)
	--nslog.print_t('get_files', files)
	for _, path in pairs(files) do

		for k, v in string.gmatch(path, "(.+LuaScript.)(.+).lua$") do
			v = string.gsub(v, "([\\/])", ".")
			path2name[path] = v     --Assets/Resources/LuaScript\src\game\main\MainEntry.lua -> src.game.main.MainEntry
		end
	end

	local folds = FileUtil.get_directories(path)
	--nslog.print_t('get_directories', folds)
	for _, path in pairs(folds) do
		collect_scripts(path, path2name)
	end
end

function reload_lua(path)

	local last_time = __last_time
	__last_time = DateUtil.time_st_ms()

	path = path or script_path
	local path2name = {}
	collect_scripts(path, path2name)
	--nslog.print_t(path2name)

	for p, name in pairs(path2name) do
		local t = FileUtil.get_last_write_time(p)
		if t > last_time then   --收集有修改的文件
			--self:hard_reload(m)
			nslog.print_t(string.format('reload: %s modify time: %d', name,t ))
			local ok, msg = hard_reload.reload(name, name)
			if not ok then
				nslog.print_t(msg)
			end
		end
	end

	nslog.print_t('reload complete')
end
