// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:True,dith:0,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:227,x:33157,y:32628,varname:node_227,prsc:2|emission-4174-OUT,clip-4872-OUT;n:type:ShaderForge.SFN_Tex2d,id:6617,x:32373,y:32552,ptovrint:False,ptlb:Base,ptin:_Base,varname:node_6617,prsc:2,tex:27b817e489385814c96183a2254bca75,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:4174,x:32724,y:32627,varname:node_4174,prsc:2|A-6617-RGB,B-8694-RGB;n:type:ShaderForge.SFN_Tex2d,id:8694,x:32373,y:32724,ptovrint:False,ptlb:node_8694,ptin:_node_8694,varname:node_8694,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4872,x:32725,y:33023,varname:node_4872,prsc:2|A-8370-R,B-4831-OUT;n:type:ShaderForge.SFN_Tex2d,id:8370,x:32373,y:32937,ptovrint:False,ptlb:node_8370,ptin:_node_8370,varname:node_8370,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:4831,x:32278,y:33207,ptovrint:False,ptlb:node_4831,ptin:_node_4831,varname:node_4831,prsc:2,min:0,cur:0.6842703,max:1;proporder:6617-8694-8370-4831;pass:END;sub:END;*/

Shader "EJoyShader/UI/TargetColor" {
    Properties {
        _Base ("Base", 2D) = "white" {}
        _node_8694 ("node_8694", 2D) = "white" {}
        _node_8370 ("node_8370", 2D) = "white" {}
        _node_4831 ("node_4831", Range(0, 1)) = 0.6842703
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
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers glcore gles gles3 metal d3d9 
            #pragma target 3.0
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _node_8694; uniform float4 _node_8694_ST;
            uniform sampler2D _node_8370; uniform float4 _node_8370_ST;
            uniform float _node_4831;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _node_8370_var = tex2D(_node_8370,TRANSFORM_TEX(i.uv0, _node_8370));
                clip((_node_8370_var.r*_node_4831) - 0.5);
////// Lighting:
////// Emissive:
                float4 _Base_var = tex2D(_Base,TRANSFORM_TEX(i.uv0, _Base));
                float4 _node_8694_var = tex2D(_node_8694,TRANSFORM_TEX(i.uv0, _node_8694));
                float3 emissive = (_Base_var.rgb+_node_8694_var.rgb);
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
            uniform sampler2D _node_8370; uniform float4 _node_8370_ST;
            uniform float _node_4831;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _node_8370_var = tex2D(_node_8370,TRANSFORM_TEX(i.uv0, _node_8370));
                clip((_node_8370_var.r*_node_4831) - 0.5);
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
            uniform sampler2D _node_8370; uniform float4 _node_8370_ST;
            uniform float _node_4831;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _node_8370_var = tex2D(_node_8370,TRANSFORM_TEX(i.uv0, _node_8370));
                clip((_node_8370_var.r*_node_4831) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
