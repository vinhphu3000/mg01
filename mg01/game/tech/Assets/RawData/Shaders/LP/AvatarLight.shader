
Shader "LP/Role/Light" {
Properties {
	_Alpha ("Alpha", Range(0,1.0)) = 1.0
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1.0)
	_MainTex ("Base Texture", 2D) = "white" {}
}

Category {
	Tags {
		"Queue"="AlphaTest+1"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
	}
	Blend SrcAlpha One
	ColorMask RGB
	Cull Off
	Lighting Off 
	ZWrite Off 

	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers glcore gles gles3 metal d3d9 

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			float _Alpha;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				return 2.0f * _TintColor * _Alpha * tex2D(_MainTex, i.texcoord);
			}
			ENDCG 
		}
	}	
}
}