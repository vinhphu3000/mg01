
require_orgu ("display.kui.UIConst")

--util
require_orgu "display.kui.util.LayoutUtil"

--base
require_orgu ("display.kui.UIHandler")
require_orgu ("display.kui.UIAbs")

--pop
require_orgu ("display.kui.pop.UIPopConst")
require_orgu ("display.kui.pop.UIPop")
require_orgu ("display.kui.pop.UIPopMgr")

--component
require_orgu ("display.kui.component.lib")

--help
local help_arr = require_orgu ("display.kui.help.lib")



local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)


local UIHandler = UIHandler
for k, v in ipairs(help_arr) do
	UIHandler.addHelp(v)
end

