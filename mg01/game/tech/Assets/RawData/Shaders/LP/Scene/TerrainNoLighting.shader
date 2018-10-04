Shader "LP/Scene/TerrainNoLighting" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Control("Control (RGBA)", 2D) = "red" {}
		_Splat3("Layer 3 (A)", 2D) = "white" {}
		_Splat2("Layer 2 (B)", 2D) = "white" {}
		_Splat1("Layer 1 (G)", 2D) = "white" {}
		_Splat0("Layer 0 (R)", 2D) = "white" {}
		_Color ("颜色", Color) = (1,1,1,1)
	}

	SubShader{
		Tags{
			"SplatCount" = "4"
			"Queue" = "Geometry-100"
			"RenderType"="Opaque"
			"IgnoreProjector" = "true"
		}
		ZWrite On
		ZTest Less
		ColorMask RGB

		Pass{
		CGPROGRAM

		// compile directives
		#pragma vertex vert_surf
		#pragma fragment frag_surf
		#pragma multi_compile_fog
		#pragma only_renderers glcore gles gles3 metal d3d9 
		#include "UnityCG.cginc"

		sampler2D _Control;
		sampler2D _Splat0,_Splat1, _Splat2, _Splat3;
		sampler2D _MainTex;
		float4 _Color;

		struct app_data_ {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
        };

		struct v2f_surf {
			float4 pos : SV_POSITION;
			float4 pack0 : TEXCOORD0; // _Control _Splat0
			float4 pack1 : TEXCOORD1; // _Splat1 _Splat2
			float4 pack2 : TEXCOORD2; // _Splat3 _Splat4
			float4 lmap : TEXCOORD3;
			UNITY_FOG_COORDS(4)
		};

		float4 _Control_ST;
		float4 _Splat0_ST;
		float4 _Splat1_ST;
		float4 _Splat2_ST;
		float4 _Splat3_ST;
		// vertex shader
		v2f_surf vert_surf(app_data_ v) {
			v2f_surf o;
			UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.pack0.xy = TRANSFORM_TEX(v.texcoord, _Control);
			o.pack0.zw = TRANSFORM_TEX(v.texcoord, _Splat0);
			o.pack1.xy = TRANSFORM_TEX(v.texcoord, _Splat1);
			o.pack1.zw = TRANSFORM_TEX(v.texcoord, _Splat2);
			o.pack2.xy = TRANSFORM_TEX(v.texcoord, _Splat3);
			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			return o;
		}

		// fragment shader
		fixed4 frag_surf(v2f_surf IN) : SV_Target{
			fixed4 splat_control = tex2D(_Control, IN.pack0.xy);
			fixed4 tmp = tex2D(_Splat0, IN.pack0.zw);
			fixed3 col = splat_control.r * tmp.rgb;
			tmp = tex2D(_Splat1, IN.pack1.xy);
			col += splat_control.g * tmp.rgb;
			tmp = tex2D(_Splat2, IN.pack1.zw);
			col += splat_control.b * tmp.rgb;
			tmp = tex2D(_Splat3, IN.pack2.xy);
			col += splat_control.a * tmp.rgb;
			col = col*_Color.rgb;
			return fixed4(col, 1.0);
		}
		ENDCG
		}
	}
	Fallback "A1/VertexLit"
}
