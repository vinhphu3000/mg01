using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections.Generic;
using Treemap;
using UnityEditor;
using Assets.Editor.Treemap;
using System.Linq;
using UnityEngine.UI;
using System.Text;
using NPOI.OpenXmlFormats.Dml;
using UnityEditor.MemoryProfiler;

namespace MemoryProfilerWindow
{
    public class TreeMapView
    {
        CrawledMemorySnapshot _unpackedCrawl;
        private ZoomArea _ZoomArea;
        private Dictionary<string, Group> _groups = new Dictionary<string, Group>();
        private List<Item> _items = new List<Item>();
        private List<Mesh> _cachedMeshes = new List<Mesh>();
        private Item _selectedItem;
        private Group _selectedGroup;
        private Item _mouseDownItem;
        private Dictionary<int, int> _dirtyObjectsy;
        MemoryProfilerWindow _hostWindow;

        private Vector2 mouseTreemapPosition { get { return _ZoomArea.ViewToDrawingTransformPoint(Event.current.mousePosition); } }

        public void Setup(MemoryProfilerWindow hostWindow, CrawledMemorySnapshot _unpackedCrawl, Dictionary<int, int> dirtyObjectsy)
        {
            this._unpackedCrawl = _unpackedCrawl;
            this._hostWindow = hostWindow;
            _dirtyObjectsy = dirtyObjectsy;
            _ZoomArea = new ZoomArea(true)
            {
                vRangeMin = -110f,
                vRangeMax = 110f,
                hRangeMin = -110f,
                hRangeMax = 110f,
                hBaseRangeMin = -110f,
                vBaseRangeMin = -110f,
                hBaseRangeMax = 110f,
                vBaseRangeMax = 110f,
                shownArea = new Rect(-110f, -110f, 220f, 220f)
            };
            RefreshCaches();
            RefreshMesh();
        }

        public void Draw()
        {
            if (_hostWindow == null)
                return;
            
            Rect r = new Rect(0f, 25f, _hostWindow.position.width - _hostWindow._inspector.width, _hostWindow.position.height - 25f);

            _ZoomArea.rect = r;
            _ZoomArea.BeginViewGUI();

            GUI.BeginGroup(r);
            Handles.matrix = _ZoomArea.drawingToViewMatrix;
            HandleMouseClick();
            RenderTreemap();
            GUI.EndGroup();

            _ZoomArea.EndViewGUI();
        }

        private void OnHoveredGroupChanged()
        {
            RefreshCachedRects(false);
        }

