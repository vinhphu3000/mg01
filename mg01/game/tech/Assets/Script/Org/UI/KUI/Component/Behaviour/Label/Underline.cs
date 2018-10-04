using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/Effects/Underline")]
public class Underline : BaseMeshEffect {
    [SerializeField]
    private Color m_underlineColor = Color.white;
    [SerializeField]
    private float m_lineWeight = 2;
    [SerializeField]
    private float m_offset = 1;
    private Text text;
    private RectTransform rectTransform;
    protected override void Awake()
    {
        base.Awake();
        text = gameObject.GetComponent<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        m_lineWeight = Mathf.Max(0, m_lineWeight);
        base.OnValidate();
    }

#endif

    public Color UnderlineColor
    {
        get
        {
            return m_underlineColor;
        }

        set
        {
            m_underlineColor = value;
            if(graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }
    }

    public float UnderlineWeight
    {
        get
        {
            return m_lineWeight;
        }
        set
        {
            if (value <= 0)
            {
                return;
            }
            
            m_lineWeight = value;
            if (graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }
    }

    struct LineInfo
    {
        public float min_x;
        public float max_x;
        public float y;
        public LineInfo(float minX, float maxX, float _y)
        {
            min_x = minX;
            max_x = maxX;
            y = _y;
        }
    }

    static private List<UIVertex> stream = new List<UIVertex>(32);
    static private UIVertex[] tempVerts = new UIVertex[4];
    static private List<LineInfo> lines = new List<LineInfo>(4);
    public override void ModifyMesh(VertexHelper toFill)
    {
        if(!IsActive())
        {
            return;
        }
        if(toFill.currentVertCount == 0)
        {
            return;
        }
        TextGenerator ulGen = new TextGenerator();
        ulGen.Populate("█", text.GetGenerationSettings(rectTransform.rect.size));
        UIVertex[] ulVerts = ulGen.GetVerticesArray();
        float unitsPerPixel = 1 / text.pixelsPerUnit;
        var uv_x = (ulVerts[0].uv0.x + ulVerts[2].uv0.x) / 2;
        //var uv_y = (ulVerts[0].uv0.y + ulVerts[2].uv0.y) / 2;
        Vector2 ulUv = new Vector2(uv_x, ulVerts[0].uv0.y);
        Vector2 ulUv2 = new Vector2(uv_x, ulVerts[2].uv0.y);
        toFill.GetUIVertexStream(stream);
        float lastX = -Mathf.Infinity;
        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minY = Mathf.Infinity;
        float Z = 0;
        lines.Clear();
        for (int i = 0; i < stream.Count; i++)
        {
            UIVertex vert = stream[i];

            Z = vert.position.z;
            if (i % 6 == 0)
            {
                if (vert.position.x < lastX)
                {
                    lines.Add(new LineInfo(minX, maxX, minY));
                    minX = Mathf.Infinity;
                    maxX = -Mathf.Infinity;
                    minY = Mathf.Infinity;
                }
                lastX = vert.position.x;
            }
            if (i % 6 == 4)
            {
                minX = Mathf.Min(minX, vert.position.x);
            }
            if (i % 6 == 2)
            {
                maxX = Mathf.Max(maxX, vert.position.x);
                minY = Mathf.Min(minY, vert.position.y);
            }
        }
        lines.Add(new LineInfo(minX, maxX, minY));

        //convert weight and offset to pixel space
        float halfWeight = m_lineWeight / 2 * unitsPerPixel;
        float offsetInPixel = m_offset * unitsPerPixel;
        for (int i = 0; i < lines.Count; i++)
        {
            LineInfo li = lines[i];

            tempVerts[0] = new UIVertex { uv0 = ulUv, color = m_underlineColor, position = new Vector3(li.min_x, li.y + halfWeight - offsetInPixel, Z) };
            tempVerts[1] = new UIVertex { uv0 = ulUv, color = m_underlineColor, position = new Vector3(li.max_x, li.y + halfWeight - offsetInPixel, Z) };
            tempVerts[2] = new UIVertex { uv0 = ulUv2, color = m_underlineColor, position = new Vector3(li.max_x, li.y - halfWeight - offsetInPixel, Z) };
            tempVerts[3] = new UIVertex { uv0 = ulUv2, color = m_underlineColor, position = new Vector3(li.min_x, li.y - halfWeight - offsetInPixel, Z) };

            toFill.AddUIVertexQuad(tempVerts);
        }
    }
}