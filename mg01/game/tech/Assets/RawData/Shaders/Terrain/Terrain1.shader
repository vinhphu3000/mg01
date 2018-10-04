Shader "Hidden/T1/Terrain/Terrain1" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Splat0 ("Layer 0 (R)", 2D) = "white" {}
}
	
SubShader {
	Tags {
		"SplatCount" = "1"
		"Queue" = "Geometry-100"
		"RenderType" = "Opaque"
		"IgnoreProjector"="true"
	}
	ZWrite On
	ZTest Less
	ColorMask RGB

	CGPROGRAM
	#pragma only_renderers glcore gles gles3 metal d3d9 
	#pragma surface surf Lambert


	struct Input {
		float2 uv_Splat0 : TEXCOORD0;
	};

	sampler2D _Splat0;
	sampler2D _MainTex;

	void surf (Input IN, inout SurfaceOutput o) {
		o.Albedo = tex2D(_Splat0, IN.uv_Splat0).rgb;
	}
	ENDCG  
	}
	Fallback "A1/VertexLit"
}
