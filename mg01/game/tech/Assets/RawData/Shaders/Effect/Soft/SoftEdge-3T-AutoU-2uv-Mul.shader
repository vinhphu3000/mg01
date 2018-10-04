// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,hqlp:True,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:6,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:4,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2306556,fgcg:0.1892301,fgcb:0.7352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:40,x:32027,y:32574,varname:node_40,prsc:2|emission-9223-OUT;n:type:ShaderForge.SFN_Tex2d,id:217,x:30638,y:32373,ptovrint:False,ptlb:Base-RGBA,ptin:_BaseRGBA,varname:_BaseRGBA,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6551-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:4009,x:31225,y:32855,varname:node_4009,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:5290,x:30635,y:32660,ptovrint:False,ptlb:Dark-RGBA,ptin:_DarkRGBA,varname:_DarkRGBA,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5576-UVOUT;n:type:ShaderForge.SFN_Panner,id:6551,x:30359,y:32363,varname:P2,prsc:1,spu:0.1,spv:0|UVIN-1385-UVOUT,DIST-3116-OUT;n:type:ShaderForge.SFN_Multiply,id:3366,x:31236,y:32696,varname:node_3366,prsc:2|A-217-A,B-5290-A;n:type:ShaderForge.SFN_Panner,id:5576,x:30357,y:32658,varname:P1,prsc:2,spu:0,spv:0.35|UVIN-1385-UVOUT,DIST-3116-OUT;n:type:ShaderForge.SFN_Slider,id:3242,x:29646,y:32575,ptovrint:True,ptlb:Auto-Speed,ptin:_AutoSpeed,varname:_AutoSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-4,cur:0.3,max:4;n:type:ShaderForge.SFN_Time,id:4769,x:29802,y:32427,varname:node_4769,prsc:0;n:type:ShaderForge.SFN_Multiply,id:3116,x:30040,y:32495,varname:node_3116,prsc:2|A-4769-T,B-3242-OUT;n:type:ShaderForge.SFN_Color,id:6223,x:30936,y:32051,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9705882,c2:0.9705882,c3:0.9705882,c4:1;n:type:ShaderForge.SFN_Multiply,id:3312,x:31238,y:32437,varname:node_3312,prsc:2|A-217-RGB,B-5290-RGB,C-6223-RGB;n:type:ShaderForge.SFN_Tex2d,id:5145,x:31224,y:33120,ptovrint:False,ptlb:Shape-Alpha(R),ptin:_ShapeAlphaR,varname:_ShapeAlphaR,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9323-OUT;n:type:ShaderForge.SFN_Multiply,id:9223,x:31749,y:32477,varname:node_9223,prsc:2|A-3312-OUT,B-4900-OUT,C-5145-R,D-3366-OUT,E-4009-A;n:type:ShaderForge.SFN_Slider,id:4900,x:31218,y:32608,ptovrint:False,ptlb:Power,ptin:_Power,varname:_Power_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:20;n:type:ShaderForge.SFN_SwitchProperty,id:9323,x:30971,y:33083,ptovrint:False,ptlb:Shape-2uv,ptin:_Shape2uv,varname:node_9323,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-1045-UVOUT,B-4617-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1045,x:30734,y:32995,varname:node_1045,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:4617,x:30733,y:33162,varname:node_4617,prsc:2,uv:1;n:type:ShaderForge.SFN_TexCoord,id:1385,x:30093,y:32242,varname:node_1385,prsc:2,uv:0;proporder:217-5290-5145-9323-6223-3242-4900;pass:END;sub:END;*/

Shader "EJoyShader/Effect/Soft/SoftEdge-3T-AutoU-2uv-Mul" {
    Properties {
        _BaseRGBA ("Base-RGBA", 2D) = "white" {}
        _DarkRGBA ("Dark-RGBA", 2D) = "white" {}
        _ShapeAlphaR ("Shape-Alpha(R)", 2D) = "white" {}
        [MaterialToggle] _Shape2uv ("Shape-2uv", Float ) = 0
        _Color ("Color", Color) = (0.9705882,0.9705882,0.9705882,1)
        _AutoSpeed ("Auto-Speed", Range(-4, 4)) = 0.3
        _Power ("Power", Range(0, 20)) = 2
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers d3d11 d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseRGBA; uniform float4 _BaseRGBA_ST;
            uniform sampler2D _DarkRGBA; uniform float4 _DarkRGBA_ST;
            uniform float _AutoSpeed;
            uniform float4 _Color;
            uniform sampler2D _ShapeAlphaR; uniform float4 _ShapeAlphaR_ST;
            uniform float _Power;
            uniform fixed _Shape2uv;
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
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                fixed4 node_4769 = _Time + _TimeEditor;
                float node_3116 = (node_4769.g*_AutoSpeed);
                half2 P2 = (i.uv0+node_3116*float2(0.1,0));
                float4 _BaseRGBA_var = tex2D(_BaseRGBA,TRANSFORM_TEX(P2, _BaseRGBA));
                float2 P1 = (i.uv0+node_3116*float2(0,0.35));
                float4 _DarkRGBA_var = tex2D(_DarkRGBA,TRANSFORM_TEX(P1, _DarkRGBA));
                float2 _Shape2uv_var = lerp( i.uv0, i.uv1, _Shape2uv );
                float4 _ShapeAlphaR_var = tex2D(_ShapeAlphaR,TRANSFORM_TEX(_Shape2uv_var, _ShapeAlphaR));
                float3 emissive = ((_BaseRGBA_var.rgb*_DarkRGBA_var.rgb*_Color.rgb)*_Power*_ShapeAlphaR_var.r*(_BaseRGBA_var.a*_DarkRGBA_var.a)*i.vertexColor.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
