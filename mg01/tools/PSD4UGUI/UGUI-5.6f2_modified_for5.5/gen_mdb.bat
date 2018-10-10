@pushd %~dp0


::UnityEditor.UI工程输出路径：			d:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\
::UnityEngine.UI工程输出路径：			d:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\GUISystem\Standalone\
::UnityEngine.UI-Editor工程输出路径：	d:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\GUISystem\
::拷贝到GUISystem后,因为UnityEditor.UI.dll 与 Standalone\UnityEditor.UI.dll 不一致,会报error CS1704: An assembly with the same name `UnityEngine.UI' has already been imported
::解决方法
::1:重启unity; 
::2:用UnityEditor.UI.dll覆盖Standalone\UnityEditor.UI.dll


::生成mdb
pdb2mdb.exe "Output\Editor\UnityEditor.UI.dll"			
pdb2mdb.exe "Output\Standalone\UnityEngine.UI.dll"
pdb2mdb.exe "Output\UnityEngine.UI.dll"					

::拷贝到unity安装目录
if exist "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem" (
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.dll" /f /s /q /a
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.mdb" /f /s /q /a
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.pdb" /f /s /q /a
)
md "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem"
::整个目录覆盖
xcopy /e "Output" "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem"


::拷贝到具体项目Library目录, 不然增删接口vs不更新
xcopy /y "Output\UnityEngine.UI.dll" "D:\work\UGUI_2017_8_8\Library\UnityAssemblies"

::exit
pause