--functions
--@author jr.zeng
--2017年10月11日 下午8:08:43
local modname = "functions"
--==================global reference=======================

local Object = Unity.Object

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================


--@now_ 立刻删除
function destroy(unityObj, now_)
    if now_ then
        Object.DestroyImmediate(unityObj)
    else
        Object.Destroy(unityObj)
    end
	return nil
end


--实例化
function instantiate(unityObj)
    
    local obj = Object.Instantiate(unityObj)
    if obj == nil then
        nslog.error("错误的对象", unityObj)    
    end
    return obj
end

--引用
function retain(ref_, refer_)
    if ref_.retain then
        ref_.retain(ref_, refer_)
    end
end

--释放引用
function release(ref_, refer_)
    if ref_.release then
        ref_.release(ref_, refer_)
    end
end