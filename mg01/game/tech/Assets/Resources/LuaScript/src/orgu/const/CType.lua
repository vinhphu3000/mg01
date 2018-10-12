--CType
--整合c#的类型及类型名称, 便于直接访问
--@author jr.zeng
--2017年10月20日 上午10:05:43
local modname = "CTypeName"
--==================global reference=======================

local pairs = pairs
local loadstring = loadstring

local function type_tbl(name, ns)
	if ns then
		return loadstring("return " .. ns .. name)()
	end
	return loadstring("return " .. name)()
end

--在C#里的命名空间
local ns_unity = "UnityEngine."
local ns_unity_ui = "UnityEngine.UI."
local ns_org = "mg.org."
local ns_kui = "mg.org.KUI."


CTypeName =
{
	--Unity             GetComponent(不带"Unity.")
	Camera              = "Camera",
	RectTransform       = "RectTransform",

	--Unity.ui          GetComponent(不带"Unity.UI.")
	Selectable          = "Selectable",
	Image               = "Image",
	Graphic             = "Graphic",

	--kui
	KText               = ns_kui.."KText",
	KButton             = ns_kui.."KButton",
	KButtonShrinkable   = ns_kui.."KButtonShrinkable",
	KInput              = ns_kui.."KInputField",
	KScrollView         = ns_kui.."KScrollView",
	KListView           = ns_kui.."KListView",
	KListViewScroll     = ns_kui.."KListViewScroll",
	KToggle             = ns_kui.."KToggle",
	KToggleGroup        = ns_kui.."KToggleGroup",
	KSlider             = ns_kui.."KSlider",
	KProgressBar        = ns_kui.."KProgressBar",
	KImage              = ns_kui.."KImage",

}

local CTypeName = CTypeName

CType = {

	--Unity 获取table时需要带"Unity."
	Camera              = type_tbl(CTypeName.Camera,        ns_unity),
	RectTransform       = type_tbl(CTypeName.RectTransform, ns_unity),

	--Unity.ui  获取table时需要带"Unity.ui"
	Selectable          = type_tbl(CTypeName.Selectable,    ns_unity_ui),
	Image               = type_tbl(CTypeName.Image,         ns_unity_ui),
	Graphic             = type_tbl(CTypeName.Graphic,         ns_unity_ui),


	--mg.org.KUI
	KText               = type_tbl(CTypeName.KText),
	KButton             = type_tbl(CTypeName.KButton),
	KButtonShrinkable   = type_tbl(CTypeName.KButtonShrinkable),
	KInput              = type_tbl(CTypeName.KInput),
	KScrollView         = type_tbl(CTypeName.KScrollView),
	KListView           = type_tbl(CTypeName.KListView),
	KListViewScroll     = type_tbl(CTypeName.KListViewScroll),
	KToggle             = type_tbl(CTypeName.KToggle),
	KToggleGroup        = type_tbl(CTypeName.KToggleGroup),
	KSlider             = type_tbl(CTypeName.KSlider),
	KProgressBar        = type_tbl(CTypeName.KProgressBar),
	KImage              = type_tbl(CTypeName.KImage),

}
