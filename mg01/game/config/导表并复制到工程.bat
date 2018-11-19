
pushd "%~dp0"

cd xls2lua
::call run_client.bat
call run_client42.bat
::call 检查图集.bat
popd

chcp 65001

call 同步客户端导表数据到工程.bat

pause
