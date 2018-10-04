// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/T1/Terrain/Projector" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
	_ProjectorParam("center(x,y),1/size",vector) = (0,0,20,0)
	_TerrainInfo("MaxHeight,1/terrainSize",vector) = (453,0.06,1.0,1.0)
}
	
	Category {
	
		Tags {
			"Queue" = "Geometry-50"
			"RenderType" = "Transparent"
			"IgnoreProjector"="true"
		}
		ZWrite off
		Offset -1, -1
		Fog { Mode Off }
		
		ColorMask RGB
		AlphaTest Greater 0
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest LEqual

		BindChannels {
			Bind "Vertex", vertex
		}
		SubShader {

			Pass {
				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma target 3.0
				#pragma only_renderers d3d9
				#pragma vertex vert
				#pragma fragment frag

	

				struct appdata_p {
					float4 vertex : POSITION;			
				};

				struct v2f {
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
				};


				

				float4 _ProjectorParam;
				float4 _TerrainInfo;
				
				sampler2D _HeightMap;

				v2f vert (appdata_p v)
				{

					v2f o = (v2f)0;

					float4 worldPos = mul(unity_ObjectToWorld,v.vertex);
				 
					float2 heightMapUV = worldPos.xz*_TerrainInfo.x;
					worldPos.y = DecodeFloatRGBA(tex2Dlod( _HeightMap, float4( heightMapUV, 0, 0 ) )) * _TerrainInfo.y + 0.02;
					o.pos = mul(UNITY_MATRIX_VP,worldPos);

					o.uv = (worldPos.xz - _ProjectorParam.xy) * _ProjectorParam.z + float2(0.5,0.5);
					return o;
				}

				sampler2D _MainTex;
				float4 _Color;
				half4 frag(v2f IN) : COLOR0
				{
					//return half4(IN.uv,0.0,1.0);
					return tex2D(_MainTex,IN.uv) * _Color;
				}
				ENDCG 
			}
		}
	}

}
