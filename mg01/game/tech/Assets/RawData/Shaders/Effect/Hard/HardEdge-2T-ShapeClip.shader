// Shader created with Shader Forge v1.10
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:0,spmd:1,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:True,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:False,aust:False,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2306556,fgcg:0.1892301,fgcb:0.7352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:40,x:32027,y:32574,varname:node_40,prsc:2|emission-1223-OUT,clip-9942-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:30986,y:32287,ptovrint:True,ptlb:Base-RGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,tex:f1b8bc6eadeddb24e9f3ba34972e9292,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:218,x:31482,y:32647,varname:node_218,prsc:2|A-2950-OUT,B-6096-OUT;n:type:ShaderForge.SFN_Slider,id:6096,x:31049,y:32796,ptovrint:False,ptlb:EdgeClip,ptin:_EdgeClip,varname:_EdgeClip,prsc:2,min:0,cur:6.74703,max:10;n:type:ShaderForge.SFN_VertexColor,id:4009,x:31411,y:32978,varname:node_4009,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9942,x:31729,y:32926,varname:node_9942,prsc:2|A-218-OUT,B-4009-A;n:type:ShaderForge.SFN_Tex2d,id:5290,x:30943,y:32523,ptovrint:False,ptlb:Shape-RGB,ptin:_ShapeRGB,varname:_ShapeRGB,prsc:2,tex:3720f407e2e235248ab00cb5ad9aed46,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2950,x:31201,y:32416,varname:node_2950,prsc:2|A-217-A,B-5290-RGB;n:type:ShaderForge.SFN_Multiply,id:1223,x:31725,y:32116,varname:node_1223,prsc:2|A-217-RGB,B-4009-RGB,C-6505-RGB;n:type:ShaderForge.SFN_Color,id:6505,x:31435,y:32279,ptovrint:False,ptlb:BaseColor,ptin:_BaseColor,varname:_BaseColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:217-5290-6096-6505;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Hard/HardEdge-2T-ShapeClip-VC" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        _ShapeRGB ("Shape-RGB", 2D) = "white" {}
        _EdgeClip ("EdgeClip", Range(0, 10)) = 6.74703
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
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
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _EdgeClip;
            uniform sampler2D _ShapeRGB; uniform float4 _ShapeRGB_ST;
            uniform float4 _BaseColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv0, _ShapeRGB));
                clip((((_BaseRGBA_var.a*_ShapeRGB_var.rgb)*_EdgeClip)*i.vertexColor.a) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_BaseRGBA_var.rgb*i.vertexColor.rgb*_BaseColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma only_renderers glcore gles gles3 metal d3d9
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _EdgeClip;
            uniform sampler2D _ShapeRGB; uniform float4 _ShapeRGB_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv0, _ShapeRGB));
                clip((((_BaseRGBA_var.a*_ShapeRGB_var.rgb)*_EdgeClip)*i.vertexColor.a) - 0.5);
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers glcore gles gles3 metal d3d9
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _EdgeClip;
            uniform sampler2D _ShapeRGB; uniform float4 _ShapeRGB_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv0, _ShapeRGB));
                clip((((_BaseRGBA_var.a*_ShapeRGB_var.rgb)*_EdgeClip)*i.vertexColor.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
