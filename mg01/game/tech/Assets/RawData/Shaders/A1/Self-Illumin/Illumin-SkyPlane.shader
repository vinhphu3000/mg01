Shader "A1/Self-Illumin/Sky Plane" {
Properties {
	_MainColor ("Main Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

Category {
	Tags { "RenderType"="Opaque" "Queue"="AlphaTest-1" "ForceNoShadowCasting"="true" "IgnoreProjector"="true" }
	Cull Off Lighting Off SeparateSpecular Off ZWrite Off ZTest Less AlphaTest Off Fog { Mode Off }
	
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers glcore gles gles3 metal d3d9

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _MainColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				return 2.0f * _MainColor * tex2D(_MainTex, i.texcoord);
			}
			ENDCG 
		}
	}	
}
}
