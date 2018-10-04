
robocopy .\xls2lua\game_data_client  ..\tech\Assets\Resources\LuaScript\data\config  /MIR /XD ".svn"
::xcopy anim_proxy ..\..\..\lss\tech\Assets\Resources\data\anim_proxy  /S /D /Y
::copy config.cw  ..\..\..\lss\tech\Assets\StreamingAssets\config.cw


::pause
