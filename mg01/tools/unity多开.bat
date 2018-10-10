set /p dir_name=input a dir_name:
mkdir %dir_name%
cd %dir_name%
mklink /J Assets ..\tech\Assets
mklink /J Library ..\tech\Library
mklink /J ProjectSettings ..\tech\ProjectSettings
echo "success!!!"
pause