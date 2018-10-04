Shader "A1/Self-Illumin/Non Ambient/Outline/Role/Diffuse Rim MatCap" {
Properties
{
	_Color ("Main Color", Color) = (1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0

	_LightColor ("Light Color", Color) = (1,1,1,1)
	_LightParams ("Intensity (X) Power (Y)", Vector) = (6,8,0,0)
	_LightDirection ("Direction ( XYZ, Camera Space)", Vector) = (-0.7071068,0,0.7071068,0)		// 待优化，如果能保证是normalize的

	_MaskColor ("Mask Color", Color) = (1,1,1,1)
	_MaskRange ("Rate (Clamp01((h - X) / Y) * Z + W)", Vector) = (-1,2,0.2,0.8)					// 最好能保证结果在[0, 1]范围

	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Float) = 0.005

	_MatCap ("MatCap (RGB)", 2D) = "white" {}
}
SubShader
{
	Tags { "RenderType"="Opaque" "Complexity"="9" }
	LOD 200
	ZTest LEqual

	CGPROGRAM
	#pragma surface surf Lambert vertex:vert noambient
	#pragma only_renderers glcore gles gles3 metal d3d9

	sampler2D _MainTex;
	fixed3 _Color;

	fixed3 _LightColor;
	fixed2 _LightParams;
	fixed3 _LightDirection;

	fixed3 _MaskColor;
	fixed4 _MaskRange;

	sampler2D _MatCap;

	struct Input
	{
		float2 uv_MainTex;
		float4 viewNormal;
		float2 n_v;
	};

	void vert (inout appdata_full v, out Input o)
	{
	o = (Input)0;
		o.viewNormal = float4( mul( UNITY_MATRIX_IT_MV, float4( v.normal, 1 ) ).xyz, -v.vertex.x );
		o.n_v = mul((float3x3)UNITY_MATRIX_MV, v.normal).xy * 0.5f + 0.5f;
	}

	void surf (Input IN, inout SurfaceOutput o)
	{
		fixed3 matcapLookup = tex2D(_MatCap, IN.n_v);
		fixed3 tex = (fixed3)tex2D(_MainTex, IN.uv_MainTex);
		fixed3 c = tex * _Color;
		//o.Albedo = c;

		fixed factor = saturate( dot( normalize( _LightDirection.xyz ), IN.viewNormal.xyz ) );
		fixed3 rimcolor = _LightParams.x * c * _LightColor * pow (factor, _LightParams.y);

		fixed range = saturate( ( IN.viewNormal.w - _MaskRange.x ) / _MaskRange.y );
		fixed rate = range * _MaskRange.z + _MaskRange.w;

		o.Emission = rate * _MaskColor * ( c + rimcolor ) * matcapLookup * 1.5;
	}
	ENDCG

	UsePass "Hidden/A1/Common/OUTLINE"
}

Fallback "A1/Self-Illumin/Non Ambient/Outline/Role/Diffuse Rim"
}