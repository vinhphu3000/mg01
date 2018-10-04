//#define FX_UsingUnityCG

Shader "EJoyShader/Effect/Frame/Effect_BM_UVFrame" {
	Properties {
		_SrcBlend("Source Blend", Int) = 1	// add material parameters - for Blend state
		_DstBlend("Dest Blend", Int) = 1	// add material parameters - for Blend state
		_AlphaRef("Alpha Ref", Range(0.005, 1.0)) = 0.005
		_CullMode("Cull", Int) = 2
		_MainTex ("Base(RGBA)", 2D) = "white" {}
		_SizeX("XNum", Float) = 2
		_SizeY("YNum", Float) = 2
		_Speed("Rate", Float) = 10
		_MaterialColor ("Material_Color", Color) = (1,1,1,1.0)
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }	
		LOD 200

		Pass 
		{
			Tags {"LightMode" = "ForwardBase"}
			Cull [_CullMode]
			Lighting off
			Blend [_SrcBlend] [_DstBlend]	// use parameters
			Zwrite off
		
			CGPROGRAM
//#ifdef PX_TARGET30
//			#pragma target 3.0
//#endif
			#pragma multi_compile_fwdbase
			#pragma vertex Effect_Vert_T_UVFrame_NoVC
			#pragma fragment Effect_Frag_MaterialColor_AbedoDotAlpha_NoVC
			#pragma fragmentoption ARB_precision_hint_fastest 
			
			#include "ProjX.cginc"
					
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
