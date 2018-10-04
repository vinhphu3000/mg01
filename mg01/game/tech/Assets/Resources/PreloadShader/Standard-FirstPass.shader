Shader "Nature/Terrain/Standard" {
	Properties {
		// set by terrain engine
		[HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
		[HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
		[HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
		[HideInInspector] [Gamma] _Metallic0 ("Metallic 0", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic2 ("Metallic 2", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic3 ("Metallic 3", Range(0.0, 1.0)) = 0.0
		[HideInInspector] _Smoothness0 ("Smoothness 0", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness2 ("Smoothness 2", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness3 ("Smoothness 3", Range(0.0, 1.0)) = 1.0

		// used in fallback on old cards & base map
		[HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
		[HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader{
			Tags{
			"SplatCount" = "4"
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
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
		#pragma only_renderers glcore gles gles3 metal d3d9 
		#pragma multi_compile_fog
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

		fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap);
		half3 bakedColor = DecodeLightmap(bakedColorTex);
		fixed4 c = fixed4(1.0, 1.0, 1.0, 1.0);
		c.rgb = col *bakedColor;
		UNITY_APPLY_FOG(IN.fogCoord, c);
		return c;
		}
			ENDCG
		}
		}

	Dependency "AddPassShader" = "Legacy Shaders/Diffuse"
	Dependency "BaseMapShader" = "Legacy Shaders/Diffuse"

	Fallback "Nature/Terrain/Diffuse"
}
