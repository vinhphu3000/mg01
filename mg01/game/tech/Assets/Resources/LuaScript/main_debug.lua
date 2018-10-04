-- main_debug
--@author jr.zeng
--@date 2017/11/17  16:28
local modname = "main_debug"
--==================global reference=======================

local _require = require
require_ = _require

function require(path)
	return _require("src."..path)
end

require "org.namespace.namespace"
require "org.lib"


local safe_call = safe_call
local tostring = tostring
local loadstring = loadstring

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)

forbid_global()

--===================module property========================

local mt = {}

function mt:main()

	local a = 1

	nslog.debug("start")

	mt:test_tracedoc()
end

function mt:test_tracedoc()


end


mt:main()