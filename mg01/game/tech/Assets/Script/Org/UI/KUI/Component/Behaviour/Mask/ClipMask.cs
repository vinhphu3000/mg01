using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipMask : UnityEngine.EventSystems.UIBehaviour {
    private bool inited = false;
    private Vector4 clipRange;
    private List<Material> ClipMaterials;

	// Use this for initialization
	protected override void Awake()
    {
        base.Awake();
        clipRange = new Vector4();
        ClipMaterials = new List<Material>();
        UpdateClipRect();
        inited = true;
	}

    protected override void OnEnable()
    {
        base.OnEnable();
        ClipMaterials.Clear();
        UpdateClipRect();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ClipMaterials.Clear();
        inited = false;
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (!inited || !IsActive()) return;
        UpdateClipRect();
        UpdateMaterials();
    }


    void UpdateClipRect()
    {
        Vector3[] corners = new Vector3[4];
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.GetWorldCorners(corners);
        clipRange[0] = corners[0].x;//minx
        clipRange[1] = corners[0].y;//miny
        clipRange[2] = corners[2].x;//maxx
        clipRange[3] = corners[2].y;//maxy
    }

    void UpdateMaterials()
    {
        for (int i = 0; i < ClipMaterials.Count; i++)
        {
            Material m = ClipMaterials[i];
            m.SetVector("_Clip", clipRange);
        }
    }

    public void AddMaterial(Material m)
    {
        if (m == null)
            return;
        if (!ClipMaterials.Contains(m))
            ClipMaterials.Add(m);
        m.SetVector("_Clip", clipRange);
    }

    public void RemoveMaterial(Material m)
    {
        if (ClipMaterials.Contains(m))
            ClipMaterials.Remove(m);
        m.SetVector("_Clip", new Vector4(-10,-10,10,10));
    }

    public void Clear()
    {
        for (int i = ClipMaterials.Count - 1; i >= 0; i--)
        {
            Material m = ClipMaterials[i];
            m.SetVector("_Clip", new Vector4(-10, -10, 10, 10));
        }
        ClipMaterials.Clear();
    }
}
