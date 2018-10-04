Shader "A1/Projection Space/Alpha Blended (Color)" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
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

				fixed4 _Color;
				sampler2D _MainTex;

				struct appdata_t {
					float3 vertex : POSITION;
					float3 normal : NORMAL;
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
					o.vertex = float4(v.vertex.x, v.vertex.y, v.normal.x, v.normal.y);
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}

				fixed4 frag (v2f i) : COLOR
				{
					return _Color * tex2D(_MainTex, i.texcoord);
				}
				ENDCG
			}
		}
	} 
}