@pushd %~dp0


::UnityEditor.UI�������·����			d:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\
::UnityEngine.UI�������·����			d:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\GUISystem\Standalone\
::UnityEngine.UI-Editor�������·����	d:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\GUISystem\
::������GUISystem��,��ΪUnityEditor.UI.dll �� Standalone\UnityEditor.UI.dll ��һ��,�ᱨerror CS1704: An assembly with the same name `UnityEngine.UI' has already been imported
::�������
::1:����unity; 
::2:��UnityEditor.UI.dll����Standalone\UnityEditor.UI.dll


::����mdb
pdb2mdb.exe "Output\Editor\UnityEditor.UI.dll"			
pdb2mdb.exe "Output\Standalone\UnityEngine.UI.dll"
pdb2mdb.exe "Output\UnityEngine.UI.dll"					

::������unity��װĿ¼
if exist "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem" (
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.dll" /f /s /q /a
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.mdb" /f /s /q /a
	del "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem\*.pdb" /f /s /q /a
)
md "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem"
::����Ŀ¼����
xcopy /e "Output" "D:\Program Files\Unity554\Editor\Data\UnityExtensions\Unity\GUISystem"


::������������ĿLibraryĿ¼, ��Ȼ��ɾ�ӿ�vs������
xcopy /y "Output\UnityEngine.UI.dll" "D:\work\UGUI_2017_8_8\Library\UnityAssemblies"

::exit
pause