
pushd "%~dp0"

cd xls2lua
call run_client.bat
::call 检查图集.bat
popd

call 同步客户端导表数据到工程.bat

pause
