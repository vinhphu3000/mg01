Shader "EJoyShader/Mesh/Creat-VGlow1-Glow2AutoV-Mask_camCap" {
    Properties {
		_CamCapture("DiffuseRGBA", 2D) = "white" {}
        _MainColor ("MainColor", Color) = (1,1,1,1)
        _GlowRGB ("GlowRGB", 2D) = "white" {}
        _GlowColor ("Glow-Color", Color) = (0.9705882,0.9705882,0.9705882,1)
        _Glowpower ("Glow-power", Range(0.1, 10)) = 2
        _GlowMaskClip ("Glow-Mask-Clip", Range(0, 1.05)) = 0.241623
        _ShapeR ("Shape(R)", 2D) = "bump" {}
        [MaterialToggle] _2UV ("2UV", Float ) = 0
        _OffsetClipV ("Offset-Clip (V)", Range(-1.5, 1)) = -0.02357209
        _GradientSpace ("Gradient-Space", Range(0, 1.2)) = 0.2416429
        _NoiseR ("Noise(R)", 2D) = "white" {}
        _MaskClip ("Mask-Clip", Range(0, 5)) = 0.6572078
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _CamCapture; uniform float4 _CamCapture_ST;
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float _Glowpower;
            uniform sampler2D _NoiseR; uniform float4 _NoiseR_ST;
            uniform float _GlowMaskClip;
            uniform float4 _GlowColor;
            uniform float _OffsetClipV;
            uniform float _MaskClip;
            uniform float _GradientSpace;
            uniform sampler2D _ShapeR; uniform float4 _ShapeR_ST;
            uniform fixed _2UV;
            uniform float4 _MainColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 node_1067 = (float2(0.0,_OffsetClipV)+i.uv1);
                float2 node_1205 = (float2(0.0,_GradientSpace)+node_1067);
                float2 _2UV_var = lerp( i.uv0, node_1205, _2UV );
                float4 _NoiseR_var = tex2D(_NoiseR,TRANSFORM_TEX(_2UV_var, _NoiseR));
                float node_8159 = (_NoiseR_var.a+0.1);
                float4 _ShapeRGBA1 = tex2D(_ShapeR,TRANSFORM_TEX(node_1067, _ShapeR));
                clip((((1.1*node_8159)*_MaskClip*i.vertexColor.a)*_ShapeRGBA1.r*_MaskClip) - 0.5);
////// Lighting:
////// Emissive:
                float4 _DiffuseRGBA_var = tex2D(_CamCapture,TRANSFORM_TEX(i.uv0, _CamCapture));
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(i.uv0, _GlowRGB));
                float4 _ShapeRGBA2_copy = tex2D(_ShapeR,TRANSFORM_TEX(node_1205, _ShapeR));
                float3 emissive = ((_DiffuseRGBA_var.rgb*_MainColor.rgb)+lerp(pow((_GlowRGB_var.rgb*1.2*_GlowColor.rgb),_Glowpower),float3(0,0,0),step(_GlowMaskClip,(node_8159*_ShapeRGBA2_copy.r*1.6*i.vertexColor.a))));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
}