        private void HandleMouseClick()
        {
            if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) && Event.current.button == 0)
            {
                if (_ZoomArea.drawRect.Contains(Event.current.mousePosition))
                {
                    Group group = _groups.Values.FirstOrDefault(i => i._position.Contains(mouseTreemapPosition));
                    Item item = _items.FirstOrDefault(i => i._position.Contains(mouseTreemapPosition));

                    if (item != null && _selectedGroup == item._group)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.MouseDown:
                                _mouseDownItem = item;
                                break;

                            case EventType.MouseUp:
                                if (_mouseDownItem == item)
                                {
                                    _hostWindow.SelectThing(item._thingInMemory);
                                    Event.current.Use();
                                }
                                break;
                        }
                    }
                    else if (group != null)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.MouseUp:
                                _hostWindow.SelectGroup(group);
                                Event.current.Use();
                                break;
                        }
                    }
                }
            }
        }

        public void SelectThing(ThingInMemory thing)
        {
            _selectedItem = _items.First(i => i._thingInMemory == thing);
            _selectedGroup = _selectedItem._group;
            RefreshCachedRects(false);
        }

        public void SelectGroup(Group group)
        {
            _selectedItem = null;
            _selectedGroup = group;
            RefreshCachedRects(false);
        }

        void RefreshCaches()
        {
            _items.Clear();
            _groups.Clear();

            foreach (ThingInMemory thingInMemory in _unpackedCrawl.allObjects)
            {
                string groupName = GetGroupName(thingInMemory);
                if (groupName.Length == 0)
                    continue;

                if (!_groups.ContainsKey(groupName))
                {
                    Group newGroup = new Group();
                    newGroup._name = groupName;
                    newGroup._items = new List<Item>();
                    _groups.Add(groupName, newGroup);
                }

                Item item = new Item(thingInMemory, _groups[groupName]);
                _items.Add(item);
                _groups[groupName]._items.Add(item);
            }

            foreach (Group group in _groups.Values)
            {
                group._items.Sort();
            }

            _items.Sort();
            RefreshCachedRects(true);
        }

        private void RefreshCachedRects(bool fullRefresh)
        {
            Rect space = new Rect(-100f, -100f, 200f, 200f);

            if (fullRefresh)
            {
                List<Group> groups = _groups.Values.ToList();
                groups.Sort();
                float[] groupTotalValues = new float[groups.Count];
                for (int i = 0; i < groups.Count; i++)
                {
                    groupTotalValues[i] = groups.ElementAt(i).totalMemorySize;
                }

                Rect[] groupRects = Utility.GetTreemapRects(groupTotalValues, space);
                for (int groupIndex = 0; groupIndex < groupRects.Length; groupIndex++)
                {
                    Group group = groups[groupIndex];
                    group._position = groupRects[groupIndex];
                }
            }

            if (_selectedGroup != null)
            {
                Rect[] rects = Utility.GetTreemapRects(_selectedGroup.memorySizes, _selectedGroup._position);

                for (int i = 0; i < rects.Length; i++)
                {
                    _selectedGroup._items[i]._position = rects[i];
                }
            }

            RefreshMesh();
        }

        public void CleanupMeshes ()
        {
            if (_cachedMeshes == null) {
                _cachedMeshes = new List<Mesh> ();
            }
            else {
                for (int i = 0; i < _cachedMeshes.Count; i++) {
                    UnityEngine.Object.DestroyImmediate (_cachedMeshes [i]);
                }
               
                _cachedMeshes.Clear ();
            }
        }

        private void RefreshMesh()
        {
            CleanupMeshes ();

            const int maxVerts = 32000;
            Vector3[] vertices = new Vector3[maxVerts];
            Color[] colors = new Color[maxVerts];
            int[] triangles = new int[maxVerts * 6 / 4];

            int meshItemIndex = 0;
            int totalItemIndex = 0;

            List<ITreemapRenderable> visible = new List<ITreemapRenderable>();
            foreach (Group group in _groups.Values)
            {
                if (group != _selectedGroup)
                {
                    visible.Add(group);
                }
                else
                {
                    foreach (Item item in group._items)
                    {
                        visible.Add(item);
                    }
                }
            }

            foreach (ITreemapRenderable item in visible)
            {
                int index = meshItemIndex * 4;
                vertices[index++] = new Vector3(item.GetPosition().xMin, item.GetPosition().yMin, 0f);
                vertices[index++] = new Vector3(item.GetPosition().xMax, item.GetPosition().yMin, 0f);
                vertices[index++] = new Vector3(item.GetPosition().xMax, item.GetPosition().yMax, 0f);
                vertices[index++] = new Vector3(item.GetPosition().xMin, item.GetPosition().yMax, 0f);

                index = meshItemIndex * 4;
                var color = item.GetColor();
                if (item == _selectedItem)
                    color *= 1.5f;

                colors[index++] = color;
                colors[index++] = color * 0.75f;
                colors[index++] = color * 0.5f;
                colors[index++] = color * 0.75f;

                index = meshItemIndex * 6;
                triangles[index++] = meshItemIndex * 4 + 0;
                triangles[index++] = meshItemIndex * 4 + 1;
                triangles[index++] = meshItemIndex * 4 + 3;
                triangles[index++] = meshItemIndex * 4 + 1;
                triangles[index++] = meshItemIndex * 4 + 2;
                triangles[index++] = meshItemIndex * 4 + 3;

                meshItemIndex++;
                totalItemIndex++;

                if (meshItemIndex >= maxVerts / 4 || totalItemIndex == visible.Count)
                {
                    Mesh mesh = new Mesh();
                    mesh.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
                    mesh.vertices = vertices;
                    mesh.triangles = triangles;
                    mesh.colors = colors;
                    _cachedMeshes.Add(mesh);

                    vertices = new Vector3[maxVerts];
                    colors = new Color[maxVerts];
                    triangles = new int[maxVerts * 6 / 4];
                    meshItemIndex = 0;
                }
            }
        }

        public void RenderTreemap()
        {
            if (_cachedMeshes == null)
                return;

            Material mat = (Material)EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
            mat.SetPass(0);

            for (int i = 0; i < _cachedMeshes.Count; i++)
            {
                Graphics.DrawMeshNow(_cachedMeshes[i], Handles.matrix);
            }
            RenderLabels();
        }

        private void RenderLabels()
        {
            if (_groups == null)
                return;

            GUI.color = Color.black;
            Matrix4x4 mat = _ZoomArea.drawingToViewMatrix;

            foreach (Group group in _groups.Values)
            {
                if (Utility.IsInside(group._position, _ZoomArea.shownArea))
                {
                    if (_selectedItem != null && _selectedItem._group == group)
                    {
                        RenderGroupItems(group);
                    }
                    else
                    {
                        RenderGroupLabel(group);
                    }
                }
            }

            GUI.color = Color.white;
        }
        private void RenderGroupLabel(Group group)
        {
            Matrix4x4 mat = _ZoomArea.drawingToViewMatrix;

            Vector3 p1 = mat.MultiplyPoint(new Vector3(group._position.xMin, group._position.yMin));
            Vector3 p2 = mat.MultiplyPoint(new Vector3(group._position.xMax, group._position.yMax));

            if (p2.x - p1.x > 30f)
            {
                Rect rect = new Rect(p1.x, p2.y, p2.x - p1.x, p1.y - p2.y);
                GUI.Label(rect, group.GetLabel());
            }
        }

        private void RenderGroupItems(Group group)
        {
            Matrix4x4 mat = _ZoomArea.drawingToViewMatrix;

            foreach (Item item in group._items)
            {
                if (Utility.IsInside(item._position, _ZoomArea.shownArea))
                {
                    Vector3 p1 = mat.MultiplyPoint(new Vector3(item._position.xMin, item._position.yMin));
                    Vector3 p2 = mat.MultiplyPoint(new Vector3(item._position.xMax, item._position.yMax));

                    if (p2.x - p1.x > 30f)
                    {
                        Rect rect = new Rect(p1.x, p2.y, p2.x - p1.x, p1.y - p2.y);
                        string row1 = item._group._name;
                        string row2 = EditorUtility.FormatBytes(item.memorySize);
                        GUI.Label(rect, row1 + "\n" + row2);
                    }
                }
            }
        }

        public string GetGroupName(ThingInMemory thing)
        {
			if (thing is NativeUnityEngineObject)
            {
                if (_dirtyObjectsy != null && _dirtyObjectsy.ContainsKey((thing as NativeUnityEngineObject).instanceID))
                    return "";
                return (thing as NativeUnityEngineObject).className ?? "MissingName";
            }
				
            if (thing is ManagedObject)
                return (thing as ManagedObject).typeDescription.name;
            return thing.GetType().Name;
        }

        public string DumpLable()
        {
            var l = new List<Group>();
            var str = new StringBuilder();
            foreach (Group group in _groups.Values)
            {
                l.Add(group);
                //str.AppendLine(group._name);
            }
            l.Sort((a, b) =>
            {
                return a._name.CompareTo(b._name);
            });
            foreach(var ll in l)
            {
                str.AppendLine(ll._name  + "\t" + EditorUtility.FormatBytes((long)ll.totalMemorySize));
            }
            return str.ToString();
        }


        public string DumpGroup(string label)
        {
            var g = _groups[label];
            var items = new List<Item>();
            foreach(var item in g._items)
            {
                items.Add(item);
            }
            items.Sort((a, b) =>
            {
                return b.memorySize.CompareTo(a.memorySize);
            });
            var rlt = new StringBuilder();
            foreach(var ll in items)
            {
                rlt.Append("@@@***\n");
                rlt.AppendLine(ll.name + "\t" + EditorUtility.FormatBytes(ll.memorySize));
                rlt.Append(DumpDetailInffo(ll._thingInMemory));
                rlt.Append("<<<<<<<<<<<<<<<<<----------------------------------->>>>>\n\n");
            }
            return rlt.ToString();
        }
        private ThingInMemory[] _shortestPath;
        private ShortestPathToRootFinder _shortestPathToRootFinder;

        public void InitDump()
        {
            _shortestPathToRootFinder = new ShortestPathToRootFinder(_unpackedCrawl);
        }

        public string DumpDetailInffo(ThingInMemory _selectedThing)
        {
            _shortestPath = _shortestPathToRootFinder.FindFor(_selectedThing);
            var str = new StringBuilder();
            var nativeObject = _selectedThing as NativeUnityEngineObject;
            if (nativeObject != null)
            {
                str.AppendFormat("\t{0}\n", "NativeUnityEngineObject");
                str.AppendFormat("\t\tName:{0}\n", nativeObject.name);
                str.AppendFormat("\t\tClassName:{0}\n", nativeObject.className);
                str.AppendFormat("\t\tClassID:{0}\n", nativeObject.classID.ToString());
                str.AppendFormat("\t\tInstanceID:{0}\n", nativeObject.instanceID.ToString());
                str.AppendFormat("\t\tIsDontDestroyOnLoad:{0}\n", nativeObject.isDontDestroyOnLoad.ToString());
                str.AppendFormat("\t\tIsPersistent:{0}\n", nativeObject.isPersistent.ToString());
                str.AppendFormat("\t\tIsManager:{0}\n", nativeObject.isManager.ToString());
                str.AppendFormat("\t\tHideFlags:{0}\n", nativeObject.hideFlags.ToString());
                str.AppendFormat("\t\tSize:{0}\n", nativeObject.size.ToString());
            }

            var managedObject = _selectedThing as ManagedObject;
            if (managedObject != null)
            {
                str.AppendLine("\tManagedObject");
                str.AppendFormat("\t\tType:{0}\n", managedObject.typeDescription.name);
                str.AppendFormat("\t\tAddress:{0}\n", managedObject.address.ToString("X"));
                str.AppendFormat("\t\tsize:{0}\n", managedObject.size.ToString());

                if (managedObject.typeDescription.name == "System.String")
                    str.AppendFormat("\t\tvalue{0}\n", StringTools.ReadString(_unpackedCrawl.managedHeap.Find(managedObject.address, _unpackedCrawl.virtualMachineInformation), _unpackedCrawl.virtualMachineInformation));
                str.Append(DumpFields(managedObject));

                //if (managedObject.typeDescription.isArray)
                //{
                //    DrawArray(managedObject);
                //}
            }

            if (_selectedThing is GCHandle)
            {
                str.AppendLine("\tGCHandle");
                str.AppendFormat("\t\tsize{0}\n", _selectedThing.size.ToString());
            }

            var staticFields = _selectedThing as StaticFields;
            if (staticFields != null)
            {
                str.AppendLine("\tStatic Fields");
                str.AppendFormat("\t\tOf type: {0}\n", staticFields.typeDescription.name);
                str.AppendFormat("\t\tsize: {0}\n", staticFields.size);

                str.Append(DumpFields(staticFields.typeDescription,
                        new BytesAndOffset() {
                            bytes = staticFields.typeDescription.staticFieldBytes,
                            offset = 0,
                            pointerSize = _unpackedCrawl.virtualMachineInformation.pointerSize
                        },
                        true));
            }

            if (managedObject == null)
            {
                str.AppendLine("\t***References:");
                str.Append(DumpLinks(_selectedThing.references));
            }

            str.AppendLine("\t***Referenced by:");
            str.Append(DumpLinks(_selectedThing.referencedBy));

            GUILayout.Space(10);
            if (_shortestPath != null)
            {
                if (_shortestPath.Length > 1)
                {
                    str.AppendLine("\t***ShortestPathToRoot");
                    str.Append(DumpLinks(_shortestPath));
                }
                string reason;
                _shortestPathToRootFinder.IsRoot(_shortestPath.Last(), out reason);
                str.AppendLine("\tThis is a root because:" + reason);
            }
            else
            {
                str.AppendLine("\tNo root is keeping this object alive. It will be collected next UnloadUnusedAssets() or scene load");
            }
            return str.ToString();
        }

        private string DumpLinks(IEnumerable<ThingInMemory> thingInMemories)
        {
            var str = new StringBuilder();
            foreach (var rb in thingInMemories)
            {
                var caption = rb == null ? "null" : rb.caption;

                var managedObject = rb as ManagedObject;
                if (managedObject != null && managedObject.typeDescription.name == "System.String")
                    caption = StringTools.ReadString(_unpackedCrawl.managedHeap.Find(managedObject.address, _unpackedCrawl.virtualMachineInformation), _unpackedCrawl.virtualMachineInformation);
                str.AppendLine("\t\t" + caption);
            }
            return str.ToString();
        }

        private string DumpFields(TypeDescription typeDescription, BytesAndOffset bytesAndOffset, bool useStatics = false)
        {
            var str = new StringBuilder();
            foreach (var field in TypeTools.AllFieldsOf(typeDescription, _unpackedCrawl.typeDescriptions, useStatics ? TypeTools.FieldFindOptions.OnlyStatic : TypeTools.FieldFindOptions.OnlyInstance))
            {
                str.AppendFormat("\t\t\t{0}\n", field.name);
                // str.Append(DumpValueFor(field, bytesAndOffset.Add(field.offset)));
            }
            return str.ToString();
        }

        private string DumpFields(ManagedObject managedObject)
        {
            var str = new StringBuilder();
            if (managedObject.typeDescription.isArray)
                return "";
            str.AppendLine("\t\tFields:");
            str.Append(DumpFields(managedObject.typeDescription, _unpackedCrawl.managedHeap.Find(managedObject.address, _unpackedCrawl.virtualMachineInformation)));
            return str.ToString();
        }


        //private void DrawValueFor(FieldDescription field, BytesAndOffset bytesAndOffset)
        //{
        //    var typeDescription = _unpackedCrawl.typeDescriptions[field.typeIndex];

        //    switch (typeDescription.name)
        //    {
        //        case "System.Int32":
        //            GUILayout.Label(_primitiveValueReader.ReadInt32(bytesAndOffset).ToString());
        //            break;
        //        case "System.Int64":
        //            GUILayout.Label(_primitiveValueReader.ReadInt64(bytesAndOffset).ToString());
        //            break;
        //        case "System.UInt32":
        //            GUILayout.Label(_primitiveValueReader.ReadUInt32(bytesAndOffset).ToString());
        //            break;
        //        case "System.UInt64":
        //            GUILayout.Label(_primitiveValueReader.ReadUInt64(bytesAndOffset).ToString());
        //            break;
        //        case "System.Int16":
        //            GUILayout.Label(_primitiveValueReader.ReadInt16(bytesAndOffset).ToString());
        //            break;
        //        case "System.UInt16":
        //            GUILayout.Label(_primitiveValueReader.ReadUInt16(bytesAndOffset).ToString());
        //            break;
        //        case "System.Byte":
        //            GUILayout.Label(_primitiveValueReader.ReadByte(bytesAndOffset).ToString());
        //            break;
        //        case "System.SByte":
        //            GUILayout.Label(_primitiveValueReader.ReadSByte(bytesAndOffset).ToString());
        //            break;
        //        case "System.Char":
        //            GUILayout.Label(_primitiveValueReader.ReadChar(bytesAndOffset).ToString());
        //            break;
        //        case "System.Boolean":
        //            GUILayout.Label(_primitiveValueReader.ReadBool(bytesAndOffset).ToString());
        //            break;
        //        case "System.Single":
        //            GUILayout.Label(_primitiveValueReader.ReadSingle(bytesAndOffset).ToString());
        //            break;
        //        case "System.Double":
        //            GUILayout.Label(_primitiveValueReader.ReadDouble(bytesAndOffset).ToString());
        //            break;
        //        case "System.IntPtr":
        //            GUILayout.Label(_primitiveValueReader.ReadPointer(bytesAndOffset).ToString("X"));
        //            break;
        //        default:

        //            if (!typeDescription.isValueType)
        //            {
        //                ThingInMemory item = GetThingAt(bytesAndOffset.ReadPointer());
        //                if (item == null)
        //                {
        //                    EditorGUI.BeginDisabledGroup(true);
        //                    GUILayout.Button("Null");
        //                    EditorGUI.EndDisabledGroup();
        //                }
        //                else
        //                {
        //                    DrawLinks(new ThingInMemory[] { item });
        //                }
        //            }
        //            else
        //            {
        //                DrawFields(typeDescription, bytesAndOffset);
        //            }
        //            break;
        //    }
        //}
    }
}
