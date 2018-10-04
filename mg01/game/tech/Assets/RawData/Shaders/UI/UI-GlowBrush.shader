// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2745,x:32758,y:32692,varname:node_2745,prsc:2|emission-3457-OUT,alpha-2777-A;n:type:ShaderForge.SFN_Tex2d,id:2777,x:31838,y:32349,ptovrint:False,ptlb:Base,ptin:_Base,varname:_Base,prsc:2,tex:c1489cdf262f53545afad899ba08f2a6,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:407,x:32175,y:32716,varname:node_407,prsc:2|A-2777-RGB,B-1709-RGB,T-4686-OUT;n:type:ShaderForge.SFN_Color,id:1709,x:31922,y:32917,ptovrint:False,ptlb:Target-Color,ptin:_TargetColor,varname:_TargetColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Slider,id:4686,x:31691,y:32760,ptovrint:False,ptlb:Lerp,ptin:_Lerp,varname:_Lerp,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:7019,x:32103,y:32176,ptovrint:True,ptlb:Glow,ptin:_Glow,varname:_Glow,prsc:1,tex:33743177ea5ffc349bd748cd7c4eb9ec,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:3457,x:32432,y:32558,varname:node_3457,prsc:2|A-407-OUT,B-7019-RGB;proporder:2777-7019-4686-1709;pass:END;sub:END;*/

Shader "EJoyShader/UI/GlowBrush" {
    Properties {
        _Base ("Base", 2D) = "white" {}
        _Glow ("Glow", 2D) = "white" {}
        _Lerp ("Lerp", Range(0, 1)) = 0
        _TargetColor ("Target-Color", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform float4 _TargetColor;
            uniform float _Lerp;
            uniform sampler2D _Glow; uniform float4 _Glow_ST;
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
                float4 _Base_var = tex2D(_Base,TRANSFORM_TEX(i.uv0, _Base));
                half4 _Glow_var = tex2D(_Glow,TRANSFORM_TEX(i.uv0, _Glow));
                float3 emissive = (lerp(_Base_var.rgb,_TargetColor.rgb,_Lerp)+_Glow_var.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,_Base_var.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
