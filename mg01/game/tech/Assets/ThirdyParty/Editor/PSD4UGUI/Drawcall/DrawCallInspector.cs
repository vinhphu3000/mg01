using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class DrawCallInspector : EditorWindow
    {
        private static DrawCallInspector _window;

        private int _maxDrawCall;
        private GameObject _panel;
        private Vector2 _scrollPosition;
        private Node _node;//表示面板的抽象数据结构
        private List<Node> _graphicNodeList;

        [MenuItem("PSD4UGUI/检视DrawCall", false, 3)]
        public static void Start()
        {
            _window = EditorWindow.GetWindow<DrawCallInspector>("DrawCall检视器");
            _window.Show();
            _window.Initialize();
        }

        private void Initialize()
        {
        }

        private void OnGUI()
        {
            try
            {
                ShowPanelField();
                ShowIntroduction();
                ShowPanelHierarchy();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void ShowIntroduction()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.TextArea("该工具主要展示UGUI合批原理及合批结果，"
                                + "为优化DrawCall提供一些依据。\n"
                                + "注意：当存在Mask情况下，工具显示的DrawCall"
                                + "数量比实际略少。", GUILayout.Width(400));
            GUILayout.EndHorizontal();
        }

        private void ShowPanelField()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("面板根节点：", GUILayout.Width(80));
            _panel = EditorGUILayout.ObjectField(_panel, typeof(GameObject), true, GUILayout.Width(300)) as GameObject;
            GUILayout.Label("面板DrawCall数量： " + _maxDrawCall.ToString());
            GUILayout.EndHorizontal();
        }

        private void ShowPanelHierarchy()
        {
            if (_panel == null)
            {
                return;
            }
            _node = GetNode(_panel, 0);
            _graphicNodeList = GetGraphicNodeList(_node);
            CalculateNodeStack(_graphicNodeList);
            CalculateNodeDrawCall(_graphicNodeList);
            GUILayout.BeginVertical();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            ShowNode(_node);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private Node GetNode(GameObject go, int nestDepth)
        {
            Node node = new Node();
            node.gameObject = go;
            node.nest = nestDepth;
            if (go.GetComponent<Graphic>() != null)
            {
                if (go.GetComponent<Image>() != null)
                {
                    node.type = NodeType.Image;
                }
                else if (go.GetComponent<Text>() != null)
                {
                    node.type = NodeType.Text;
                }
            }
            int childNestDepth = nestDepth + 1;
            int childCount = go.transform.childCount;
            node.InitChildren(childCount);
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                Node childNode = GetNode(child, childNestDepth);
                childNode.parent = node;
                node.AddChild(childNode);
            }
            return node;
        }

        private List<Node> GetGraphicNodeList(Node node)
        {
            List<Node> graphicNodeList = new List<Node>();
            if (node.gameObject.activeInHierarchy == true)
            {
                node.stack = 0;
                if (node.type == NodeType.Text || node.type == NodeType.Image)
                {
                    graphicNodeList.Add(node);
                }
                for (int i = 0; i < node.GetChildCount(); i++)
                {
                    graphicNodeList.AddRange(GetGraphicNodeList(node.GetChild(i)));
                }
            }
            return graphicNodeList;
        }

        private void CalculateNodeStack(List<Node> graphicNodeList)
        {
            for (int i = 0; i < graphicNodeList.Count; i++)
            {
                Rect nodeRect = graphicNodeList[i].GetWorldRect();
                int stack = 0;
                for (int j = i - 1; j >= 0; j--)
                {
                    Rect beneathNodeRect = graphicNodeList[j].GetWorldRect();
                    if (nodeRect.Overlaps(beneathNodeRect) == true)
                    {
                        if (Node.CanBatch(graphicNodeList[i], graphicNodeList[j]) == true)
                        {
                            stack = Mathf.Max(stack, graphicNodeList[j].stack);
                        }
                        else
                        {
                            stack = Mathf.Max(stack, graphicNodeList[j].stack + 1);
                        }
                    }
                }
                graphicNodeList[i].stack = stack;
            }
        }

        private void CalculateNodeDrawCall(List<Node> graphicNodeList)
        {
            graphicNodeList.Sort(Node.Sort);
            /*
            for(int i = 0; i < graphicNodeList.Count; i++)
            {
                Debug.Log(graphicNodeList[i].gameObject.name + " -- " + graphicNodeList[i].stack, graphicNodeList[i].gameObject);
            }
             */
            int drawCall = 0;
            for (int i = 0; i < graphicNodeList.Count; i++)
            {
                if (graphicNodeList[i].isSetDrawCall == false)
                {
                    drawCall += 1;
                    graphicNodeList[i].drawCall = drawCall;
                    graphicNodeList[i].isSetDrawCall = true;
                    for (int j = i + 1; j < graphicNodeList.Count; j++)
                    {
                        if (graphicNodeList[j].stack == graphicNodeList[i].stack
                            || graphicNodeList[j].stack == (graphicNodeList[i].stack + 1))
                        {
                            if (Node.CanBatch(graphicNodeList[j], graphicNodeList[i]) == true)
                            {
                                graphicNodeList[j].drawCall = graphicNodeList[i].drawCall;
                                graphicNodeList[j].isSetDrawCall = true;
                            }
                        }
                    }
                }
            }
            _maxDrawCall = drawCall;
        }

        private void ShowNode(Node node)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(node.nest * 20);
            Color backgroundColor = GUI.backgroundColor;
            if (node.gameObject.activeInHierarchy)
            {
                if (node.type == NodeType.Image)
                {
                    GUI.backgroundColor = Color.green;
                }
                else if (node.type == NodeType.Text)
                {
                    GUI.backgroundColor = Color.blue;
                }
            }
            bool visible = GUILayout.Toggle(node.gameObject.activeSelf, "", GUILayout.Width(15));
            node.gameObject.SetActive(visible);
            EditorGUILayout.ObjectField(node.gameObject, typeof(GameObject), false, GUILayout.Width(200));
            if (node.gameObject.activeInHierarchy && (node.type == NodeType.Image || node.type == NodeType.Text))
            {
                GUILayout.Label("DrawCall: " + node.drawCall.ToString(), GUILayout.Width(100));
                GUILayout.Label("Stack Value: " + node.stack.ToString(), GUILayout.Width(100));
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.EndHorizontal();
            for (int i = 0; i < node.GetChildCount(); i++)
            {
                Node childNode = node.GetChild(i);
                ShowNode(childNode);
            }
        }
    }

    public class Node
    {
        public GameObject gameObject;
        public Node parent;
        public NodeType type = NodeType.Normal;
        public int nest = 0; //嵌套深度
        public int stack = 0; //可视元素相互交叠深度 
        public int drawCall = 0;//drawCall编号
        public bool isSetDrawCall = false; //标记是否已经计算过drawCall编号

        private List<Node> _children;
        private Rect _worldRect;
        private bool _isWorldRectCalculated;
        private Color _color;
        private bool _isColorInitialized;

        private Material _materialForRendering;

        public Node() { }

        public Rect GetWorldRect()
        {
            if (_isWorldRectCalculated == false)
            {
                _isWorldRectCalculated = true;
                _worldRect = gameObject.GetComponent<RectTransform>().rect;
                Node current = this;
                while (current != null)
                {
                    Transform trans = current.gameObject.transform;
                    _worldRect.x += trans.localPosition.x;
                    _worldRect.y += trans.localPosition.y;
                    current = current.parent;
                }
            }
            return _worldRect;
        }

        public Material MaterialForRendering
        {
            get
            {
                if (_materialForRendering == null)
                {
                    Graphic g = gameObject.GetComponent<Graphic>();
                    if (g != null)
                    {
                        _materialForRendering = g.materialForRendering;
                    }
                }
                return _materialForRendering;
            }
        }

        public void InitChildren(int capacity)
        {
            if (capacity > 0)
            {
                _children = new List<Node>(capacity);
            }
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
        }

        public Node GetChild(int index)
        {
            return _children[index];
        }

        public int GetChildCount()
        {
            if (_children == null)
            {
                return 0;
            }
            return _children.Count;
        }

        public override string ToString()
        {
            return this.gameObject.name;
        }

        public static bool CanBatch(Node a, Node b)
        {
            if (a.type != b.type)
            {
                return false;
            }
            if (a.MaterialForRendering == b.MaterialForRendering)
            {
                return true;
            }
            return false;
        }

        public static int Sort(Node a, Node b)
        {
            if (a.stack > b.stack)
            {
                return 1;
            }
            if (a.stack < b.stack)
            {
                return -1;
            }
            if (a.stack == b.stack)
            {
                if (a.type != b.type)
                {
                    if (a.type == NodeType.Text)
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }

    }

    public enum NodeType
    {
        Normal,
        Image,
        Text
    }

}