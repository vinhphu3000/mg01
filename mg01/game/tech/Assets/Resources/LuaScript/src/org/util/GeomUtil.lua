--GeomUtil
--@author jr.zeng
--2017年9月8日 下午3:41:12
local modname = "GeomUtil"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
_ENV[modname] = {}
local GeomUtil = _ENV[modname]

--===================module content========================





--//-------∽-★-∽------∽-★-∽--------∽-★-∽角度相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

--将角度调整为0~360度
function GeomUtil.clampAngle360(angle_)
    
    if angle_ >=0 and angle_ <= 360 then
        return angle_;
    end
    while angle_ < 0 do
        angle_ = angle_ + 360;
    end 
    while angle_ >= 360 do
        angle_ = angle_ - 360;
    end 
    return angle_;    
end


--将角度调整为0~-360度
function GeomUtil.clampAngle360r(angle_)

    if angle_ <=0 and angle_ >= -360 then
        return angle_;
    end
    while angle_ > 0 do
        angle_ = angle_ - 360;
    end 
    while angle_ <= -360 do
        angle_ = angle_ + 360;
    end 
    return angle_;    
end

return GeomUtil