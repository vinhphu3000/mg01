@echo off


chcp 936
color 7

rmdir /S /Q tmp\\lua_data_raw_c_0
rmdir /S /Q tmp\\lua_data_cmb_c_0
rmdir /S /Q .\\game_data_client


::set raw_path="\"./tmp/lua_data_raw_c_42/\""
set raw_path=nil
::set combine_path="\"./tmp/lua_data_cmb_c_42/\""
set combine_path=nil
set export_path="\"./game_data_client/\""
::set export_path=nil

.\tools\lua\lua.exe lua/xls2lua.lua {client=true,combine=true,out_path=%raw_path%,combine_path=%combine_path%,export_path=%export_path%}

if ERRORLEVEL 1 ( 
	color 4
    pause
    exit
)


color 2

echo.
echo convert CLIENT SUCCEED!!!!!!!!!!!!!!!!!!!!!!!!!!
pause
