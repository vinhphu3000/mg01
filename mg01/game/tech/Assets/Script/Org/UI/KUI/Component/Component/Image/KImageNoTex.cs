/* ==============================================================================
 * KImage_空白
 * @author jr.zeng
 * 2017/6/19 11:16:37
 * ==============================================================================*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KImageNoTex : KImage
    {

        [SerializeField]
        bool m_needColor = false;   //默认透明


        public KImageNoTex()
        {

        }

        public override void Initialize()
        {
            useLegacyMeshGeneration = false;
            base.Initialize();
        }


        public bool NeedColor
        {
            get { return m_needColor; }
            set {  m_needColor = value; }
        }


        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (m_needColor)
            {
                var r = GetPixelAdjustedRect();
                var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);

                Color32 color32 = color;
                vh.Clear();
                vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(0f, 0f));
                vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(0f, 1f));
                vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(1f, 1f));
                vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(1f, 0f));

                vh.AddTriangle(0, 1, 2);
                vh.AddTriangle(2, 3, 0);
            }
        }

    }

}