Shader "Hidden/T1/Terrain/TerrainNull" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "Black" {}
	}
	
	SubShader {
		Tags {
			"Queue" = "Transparent+500"
			"RenderType" = "Transparent"
			"IgnoreProjector"="true"
			"LightMode" = "Vertex"
		}

		Lighting Off
		Cull Front
		
		ZWrite Off
		ZTest Less
		Blend Zero One
		ColorMask 0


		CGPROGRAM
		#pragma surface surf vEmpty
		#pragma only_renderers glcore gles gles3 metal d3d9 

		inline fixed4 LightingvEmpty (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return fixed4(0,0,0,0);
		}


		inline fixed4 LightingvEmpty_PrePass (SurfaceOutput s, half4 light)
		{
			return fixed4(0,0,0,0);
		}

		inline half4 LightingvEmpty_DirLightmap (SurfaceOutput s, fixed4 color, fixed4 scale, bool surfFuncWritesNormal)
		{
			return fixed4(0,0,0,0);
		}

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
		void vert (inout appdata_full v) {
		  v.vertex.xyz = 0.0;
		}
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb;
		}
		ENDCG
	}
	Fallback off
}
