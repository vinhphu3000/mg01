--ResMgr
--@author jr.zeng
--2017年9月29日 下午8:21:09
local modname = "ResMgr"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = CCModule	--父类
_ENV[modname] = class(modname, super)
local ResMgr = _ENV[modname]

--===================module content========================

function ResMgr:__ctor()

end

function ResMgr:__setup()
    
    AssetCache:setup()
	GameObjCache:setup()
	SprAtlasCache:setup()
	LevelMgr:setup()

end

function ResMgr:__clear()

    AssetCache:clear()
	GameObjCache:clear()
	SprAtlasCache:clear()
	LevelMgr:clear()
    
    
end

function ResMgr:setup_event()



end

function ResMgr:clear_event()


end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

return ResMgr