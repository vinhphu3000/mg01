
Shader "Hidden/A1/Common" {
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range(0.0005, 0.005)) = 0.005
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			//ZWrite On
			Blend Off
			ZTest Less
			Offset 1, 2.5

			CGINCLUDE
			#include "UnityCG.cginc"
			ENDCG

			CGPROGRAM
			#pragma vertex vert_outline
			#pragma fragment frag_outline
			#pragma only_renderers glcore gles gles3 metal d3d9

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : POSITION;
			};
	
			uniform fixed _Outline;
			uniform fixed4 _OutlineColor;

			v2f vert_outline(appdata v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				fixed3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				fixed2 offset = normalize(TransformViewToProjection(norm.xy));

				//o.pos.z += 0.001;
				o.pos.xy += offset * _Outline * o.pos.w;
				return o;
			}

			half4 frag_outline(v2f i) : COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}
	}
}