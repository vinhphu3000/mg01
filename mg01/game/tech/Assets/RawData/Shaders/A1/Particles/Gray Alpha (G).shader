﻿Shader "A1/Particles/Gray Alpha Blended (G)" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Gray Texture (G)", 2D) = "white" {}
	}

	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off Fog { Mode Off }
	
		SubShader {
			Pass {

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma only_renderers glcore gles gles3 metal d3d9

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed4 _TintColor;

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
					half color = tex2D(_MainTex, i.texcoord).g * 2;
					return i.color * _TintColor * half4(color, color, color, color);
				}
				ENDCG
			}
		}
	}
}