Shader "LP/Role/Role_Outline_me" {
    Properties {
        _BassRGB ("Bass-RGB", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Float) = 0.002
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _Soft ("Soft", Range(0.0, 1)) = 1
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest+12"
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest LEqual
            Blend Off
            
            // Stencil {
            //     Ref 1
            //     Comp Always
            //     Pass Replace 
            //     Fail keep 
            //     ZFail keep
            // }

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
        UsePass "Hidden/LP/Common/OUTLINE"
    }
    FallBack "A1/VertexLit"
}
