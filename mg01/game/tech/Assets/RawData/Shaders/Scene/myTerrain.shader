Shader "a2/Terrain/Diffuse" {
	Properties {
		_Mask ("Control (RGBA)", 2D) = "red" {}
		_R ("Layer 0 (R)", 2D) = "white" {}
		_G ("Layer 1 (G)", 2D) = "white" {}
		_B ("Layer 2 (B)", 2D) = "white" {}
		_A ("Layer 3 (A)", 2D) = "white" {}
		
	}

	CGINCLUDE
		#pragma surface surf Lambert vertex:SplatmapVert finalcolor:myfinal exclude_path:prepass exclude_path:deferred
		#pragma multi_compile_fog

		struct Input
		{
			float2 uv_R : TEXCOORD0;
			float2 uv_G : TEXCOORD1;
			float2 uv_B : TEXCOORD2;
			float2 uv_A : TEXCOORD3;
			float2 tc_Mask : TEXCOORD4;
			UNITY_FOG_COORDS(5)
		};

		sampler2D _Mask;
		float4 _Mask_ST;
		sampler2D _R,_G,_B,_A;


		void SplatmapVert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			data.tc_Mask = TRANSFORM_TEX(v.texcoord, _Mask);	// Need to manually transform uv here, as we choose not to use 'uv' prefix for this texcoord.
			float4 pos = mul (UNITY_MATRIX_MVP, v.vertex);
			UNITY_TRANSFER_FOG(data, pos);
		}

		void SplatmapMix(Input IN, out half4 splat_Mask, out half weight, out fixed4 mixedDiffuse, inout fixed3 mixedNormal)
		{
			splat_Mask = normalize(tex2D(_Mask, IN.tc_Mask));
			weight = dot(splat_Mask, half4(1,1,1,1));

			// #ifndef UNITY_PASS_DEFERRED
				// Normalize weights before lighting and restore weights in applyWeights function so that the overal
				// lighting result can be correctly weighted.
				// In G-Buffer pass we don't need to do it if Additive blending is enabled.
				// TODO: Normal blending in G-buffer pass...
				// splat_Mask /= (weight + 1e-3f); // avoid NaNs in splat_Mask
			// #endif

			//#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
			//	clip(weight - 0.0039 /*1/255*/);
			//#endif
			
			

			mixedDiffuse = 0.0f;
			/// mixedDiffuse = tex2D(_G,IN.uv_G);
			mixedDiffuse += splat_Mask.r * tex2D(_R, IN.uv_R);
			mixedDiffuse += splat_Mask.g * tex2D(_G, IN.uv_G);
			mixedDiffuse += splat_Mask.b * tex2D(_B, IN.uv_B);
			mixedDiffuse += splat_Mask.a * tex2D(_A, IN.uv_A);
		}

		void SplatmapApplyWeight(inout fixed4 color, fixed weight)
		{
			// color.rgb *= weight;
			color.a = 1.0f;
		}

		void SplatmapApplyFog(inout fixed4 color, Input IN)
		{
			UNITY_APPLY_FOG(IN.fogCoord, color);
		}




		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 splat_Mask;
			half weight;
			fixed4 mixedDiffuse;
			SplatmapMix(IN, splat_Mask, weight, mixedDiffuse, o.Normal);
			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = weight;
		}

		void myfinal(Input IN, SurfaceOutput o, inout fixed4 color)
		{
			SplatmapApplyWeight(color, o.Alpha);
			SplatmapApplyFog(color, IN);
		}

	ENDCG

	Category {
		Tags {
			"SplatCount" = "4"
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}
		SubShader { // for sm3.0+ targets
			CGPROGRAM
				#pragma target 3.0
			ENDCG
		}
		SubShader { // for sm2.0 targets
			CGPROGRAM
			ENDCG
		}
	}

	Fallback "VertexLit"
}
