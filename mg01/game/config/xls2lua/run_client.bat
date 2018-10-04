SET PYTHONPATH=./tools/Python27/Lib/site-packages
@echo off

color 07
::gbk ±àÂë
chcp 936 

set run_path=%~dp0

rmdir /S /Q tmp\\lua_data_tmp_c
mkdir tmp\\lua_data_tmp_c
rmdir /S /Q game_data_client
mkdir game_data_client

::pushd "%~dp0"
::popd

.\tools\Python27\python.exe python/src/main.py is_client 
::.\tools\Python27\python.exe python/src/main.py is_client no_skip

if ERRORLEVEL 1 (
	color 04 
	pause
)

::utf-8 ±àÂë
chcp 65001
if ERRORLEVEL 1 (
	color 04 
	pause
)


.\tools\lua\lua.exe lua/main.lua {is_client=true,export_tmp=false}
::lua5.1.exe lua/main.lua client


if ERRORLEVEL 1 (
	color 04 
	pause
)


color 02
::chcp 936 

echo.
echo CONVERT SUCCESS!!!!!!!!!!!!!!!!!!!!!!!!!!

::pause