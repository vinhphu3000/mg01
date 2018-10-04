Shader "Hidden/T1/Terrain/Terrain2" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "red" {}
	_Splat1 ("Layer 1 (G)", 2D) = "white" {}
	_Splat0 ("Layer 0 (R)", 2D) = "white" {}
}
	
SubShader {
	Tags {
		"SplatCount" = "2"
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
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
};

sampler2D _Control;
sampler2D _Splat0,_Splat1;
sampler2D _MainTex;

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_control = tex2D (_Control, IN.uv_Control);
	fixed4 tmp;
	fixed3 col;
	tmp = tex2D (_Splat0, IN.uv_Splat0);
	col  = splat_control.r * tmp.rgb;
	tmp = tex2D (_Splat1, IN.uv_Splat1);
	col += splat_control.g * tmp.rgb;
	o.Albedo = col;
}
ENDCG  
}

Fallback "A1/VertexLit"
}
