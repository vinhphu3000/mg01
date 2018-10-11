--图集缓存
--@author jr.zeng
--2017年11月2日 下午3:52:18
local modname = "SprAtlasCache"
--==================global reference=======================

local csSprAtlasCache = mg.org.SprAtlasCache.me

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = CCModule	--父类
SprAtlasCache = class(modname, super, _ENV)
local SprAtlasCache = SprAtlasCache

local Refer = Refer

--===================module content========================

function SprAtlasCache:__ctor()
    
end

function SprAtlasCache:__setup()

    self:ctor()

end

function SprAtlasCache:__clear()
    
end


function SprAtlasCache:setup_event()

end

function SprAtlasCache:clear_event()

end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--设置图片(同步加载)
--@refer_ 必须引用资源
--@image_ UI.Image
function SprAtlasCache:setSprite(refer_, image_, url_, spriteName_, nativeSize)

    if nativeSize == nil then
        nativeSize = true
    end

    local referId = Refer.format(refer_)
    csSprAtlasCache:SetSprite(referId, image_,  url_, spriteName_,  nativeSize)
end

--加载图片(异步加载)
--@refer_ 必须引用资源
--@image_ UI.Image
function SprAtlasCache:loadSprite(refer_, image_, url_, spriteName_, nativeSize)

	nativeSize = nativeSize or false

    local referId = Refer.format(refer_)
    csSprAtlasCache:LoadSprite(referId,  image_, url_, spriteName_,  nativeSize)
end

--释放图集的引用
function SprAtlasCache:releaseSprite(url_, refer_)

    local referId = Refer.format(refer_)
    csSprAtlasCache:ReleaseSprite(url_, referId )
end


--卸载图集
function SprAtlasCache:unloadSprite( url_)

    csSprAtlasCache:UnloadSprite(url_)
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


return SprAtlasCache