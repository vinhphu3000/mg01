// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/TerrainEngine/Details/WavingDoublePass2" {
	Properties {
		_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
		_Cutoff ("Cutoff", float) = 0.5
	}

	SubShader {
		Tags {
			"Queue" = "Geometry+200"
			"IgnoreProjector"="True"
			"RenderType"="Grass"
		}
		Cull Off
		LOD 200
		ColorMask RGB
			
		CGPROGRAM
			#pragma surface surf Lambert vertex:WavingGrassVert1 addshadow
			#pragma target 3.0
			#pragma only_renderers d3d9
			
			#include "TerrainEngine.cginc"

			sampler2D _MainTex;
			sampler2D _TimeMap;
			sampler2D _ForceMap;
			fixed _Cutoff;

			fixed4 TerrainWaveGrass1 (inout float4 vertex, float waveAmount, fixed4 color)
			{
				float4 _waveXSize = float4(0.012, 0.02, 0.06, 0.024) * _WaveAndDistance.y;
				float4 _waveZSize = float4 (0.006, .02, 0.02, 0.05) * _WaveAndDistance.y;
				float4 waveSpeed = float4 (0.3, .5, .4, 1.2) * 4;

				float4 _waveXmove = float4(0.012, 0.02, -0.06, 0.048) * 2;
				float4 _waveZmove = float4 (0.006, .02, -0.02, 0.1);

				float4 waves;
				waves = vertex.x * _waveXSize;
				waves += vertex.z * _waveZSize;

				// Add in time to model them over time
				waves += _WaveAndDistance.x * waveSpeed;

				float4 s, c;
				waves = frac (waves);
				FastSinCos (waves, s,c);

				s = s * s;
				
				s = s * s;

				float lighting = dot (s, normalize (float4 (1,1,.4,.2))) * .7;

				s = s * waveAmount;

				float3 waveMove = float3 (0,0,0);
				waveMove.x = dot (s, _waveXmove);
				waveMove.z = dot (s, _waveZmove);

				vertex.xz -= waveMove.xz * 0.06;
				
				// apply color animation
				fixed3 waveColor = lerp (fixed3(0.5,0.5,0.5), _WavingTint.rgb, lighting);
				
				// Fade the grass out before detail distance.
				// Saturate because Radeon HD drivers on OS X 10.4.10 don't saturate vertex colors properly.
				float3 offset = vertex.xyz - _CameraPosition.xyz;
				color.a = saturate (2 * (_WaveAndDistance.w - dot (offset, offset)) * _CameraPosition.w);
				
				return fixed4(2 * waveColor * color.rgb, color.a);
			}

			float4 PlayerPos;
			/// x for radius delta,y radius
			float4 CameraInfo;

			void WavingGrassVert1 (inout appdata_full v)
			{
				// MeshGrass v.color.a: 1 on top vertices, 0 on bottom vertices
				// _WaveAndDistance.z == 0 for MeshLit
				float vertexPosFade = v.color.a;
				float waveAmount = v.color.a * _WaveAndDistance.z;

				v.color = TerrainWaveGrass1 (v.vertex, waveAmount, v.color);
				float3 worldPos = mul(unity_ObjectToWorld,v.vertex);

				float3 uv = saturate((worldPos - PlayerPos.xyz)/64.0f + 0.5f);
	
				float forceTime = tex2Dlod(_TimeMap,float4(uv.xz,0,1)).r;
				float4 force = tex2Dlod(_ForceMap,float4(uv.xz,0,1));

				float4 forceInObjSpace = mul(float4((force.xyz - 1.0f)*2.0f,0.0f),unity_ObjectToWorld);
				
				float time_passed = PlayerPos.w - forceTime;
				float3 strong_period = forceInObjSpace.xyz* sin(time_passed * 4)* force*0.8;	//摇晃节奏

				//摇晃衰减
				float strong_decrease = exp(-time_passed * 1.2);
				float3 strong_fac = strong_period * strong_decrease * vertexPosFade;

				v.vertex.xyz += strong_fac;	
			}

			struct Input {
				float2 uv_MainTex;
				fixed4 color : COLOR;
			};

			void surf (Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				#ifndef SHADER_API_D3D11
				// gamma ajust
				c.r = pow(c.r, 2.2);
				c.g = pow(c.g, 2.2);
				c.b = pow(c.b, 2.2);
				#endif

				o.Albedo = c.rgb;
				
				clip (c.a - _Cutoff);
				o.Alpha = 1.0f;
			}
		ENDCG
	}
	
	SubShader {
		Tags {
			"Queue" = "Geometry+200"
			"IgnoreProjector"="True"
			"RenderType"="Grass"
		}
		Cull Off
		LOD 200
		//ColorMask RGB
		
		Pass {
			Tags { "LightMode" = "Vertex" }
			Material {
				Diffuse (1,1,1,1)
				Ambient (1,1,1,1)
			}
			Lighting On
			ColorMaterial AmbientAndDiffuse
			AlphaTest Greater [_Cutoff]
			SetTexture [_MainTex] { combine texture * primary DOUBLE, texture }
		}
		Pass {
			Tags { "LightMode" = "VertexLMRGBM" }
			AlphaTest Greater [_Cutoff]
			BindChannels {
				Bind "Vertex", vertex
				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord", texcoord1 // main uses 1st uv
			}
			SetTexture [unity_Lightmap] {
				matrix [unity_LightmapMatrix]
				combine texture * texture alpha DOUBLE
			}
			SetTexture [_MainTex] { combine texture * previous QUAD, texture }
		}
	}
	
	Fallback Off
}
