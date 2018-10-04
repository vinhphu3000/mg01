Shader "A1/Self-Illumin/Non Ambient/Diffuse Flow" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0

	_FlowMap ("FalowMap", 2D) = "white" {}
	_FlowSpeed("Flow Speed",float) = 1.0
	_FlowColor("Flow Color",Color) = (1,1,1,1)
}
SubShader {
	Tags { "RenderType"="Opaque" "Complexity"="4" }
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert noambient
#pragma only_renderers glcore gles gles3 metal d3d9
sampler2D _MainTex;
sampler2D _FlowMap;
float _FlowSpeed;
fixed3 _FlowColor;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
	float2 uv_FlowMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c2 = tex2D(_FlowMap, IN.uv_FlowMap + _Time.xx * _FlowSpeed);
	fixed4 c = tex * _Color;
	//o.Albedo = c.rgb + c2.rgb * tex.a;		// 没有实时光，关闭环境光后没有任何意义
	o.Emission = c.rgb * _Color.a + c2.rgb * tex.a * _FlowColor;
	//o.Alpha = 1.0;
}
ENDCG
} 

Fallback "A1/Self-Illumin/Diffuse"
}
