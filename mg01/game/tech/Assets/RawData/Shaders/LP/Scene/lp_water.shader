// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "LP/Scene/OceanShoreLine" {
	Properties {  
		_WaterTex ("Normal Map (RGB), Foam (A)", 2D) = "white" {}
		_WaterTex2 ("Normal Map (RGB), Foam (B)", 2D) = "white" {}
		_Tiling ("Wave Scale", Range(0.00025, 0.09)) = 0.25
		_WaveSpeed("Wave Speed", Float) = 0.4
		_SpecularRatio ("Specular Ratio", Range(400, 800)) = 200
		_BottomColor("Bottom Color",Color) = (0,0,0,0)
		_TopColor("Top Color",Color) = (0,0,0,0)
		_Alpha("Alpha", Range(0, 1)) = 1
		_LightColor("灯光颜色", Color) = (1, 1, 1, 1)
		_myLightPos("灯光位置", Vector) = (100, 100, 100)
	}   
	                  
	SubShader {    
		LOD 150	  
		Tags {
			"Queue"="Transparent-100"
			 "RenderType"="Transparent" 
			"IgnoreProjector" = "True"
		}    
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{ 
			CGPROGRAM
			#pragma vertex Ocean_Vert
			#pragma fragment Frag
			#pragma only_renderers glcore gles gles3 metal d3d9
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			uniform float _Tiling;
			uniform float _WaveSpeed;
			uniform float _SpecularRatio;

			uniform sampler2D _WaterTex;
			uniform sampler2D _WaterTex2;

			uniform float4x4 _ProjMatrix;
			uniform float4 _LightColor;
			uniform float3 _myLightPos;

			uniform float4 _BottomColor;
			uniform float4 _TopColor;

			uniform float _Alpha;


			struct Ocean_VS_OUT
			{
			    float4 position  : POSITION;
			    float3 worldPos  : TEXCOORD0;
			    float4 tilingAndOffset:TEXCOORD2;
			    float4 color : TEXCOORD3;
				UNITY_FOG_COORDS(4)
			};

			Ocean_VS_OUT Ocean_Vert(appdata_full v)
			{
			    Ocean_VS_OUT o;
			    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			    o.position = mul(UNITY_MATRIX_MVP, v.vertex);
			    o.tilingAndOffset.z =frac( _Time.x * _WaveSpeed);
			    o.tilingAndOffset.xy = o.worldPos.xz*_Tiling;
  				o.tilingAndOffset.w =frac( _Time.z * _WaveSpeed/100);
			    o.color = v.color;
				UNITY_TRANSFER_FOG(o, o.position);
			    return o;
			}

			float4 Ocean_Frag(Ocean_VS_OUT IN)
			{
			    float3 lightColor=_LightColor.xyz*2;
			    float3 worldView = -normalize(IN.worldPos - _WorldSpaceCameraPos);

			    float2 tiling = IN.tilingAndOffset.xy;

			    float4 nmap1 = tex2D(_WaterTex, tiling.yx +float2(IN.tilingAndOffset.z,0));
			    float4 nmap2 = tex2D(_WaterTex2, tiling.yx -float2(IN.tilingAndOffset.w,0));

			    float3 worldNormal  = normalize((nmap1.xyz+nmap2.xyz)*2-2);

			    float dotLightWorldNomal = dot(worldNormal, float3(0,1,0));

			    float3 light = normalize(_myLightPos.xyz);
			    float3 specularReflection = float3(0,0,0) ;

				float dotSpecular = dot(worldNormal,  normalize( worldView+light));
				specularReflection = pow(max(0.0, dotSpecular), _SpecularRatio);


			    float4 col;
			    float fresnel = 0.5*dotLightWorldNomal+0.5;
			    col.rgb = lerp(_BottomColor.xyz, _TopColor.xyz, fresnel);

			    col.rgb+=specularReflection;

			    col.rgb*=lightColor;

			    col.a = _Alpha*IN.color.r;
				UNITY_APPLY_FOG(IN.fogCoord, col);
			    return col;
			}
			

			float4 Frag(Ocean_VS_OUT IN):COLOR 
			{
				float4 col = Ocean_Frag(IN);  
				return col;
			}
			
		ENDCG	
		}  
		
	} 
	FallBack "VertexLit"
}
