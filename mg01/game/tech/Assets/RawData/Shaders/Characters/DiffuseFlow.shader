Shader "DiffuseFlow" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_FlowMap("Flow Map",2D) = "Black"{}
	_FlowSpeed("Flow Speed",float) = 1.0 
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert
#pragma only_renderers glcore gles gles3 metal d3d9
#include "UnityCG.cginc"

sampler2D _MainTex;
sampler2D _FlowMap;
fixed4 _Color;
float _FlowSpeed;

struct Input {
	float2 uv_MainTex;
	float2 uv_FlowMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 c2 = tex2D(_FlowMap, IN.uv_FlowMap + _Time.xx * _FlowSpeed);
	o.Albedo = c.rgb + c2.rgb * c.a;
	o.Alpha = 1.0;
}
ENDCG
}

Fallback "VertexLit"
}
