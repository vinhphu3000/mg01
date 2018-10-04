Shader "A1/Self-Illumin/Non Ambient/Outline/Role/Diffuse Rim Flow (Gloss B)" {
Properties
{
	_Color ("Main Color", Color) = (1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0

	_GlossTex ("Gloss (B)", 2D) = "white" {}
	_FlowMap ("FalowMap", 2D) = "white" {}
	_FlowSpeed("Flow Speed",float) = 1.0
	_FlowColor("Flow Color",Color) = (1,1,1,1)

	_LightColor ("Light Color", Color) = (1,1,1,1)
	_LightParams ("Intensity (X) Power (Y)", Vector) = (6,8,0,0)
	_LightDirection ("Direction ( XYZ, Camera Space)", Vector) = (-0.7071068,0,0.7071068,0)

	_MaskColor ("Mask Color", Color) = (1,1,1,1)
	_MaskRange ("Rate (Clamp01((h - X) / Y) * Z + W)", Vector) = (-1,2,0.2,0.8)

	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Float) = 0.005
}
SubShader
{
	Tags { "RenderType"="Opaque" "Complexity"="9" }
	LOD 200
	
	CGPROGRAM
	#pragma surface surf Lambert vertex:vert noambient
	#pragma only_renderers glcore gles gles3 metal d3d9

	sampler2D _MainTex;
	fixed3 _Color;

	sampler2D _GlossTex;
	sampler2D _FlowMap;
	float _FlowSpeed;
	fixed3 _FlowColor;

	fixed3 _LightColor;
	fixed2 _LightParams;
	fixed3 _LightDirection;

	fixed3 _MaskColor;
	fixed4 _MaskRange;

	struct Input
	{
		float2 uv_MainTex;
		float2 uv_FlowMap;
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
		fixed gloss = tex2D(_GlossTex, IN.uv_MainTex).b;
		fixed3 c2 = tex2D(_FlowMap, IN.uv_FlowMap + _Time.xx * _FlowSpeed);
		fixed3 c = tex * _Color;
		//o.Albedo = c.rgb + c2 * tex.a;	// 没有实时光，关闭环境光后没有任何意义

		fixed factor = saturate( dot( normalize( _LightDirection.xyz ), IN.viewNormal.xyz ) );
		fixed3 rimcolor = _LightParams.x * c * _LightColor * pow (factor, _LightParams.y);

		fixed range = saturate( ( IN.viewNormal.w - _MaskRange.x ) / _MaskRange.y );
		fixed rate = range * _MaskRange.z + _MaskRange.w;

		o.Emission = rate * _MaskColor * ( c + rimcolor ) + c2 * gloss * _FlowColor;
	}
	ENDCG

	UsePass "Hidden/A1/Common/OUTLINE"
} 

Fallback "A1/Self-Illumin/Non Ambient/Role/Diffuse Rim Flow (Gloss B)"
}