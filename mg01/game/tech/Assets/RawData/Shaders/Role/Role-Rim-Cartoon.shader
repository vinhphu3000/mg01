// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:2,spmd:1,grmd:0,uamb:False,mssp:False,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.49126,fgcg:0.4463668,fgcb:0.6323529,fgca:1,fgde:0.01,fgrn:20,fgrf:50,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4883,x:32725,y:32712,varname:node_4883,prsc:2|emission-7801-OUT;n:type:ShaderForge.SFN_Tex2d,id:1114,x:31700,y:32878,ptovrint:False,ptlb:Bass-RGB,ptin:_BassRGB,varname:_BassRGB,prsc:2,tex:d1c5c311c0cb8fa4fac17e1ca4a70c9f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ViewVector,id:9327,x:31149,y:33038,varname:node_9327,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4406,x:31152,y:33246,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4011,x:31442,y:33136,varname:node_4011,prsc:2,dt:4|A-9327-OUT,B-4406-OUT;n:type:ShaderForge.SFN_Lerp,id:7801,x:32284,y:33037,varname:node_7801,prsc:2|A-2145-OUT,B-1114-RGB,T-1791-OUT;n:type:ShaderForge.SFN_Color,id:5440,x:31690,y:32558,ptovrint:False,ptlb:node_5440,ptin:_node_5440,varname:_node_5440,prsc:2,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Blend,id:2145,x:32001,y:32700,varname:node_2145,prsc:2,blmd:14,clmp:True|SRC-5440-RGB,DST-1114-RGB;n:type:ShaderForge.SFN_Multiply,id:8175,x:31960,y:33376,varname:node_8175,prsc:2|A-4011-OUT,B-608-OUT;n:type:ShaderForge.SFN_Slider,id:608,x:31349,y:33405,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:_Edge,prsc:2,min:0.6,cur:0.6,max:12;n:type:ShaderForge.SFN_Power,id:5370,x:31860,y:33191,varname:node_5370,prsc:2|VAL-4011-OUT,EXP-608-OUT;n:type:ShaderForge.SFN_Posterize,id:1791,x:32221,y:33305,varname:node_1791,prsc:2|IN-5370-OUT,STPS-7598-OUT;n:type:ShaderForge.SFN_Vector1,id:7598,x:31858,y:33560,varname:node_7598,prsc:2,v1:3;proporder:1114-5440-608;pass:END;sub:END;*/

Shader "EJoyShader/Role/Rim-Cartoon" {
    Properties {
        _BassRGB ("Bass-RGB", 2D) = "white" {}
        _node_5440 ("node_5440", Color) = (1,0,0,1)
        _Edge ("Edge", Range(0.6, 12)) = 0.6
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
            uniform sampler2D _BassRGB; uniform float4 _BassRGB_ST;
            uniform float4 _node_5440;
            uniform float _Edge;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _BassRGB_var = tex2D(_BassRGB,TRANSFORM_TEX(i.uv0, _BassRGB));
                float node_4011 = 0.5*dot(viewDirection,i.normalDir)+0.5;
                float node_7598 = 3.0;
                float3 emissive = lerp(saturate(( _node_5440.rgb > 0.5 ? (_BassRGB_var.rgb + 2.0*_node_5440.rgb -1.0) : (_BassRGB_var.rgb + 2.0*(_node_5440.rgb-0.5)))),_BassRGB_var.rgb,floor(pow(node_4011,_Edge) * node_7598) / (node_7598 - 1));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    }
