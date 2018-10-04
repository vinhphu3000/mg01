
Shader "MADFINGER/Transparent/GodRays" {

Properties {
	_MainTex ("Base texture", 2D) = "white" {}
	_Multiplier("Multiplier", float) = 1
}

	
SubShader {
	
	
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
	//Blend One One
	Blend One OneMinusSrcColor
	Cull Off Lighting Off ZWrite Off ZTest Off Fog { Color (0,0,0,0) }
	
	LOD 100
	
	CGINCLUDE	
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
	float _Multiplier;
	
	
	struct v2f {
		float4	pos	: SV_POSITION;
		float2	uv		: TEXCOORD0;
		fixed4	color	: TEXCOORD1;
	};

	float4 MyLerp(float4 from,float4 to,float t)
	{
		return from + t * (to - from);
	}
	
	v2f vert (appdata_full v)
	{
		v2f 		o;
		float3	viewPos	= mul(UNITY_MATRIX_MV,v.vertex);
		float		dist		= length(viewPos);
		float4 vpos = v.vertex;			
		o.uv		= v.texcoord.xy;
		o.pos	= mul(UNITY_MATRIX_MVP, vpos);
		o.color	= v.color * _Multiplier;
						
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{			
			return tex2D (_MainTex, i.uv.xy) * i.color;
		}
		ENDCG 
	}	
}


}

