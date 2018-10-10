@pushd %~dp0


pdb2mdb.exe "Output\Editor\UnityEditor.UI.dll"
pdb2mdb.exe "Output\Standalone\UnityEngine.UI.dll"
pdb2mdb.exe "Output\UnityEngine.UI.dll"

if exist "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem" (
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.dll" /f /s /q /a
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.mdb" /f /s /q /a
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.pdb" /f /s /q /a
)
md "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem"
xcopy /e "Output" "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem"


xcopy /y "Output\UnityEngine.UI.dll" "D:\work\UGUI_2017_8_8\Library\UnityAssemblies"

::要覆盖Standalone才能立即生效
xcopy /y "Output\UnityEngine.UI.dll" "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\Standalone"
xcopy /y "Output\UnityEngine.UI.dll.mdb" "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\Standalone"


::exit
pause