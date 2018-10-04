// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:0,nrsp:0,limd:3,spmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.8308824,fgcg:0.2871431,fgcb:0.2871431,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:7020,x:33980,y:32801,varname:node_7020,prsc:2|emission-6535-OUT;n:type:ShaderForge.SFN_Tex2d,id:5443,x:33435,y:32803,ptovrint:False,ptlb:BaseMap(RGB),ptin:_BaseMapRGB,varname:node_5443,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:187,x:33427,y:33035,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_187,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:6535,x:33718,y:32892,varname:node_6535,prsc:2|A-5443-RGB,B-187-RGB,C-1751-OUT;n:type:ShaderForge.SFN_Slider,id:1751,x:33335,y:33263,ptovrint:False,ptlb:Light,ptin:_Light,varname:node_1751,prsc:2,min:0,cur:1,max:4;proporder:5443-187-1751;pass:END;sub:END;*/

Shader "EJoyShader/Scene/Scene-Fog-Color" {
    Properties {
        _BaseMapRGB ("BaseMap(RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Light ("Light", Range(0, 4)) = 1
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
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _BaseMapRGB; uniform float4 _BaseMapRGB_ST;
            uniform float4 _Color;
            uniform float _Light;
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
                float4 _BaseMapRGB_var = tex2D(_BaseMapRGB,TRANSFORM_TEX(i.uv0, _BaseMapRGB));
                float3 emissive = (_BaseMapRGB_var.rgb*_Color.rgb*_Light);
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
