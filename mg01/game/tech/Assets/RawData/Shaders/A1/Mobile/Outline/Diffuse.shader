// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "A1/Mobile/Outline/Diffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Float) = 0.005
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Complexity"="5" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		#pragma only_renderers glcore gles gles3 metal d3d9

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG

		UsePass "Hidden/A1/Common/OUTLINE"
	}

	Fallback "A1/Mobile/Diffuse"
}
