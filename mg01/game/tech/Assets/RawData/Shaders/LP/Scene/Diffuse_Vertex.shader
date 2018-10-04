Shader "LP/Scene/Diffuse_VertexColor" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert vertex:vert
#pragma only_renderers glcore gles gles3 metal d3d9

sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
	float4 vertexColor;
};

void vert (inout appdata_full v, out Input o)
{
	 UNITY_INITIALIZE_OUTPUT(Input,o);
	 o.vertexColor = v.color;
}

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.vertexColor;
	o.Albedo = c.rgb;
	o.Alpha = 1.0;
}
ENDCG
}

Fallback "A1/VertexLit"
}
