// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5297399,fgcg:0.4848616,fgcb:0.8676471,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1528,x:32899,y:32616,varname:node_1528,prsc:2|emission-2792-OUT;n:type:ShaderForge.SFN_Tex2d,id:3009,x:32395,y:32642,ptovrint:False,ptlb:BaseRGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3458,x:32153,y:32813,ptovrint:False,ptlb:GlowRGB,ptin:_GlowRGB,varname:_GlowRGB,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9706,x:32442,y:32890,varname:node_9706,prsc:2|A-3458-RGB,B-33-RGB;n:type:ShaderForge.SFN_Color,id:33,x:32155,y:33101,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:2792,x:32704,y:32763,varname:node_2792,prsc:2|A-3009-RGB,B-9706-OUT;proporder:3009-3458-33;pass:END;sub:END;*/

Shader "EJoyShader/Mesh/MeshGlow-Effect" {
    Properties {
        _BaseRGBA ("BaseRGBA", 2D) = "white" {}
        _GlowRGB ("GlowRGB", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float4 _Color;
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
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(i.uv0, _GlowRGB));
                float3 emissive = (_BaseRGBA_var.rgb+(_GlowRGB_var.rgb*_Color.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
