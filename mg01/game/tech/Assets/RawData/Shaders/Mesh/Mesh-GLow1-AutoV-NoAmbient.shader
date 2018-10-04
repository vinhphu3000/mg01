// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:True,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5297399,fgcg:0.4848616,fgcb:0.8676471,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1528,x:32899,y:32616,varname:node_1528,prsc:2|emission-6770-OUT;n:type:ShaderForge.SFN_Tex2d,id:3009,x:32177,y:32610,ptovrint:False,ptlb:Base(RGBA),ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,tex:8f9193b584fd895498c4ae9765c146cf,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3458,x:32130,y:32892,ptovrint:False,ptlb:Glow(RGB),ptin:_GlowRGB,varname:_GlowRGB,prsc:2,ntxv:0,isnm:False|UVIN-9653-UVOUT;n:type:ShaderForge.SFN_Multiply,id:9706,x:32412,y:32881,varname:node_9706,prsc:2|A-3009-A,B-3458-RGB,C-33-RGB,D-1444-RGB;n:type:ShaderForge.SFN_Color,id:33,x:32154,y:33179,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Time,id:3510,x:31547,y:33082,varname:node_3510,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1899,x:31822,y:33154,varname:node_1899,prsc:2|A-3510-T,B-581-OUT;n:type:ShaderForge.SFN_Slider,id:581,x:31396,y:33277,ptovrint:False,ptlb:Auto-Speed,ptin:_AutoSpeed,varname:_AutoSpeed,prsc:2,min:-1,cur:-0.2,max:1;n:type:ShaderForge.SFN_Panner,id:9653,x:31843,y:32891,varname:node_9653,prsc:2,spu:0,spv:1|UVIN-432-OUT,DIST-1899-OUT;n:type:ShaderForge.SFN_TexCoord,id:5245,x:31356,y:32673,varname:node_5245,prsc:2,uv:1;n:type:ShaderForge.SFN_TexCoord,id:4363,x:31353,y:32837,varname:node_4363,prsc:2,uv:0;n:type:ShaderForge.SFN_SwitchProperty,id:432,x:31603,y:32747,ptovrint:False,ptlb:Glow-2uv,ptin:_Glow2uv,varname:node_432,prsc:2,on:False|A-5245-UVOUT,B-4363-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1444,x:32172,y:33422,ptovrint:False,ptlb:GlowMask(RGB),ptin:_GlowMaskRGB,varname:node_1444,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:6770,x:32621,y:32696,varname:node_6770,prsc:2|A-3009-RGB,B-9706-OUT;proporder:3009-3458-33-581-432-1444;pass:END;sub:END;*/

Shader "EJoyShader/Mesh/MeshGlow-Mask-AutoV-NoAmbient" {
    Properties {
        _BaseRGBA ("Base(RGBA)", 2D) = "white" {}
        _GlowRGB ("Glow(RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _AutoSpeed ("Auto-Speed", Range(-1, 1)) = -0.2
        [MaterialToggle] _Glow2uv ("Glow-2uv", Float ) = 0
        _GlowMaskRGB ("GlowMask(RGB)", 2D) = "white" {}
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
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform sampler2D _GlowRGB; uniform float4 _GlowRGB_ST;
            uniform float4 _Color;
            uniform float _AutoSpeed;
            uniform fixed _Glow2uv;
            uniform sampler2D _GlowMaskRGB; uniform float4 _GlowMaskRGB_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(i.uv0, _BaseRGBA));
                float4 node_3510 = _Time + _TimeEditor;
                float2 node_9653 = (lerp( i.uv1, i.uv0, _Glow2uv )+(node_3510.g*_AutoSpeed)*float2(0,1));
                float4 _GlowRGB_var = tex2D(_GlowRGB,TRANSFORM_TEX(node_9653, _GlowRGB));
                float4 _GlowMaskRGB_var = tex2D(_GlowMaskRGB,TRANSFORM_TEX(i.uv0, _GlowMaskRGB));
                float3 emissive = (_BaseRGBA_var.rgb+(_BaseRGBA_var.a*_GlowRGB_var.rgb*_Color.rgb*_GlowMaskRGB_var.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack Off
    CustomEditor "ShaderForgeMaterialInspector"
}
