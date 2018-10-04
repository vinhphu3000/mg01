Shader "Hidden/T1/Details/BillboardWavingDoublePass2" {
	Properties{
		_WavingTint("Fade Color", Color) = (.7,.6,.5, 0)
		_MainTex("Base (RGB) Alpha (A)", 2D) = "white" {}
		// _LightMap("LightMap", 2D) = "white" {}
		//_WaveAndDistance("Wave and distance", Vector) = (12, 3.6, 1, 1)
	}

    SubShader{
    	Tags{
			"Queue" = "Geometry-100"
    		"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
    	}
    	Cull Off
    	LOD 200
    	ColorMask RGB
    	Blend SrcAlpha OneMinusSrcAlpha


    	// ------------------------------------------------------------
    	// Surface shader code generated out of a CGPROGRAM block:


    	// ---- forward rendering base pass:
    Pass {
    		Name "FORWARD"
    		Tags { "LightMode" = "ForwardBase" }

    CGPROGRAM
    // compile directives
    #pragma vertex vert_surf
    #pragma fragment frag_surf
    #pragma glsl_no_auto_normalization
    #pragma only_renderers glcore gles gles3 metal d3d9
    #pragma multi_compile_fog
    #include "HLSLSupport.cginc"
    #include "UnityShaderVariables.cginc"

    #define UNITY_PASS_FORWARDBASE
    #include "UnityCG.cginc"
    #include "Lighting.cginc"
    #include "AutoLight.cginc"

    // Original surface shader snippet:

    	#include "UnityCG.cginc"
    	//#pragma glsl_no_auto_normalization

    	float4 _WavingTint;
    	float4 _WaveAndDistance;
    	float4 _M1CamRight;
    	float4 _M1CamUp;
    	float3 _M1CamForward;

    	// ---- Grass helpers

    	// Calculate a 4 fast sine-cosine pairs
    	// val: 	the 4 input values - each must be in the range (0 to 1)
    	// s:		The sine of each of the 4 values
    	// c:		The cosine of each of the 4 values
    	void FastSinCos(float4 val, out float4 s, out float4 c) {
    		val = val * 6.408849 - 3.1415927;
    		// powers for taylor series
    		float4 r5 = val * val;					// wavevec ^ 2
    		float4 r6 = r5 * r5;						// wavevec ^ 4;
    		float4 r7 = r6 * r5;						// wavevec ^ 6;
    		float4 r8 = r6 * r5;						// wavevec ^ 8;

    		float4 r1 = r5 * val;					// wavevec ^ 3
    		float4 r2 = r1 * r5;						// wavevec ^ 5;
    		float4 r3 = r2 * r5;						// wavevec ^ 7;


    													//Vectors for taylor's series expansion of sin and cos
    		float4 sin7 = { 1, -0.16161616, 0.0083333, -0.00019841 };
    		float4 cos8 = { -0.5, 0.041666666, -0.0013888889, 0.000024801587 };

    		// sin
    		s = val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;

    		// cos
    		c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w;
    	}



    	void TerrainWaveGrass1(inout float4 vertex, float waveAmount, fixed4 color)
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
    		waves = frac(waves);
    		FastSinCos(waves, s, c);

    		s = s * s;

    		s = s * s;

    		s = s * waveAmount;

    		float3 waveMove = float3 (0, 0, 0);
    		waveMove.x = dot(s, _waveXmove);
    		waveMove.z = dot(s, _waveZmove);

    		vertex.xz -= waveMove.xz * _WaveAndDistance.z;
    	}

    	void WavingGrassBillboardVert1(inout appdata_full v)
    	{
    		half movefactor = v.color.a;
    		float3 rltPos = v.vertex.xyz;
    		rltPos += v.normal.x * _M1CamRight.xyz;
    		rltPos += v.normal.y * _M1CamUp.xyz;

    		v.vertex.xyz = rltPos;

    		// wave amount defined by the grass height
    		float waveAmount = v.color.a * _WaveAndDistance.z;
    		TerrainWaveGrass1(v.vertex, waveAmount, v.color);
            v.color.a = 1.0;
    	}

        sampler2D _MainTex;
        // sampler2D _LightMap;

        // with lightmaps:
        struct v2f_surf {
          float4 pos : SV_POSITION;
          float2 pack0 : TEXCOORD0; // _MainTex
          fixed4 color : COLOR0;
          UNITY_FOG_COORDS(2)
        };
        float4 _MainTex_ST;

        // vertex shader
        v2f_surf vert_surf (appdata_full v) {
          v2f_surf o = (v2f_surf)0;
          WavingGrassBillboardVert1 (v);
          o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
          o.pack0 = TRANSFORM_TEX(v.texcoord, _MainTex);
          o.color = v.color;
          // o.pack0.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
          UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
          return o;
        }

        // fragment shader
        fixed4 frag_surf (v2f_surf IN) : SV_Target {
        	fixed4 c = tex2D(_MainTex, IN.pack0.xy)*IN.color *_WavingTint;
        	clip(c.a - 0.5);

        	// fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(_LightMap, IN.pack0.zw);
        	// half3 bakedColor = DecodeLightmap(bakedColorTex);

        	// realtime lighting: call lighting function
        	// c.rgb *= max(0.85,bakedColor);
         	UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
         	UNITY_OPAQUE_ALPHA(c.a);
          return c;
        }

        ENDCG

        }
    }
	Fallback "Transparent/Cutout/VertexLit"
}
