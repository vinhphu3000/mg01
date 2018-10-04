Shader "A1/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
		Tags { "Queue" = "Geometry" "RenderType"="Opaque" }
		LOD 200
	Pass {
			ColorMask RGB

			CGPROGRAM
			// compile directives
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_fog
			#pragma only_renderers glcore gles gles3 metal d3d9 
			#include "UnityCG.cginc"

			struct app_data_ {
	            float4 vertex : POSITION;
	            float2 texcoord : TEXCOORD0;
	            float2 texcoord1 : TEXCOORD1;
	        };

			//#pragma surface surf Lambert alphatest:_Cutoff
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			fixed4 _Color;
			// no lightmaps:
			struct v2f_surf {
			  float4 pos : SV_POSITION;
			  float2 pack0 : TEXCOORD0; // _MainTex
			  UNITY_FOG_COORDS(1)
			  float2 lmap : TEXCOORD2;
			};
			

			// vertex shader
			v2f_surf vert_surf (app_data_ v) {
			  v2f_surf o;
			  UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
			  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			  o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
			  o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			  UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			  return o;
			}

			// fragment shader
			fixed4 frag_surf (v2f_surf IN) : SV_Target {
				fixed4 basecolor = tex2D(_MainTex, IN.pack0.xy)*_Color;
				fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap);
				half3 bakedColor = DecodeLightmap(bakedColorTex);
				fixed4 c = fixed4(1.0, 1.0, 1.0, 1.0);
				c.rgb = basecolor.rgb *bakedColor;
				c.a = basecolor.a;
				UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
				return c;
			}
			ENDCG

		}
	}
	FallBack "A1/VertexLit"
}