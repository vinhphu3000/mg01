Shader "A1/Self-Illumin/Non Ambient/Diffuse Rim Two Sided" {
Properties
{
	_Color ("Main Color", Color) = (1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0

	_LightColor ("Light Color", Color) = (1,1,1,1)
	_LightParams ("Intensity (X) Power (Y)", Vector) = (6,8,0,0)
	_LightDirection ("Direction ( XYZ, Camera Space)", Vector) = (-0.7071068,0,0.7071068,0)		// 待优化，如果能保证是normalize的
}
SubShader
{
	Tags { "RenderType"="Opaque" "Complexity"="4" }
	LOD 200

	CGPROGRAM
	#pragma surface surf Lambert vertex:vert noambient
	#pragma only_renderers glcore gles gles3 metal d3d9

	sampler2D _MainTex;
	fixed3 _Color;

	fixed3 _LightColor;
	fixed2 _LightParams;
	fixed3 _LightDirection;

	struct Input
	{
		float2 uv_MainTex;
		float4 viewNormal;
	};

	void vert (inout appdata_full v, out Input o)
	{
		o = (Input)0;
		o.viewNormal = float4( mul( UNITY_MATRIX_IT_MV, float4( v.normal, 1 ) ).xyz, -v.vertex.x );
	}

	void surf (Input IN, inout SurfaceOutput o)
	{
		fixed3 tex = tex2D(_MainTex, IN.uv_MainTex);
		fixed3 c = tex * _Color;
		//o.Albedo = c;

		fixed factor = 1 - saturate( dot( normalize( _LightDirection.xyz ), IN.viewNormal.xyz ) );
		fixed3 rimcolor = _LightParams.x * c * _LightColor * pow (factor, _LightParams.y);

		o.Emission = c + rimcolor;
	}
	ENDCG
} 

Fallback "A1/Self-Illumin/Diffuse"
}