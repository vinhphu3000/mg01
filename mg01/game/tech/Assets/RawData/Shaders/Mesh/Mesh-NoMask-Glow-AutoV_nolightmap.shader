Shader "EJoyShader/Mesh/MeshGlow-2UV-AutoV_no_lightmap" {
    Properties {
        _BaseRGBA ("BaseRGBA", 2D) = "white" {}
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        _GlowRGB ("GlowRGB", 2D) = "white" {}
        _GlowColor ("GlowColor", Color) = (1,1,1,1)
        _AutoSpeed ("Auto-Speed", Range(-2, 2)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
            #include "UnityCG.cginc"

            #pragma multi_compile_fog
			#pragma only_renderers glcore gles gles3 metal d3d9 
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float4 _BaseColor;
            uniform float4 _GlowColor;
            uniform float _AutoSpeed;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float3 diffuseColor = _BaseRGBA_var.rgb;
                float3 diffuse = diffuseColor*_BaseColor.rgb;
////// Emissive:
                float4 node_3510 = _Time + _TimeEditor;
                float2 node_9653 = i.uv0 +(node_3510.g*_AutoSpeed)*float2(0,1);
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(node_9653, _GlowRGB));
                float3 emissive = (_BaseRGBA_var.a*_GlowRGB_var.rgb*_GlowColor.rgb);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
