// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/TerrainEngine/Details/BillboardWavingDoublePass2" {
	Properties {
		_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
		_Cutoff ("Cutoff", float) = 0.5
	}
	
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "TerrainEngine.cginc"


		sampler2D _TimeMap;
		sampler2D _ForceMap;
		float4 PlayerPos;
		
		struct v2f {
			float4 pos : POSITION;
			fixed4 color : COLOR;
			float4 uv : TEXCOORD0;
		};

		fixed4 TerrainWaveGrass2 (inout float4 vertex, float waveAmount, fixed4 color)
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

			vertex.xz -= waveMove.xz * _WaveAndDistance.z;
			
			// apply color animation
			fixed3 waveColor = lerp (fixed3(0.5,0.5,0.5), _WavingTint.rgb, lighting);
			
			// Fade the grass out before detail distance.
			// Saturate because Radeon HD drivers on OS X 10.4.10 don't saturate vertex colors properly.
			float3 offset = vertex.xyz - _CameraPosition.xyz;
			color.a = saturate (2 * (_WaveAndDistance.w - dot (offset, offset)) * _CameraPosition.w);
			
			return fixed4(2 * waveColor * color.rgb, color.a);
		}

		void TerrainBillboardGrass2( inout float4 pos, float2 offset )
		{
			float3 grasspos = pos.xyz - _CameraPosition.xyz;
			if (dot(grasspos, grasspos) > _WaveAndDistance.w)
				offset = 0.0;
		    pos.xyz += offset.x * _CameraRight.xyz;
			pos.xyz += offset.y * _CameraUp.xyz;
		}
		void WavingGrassBillboardVert2 (inout appdata_full v)
		{
			float vertexPosFade = v.color.a;
				float3 worldPos = mul(unity_ObjectToWorld,v.vertex);

				float3 uv = saturate((worldPos - PlayerPos.xyz)/64.0f + 0.5f);
	
				float forceTime = tex2Dlod(_TimeMap,float4(uv.xz,0,1)).r;
				float4 force = tex2Dlod(_ForceMap,float4(uv.xz,0,1));

				float4 forceInObjSpace = mul(float4((force.xyz - 1.0f)*2.0f,0.0f),unity_ObjectToWorld);
				
				float time_passed = PlayerPos.w - forceTime;
				float3 strong_period = forceInObjSpace.xyz * sin(time_passed * 4) * force.w*0.8;	//摇晃节奏

				//摇晃衰减
				float strong_decrease = exp(-time_passed * 1.2);
				float3 strong_fac = strong_period * strong_decrease * vertexPosFade;

			TerrainBillboardGrass2 (v.vertex, v.tangent.xy);
			// wave amount defined by the grass height
			float waveAmount = v.tangent.y;


			v.color = TerrainWaveGrass2 (v.vertex, waveAmount, v.color);
			v.vertex.xyz += strong_fac;
		}


		v2f BillboardVert (appdata_full v) {
			v2f o;
			WavingGrassBillboardVert2 (v);
			o.color = v.color;
			
			o.color.rgb *= ShadeVertexLights (v.vertex, v.normal);
			
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	
			o.uv = v.texcoord;

			return o;
		}
	ENDCG

	SubShader {
		Tags {
			"Queue" = "Geometry+200"
			"IgnoreProjector"="True"
			"RenderType"="GrassBillboard"
		}
		Cull Off
		LOD 200
		ColorMask RGB
				
		CGPROGRAM

			#pragma target 3.0		
			#pragma surface surf Lambert vertex:WavingGrassBillboardVert2 addshadow
			#pragma only_renderers d3d9
	

			sampler2D _MainTex;
			fixed _Cutoff;

			struct Input {
				float2 uv_MainTex;
				fixed4 color : COLOR;
			};

			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				#ifndef SHADER_API_D3D11
				// gamma ajust
				c.r = pow(c.r, 2.2);
				c.g = pow(c.g, 2.2);
				c.b = pow(c.b, 2.2);
				#endif
				
				o.Albedo = c.rgb;

				clip (c.a - _Cutoff);
				
				//o.Alpha *= IN.color.a;
				o.Alpha = 1.0f;
			}

		ENDCG			
	}
	Fallback Off
}
