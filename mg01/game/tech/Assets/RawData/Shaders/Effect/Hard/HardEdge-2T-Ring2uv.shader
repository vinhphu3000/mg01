// Shader created with Shader Forge v1.10
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:0,spmd:1,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:True,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:False,aust:False,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2306556,fgcg:0.1892301,fgcb:0.7352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:40,x:32027,y:32574,varname:node_40,prsc:2|emission-9872-OUT,clip-821-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:31238,y:32235,ptovrint:True,ptlb:Base-RGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,tex:f1b8bc6eadeddb24e9f3ba34972e9292,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:6096,x:31069,y:32859,ptovrint:False,ptlb:EdgeClip,ptin:_EdgeClip,varname:_EdgeClip,prsc:2,min:0,cur:1,max:5;n:type:ShaderForge.SFN_VertexColor,id:4009,x:31188,y:32981,varname:node_4009,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:5290,x:31260,y:32644,ptovrint:False,ptlb:Shape2uv-RGB,ptin:_Shape2uvRGB,varname:_ShapeRGB,prsc:2,tex:56c9a3a8770b44f4dbeb598d7fa2db0c,ntxv:2,isnm:False|UVIN-7969-OUT;n:type:ShaderForge.SFN_Multiply,id:7969,x:31009,y:32616,varname:node_7969,prsc:2|A-8669-OUT,B-8377-UVOUT;n:type:ShaderForge.SFN_Slider,id:8669,x:30597,y:32540,ptovrint:False,ptlb:2UV,ptin:_2UV,varname:node_8669,prsc:2,min:0.9,cur:0.9,max:12;n:type:ShaderForge.SFN_TexCoord,id:8377,x:30724,y:32708,varname:node_8377,prsc:2,uv:1;n:type:ShaderForge.SFN_Add,id:9872,x:31815,y:32396,varname:node_9872,prsc:2|A-6638-OUT,B-7647-RGB;n:type:ShaderForge.SFN_Color,id:7647,x:31575,y:32216,ptovrint:False,ptlb:ColorAdd,ptin:_ColorAdd,varname:node_7647,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:821,x:31661,y:32873,varname:node_821,prsc:2|A-6096-OUT,B-4009-A,C-5290-RGB,D-217-RGB;n:type:ShaderForge.SFN_Multiply,id:6638,x:31604,y:32417,varname:node_6638,prsc:2|A-217-RGB,B-1359-RGB,C-5290-RGB,D-4009-RGB;n:type:ShaderForge.SFN_Color,id:1359,x:31255,y:32428,ptovrint:False,ptlb:ColorMul,ptin:_ColorMul,varname:_ColorMul_copy,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:217-5290-6096-8669-7647-1359;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Hard/HardEdge-2T-Ring2uv" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        _Shape2uvRGB ("Shape2uv-RGB", 2D) = "black" {}
        _EdgeClip ("EdgeClip", Range(0, 5)) = 1
        _2UV ("2UV", Range(0.9, 12)) = 0.9
        _ColorAdd ("ColorAdd", Color) = (0.5,0.5,0.5,1)
        _ColorMul ("ColorMul", Color) = (0.5,0.5,0.5,1)
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
            uniform sampler2D _Shape2uvRGB; uniform float4 _Shape2uvRGB_ST;
            uniform float _2UV;
            uniform float4 _ColorAdd;
            uniform float4 _ColorMul;
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
                float2 node_7969 = (_2UV*i.uv1);
                float4 _Shape2uvRGB_var = tex2D(_Shape2uvRGB,TRANSFORM_TEX(node_7969, _Shape2uvRGB));
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                clip((_EdgeClip*i.vertexColor.a*_Shape2uvRGB_var.rgb*_BaseRGBA_var.rgb) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = ((_BaseRGBA_var.rgb*_ColorMul.rgb*_Shape2uvRGB_var.rgb*i.vertexColor.rgb)+_ColorAdd.rgb);
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
            uniform sampler2D _Shape2uvRGB; uniform float4 _Shape2uvRGB_ST;
            uniform float _2UV;
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
                float2 node_7969 = (_2UV*i.uv1);
                float4 _Shape2uvRGB_var = tex2D(_Shape2uvRGB,TRANSFORM_TEX(node_7969, _Shape2uvRGB));
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                clip((_EdgeClip*i.vertexColor.a*_Shape2uvRGB_var.rgb*_BaseRGBA_var.rgb) - 0.5);
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
            uniform sampler2D _Shape2uvRGB; uniform float4 _Shape2uvRGB_ST;
            uniform float _2UV;
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
                float2 node_7969 = (_2UV*i.uv1);
                float4 _Shape2uvRGB_var = tex2D(_Shape2uvRGB,TRANSFORM_TEX(node_7969, _Shape2uvRGB));
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                clip((_EdgeClip*i.vertexColor.a*_Shape2uvRGB_var.rgb*_BaseRGBA_var.rgb) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
