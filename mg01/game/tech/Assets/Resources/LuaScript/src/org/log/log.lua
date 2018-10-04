--log
--@author jr.zeng
--2017年4月14日 下午5:29:21
local modname = "log"
--==================global reference=======================

local type = type
local pairs = pairs
local tostring = tostring
local string = string
local format = string.format
local next = next
local assert = assert
local unpack = table.unpack
local setmetatable = setmetatable

local print = print
local warn = nil


--===================namespace========================
local ns = nil  --不放在org,方便调用
local using = {"org"}
local _ENV = namespace(ns, using)

local os = os
local t2str = t2str
local upv2t = upv2t
local StringUtil = StringUtil

--===================module property========================


--log的输出级别
G_LOG_LV = 
    {
        --调试
        DEBUG   = 0,
        --信息
        INFO    = 1,
        --警告
        WARN    = 2,
        --错误
        ERROR   = 3,
        --崩溃
        FATAL   = 4,
    }

--log名称
G_LOG_NAME = {}

for k, v in pairs(G_LOG_LV) do
    G_LOG_NAME[v] = string.lower(k)
end

local G_LOG_CRASH_MAX = 3    --打印崩溃日志的最大次数
local G_LOG_LV = G_LOG_LV
local G_LOG_NAME = G_LOG_NAME

local __logLevel = 0        --当前日志等级
local __logTime = true      --是否打印时间
local __logDebug = true

--===================module content========================

local log = { }

log.putout_fun = nil   --额外输出回调

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--设置当前日志等级
function log.set_log_lv(level)
    __logLevel = level
end

--设置是否打印时间
function log.set_log_time(b)
    __logTime = b
end

--设置输出函数
function log.set_print(fun)
    print = fun
end

--设置警告函数
function log.set_warn(fun)
    warn = fun
end

--//-------~★~-------~★~-------~★~log原方法~★~-------~★~-------~★~-------//

--local __metaLogFun = false  --log元方法
--
--function log.setMetaLog(fun)
--	__metaLogFun = fun
--end

--setmetatable(log, {
--	__call = function(self, ...)        --为了可以直接调log()
--		if __metaLogFun then
--			__metaLogFun(...)
--		end
--	end
--})


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


local function pack(...)

    local arg = {...}
    local len = #arg
    if len == 0 then
        return ""
    end

    if len == 1 then
        return tostring(arg[1])
    end

    local str = ""
    for i,v in ipairs(arg) do
        --str = str .. tostring(v) .. "\t"
        str = str .. tostring(v) .. "  "
    end
    --str = str .. "\n"
    return str
end

local function __print(...)

    print(...)
    --    if __logFile then
    --        --需要写入文件
    --        __logFile:write(content .. "\n")
    --        __logFile:flush()
    --    end
end

local function __warn(...)
    if warn then
        warn(...)
    else
        print(...)
    end
end

local function __logInDetail(level, strFormat, ...)

    level = level or G_LOG_LV.DEBUG
    if level < __logLevel then
        --等级不足
        return end

    local info = nil
    if strFormat then
        --info = string.format(strFormat, ...)
        info = pack(strFormat, ...)
    else
        info = ""
    end

    local str
    --if __logTime then
    --    --记录时间
    --    str = format("[%s][%s]%s", G_LOG_NAME[level], os.date(), info)
    --else
        str = format("[%s]%s", G_LOG_NAME[level], info)
    --end

    if level == G_LOG_LV.WARN then
        __warn(str)
    elseif level >= G_LOG_LV.ERROR then
        --错误
        --local trace = debug.traceback("", 2)
        --str = str + "\n" + trace
        assert(false, str)
        --__print(str)
    else

	    str = debug.traceback(str, 2)
        __print(str)
    end
end


