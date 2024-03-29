// Shader created with Shader Forge v1.10
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:0,spmd:1,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:True,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:False,aust:False,igpj:True,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2306556,fgcg:0.1892301,fgcb:0.7352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:40,x:32027,y:32574,varname:node_40,prsc:2|emission-5324-OUT,clip-9942-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:30920,y:32194,ptovrint:True,ptlb:Base-RGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,tex:f1b8bc6eadeddb24e9f3ba34972e9292,ntxv:0,isnm:False|UVIN-6536-OUT;n:type:ShaderForge.SFN_Multiply,id:218,x:31479,y:32714,varname:node_218,prsc:2|A-2950-OUT,B-6096-OUT;n:type:ShaderForge.SFN_Slider,id:6096,x:31039,y:32837,ptovrint:False,ptlb:EdgeClip,ptin:_EdgeClip,varname:_EdgeClip,prsc:2,min:0,cur:6.74703,max:10;n:type:ShaderForge.SFN_VertexColor,id:4009,x:31463,y:32969,varname:node_4009,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9942,x:31729,y:32926,varname:node_9942,prsc:2|A-218-OUT,B-4009-A;n:type:ShaderForge.SFN_Tex2d,id:5290,x:30910,y:32545,ptovrint:False,ptlb:Shape-RGB,ptin:_ShapeRGB,varname:_ShapeRGB,prsc:2,tex:3720f407e2e235248ab00cb5ad9aed46,ntxv:0,isnm:False|UVIN-4187-UVOUT;n:type:ShaderForge.SFN_Multiply,id:2950,x:31236,y:32568,varname:node_2950,prsc:2|A-1286-OUT,B-5290-RGB;n:type:ShaderForge.SFN_Multiply,id:1223,x:31613,y:32221,varname:node_1223,prsc:2|A-217-RGB,B-6505-RGB,C-5290-RGB;n:type:ShaderForge.SFN_Color,id:6505,x:31375,y:32364,ptovrint:False,ptlb:BaseColor,ptin:_BaseColor,varname:_BaseColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_TexCoord,id:4187,x:30487,y:32410,varname:node_4187,prsc:2,uv:1;n:type:ShaderForge.SFN_SwitchProperty,id:6536,x:30737,y:32194,ptovrint:False,ptlb:2uv,ptin:_2uv,varname:node_6536,prsc:2,on:False|A-3699-UVOUT,B-4187-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3699,x:30493,y:32013,varname:node_3699,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:1286,x:31152,y:32322,varname:node_1286,prsc:2|A-217-A,B-6527-OUT;n:type:ShaderForge.SFN_Slider,id:6527,x:30763,y:32386,ptovrint:False,ptlb:BaseAlpha-Add,ptin:_BaseAlphaAdd,varname:node_6527,prsc:2,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Color,id:2918,x:31603,y:32405,ptovrint:False,ptlb:Add-Color,ptin:_AddColor,varname:node_2918,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:5324,x:31864,y:32324,varname:node_5324,prsc:2|A-1223-OUT,B-2918-RGB;n:type:ShaderForge.SFN_Normalize,id:6851,x:31187,y:33264,varname:node_6851,prsc:2;proporder:217-6536-5290-6096-6527-6505-2918;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Hard/HardEdge-3T-AutoU-2uv" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        [MaterialToggle] _2uv ("2uv", Float ) = 0
        _ShapeRGB ("Shape-RGB", 2D) = "white" {}
        _EdgeClip ("EdgeClip", Range(0, 10)) = 6.74703
        _BaseAlphaAdd ("BaseAlpha-Add", Range(0, 1)) = 0.5
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
        _AddColor ("Add-Color", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
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
            #pragma multi_compile_fwdbase
            #pragma only_renderers glcore gles gles3 metal d3d9
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform float _EdgeClip;
            uniform sampler2D _ShapeRGB; uniform float4 _ShapeRGB_ST;
            uniform float4 _BaseColor;
            uniform fixed _2uv;
            uniform float _BaseAlphaAdd;
            uniform float4 _AddColor;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 _2uv_var = lerp( i.uv0, i.uv1, _2uv );
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(_2uv_var, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv1, _ShapeRGB));
                clip(((((_BaseRGBA_var.a+_BaseAlphaAdd)*_ShapeRGB_var.rgb)*_EdgeClip)*i.vertexColor.a) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = ((_BaseRGBA_var.rgb*_BaseColor.rgb*_ShapeRGB_var.rgb)+_AddColor.rgb);
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
            uniform fixed _2uv;
            uniform float _BaseAlphaAdd;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float2 uv1 : TEXCOORD6;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 _2uv_var = lerp( i.uv0, i.uv1, _2uv );
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(_2uv_var, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv1, _ShapeRGB));
                clip(((((_BaseRGBA_var.a+_BaseAlphaAdd)*_ShapeRGB_var.rgb)*_EdgeClip)*i.vertexColor.a) - 0.5);
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
            uniform fixed _2uv;
            uniform float _BaseAlphaAdd;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float2 _2uv_var = lerp( i.uv0, i.uv1, _2uv );
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(_2uv_var, _BaseRGBA));
                float4 _ShapeRGB_var = tex2D(_ShapeRGB,TRANSFORM_TEX(i.uv1, _ShapeRGB));
                clip(((((_BaseRGBA_var.a+_BaseAlphaAdd)*_ShapeRGB_var.rgb)*_EdgeClip)*i.vertexColor.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
