Shader "A1/Self-Illumin/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert
#pragma only_renderers glcore gles gles3 metal d3d9

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c = tex * _Color;
	o.Albedo = c.rgb;
	o.Emission = c.rgb;
	o.Alpha = c.a;
}
ENDCG
} 
FallBack "A1/Self-Illumin/VertexLit"
}
