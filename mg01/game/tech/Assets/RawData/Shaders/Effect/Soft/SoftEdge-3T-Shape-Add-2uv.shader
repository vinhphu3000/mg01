// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:0,spmd:1,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:True,tesm:0,blpr:3,bsrc:0,bdst:6,culm:2,dpts:2,wrdp:True,dith:0,ufog:False,aust:False,igpj:False,qofs:0,qpre:4,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2306556,fgcg:0.1892301,fgcb:0.7352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:40,x:32027,y:32574,varname:node_40,prsc:2|emission-1617-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:30051,y:32364,ptovrint:False,ptlb:Base-RGB,ptin:_BaseRGB,varname:_BaseRGBA,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:6096,x:30489,y:32158,ptovrint:False,ptlb:EdgeClip,ptin:_EdgeClip,varname:_EdgeClip,prsc:2,min:0,cur:7.494126,max:30;n:type:ShaderForge.SFN_Tex2d,id:5290,x:30055,y:32602,ptovrint:False,ptlb:Dark-RGB,ptin:_DarkRGB,varname:_DarkRGBA,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3312,x:31105,y:32303,varname:node_3312,prsc:2|A-217-RGB,B-6096-OUT,C-5145-RGB,D-5670-OUT,E-2735-OUT;n:type:ShaderForge.SFN_Tex2d,id:5145,x:30450,y:33002,ptovrint:False,ptlb:Shape-Alpha(RGBA)),ptin:_ShapeAlphaRGBA,varname:_ShapeAlphaR,prsc:2,ntxv:0,isnm:False|UVIN-9323-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:9323,x:30224,y:33002,ptovrint:False,ptlb:Shape-2uv,ptin:_Shape2uv,varname:node_9323,prsc:2,on:True|A-1045-UVOUT,B-4617-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1045,x:29986,y:32925,varname:node_1045,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:4617,x:29986,y:33081,varname:node_4617,prsc:2,uv:1;n:type:ShaderForge.SFN_Multiply,id:2735,x:30522,y:32452,varname:node_2735,prsc:2|A-217-RGB,B-5670-OUT,C-5290-RGB,D-5145-A;n:type:ShaderForge.SFN_Slider,id:5670,x:29778,y:32184,ptovrint:False,ptlb:BaseClip-Mul,ptin:_BaseClipMul,varname:node_5670,prsc:2,min:0,cur:1,max:30;n:type:ShaderForge.SFN_Power,id:8005,x:31516,y:32172,varname:node_8005,prsc:2|VAL-3312-OUT,EXP-813-OUT;n:type:ShaderForge.SFN_Slider,id:813,x:30976,y:32074,ptovrint:False,ptlb:Power,ptin:_Power,varname:_EdgeClip_copy,prsc:2,min:0,cur:4,max:30;n:type:ShaderForge.SFN_Color,id:5296,x:31262,y:32553,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5296,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1617,x:31468,y:32664,varname:node_1617,prsc:2|A-8005-OUT,B-5296-RGB;proporder:217-5290-5145-9323-6096-5670-813-5296;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Soft/SoftEdge-3T-Shape-Add-2uv" {
    Properties {
        _BaseRGB ("Base-RGB", 2D) = "white" {}
        _DarkRGB ("Dark-RGB", 2D) = "white" {}
        _ShapeAlphaRGBA ("Shape-Alpha(RGBA))", 2D) = "white" {}
        [MaterialToggle] _Shape2uv ("Shape-2uv", Float ) = 0
        _EdgeClip ("EdgeClip", Range(0, 30)) = 7.494126
        _BaseClipMul ("BaseClip-Mul", Range(0, 30)) = 1
        _Power ("Power", Range(0, 30)) = 4
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "Queue"="Overlay"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcColor
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers glcore gles gles3 metal d3d9
            #pragma target 3.0
            uniform sampler2D _BaseRGB; uniform float4 _BaseRGB_ST;
            uniform float _EdgeClip;
            uniform sampler2D _DarkRGB; uniform float4 _DarkRGB_ST;
            uniform sampler2D _ShapeAlphaRGBA; uniform float4 _ShapeAlphaRGBA_ST;
            uniform fixed _Shape2uv;
            uniform float _BaseClipMul;
            uniform float _Power;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _BaseRGB_var = tex2D(_BaseRGB,TRANSFORM_TEX(i.uv0, _BaseRGB));
                float2 _Shape2uv_var = lerp( i.uv0, i.uv1, _Shape2uv );
                float4 _ShapeAlphaRGBA_var = tex2D(_ShapeAlphaRGBA,TRANSFORM_TEX(_Shape2uv_var, _ShapeAlphaRGBA));
                float4 _DarkRGB_var = tex2D(_DarkRGB,TRANSFORM_TEX(i.uv0, _DarkRGB));
                float3 emissive = (pow((_BaseRGB_var.rgb*_EdgeClip*_ShapeAlphaRGBA_var.rgb*_BaseClipMul*(_BaseRGB_var.rgb*_BaseClipMul*_DarkRGB_var.rgb*_ShapeAlphaRGBA_var.a)),_Power)*_Color.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
