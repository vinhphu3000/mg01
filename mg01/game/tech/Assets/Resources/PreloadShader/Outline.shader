﻿
Shader "Hidden/LP/Common" {
	Properties {
        _BassRGB ("Bass-RGB", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Float) = 0.002
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _Soft ("Soft", Range(0.0, 1)) = 1
       	_Color("color multiply", Color) = (1, 1, 1, 1)
	}
	SubShader {
        Tags {
            "Queue"="AlphaTest+10"
            "RenderType"="Opaque"
        }
		LOD 200

		Pass {
            Name "ROLEBASE"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite true
            ZTest LEqual
            Blend Off
            Stencil {
                Ref 1
                Comp Always
                Pass Replace 
                Fail keep 
                ZFail keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma only_renderers glcore gles gles3 metal d3d9
            uniform sampler2D _BassRGB; uniform float4 _BassRGB_ST;
            uniform float4 _RimColor;
            uniform float _Soft;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				float4 base = tex2D(_BassRGB,TRANSFORM_TEX(i.uv0, _BassRGB));
                base.rgb = lerp((_RimColor.rgb + base.rgb), base.rgb, _Soft);
                return base;
            }
            ENDCG
        }

        Pass {
            Name "ROLEBASECOLOR"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest LEqual
            Blend Off
            Stencil {
                Ref 1
                Comp Always
                Pass Replace 
                Fail keep 
                ZFail keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma only_renderers glcore gles gles3 metal d3d9
            uniform sampler2D _BassRGB; uniform float4 _BassRGB_ST;
            uniform float4 _RimColor;
            uniform float4 _Color;
            uniform float _Soft;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				float4 base = tex2D(_BassRGB,TRANSFORM_TEX(i.uv0, _BassRGB));
				base.rgb = base.rgb*_Color.rgb*base.a + base.rgb *saturate(5 - base.a*100);
                base.rgb = lerp((_RimColor.rgb + base.rgb), base.rgb, _Soft);
                return base;
            }
            ENDCG
        }


		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			//ZWrite On
			Blend Off
			ZTest Less
			Offset 9, 10

			CGINCLUDE
			#include "UnityCG.cginc"
			ENDCG

			CGPROGRAM
			#pragma vertex vert_outline
			#pragma fragment frag_outline
			#pragma only_renderers glcore gles gles3 metal d3d9

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : POSITION;
			};
	
			uniform fixed _Outline;
			uniform fixed4 _OutlineColor;

			v2f vert_outline(appdata v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				fixed3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				fixed2 offset = normalize(TransformViewToProjection(norm.xy));

				//o.pos.z += 0.001;
				o.pos.xy += offset * _Outline * o.pos.w;
				return o;
			}

			half4 frag_outline(v2f i) : COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}
	}
}