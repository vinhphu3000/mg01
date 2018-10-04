Shader "A1/Transparent/Shadow" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader{
			Tags {"Queue" = "Transparent-1" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 200
		pass {
				Blend SrcAlpha OneMinusSrcAlpha
				Cull Off
				Lighting Off
				ZWrite Off
				Fog { Mode Off }
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
				#pragma only_renderers glcore gles gles3 metal d3d9 

				struct VertexInput {
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
				};

				struct VertexOutput {
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
				};

				sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				VertexOutput vert(VertexInput v) {
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;
				}

				float4 frag(VertexOutput i) : COLOR{
					return tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
				}
			ENDCG
		}
	}
}
