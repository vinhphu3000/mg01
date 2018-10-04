

local require = require



--//-------~★~-------~★~-------~★~require~★~-------~★~-------~★~-------//

function require_orgu(path)
	return require("src.orgu." .. path)
end

--const
require_orgu "const"
require_orgu "const.CType"
--
require_orgu "App"

--util
require_orgu "util.lib"

--module
require_orgu "module.CCModule"

--coroutine
require_orgu "coroutine.coroutine_mgr"

--event
require_orgu "event.EvtCenter"

--res
require_orgu "res.lib"

--sound
require_orgu "sound.SoundMgr"




--display
require_orgu "display.lib"

require_orgu "keyboard.KeyCode"
require_orgu "keyboard.Keyboard"

require_orgu "action.lib"
