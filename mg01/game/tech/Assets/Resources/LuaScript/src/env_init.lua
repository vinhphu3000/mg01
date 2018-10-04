--env_init
--@author jr.zeng
--2017年9月19日 下午5:20:33
local modname = "env_init"
--==================global reference=======================

local string = string
local xpcall = xpcall
local _require = require



--//-------~★~-------~★~safe_call~★~-------~★~-------//

local function on_error(err_str)

    local str = debug.traceback(err_str, 2)
    log_error(str)
end

--安全调用
--return is_success, 函数返回值
function safe_call(func, target, ...)
	if target ~= nil then
		return xpcall(func, on_error, target, ...)
	end
	return xpcall(func, on_error, ...)
end


--//-------~★~-------~★~require~★~-------~★~-------//

--require_ = _require
--function require(path)
--    return _require("src."..path)     --万一不止用到src
--end

--//-------~★~-------~★~unity~★~-------~★~-------//

Unity = UnityEngine

--//-------~★~-------~★~namespace~★~-------~★~-------//

require "src.org.namespace.namespace"
require "src.org.lib"
require "src.orgu.lib"

local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)

Unity = UnityEngine     --Unity简称，写到g_ns

--日志
local log = log
log.set_print(log_print)
--g_ns.log.set_warn(log_warn)    --不使用unity的warning, 因为logfile会过滤掉

local log_lv = G_LOG_LV.DEBUG
--local log_lv = g_ns.G_LOG_LV.INFO
log.set_log_lv(log_lv)
log.set_log_time(true)
log.add_log_mod({['all'] = true})

log.print_t("CTypeName", CTypeName)
log.print_r("CType", CType)


--锁定G表
--g表可写入白名单
local g_white =
{
	["log_print"] = 1,      --打印log
	["log_warn"] = 1,       --打印警告
	["log_error"] = 1,      --打印错误
	["log_trace"] = 1,      --输出日志
	["console_print"] = 1,  --控制台打印
	--以上是c#接口
}

local function forbid_global(whiteList)

	whiteList = whiteList or {}

	--禁用G表
	local env = _G
	setmetatable(env,  {__newindex =
	function(t, k, v)
		if whiteList[k] then
			rawset(t, k, v)
		else
			assert(false, "can not write global "..k)
		end
	end  }
	)
end

forbid_global(g_white)