|类名|修改处|作用|涉及的源码方法
|--- |-----|----|---
|Navigation|defaultNav.m_Mode=Mode.Automatic改为Mode.None|关闭按钮的键盘方向键导航|defaultNavigation
|SetPropertyUtility|Set[Float,Bool,Int]方法，并且在若干类中替换SetStruct|解决SetStruct方法中Equals方法引起的BUG(原注释)|类
|Graphic|m_Grey|在子类中修改一个UV坐标值，shader中置灰|ApplyOverlayColorAndGrey(在子类)
|Graphic|alpha|改变color数组的透明通道|独立的property
|Image|m_OverlayColor|实现PSD中的颜色叠加功能|OnPopulateMesh
|Image|m_Rotation|实现图片的旋转|GenerateSimpleSprite
|Image|m_SpritePadding|通过Trim掉部分区域，减小顶点包裹的矩形面积，减少填充率使用,在ImageCreator中被设置|GetDrawDimensions<br>GenerateSlicedSprite
|Image|GenerateSlicedSprite|注释写的是增加了范围判断，一大段算UV坐标的代码|GenerateSlicedSprite
|ImageEditor|以上几个Image新增属性的InspectorGUI|设置变量|OnInspectorGUI
|Text|OnPopulateMesh|支持了置灰m_Grey|OnPopulateMesh
|TextEditor|m_Grey,langId的设置，但是没有看到langId的引用，应该是预留的|OnInspectorGUI
|Selectable|m_Transition从默认的ColorTint设置成None|目测是为了禁用这种按钮状态交互，看不出必要性|m_Transition
|ScrollRect|新增m_NeedCheckCorner|解决初始化的时候mCorners为Vector.zero时任然进行滚动，导致滚动去抽动（绵细加的patch,她应该更清楚|LateUpdate