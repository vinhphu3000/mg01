Shader "A1/VertexLit" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Spec Color", Color) = (1,1,1,1)
	_Emission ("Emissive Color", Color) = (0,0,0,0)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.7
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100

	Cull Back
	Blend Off
	ZWrite On
	ZTest LEqual
	Offset 0, 0
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		
		Material {
			Diffuse [_Color]
			Ambient [_Color]
			Shininess [_Shininess]
			Specular [_SpecColor]
			Emission [_Emission]
		} 
		Lighting On
		SeparateSpecular On
		SetTexture [_MainTex] {
			Combine texture * primary DOUBLE, texture * primary
		} 
	}
	
	// Lightmapped, encoded as dLDR
	Pass {
		Tags { "LightMode" = "VertexLM" }
		
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			constantColor [_Color]
			combine texture * constant
		}
		SetTexture [_MainTex] {
			combine texture * previous DOUBLE, texture * primary
		}
	}
	
	// Lightmapped, encoded as RGBM
	Pass {
		Tags { "LightMode" = "VertexLMRGBM" }
		
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord1", texcoord1 // unused
			Bind "texcoord", texcoord2 // main uses 1st uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture * texture alpha DOUBLE
		}
		SetTexture [unity_Lightmap] {
			constantColor [_Color]
			combine previous * constant
		}
		SetTexture [_MainTex] {
			combine texture * previous QUAD, texture * primary
		}
	}
	
	// Pass to render object as a shadow caster
	Pass {
		Name "ShadowCaster"
		Tags { "LightMode" = "ShadowCaster" }
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_shadowcaster
		#pragma only_renderers glcore gles gles3 metal d3d9
		#include "UnityCG.cginc"

		struct v2f { 
			V2F_SHADOW_CASTER;
		};

		v2f vert( appdata_base v )
		{
			v2f o;
			TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			return o;
		}

		float4 frag( v2f i ) : SV_Target
		{
			SHADOW_CASTER_FRAGMENT(i)
		}
		ENDCG
	}
	
// 	// Pass to render object as a shadow collector
// 	Pass {
// 		Name "ShadowCollector"
// 		Tags { "LightMode" = "ShadowCollector" }
		
// 		Fog {Mode Off}
// 		ZWrite On ZTest LEqual

// CGPROGRAM
// #pragma vertex vert
// #pragma fragment frag
// #pragma multi_compile_shadowcollector 
// #pragma only_renderers glcore gles gles3 metal d3d9

// #define SHADOW_COLLECTOR_PASS
// #include "UnityCG.cginc"

// struct appdata {
// 	float4 vertex : POSITION;
// };

// struct v2f {
// 	V2F_SHADOW_COLLECTOR;
// };

// v2f vert (appdata v)
// {
// 	v2f o;
// 	TRANSFER_SHADOW_COLLECTOR(o)
// 	return o;
// }

// fixed4 frag (v2f i) : COLOR
// {
// 	SHADOW_COLLECTOR_FRAGMENT(i)
// }
// ENDCG

// 	}
}

}
