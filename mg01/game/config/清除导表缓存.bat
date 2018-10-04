
pushd "%~dp0"

cd xls2lua

rmdir /S /Q tmp\\lua_data_enum_c
rmdir /S /Q tmp\\lua_data_enum_s
rmdir /S /Q tmp\\lua_data_raw_c
rmdir /S /Q tmp\\lua_data_raw_s

del tmp\\record_xls.json

popd

pause