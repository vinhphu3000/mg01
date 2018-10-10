


local require = require

if setfenv then
	--lua5.1时,作兼容
	table.unpack = unpack

	table.pack = function (...)
		return {...}
	end
end

-- seed the random with a strongly varying seed
math.randomseed(os.clock() * 1E11)



--//-------~★~-------~★~-------~★~require~★~-------~★~-------~★~-------//

function require_org(path)
	return require("src.org." .. path)
end

--namespace
--require_org "namespace.lib"

--define
--require_org "define"

--util
require_org "util.lib"

--日志
require_org "log.log"

--pool
require_org "pool.lib"

--ds
require_org "ds.lib"

--ref
require_org "ref.Refer"
require_org "ref.Ref"
require_org "ref.AutoRelease"

--event
require_org "event.Observer"
require_org "event.SubjectEvent"
require_org "event.Subject"
require_org "event.CBoard"


--time
require_org "time.TimerMgr"

--action
require_org "action.lib"

