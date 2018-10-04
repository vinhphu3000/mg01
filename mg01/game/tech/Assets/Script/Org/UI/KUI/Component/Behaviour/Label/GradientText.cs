using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/GradientText")]
public class GradientText :  BaseMeshEffect
{
    private const int ONE_TEXT_VERTEX = 6;

    [SerializeField]
    public Color32[] gradientColors;
    [SerializeField]
    public float[] gradientPoses;
    
    readonly UIVertex[] m_TempVerts = new UIVertex[4];

    public override void ModifyMesh (VertexHelper helper)
    {
        if (!IsActive() || helper.currentVertCount == 0)
            return;

        if ((gradientColors.Length != gradientPoses.Length) || gradientColors.Length == 0)
            return;
 
        List<UIVertex> vertList = new List<UIVertex>();
        helper.GetUIVertexStream(vertList);
        helper.Clear();

        List<UIVertex> leftSide = new List<UIVertex>();
        List<UIVertex> rightSide = new List<UIVertex>();

        Color32 topColor = gradientColors[0];
        Color32 bottomColor = gradientColors[gradientColors.Length - 1];

        UIVertex vertLeft = new UIVertex();
        UIVertex vertRight = new UIVertex();

        for (int i = 0; i < vertList.Count; i += 6)
        {
            leftSide.Clear();
            rightSide.Clear();
            
            UIVertex topLeft = vertList[i];
            UIVertex topRight = vertList[i+1];
            UIVertex bottomRight = vertList[i+2];
            UIVertex bottomLeft = vertList[i+4];

            for (int j = 0; j < gradientColors.Length; j++)
            {
                float pos = gradientPoses[j];

                vertLeft.position = Vector3.Lerp(topLeft.position, bottomLeft.position, pos);
                vertLeft.color = gradientColors[j];
                vertLeft.uv0 = Vector2.Lerp(topLeft.uv0, bottomLeft.uv0, pos);
                vertLeft.uv1 = Vector2.Lerp(topLeft.uv1, bottomLeft.uv1, pos);

                vertRight.position = Vector3.Lerp(topRight.position, bottomRight.position, pos);
                vertRight.color = gradientColors[j];
                vertRight.uv0 = Vector2.Lerp(topRight.uv0, bottomRight.uv0, pos);
                vertRight.uv1 = Vector2.Lerp(topRight.uv1, bottomRight.uv1, pos);

                leftSide.Add(vertLeft);
                rightSide.Add(vertRight);
            }

            for (int k = 0; k < leftSide.Count - 1; k++) {
                m_TempVerts[0] = leftSide[k];
                m_TempVerts[1] = rightSide[k];
                m_TempVerts[2] = rightSide[k + 1];
                m_TempVerts[3] = leftSide[k + 1];
                helper.AddUIVertexQuad(m_TempVerts);
            }
        }

    }
}