--打印table(排除cls)
local function __print_t(...)

    local str = ""
    local arg = {...}
    local len = #arg
    if len > 0 then
	    local v
	    for i=1, len  do
		    v = arg[i]
		    if v == nil then
			    str = str .. "nil  "
		    elseif type(v) == "table" then
			    local s
			    --if true then
			    if __logDebug then
				    s = t2str(v, 4, true)   --默认扫描3层
			    else
				    --release不打印表
				    s = tostring(v)
			    end

                str = str .. "\n" .. s .. "\n"
            else
                str = str .. tostring(v) .. "  "
            end
        end
    end

	--if __logTime then
	--	--记录时间
	--	str = format("[%s]%s", os.date(), str)
	--end

	if __logDebug then
		str = debug.traceback(str, 2)
	end
    __print(str)
end

--打印table
local function __print_r(...)

    local str = ""
    local arg = {...}
    local len = #arg
    if len > 0 then
	    local v
        for i=1, len  do
	        v = arg[i]
	        if v == nil then
		        str = str .. "nil  "
	        elseif type(v) == "table" then
                local s = t2str(v, 4)   --默认扫描3层
                str = str .. "\n" .. s .. "\n"
            else
                str = str .. tostring(v) .. "  "
            end
        end
    end

	--if __logTime then
	--	--记录时间
	--	str = format("[%s]%s", os.date(), str)
	--end

    __print(str)
end

--打印debug
function log.debug(strFormat, ...)
    __logInDetail(G_LOG_LV.DEBUG, strFormat, ...)
end

--打印信息
function log.info(strFormat, ...)
    __logInDetail(G_LOG_LV.INFO, strFormat, ...)
end

--打印警告
function log.warn(strFormat, ...)
    __logInDetail(G_LOG_LV.WARN, strFormat, ...)
end

--打印错误
function log.error(strFormat, ...)
    __logInDetail(G_LOG_LV.ERROR, strFormat, ...)
end

--打印
function log.print(...)
    __print(...)
end

--打印table(排除cls)
function log.print_t(...)
    __print_t(...)
end

--打印table
function log.print_r(...)
    __print_r(...)
end

--打印upvalue
--@tbl_dep 扫描table的层数
--@level 栈级别
function log.print_upv(dep, level)
    dep = dep or 3  --默认扫描3层
    level = 3 + (level or 0)   --第3层才是调用层
    local t = upv2t(level)
    local str = t2str(t, dep, true) 
    __print("[upv] "..str)
end

--//-------~★~-------~★~-------~★~模块日志~★~-------~★~-------~★~-------//

local __logAllMod = false   --打印所有mod

local __mod2logout = {}
local __mod2logoff = {}
local __mod2log = {}

--添加mod日志开关
--@mod2on 如果all是off， 只输出这个里面的mod
--@mod2off 如果all是on， 只屏蔽这个里面的mod
function log.add_log_mod(mod2on, mod2off)

	if mod2on then
		TableUtil.merge_const(__mod2logout, mod2on)
	end
	if mod2off then
		TableUtil.merge_const(__mod2logoff, mod2off)
	end

	__logAllMod = __mod2logout["all"] ~= nil
end

local function canLog(mod)
	if __logAllMod then
		return not __mod2logoff[mod]
	end
    return __mod2logout[mod]
end

local __logModFun = false
local __logModName = false

local function mod_log(...)
    __logModFun(__logModName, ...)
end

local __modMt =
{
	debug = log.debug,
	info = log.info,
	warn = log.warn,
	error = log.error,
	print = log.print,
	print_t = log.print_t,
	print_r = log.print_r,
	print_upv = log.print_upv,
}

local function empty_log()

end

function log.get_nslog(nsname, modname)

    if modname then
        modname = nsname .. ">" .. modname
    else
        modname = nsname
    end

    local mt = {print_upv = __modMt.print_upv}
    setmetatable(mt, {
        __index = function(self, k)
            if canLog(nsname) then
                __logModFun = __modMt[k]
                __logModName = "[" .. modname .. "]"
                return mod_log
            else
                return empty_log
            end
        end,
        __newindex = function(self, k, v)
            assert(false, "do not write nslog")
        end,
    })
    return mt
end

local g_ns = g_ns
g_ns["log"] = log
g_ns["nslog"] = log.get_nslog(g_ns.__nsname__) --为根空间添加日志

return log