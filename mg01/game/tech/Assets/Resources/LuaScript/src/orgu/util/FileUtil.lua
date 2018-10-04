-- FileUtil
--@author jr.zeng
--@date 2018/7/16  14:58
local modname = "FileUtil"
--==================global reference=======================


local csFileUtility = mg.org.FileUtility

--===================namespace========================
local ns = 'org'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
FileUtil = FileUtil or {}
local FileUtil = FileUtil

--===================module content========================







--//-------~★~-------~★~-------~★~C#接口~★~-------~★~-------~★~-------//

--合并路径
function FileUtil.combine_path(path1, path2)
	return csFileUtility.CombinePath(path1, path2)
end

--获取目录下的全部文件
function FileUtil.get_files(path)
	return csFileUtility.GetFiles(path)
end

--获取目录下的全部文件夹
function FileUtil.get_directories(path)
	return csFileUtility.GetDirectories(path)
end

--获取文件的最新写入时间(ms)
function FileUtil.get_last_write_time(path)
	return csFileUtility.GetLastWriteTime(path)
end


return FileUtil